using System.IO;

namespace Service.Core.Utility.FileWatch
{
	public class FileWatcher
	{
		/// <summary>
		///
		/// </summary>
		public enum NotifyFilters
		{
			/// <summary>
			/// The attributes of the file or folder.
			/// </summary>
			Attributes = System.IO.NotifyFilters.Attributes,
			/// <summary>
			/// The time the file or folder was created.
			/// </summary>
			CreationTime = System.IO.NotifyFilters.CreationTime,
			/// <summary>
			/// The name of the directory.
			/// </summary>
			DirectoryName = System.IO.NotifyFilters.DirectoryName,
			/// <summary>
			/// The name of the file.
			/// </summary>
			FileName = System.IO.NotifyFilters.FileName,
			/// <summary>
			/// The date the file or folder was last opened.
			/// </summary>
			LastAccess = System.IO.NotifyFilters.LastAccess,
			/// <summary>
			/// The date the file or folder last had anything written to it.
			/// </summary>
			LastWrite = System.IO.NotifyFilters.LastWrite,
			/// <summary>
			/// The security settings of the file or folder.
			/// </summary>
			Security = System.IO.NotifyFilters.Security,
			/// <summary>
			/// The size of the file or folder.
			/// </summary>
			Size = System.IO.NotifyFilters.Size,
			/// <summary>
			/// Overriden value added for parsing of NotifyFilters in config file.
			/// Used to initialize the internal FileSystemWatcher variable.
			/// </summary>
			None = 0
		}

		private static NotifyFilters notifyFilter;

		public static FileSystemWatcher GetFileWatcher(FileWatcherParameters parameters)
		{
			FileSystemWatcher fileWatcher = new FileSystemWatcher();
			fileWatcher.Filter = parameters.Filter;
			fileWatcher.Path = parameters.Path;
			fileWatcher.NotifyFilter = parameters.NotifyFilter;
			return fileWatcher;
		}
	}
}