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

namespace Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Commands
{
    using Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Models;
    using System.Management.Automation;

    [Cmdlet("New", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ApiManagementSamplingSetting")]
    [OutputType(typeof(PsApiManagementSamplingSetting))]
    public class NewAzureApiManagementSamplingSetting : AzureApiManagementCmdletBase
    {
        [Parameter(
           ValueFromPipelineByPropertyName = false,
           Mandatory = false,
           HelpMessage = "The Type of Sampling. This parameter is optional.")]
        [ValidateSet("fixed")]
        public string SamplingType { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = false,
            Mandatory = false,
            HelpMessage = "Rate of Sampling for Fixed Rate Sampling. This parameter is optional.")]        
        public double? Percentage { get; set; }

        public override void ExecuteApiManagementCmdlet()
        {
            WriteObject(
                new PsApiManagementSamplingSetting
                {
                    SamplingType = SamplingType,
                    Percentage = Percentage
                });
        }
    }
}
