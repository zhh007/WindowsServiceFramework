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

using System;
using System.IO;
using System.Reflection;

namespace MonitorServiceAdmin {
	internal static class ProgramStarter {
		internal const string DependencyPrefix = "MonitorServiceAdmin.Assemblies.";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main() {
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolver);
			Program.Run();
		}

		private static Assembly AssemblyResolver(object sender, ResolveEventArgs args) {
			string[] name = args.Name.Split(',');
			Assembly tempAssembly = Assembly.GetExecutingAssembly();
			using (Stream strm = tempAssembly.GetManifestResourceStream(DependencyPrefix + (!DependencyPrefix.LastIndexOf(".").Equals(DependencyPrefix.Length - 1) ? "." : string.Empty) + name[0] + ".dll")) {
				byte[] block = new byte[strm.Length];
				strm.Read(block, 0, block.Length);
				Assembly tempAssem2 = Assembly.Load(block);
				return tempAssem2;
			}
		}
	}
}