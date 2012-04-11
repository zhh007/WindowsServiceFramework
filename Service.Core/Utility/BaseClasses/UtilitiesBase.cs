using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using Service.Core.Log;
using Service.Core.WindowsService.Utility;

namespace Service.Core.Utility.BaseClasses {

	public abstract class UtilitiesBase {

		public static IPAddress GetIPv4Address(bool returnLoopback) {
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

			foreach (IPAddress address in host.AddressList) {
				if (returnLoopback && IPAddress.IsLoopback(address))
					return address;

				// It's gotta be IPv4, not an aoutconfigured IP, and not the loopback.
				if (address.AddressFamily == AddressFamily.InterNetwork && !address.ToString().Contains("169.254") && !IPAddress.IsLoopback(address))
					return address;
			}

			return null;
		}

		public static IPAddress GetIPv4Address() {
			return GetIPv4Address(false);
		}

		/// <summary>
		/// Creates the directory.
		/// </summary>
		/// <param name="directoryName">Name of the directory.</param>
		/// <returns>The directory created, even if it wasn't actually created.</returns>
		public static string CreateDirectory(string directoryName) {
			Logging.Log(LogLevelEnum.Debug, "directoryName: " + directoryName);
			if (directoryName.StartsWith(".")) {
				directoryName = directoryName.Substring(1);
				Logging.Log(LogLevelEnum.Debug, "directoryName: " + directoryName);
			}
			else if (!directoryName.StartsWith(Path.DirectorySeparatorChar.ToString()) && !directoryName.Contains(":")) {
				directoryName = String.Concat(Path.DirectorySeparatorChar, directoryName);
				Logging.Log(LogLevelEnum.Debug, "directoryName: " + directoryName);
			}

			if (!directoryName.Contains(":")) {
				directoryName = string.Concat(AssemblyDirectory, directoryName);
				Logging.Log(LogLevelEnum.Debug, "directoryName: " + directoryName);
			}

			Logging.Log(LogLevelEnum.Debug, "Directory.Exists(directoryName): " + Directory.Exists(directoryName).ToString());
			if (!Directory.Exists(directoryName)) {
				Directory.CreateDirectory(directoryName);
			}

			return directoryName;
		}

		#region Collection IsNullOrEmpty method.

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool IsNullOrEmpty(ICollection obj) {
			return (obj == null || obj.Count == 0);
		}

		#endregion Collection IsNullOrEmpty method.

		public static string AssemblyDirectory {
			get {
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		public static XmlDocument LoadXmlDocument(string file) {
			FileStream fs = new FileStream(Path.Combine(AssemblyDirectory, file), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fs);
			return xmlDoc;
		}

		public static void SaveXmlDocument(XmlDocument xmlDoc, string file) {
			xmlDoc.Save(Path.Combine(AssemblyDirectory, file));
		}

		#region Various utility methods

		//
		/// <summary>
		/// Gets the Julian date.
		/// See this for details on this method: http://stackoverflow.com/questions/5248827/convert-datetime-to-julian-date-in-c-sharp-tooadate-safe
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns></returns>
		public static double ToJulianDate(DateTime date) {
			return date.ToOADate() + 2415018.5;
		}

		public static bool IsNumeric(string checkString) {
			try {
				int a = int.Parse(checkString);
				return true;
			}
			catch {
				return false;
			}
		}

		#endregion Various utility methods
	}
}