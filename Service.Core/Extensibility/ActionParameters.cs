using System.IO;

namespace Service.Core.ServiceExtensibility
{
	public class ActionParameters
	{
		public enum ActionTypes
		{
			Manual,
			FileWatcher,
			Schedule,
			Timer
		}

		public ActionTypes ActionType { get; set; }

		public WatcherChangeTypes ChangeType { get; set; }

		public string FullPath { get; set; }

		public string Name { get; set; }

		public string OldFullPath { get; set; }

		public string OldName { get; set; }
	}
}