using System;
using System.Drawing;
using System.Windows.Forms;
using EveOPreview.Services;
using EveOPreview.Services.Interop;

namespace EveOPreview.View
{
	/// <summary>
	/// Fullscreen dimming overlay that visualizes the snap grid. The grid originates
	/// at the (0, 0) point of the screen coordinates - exactly the origin the snap
	/// logic rounds the preview positions to. The window is click-through and never
	/// activated. It is topmost (so it dims the game windows even when they are
	/// activated), but sits at the BOTTOM of the topmost band - previews and the
	/// settings window stay above it
	/// </summary>
	sealed class GridOverlayForm : Form
	{
		private int _stepX = 100;
		private int _stepY = 100;
		private int _offsetX;
		private int _offsetY;

		public GridOverlayForm()
		{
			this.FormBorderStyle = FormBorderStyle.None;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.Manual;
			this.Bounds = SystemInformation.VirtualScreen;
			this.BackColor = Color.Black;
			this.Opacity = 0.45;
			this.DoubleBuffered = true;
			this.TopMost = true;
		}

		/// <summary>
		/// Pushes this window below every other topmost window: activating a client must
		/// not cover the grid (clients are not topmost), while the previews and the
		/// settings window (topmost) must stay above it
		/// </summary>
		public void SinkBelowOtherTopmostWindows()
		{
			for (int i = 0; i < 64; i++)
			{
				IntPtr next = User32NativeMethods.GetWindow(this.Handle, User32NativeMethods.GW_HWNDNEXT);

				if (next == IntPtr.Zero)
				{
					return;
				}

				uint exStyle = User32NativeMethods.GetWindowLong(next, InteropConstants.GWL_EXSTYLE);
				if ((exStyle & InteropConstants.WS_EX_TOPMOST) == 0)
				{
					// The next window belongs to the normal band - the bottom of the
					// topmost band is reached
					return;
				}

				User32NativeMethods.SetWindowPos(this.Handle, next, 0, 0, 0, 0,
					User32NativeMethods.SWP_NOMOVE | User32NativeMethods.SWP_NOSIZE | User32NativeMethods.SWP_NOACTIVATE);
			}
		}

		protected override bool ShowWithoutActivation => true;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= (int)(InteropConstants.WS_EX_TOOLWINDOW | InteropConstants.WS_EX_TRANSPARENT | InteropConstants.WS_EX_NOACTIVATE);
				return createParams;
			}
		}

		public void SetGridStep(int stepX, int stepY, int offsetX, int offsetY)
		{
			this._stepX = Math.Max(4, stepX);
			this._stepY = Math.Max(4, stepY);
			this._offsetX = offsetX;
			this._offsetY = offsetY;
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Rectangle screen = SystemInformation.VirtualScreen;

			using (Pen pen = new Pen(Color.FromArgb(160, Color.White)))
			{
				// Grid lines sit at (offset + k * step) in screen coordinates
				int firstX = this._offsetX + (int)(Math.Ceiling((double)(screen.Left - this._offsetX) / this._stepX) * this._stepX);
				for (int x = firstX; x <= screen.Right; x += this._stepX)
				{
					e.Graphics.DrawLine(pen, x - screen.Left, 0, x - screen.Left, this.Height);
				}

				int firstY = this._offsetY + (int)(Math.Ceiling((double)(screen.Top - this._offsetY) / this._stepY) * this._stepY);
				for (int y = firstY; y <= screen.Bottom; y += this._stepY)
				{
					e.Graphics.DrawLine(pen, 0, y - screen.Top, this.Width, y - screen.Top);
				}
			}
		}
	}
}
