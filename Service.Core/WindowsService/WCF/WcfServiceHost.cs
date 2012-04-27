#region

// -----------------------------------------------------
// MIT License
// Copyright (C) 2012 John M. Baughman (jbaughmanphoto.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------

#endregion

using System;
using Service.Core.Log;
using Service.Core.WindowsService.Utility;
using ServiceModel = System.ServiceModel;
using ServiceModelChannels = System.ServiceModel.Channels;
using ServiceModelDescription = System.ServiceModel.Description;

namespace Service.Core.WindowsService.WCF {
	internal class WcfServiceHost<TService, TContract> {
		private ServiceModel.ServiceHost serviceHost = null;

		public string ServiceAddress { get; private set; }

		public string MexServiceAddress { get; private set; }

		public void Start() {
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
			if (metadataBehavior == null) {
				metadataBehavior = new ServiceModelDescription.ServiceMetadataBehavior();
				serviceHost.Description.Behaviors.Add(metadataBehavior);
			}
			serviceHost.AddServiceEndpoint(typeof(ServiceModelDescription.IMetadataExchange), binding, MexServiceAddress);

			serviceHost.Open();
			Logging.Log(LogLevelEnum.Info, "WCF service started");
		}

		public void Stop() {
			if (serviceHost != null) {
				Logging.Log(LogLevelEnum.Info, "Stopping WCF service");
				serviceHost.Close();
				serviceHost = null;
				Logging.Log(LogLevelEnum.Info, "WCF service stopped");
			}
		}
	}
}