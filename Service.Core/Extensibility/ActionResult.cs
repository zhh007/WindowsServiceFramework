using System;

namespace Service.Core.ServiceExtensibility
{
	public class ActionResult
	{
		public bool Success { get; set; }

		public string Message { get; set; }

		public Exception Exception { get; set; }
	}
}