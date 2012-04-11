using System.Collections.Generic;
using SC_Wcf = Service.Core.WindowsService.WCF;
using ServiceDb = Service.Core.StatusDatabase;

namespace MonitorService.WCF
{
	public static class ServiceCommandExtentionMethods
	{
		public static List<SC_Wcf.ServiceCommandStruct> Convert(this List<ServiceDb.ServiceCommandStruct> serviceCommands)
		{
			List<SC_Wcf.ServiceCommandStruct> returnList = new List<SC_Wcf.ServiceCommandStruct>();
			serviceCommands.ForEach(sc => returnList.Add(new SC_Wcf.ServiceCommandStruct
			{
				CommandId = sc.CommandId,
				CommandDescription = sc.CommandDescription
			}));
			return returnList;
		}
	}
}