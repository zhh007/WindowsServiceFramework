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

using System.Collections.Generic;
using SC_Wcf = Service.Core.WindowsService.WCF;
using ServiceDb = Service.Core.StatusDatabase;

namespace MonitorService.WCF {
	public static class ServiceCommandExtentionMethods {

		public static List<SC_Wcf.ServiceCommandStruct> Convert(this List<ServiceDb.ServiceCommandStruct> serviceCommands) {
			List<SC_Wcf.ServiceCommandStruct> returnList = new List<SC_Wcf.ServiceCommandStruct>();
			serviceCommands.ForEach(sc => returnList.Add(new SC_Wcf.ServiceCommandStruct {
				CommandId = sc.CommandId,
				CommandDescription = sc.CommandDescription
			}));
			return returnList;
		}
	}
}