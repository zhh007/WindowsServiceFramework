using System;
using System.Collections.Generic;
using System.IO;
using Service.Core.Utility.Utility;

namespace Service.Core.Utility.FileWatch
{
	public class FileWatchers
	{
		private static readonly Lazy<FileWatchers> instance = new Lazy<FileWatchers>(() => new FileWatchers());

		public Dictionary<string, FileSystemWatcher> Watchers { get; set; }

		private FileWatchers() { }

		public static FileWatchers Instance
		{
			get
			{
				if (instance.Value.Watchers == null)
				{
					instance.Value.Watchers = new Dictionary<string, FileSystemWatcher>();
				}

				Settings.Instance.FileWatcherSystemIds.ForEach(
					p =>
					{
						instance.Value.Watchers.Add(p, FileWatcher.GetFileWatcher(new FileWatcherParameters
																					  {
																						  Filter = Settings.Instance.FileWatcherFilter[p],
																						  NotifyFilter = Settings.Instance.FileWatcherNotifyFilter,
																						  Path = Settings.Instance.FileWatcherPath[p]
																					  }));
					});

				return instance.Value;
			}
		}

		~FileWatchers()
		{
			foreach (KeyValuePair<string, FileSystemWatcher> fileWatcher in Watchers)
			{
				fileWatcher.Value.Dispose();
			}
		}
	}
}