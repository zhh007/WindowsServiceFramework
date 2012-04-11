using System.Collections.Generic;

namespace Service.Core.Log
{
	interface ILogger
	{
		List<LogRecord> Parse(LogLevelEnum filter);

		List<LogRecord> Parse();
	}
}