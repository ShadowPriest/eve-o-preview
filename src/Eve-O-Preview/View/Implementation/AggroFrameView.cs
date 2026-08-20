using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using EveOPreview.Services;
using EveOPreview.Services.Interop;

namespace EveOPreview.View
{
	/// <summary>
	/// The flashing 'aggro' frame shown over a thumbnail: a gradient fill running
	/// from the window edges towards the center, from the alert color to fully
	/// transparent. Implemented as a separate per-pixel-alpha layered window -
	/// the regular text overlay is a color-key window and physically cannot
	/// display translucent gradients.
	/// The window is click-through and never takes activation, so it does not
	/// interfere with the thumbnail mouse handling in any way
	/// </summary>
	sealed class AggroFrameView : Form
	{
		#region Private constants
		// Half-period of the flashing; the phase is derived from the system clock,
		// so the frames of every thumbnail blink in sync
		private const int BLINK_HALF_PERIOD_MS = 400;
		#endregion

		#region Private fields
		private readonly Timer _blinkTimer;

		private AggroLevel _level;
		private Color _renderedColor;
		private int _renderedFillPercent;
		private Size _renderedSize;
		private Point _location;
		private bool _isWindowShown;
		#endregion

		public AggroFrameView()
		{
			this.FormBorderStyle = FormBorderStyle.None;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.Manual;
			this.TopMost = true;

			this._level = AggroLevel.None;

			// The timer only drives the blink phase; it runs while a frame is displayed
			this._blinkTimer = new Timer { Interval = 100 };
			this._blinkTimer.Tick += (sender, e) => this.ApplyBlinkPhase();
		}

