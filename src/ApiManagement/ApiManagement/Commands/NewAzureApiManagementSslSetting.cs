//  
// Copyright (c) Microsoft.  All rights reserved.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

namespace Microsoft.Azure.Commands.ApiManagement.Commands
{
    using System.Collections;
    using System.Management.Automation;
    using Microsoft.Azure.Commands.ApiManagement.Models;
    using ResourceManager.Common;

    [Cmdlet("New", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ApiManagementSslSetting")]
    [OutputType(typeof(PsApiManagementSslSettings))]
    public class NewAzureApiManagementSslSetting : AzureRMCmdlet
    {
        [Parameter(
          HelpMessage = "Frontend Security protocols settings. This parameter is optional.")]
        public Hashtable FrontendProtocols { get; set; }

        [Parameter(
          HelpMessage = "Backend Security protocol settings. This parameter is optional.")]
        public Hashtable BackendProtocols { get; set; }

        [Parameter(
               HelpMessage = "Ssl cipher suites settings in the specified order. This parameter is optional.")]
        public Hashtable CipherSuites { get; set; }

        [Parameter(
               HelpMessage = "Server protocol settings like Http2. This parameter is optional.")]
        public Hashtable ServerProtocols { get; set; }

        public override void ExecuteCmdlet()
        {
            var sslSettings = new PsApiManagementSslSettings();

            if (FrontendProtocols != null && FrontendProtocols.Count > 0)
            {
                sslSettings.FrontendProtocols = FrontendProtocols;
            }

            if (BackendProtocols != null && BackendProtocols.Count > 0)
            { 
                sslSettings.BackendProtocols = BackendProtocols;
            }

            if (CipherSuites != null && CipherSuites.Count > 0)
            {
                sslSettings.CipherSuites = CipherSuites;
            }

            if (ServerProtocols != null && ServerProtocols.Count > 0)
            {
                sslSettings.ServerProtocols = ServerProtocols;
            }

            WriteObject(sslSettings);
        }
    }
}
