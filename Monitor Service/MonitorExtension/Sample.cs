using System.ComponentModel.Composition;
using Service.Core.ServiceExtensibility;

namespace SampleExtension
{
	[Export(typeof(IExtensibility))]
	public class Sample : IExtensibility
	{
		[Export("PerformAction")]
		public ActionResult PerformAction(ActionParameters actionParmeters)
		{
			string testValue = "TestValue";
			return new ActionResult { Success = true, Message = "Made it into the extension! TestValue = " + testValue };
		}
	}
}