		// The frame must never steal the focus from the game clients
		protected override bool ShowWithoutActivation => true;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= (int)(InteropConstants.WS_EX_LAYERED
											| InteropConstants.WS_EX_TRANSPARENT
											| InteropConstants.WS_EX_TOOLWINDOW
											| InteropConstants.WS_EX_NOACTIVATE);
				return createParams;
			}
		}

		public void SetState(AggroLevel level, Color color, int fillPercent, Rectangle bounds)
		{
			if (level == AggroLevel.None)
			{
				this.Clear();
				return;
			}

			bool renderRequired = (this._level == AggroLevel.None)
								|| (this._renderedColor != color)
								|| (this._renderedFillPercent != fillPercent)
								|| (this._renderedSize != bounds.Size);

			this._level = level;

			if (renderRequired)
			{
				this.RenderLayer(bounds, color, fillPercent);

				this._renderedColor = color;
				this._renderedFillPercent = fillPercent;
				this._renderedSize = bounds.Size;
				this._location = bounds.Location;
			}
			else if (this._location != bounds.Location)
			{
				// A pure move does not need a repaint - the layered surface travels with the window
				User32NativeMethods.SetWindowPos(this.Handle, IntPtr.Zero, bounds.X, bounds.Y, 0, 0,
					User32NativeMethods.SWP_NOSIZE | User32NativeMethods.SWP_NOZORDER | User32NativeMethods.SWP_NOACTIVATE);
				this._location = bounds.Location;
			}

			if (!this._blinkTimer.Enabled)
			{
				this._blinkTimer.Start();
			}

			this.ApplyBlinkPhase();
		}

		public void Clear()
		{
			this._level = AggroLevel.None;
			this._blinkTimer.Stop();
			this.SetWindowShown(false);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._blinkTimer.Dispose();
			}

			base.Dispose(disposing);
		}

		private void ApplyBlinkPhase()
		{
			if (this._level == AggroLevel.None)
			{
				return;
			}

			bool isOnPhase = (Environment.TickCount64 / AggroFrameView.BLINK_HALF_PERIOD_MS) % 2 == 0;
			this.SetWindowShown(isOnPhase);
		}

		private void SetWindowShown(bool show)
		{
			if (this._isWindowShown == show)
			{
				return;
			}

			this._isWindowShown = show;

			// ShowWindow instead of the WinForms Show()/Hide(): the form must never
			// go through the regular activation/visibility pipeline
			User32NativeMethods.ShowWindow(this.Handle, show ? InteropConstants.SW_SHOWNOACTIVATE : 0 /* SW_HIDE */);
		}

		/// <summary>
		/// Renders the gradient into an offscreen ARGB bitmap and pushes it to the
		/// window via UpdateLayeredWindow. The frame is four edge-to-center gradient
		/// trapezoids meeting on the corner diagonals, so the bands never overlap
		/// and the corners blend cleanly
		/// </summary>
		private void RenderLayer(Rectangle bounds, Color color, int fillPercent)
		{
			int width = Math.Max(bounds.Width, 1);
			int height = Math.Max(bounds.Height, 1);

			using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					AggroFrameView.DrawGradientFrame(graphics, width, height, color, fillPercent);
				}

				IntPtr screenDc = User32NativeMethods.GetDC(IntPtr.Zero);
				IntPtr memoryDc = Gdi32NativeMethods.CreateCompatibleDC(screenDc);
				IntPtr hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
				IntPtr previousBitmap = Gdi32NativeMethods.SelectObject(memoryDc, hBitmap);

				try
				{
					User32NativeMethods.POINT destination = new User32NativeMethods.POINT(bounds.X, bounds.Y);
					User32NativeMethods.SIZE size = new User32NativeMethods.SIZE(width, height);
					User32NativeMethods.POINT source = new User32NativeMethods.POINT(0, 0);
					User32NativeMethods.BLENDFUNCTION blend = new User32NativeMethods.BLENDFUNCTION
					{
						BlendOp = User32NativeMethods.AC_SRC_OVER,
						BlendFlags = 0,
						SourceConstantAlpha = 255,
						AlphaFormat = User32NativeMethods.AC_SRC_ALPHA
					};

					User32NativeMethods.UpdateLayeredWindow(this.Handle, screenDc, ref destination, ref size, memoryDc, ref source, 0, ref blend, User32NativeMethods.ULW_ALPHA);
				}
				finally
				{
					Gdi32NativeMethods.SelectObject(memoryDc, previousBitmap);
					Gdi32NativeMethods.DeleteObject(hBitmap);
					Gdi32NativeMethods.DeleteDC(memoryDc);
					User32NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
				}
			}
		}

		private static void DrawGradientFrame(Graphics graphics, int width, int height, Color color, int fillPercent)
		{
			fillPercent = Math.Min(Math.Max(fillPercent, 1), 100);

			// The gradient depth: 100% reaches the window center (a full fill),
			// small values leave just a thin glowing border
			int depth = Math.Max(1, (int)Math.Round(Math.Min(width, height) / 2.0 * fillPercent / 100.0));

			Color edgeColor = Color.FromArgb(255, color.R, color.G, color.B);
			Color innerColor = Color.FromArgb(0, color.R, color.G, color.B);

			// (band polygon, gradient start point, gradient end point)
			(Point[] Polygon, Point From, Point To)[] bands =
			{
				// Top
				(new[] { new Point(0, 0), new Point(width, 0), new Point(width - depth, depth), new Point(depth, depth) },
					new Point(0, 0), new Point(0, depth)),
				// Bottom
				(new[] { new Point(0, height), new Point(width, height), new Point(width - depth, height - depth), new Point(depth, height - depth) },
					new Point(0, height), new Point(0, height - depth)),
				// Left
				(new[] { new Point(0, 0), new Point(depth, depth), new Point(depth, height - depth), new Point(0, height) },
					new Point(0, 0), new Point(depth, 0)),
				// Right
				(new[] { new Point(width, 0), new Point(width, height), new Point(width - depth, height - depth), new Point(width - depth, depth) },
					new Point(width, 0), new Point(width - depth, 0))
			};

			foreach ((Point[] polygon, Point from, Point to) in bands)
			{
				using (LinearGradientBrush brush = new LinearGradientBrush(from, to, edgeColor, innerColor))
				{
					// Kills the one-pixel seam GDI+ paints at the gradient start line
					brush.WrapMode = WrapMode.TileFlipXY;
					graphics.FillPolygon(brush, polygon);
				}
			}
		}
	}
}
