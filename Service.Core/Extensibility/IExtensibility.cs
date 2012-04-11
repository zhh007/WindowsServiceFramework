using System.ComponentModel.Composition;

namespace Service.Core.ServiceExtensibility
{
	public interface IExtensibility
	{
		[Export("PerformAction")]
		ActionResult PerformAction(ActionParameters actionParameters);
	}
}