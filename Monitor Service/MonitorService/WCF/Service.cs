using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ServiceUtility;

namespace SelfInstallingWindowsService.WCF
{
	[DataContract]
	public struct ServiceCommandStruct
	{
		public int CommandId;
		public string CommandDescription;
	}
	
	[DataContract(Name="ServiceStatus")]
	public enum ServiceStatusEnum
	{
		Nothing = 0,
		[EnumMember]
		Stopped = 1,
		[EnumMember]
		StartPending = 2,
		[EnumMember]
		StopPending = 3,
		[EnumMember]
		Running = 4,
		[EnumMember]
		ContinuePending = 5,
		[EnumMember]
		PausePending = 6,
		[EnumMember]
		Paused = 7,
		[EnumMember]
		PerformingAction = 10
	}

	[DataContract]
	public class Service
	{
		[DataMember]
		public int ServiceId { get; set; }
		[DataMember]
		public ServiceStatusEnum ServiceStatus { get; set; }
		[DataMember]
		public string ServiceName { get; set; }
		[DataMember]
		public string ServiceDescription { get; set; }
		[DataMember]
		public string ServiceDisplayName { get; set; }
		[DataMember]
		public string InstallPath { get; set; }
		[DataMember]
		public List<ServiceCommandStruct> ServiceCommands { get; set; }
	}
}
