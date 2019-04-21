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

namespace Microsoft.Azure.Commands.ApiManagement.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.Azure.Commands.ApiManagement.Helpers;

    public class PsApiManagementSslSettings
    {
        public PsApiManagementSslSettings()
        {
        }

        internal PsApiManagementSslSettings(IDictionary<string, string> customProperties)
            : this()
        {
            if (customProperties == null)
            {
                return;
            }

            FrontendProtocols = new Hashtable();
            BackendProtocols = new Hashtable();
            CipherSuites = new Hashtable();
            ServerProtocols = new Hashtable();

            foreach(KeyValuePair<string, string> sslProperty in customProperties)
            {
                if (sslProperty.Key.StartsWith(Constants.FrontendProtocolSettingPrefix))
                {
                    FrontendProtocols.Add(sslProperty.Key.Replace(Constants.FrontendProtocolSettingPrefix, ""), sslProperty.Value);
                }
                else if (sslProperty.Key.StartsWith(Constants.BackendProtocolSettingPrefix))
                {
                    BackendProtocols.Add(sslProperty.Key.Replace(Constants.BackendProtocolSettingPrefix, ""), sslProperty.Value);
                }
                else if (sslProperty.Key.StartsWith(Constants.CipherSettingPrefix))
                {
                    CipherSuites.Add(sslProperty.Key.Replace(Constants.CipherSettingPrefix, ""), sslProperty.Value);
                }
                else if (sslProperty.Key.StartsWith(Constants.ServerSettingPrefix))
                {
                    ServerProtocols.Add(sslProperty.Key.Replace(Constants.ServerSettingPrefix, ""), sslProperty.Value);
                }
            }
        }

        public Hashtable FrontendProtocols { get; set; }

        public Hashtable BackendProtocols { get; set; }

        public Hashtable CipherSuites { get; set; }

        public Hashtable ServerProtocols { get; set; }
    }
}
