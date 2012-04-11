using Service.Core.Massive.SQLite;
using SC_Utility = Service.Core.Utility;

namespace Service.Core.StatusDatabase.DataTableClass
{
	public class ServiceCommand : DynamicModel
	{
		public ServiceCommand() : base(SC_Utility.Utility.Settings.Instance.ConnectionStringSettings, "ServiceCommand") { }
	}
}