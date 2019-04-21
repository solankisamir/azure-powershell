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

namespace Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Models
{
    public class PsApiManagementDiagnostic : PsApiManagementArmResource
    {
        /// <summary>
        /// Gets or sets the DiagnosticId of the resource
        /// </summary>
        public string DiagnosticId { get; set; }

        /// <summary>
        /// Gets or sets specifies for what type of messages sampling settings
        /// should not apply. Possible values include: 'allErrors'
        /// </summary>
        public string AlwaysLog { get; set; }

        /// <summary>
        /// Gets or sets resource Id of a target logger.
        /// </summary>        
        public string LoggerId { get; set; }

        /// <summary>
        /// Gets or sets sampling settings for Diagnostic.
        /// </summary>
        public PsApiManagementSamplingSetting Sampling { get; set; }

        /// <summary>
        /// Gets or sets diagnostic settings for incoming/outgoing HTTP
        /// messages to the Gateway.
        /// </summary>
        public PsApiManagementPipelineDiagnosticSetting Frontend { get; set; }

        /// <summary>
        /// Gets or sets diagnostic settings for incoming/outgoing HTTP
        /// messages to the Backend
        /// </summary>
        public PsApiManagementPipelineDiagnosticSetting Backend { get; set; }
    }
}
