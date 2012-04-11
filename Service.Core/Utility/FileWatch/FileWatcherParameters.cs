using System.IO;

namespace Service.Core.Utility.FileWatch
{
	public class FileWatcherParameters
	{
		public string Filter { get; set; }

		public string Path { get; set; }

		public NotifyFilters NotifyFilter { get; set; }
	}
}