#region

// -----------------------------------------------------
// MIT License
// Copyright (C) 2012 John M. Baughman (jbaughmanphoto.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------

#endregion

using System.IO;

namespace Service.Core.Utility.FileWatch {
	public class FileWatcher {
		/// <summary>
		///
		/// </summary>
		public enum NotifyFilters {
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

		public static FileSystemWatcher GetFileWatcher(FileWatcherParameters parameters) {
			FileSystemWatcher fileWatcher = new FileSystemWatcher();
			fileWatcher.Filter = parameters.Filter;
			fileWatcher.Path = parameters.Path;
			fileWatcher.NotifyFilter = parameters.NotifyFilter;
			return fileWatcher;
		}
	}
}