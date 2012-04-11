using System;
using MonitorService.Utility;
using Service.Core.Log;
using ServiceModel = System.ServiceModel;
using ServiceModelChannels = System.ServiceModel.Channels;
using ServiceModelDescription = System.ServiceModel.Description;

namespace MonitorService.WCF
{
	internal class WcfServiceHost<TService, TContract>
	{
		private ServiceModel.ServiceHost serviceHost = null;

		public string ServiceAddress { get; private set; }

		public string MexServiceAddress { get; private set; }

		public void Start()
		{
			Logging.Log(LogLevelEnum.Info, "Starting WCF service");
			ServiceAddress = string.Format("net.tcp://{0}:{1}/", Utilities.GetIPv4Address(Settings.Instance.UseLoopback).ToString(), Settings.Instance.WcfPort);
			MexServiceAddress = string.Format("net.tcp://{0}:{1}/mex/", Utilities.GetIPv4Address().ToString(), Settings.Instance.WcfMexPort);
			Logging.Log(LogLevelEnum.Debug, string.Format("Service host address: {0}", ServiceAddress));
			Logging.Log(LogLevelEnum.Debug, string.Format("MEX Service host address: {0}", MexServiceAddress));
			serviceHost = new ServiceModel.ServiceHost(typeof(TService), new Uri(ServiceAddress));
			serviceHost.AddServiceEndpoint(typeof(TContract), new ServiceModel.NetTcpBinding(ServiceModel.SecurityMode.None), "");

			// Add TCP MEX endpoint
			ServiceModelChannels.BindingElement bindingElement = new ServiceModelChannels.TcpTransportBindingElement();
			ServiceModelChannels.CustomBinding binding = new ServiceModelChannels.CustomBinding(bindingElement);
			ServiceModelDescription.ServiceMetadataBehavior metadataBehavior = serviceHost.Description.Behaviors.Find<ServiceModelDescription.ServiceMetadataBehavior>();
			if (metadataBehavior == null)
			{
				metadataBehavior = new ServiceModelDescription.ServiceMetadataBehavior();
				serviceHost.Description.Behaviors.Add(metadataBehavior);
			}
			serviceHost.AddServiceEndpoint(typeof(ServiceModelDescription.IMetadataExchange), binding, MexServiceAddress);

			serviceHost.Open();
			Logging.Log(LogLevelEnum.Info, "WCF service started");
		}

		public void Stop()
		{
			if (serviceHost != null)
			{
				Logging.Log(LogLevelEnum.Info, "Stopping WCF service");
				serviceHost.Close();
				serviceHost = null;
				Logging.Log(LogLevelEnum.Info, "WCF service stopped");
			}
		}
	}
}