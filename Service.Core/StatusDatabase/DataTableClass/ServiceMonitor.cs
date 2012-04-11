using Service.Core.Massive.SQLite;
using SC_Utility = Service.Core.Utility;

namespace Service.Core.StatusDatabase.DataTableClass
{
	public class ServiceMonitor : DynamicModel
	{
		public ServiceMonitor() : base(SC_Utility.Utility.Settings.Instance.ConnectionStringSettings, "ServiceMonitor") { }
	}
}