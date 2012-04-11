using System.ComponentModel;
using System.ServiceProcess;

namespace Service.Core.WindowsService.Service
{
	[RunInstaller(true)]
	public partial class ServiceInstaller : System.Configuration.Install.Installer
	{
		protected void InstallService(ServiceAttributes serviceAttributes)
		{
			if (serviceAttributes.ServiceAccount == ServiceAccount.User)
			{
				Installers.Add(new ServiceProcessInstaller
				{
					Account = ServiceAccount.User,
					Username = serviceAttributes.UserName,
					Password = serviceAttributes.Password,
				});
			}
			else
			{
				Installers.Add(new ServiceProcessInstaller { Account = serviceAttributes.ServiceAccount });
			}

			Installers.Add(new System.ServiceProcess.ServiceInstaller
			{
				ServiceName = serviceAttributes.ServiceName,
				Description = serviceAttributes.ServiceDescription,
				DisplayName = serviceAttributes.ServiceDisplayName,
				StartType = serviceAttributes.ServiceStartMode
			});
		}
	}
}