using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Service.Core.ExceptionHandler.Utility
{
	public class Utilities
	{
		#region Collection IsNullOrEmpty method.

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool IsNullOrEmpty(ICollection obj)
		{
			return (obj == null || obj.Count == 0);
		}

		#endregion Collection IsNullOrEmpty method.

		public static IPAddress GetIPv4Address()
		{
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

			foreach (IPAddress address in host.AddressList)
			{
				if (address.AddressFamily.ToString() == "InterNetwork")
					return address;
			}

			return null;
		}

		public static DateTime AssemblyBuildDate(Assembly assembly, bool forceFileDate = false)
		{
			Version version = assembly.GetName().Version;
			DateTime build;

			if (forceFileDate)
			{
				return AssemblyFileTime(assembly);
			}
			else
			{
				build = DateTime.Parse("01/01/2000").AddDays(version.Build).AddSeconds(version.Revision * 2);
				if (TimeZone.IsDaylightSavingTime(DateTime.Now, TimeZone.CurrentTimeZone.GetDaylightChanges(DateTime.Now.Year)))
				{
					build = build.AddHours(1);
				}

				if (build > DateTime.Now || version.Build < 730 || version.Revision == 0)
				{
					build = AssemblyFileTime(assembly);
				}
			}

			return build;
		}

		private static DateTime AssemblyFileTime(Assembly assembly)
		{
			try
			{
				return File.GetLastWriteTime(assembly.Location);
			}
			catch
			{
				return DateTime.MaxValue;
			}
		}
	}
}