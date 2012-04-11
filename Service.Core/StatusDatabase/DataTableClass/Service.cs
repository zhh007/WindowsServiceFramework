using Service.Core.Massive.SQLite;
using SC_Utility = Service.Core.Utility;

namespace Service.Core.StatusDatabase.DataTableClass
{
	public class Service : DynamicModel
	{
		public Service() : base(SC_Utility.Utility.Settings.Instance.ConnectionStringSettings, "Service", "ServiceId") { }
	}
}