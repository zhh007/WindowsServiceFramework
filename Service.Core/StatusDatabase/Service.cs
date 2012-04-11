using System.Collections.Generic;

namespace Service.Core.StatusDatabase
{
	public struct ServiceCommandStruct
	{
		public int CommandId;
		public string CommandDescription;
	}

	public class Service
	{
		public enum ServiceStatusEnum
		{
			Stopped = 1,
			StartPending = 2,
			StopPending = 3,
			Running = 4,
			ContinuePending = 5,
			PausePending = 6,
			Paused = 7,
			PerformingAction = 10
		}

		public int ServiceId { get; set; }

		public ServiceStatusEnum ServiceStatus { get; set; }

		public string ServiceName { get; set; }

		public string ServiceDescription { get; set; }

		public string ServiceDisplayName { get; set; }

		public int LocationId { get; set; }

		public int SystemId { get; set; }

		public int ApplicationId { get; set; }

		public string InstallPath { get; set; }

		public List<ServiceCommandStruct> ServiceCommands { get; set; }

		public string ServiceStatusDatabasePath { get; set; }

		public bool IsHub { get; set; }
	}
}