using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EveOPreview
{
	// A really very primitive exception handler stuff here
	// No IoC, no fancy DI containers - just a plain exception stacktrace dump
	// If this code is called then something was gone really bad
	// so even the DI infrastructure might be dead already.
	// So this dumb and non elegant approach is used
	sealed class ExceptionHandler
	{
		private const string EXCEPTION_DUMP_FILE_NAME = "EVE-O-Preview.log";
		private const string EXCEPTION_MESSAGE = "EVE-O-Preview has encountered a problem and needs to close. Additional information has been saved in the crash log file.";

		public void SetupExceptionHandlers()
		{
			if (System.Diagnostics.Debugger.IsAttached)
			{
				return;
			}

			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += delegate (Object sender, ThreadExceptionEventArgs e)
			{
				this.ExceptionEventHandler(e.Exception);
			};

			AppDomain.CurrentDomain.UnhandledException += delegate (Object sender, UnhandledExceptionEventArgs e)
			{
				this.ExceptionEventHandler(e.ExceptionObject as Exception);
			};
		}

		// The resource lookup is the one part of this handler that can fail on its own
		// (a missing satellite assembly, a broken resource stream), so the English text
		// stays available as a fallback
		private static string GetExceptionMessage()
		{
			try
			{
				return Localization.Strings.Crash_Message;
			}
			catch (Exception)
			{
				return ExceptionHandler.EXCEPTION_MESSAGE;
			}
		}

		private void ExceptionEventHandler(Exception exception)
		{
			try
			{
				String exceptionMessage = exception.ToString();
				File.WriteAllText(ExceptionHandler.EXCEPTION_DUMP_FILE_NAME, exceptionMessage);

				MessageBox.Show(ExceptionHandler.GetExceptionMessage(), @"EVE-O-Preview", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch
			{
				// We are in unstable state now so even this operation might fail
				// Still we actually don't care anymore - anyway the application has been cashed
			}

			System.Environment.Exit(1);
		}
	}
}