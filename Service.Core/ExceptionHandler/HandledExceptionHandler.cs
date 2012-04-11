namespace Service.Core.ExceptionHandler
{
	public class HandledExceptionHandler : ExceptionHandlerBase<HandledExceptionHandler>
	{
		private static HandledExceptionHandler instance = null;

		private HandledExceptionHandler() { }

		public static HandledExceptionHandler Instance()
		{
			instance = new HandledExceptionHandler();
			return instance;
		}
	}
}