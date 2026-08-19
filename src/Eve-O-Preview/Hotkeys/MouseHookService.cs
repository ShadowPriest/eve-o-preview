using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace EveOPreview.UI.Hotkeys
{
	sealed class MouseHookService : IMouseHookService, IDisposable
	{
		#region Native methods
		private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

		[StructLayout(LayoutKind.Sequential)]
		private struct MSLLHOOKSTRUCT
		{
			public int X;
			public int Y;
			public uint MouseData;
			public uint Flags;
			public uint Time;
			public IntPtr ExtraInfo;
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll")]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern short GetKeyState(int nVirtKey);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		private const int WH_MOUSE_LL = 14;
		private const int WM_MOUSEWHEEL = 0x020A;
		private const int WM_MBUTTONDOWN = 0x0207;
		private const int WM_XBUTTONDOWN = 0x020B;

		private const int VK_SHIFT = 0x10;
		private const int VK_CONTROL = 0x11;
		private const int VK_MENU = 0x12;
		#endregion

		#region Private fields
		// The delegate is stored in a field so that it is not reclaimed by GC while the hook is set
		private readonly LowLevelMouseProc _hookProc;
		private readonly Dictionary<string, Action> _bindings;
		private IntPtr _hookHandle;
		private SynchronizationContext _synchronizationContext;
		#endregion

		public MouseHookService()
		{
			this._hookProc = this.HookCallback;
			this._bindings = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase);
			this._hookHandle = IntPtr.Zero;
		}

		public void Register(string binding, Action action)
		{
			if (!MouseBinding.TryParse(binding, out Keys modifiers, out string button))
			{
				return;
			}

			this._bindings[MouseBinding.Compose(modifiers, button)] = action;

			// The UI thread context is captured on the first registration
			this._synchronizationContext = this._synchronizationContext ?? SynchronizationContext.Current;

			if (this._hookHandle == IntPtr.Zero)
			{
				this._hookHandle = MouseHookService.SetWindowsHookEx(MouseHookService.WH_MOUSE_LL, this._hookProc, MouseHookService.GetModuleHandle(null), 0);
			}
		}

		public void UnregisterAll()
		{
			this._bindings.Clear();

			if (this._hookHandle == IntPtr.Zero)
			{
				return;
			}

			MouseHookService.UnhookWindowsHookEx(this._hookHandle);
			this._hookHandle = IntPtr.Zero;
		}

		public void Dispose()
		{
			this.UnregisterAll();
		}

		private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode < 0)
			{
				return MouseHookService.CallNextHookEx(this._hookHandle, nCode, wParam, lParam);
			}

			string button = null;
			int message = wParam.ToInt32();

			switch (message)
			{
				case MouseHookService.WM_MOUSEWHEEL:
				{
					MSLLHOOKSTRUCT data = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
					short delta = (short)((data.MouseData >> 16) & 0xFFFF);
					button = delta > 0 ? MouseBinding.SCROLL_UP : MouseBinding.SCROLL_DOWN;
					break;
				}
				case MouseHookService.WM_MBUTTONDOWN:
					button = MouseBinding.MIDDLE_BUTTON;
					break;
				case MouseHookService.WM_XBUTTONDOWN:
				{
					MSLLHOOKSTRUCT data = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
					short xButton = (short)((data.MouseData >> 16) & 0xFFFF);
					button = xButton == 1 ? MouseBinding.X_BUTTON_1 : MouseBinding.X_BUTTON_2;
					break;
				}
			}

			if (button == null)
			{
				return MouseHookService.CallNextHookEx(this._hookHandle, nCode, wParam, lParam);
			}

			Keys modifiers = Keys.None;
			if (MouseHookService.GetKeyState(MouseHookService.VK_CONTROL) < 0)
			{
				modifiers |= Keys.Control;
			}
			if (MouseHookService.GetKeyState(MouseHookService.VK_SHIFT) < 0)
			{
				modifiers |= Keys.Shift;
			}
			if (MouseHookService.GetKeyState(MouseHookService.VK_MENU) < 0)
			{
				modifiers |= Keys.Alt;
			}

			if (!this._bindings.TryGetValue(MouseBinding.Compose(modifiers, button), out Action action))
			{
				return MouseHookService.CallNextHookEx(this._hookHandle, nCode, wParam, lParam);
			}

			// Run the bound action on the UI thread and swallow the mouse event
			// (same behavior as keyboard hotkeys registered via RegisterHotKey)
			if (this._synchronizationContext != null)
			{
				this._synchronizationContext.Post(_ => action(), null);
			}
			else
			{
				action();
			}

			return new IntPtr(1);
		}
	}
}
