using System.IO;

namespace Service.Core.Utility.FileWatch
{
	public class FileWatcherEventAttributes
	{
		public WatcherChangeTypes ChangeType { get; set; }

		public string FullPath { get; set; }

		public string Name { get; set; }

		public string OldFullPath { get; set; }

		public string OldName { get; set; }
	}
}