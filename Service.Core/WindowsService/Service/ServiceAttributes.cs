using System.ServiceProcess;

namespace Service.Core.WindowsService.Service
{
	public class ServiceAttributes
	{
		public string ServiceName { get; set; }

		public string ServiceDescription { get; set; }

		public string ServiceDisplayName { get; set; }

		public ServiceStartMode ServiceStartMode { get; set; }

		public ServiceAccount ServiceAccount { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }
	}
}