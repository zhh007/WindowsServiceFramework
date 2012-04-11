using MonitorService.Utility;
using SC_Svc = Service.Core.WindowsService.Service;

namespace MonitorService.Service
{
	public partial class ServiceInstaller : SC_Svc.ServiceInstaller
	{
		public ServiceInstaller()
		{
			base.InstallService(new SC_Svc.ServiceAttributes
									{
										ServiceAccount = System.ServiceProcess.ServiceAccount.LocalService,
										ServiceName = Settings.Instance.ServiceName,
										ServiceDescription = Settings.Instance.ServiceDisplayName,
										ServiceDisplayName = Settings.Instance.ServiceDisplayName,
										ServiceStartMode = System.ServiceProcess.ServiceStartMode.Manual
									});
		}
	}
}