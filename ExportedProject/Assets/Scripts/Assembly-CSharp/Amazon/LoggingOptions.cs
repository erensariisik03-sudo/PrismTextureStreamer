using System;

namespace Amazon
{
	[Flags]
	public enum LoggingOptions
	{
		None = 0,
		Log4Net = 1,
		SystemDiagnostics = 2,
		UnityLogger = Log4Net | SystemDiagnostics
	}
}
