using System.Collections.Generic;
using System.ServiceModel;
using SC_Wcf = Service.Core.WindowsService.WCF;

namespace Service.Core.WindowsService.WCF
{
	[ServiceContract]
	public interface IWcfContract
	{
		// TODO: Replace this method in the interface file and here with any method that is needed for operational function.
		[OperationContract]
		string ContractMethod();

		[OperationContract]
		List<SC_Wcf.Service> GetServices();

		[OperationContract]
		void SendServiceCommand(int serviceId, int serviceCommand);

		#region Windows Service control methods

		[OperationContract]
		bool RestartWindowsService();

		[OperationContract]
		bool StartWindowsService();

		[OperationContract]
		bool StopWindowsService();

		#endregion Windows Service control methods
	}
}