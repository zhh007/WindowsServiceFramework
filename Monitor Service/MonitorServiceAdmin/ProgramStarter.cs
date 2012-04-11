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