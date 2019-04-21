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
    using System;
    using System.Management.Automation;
    using Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Models;

    [Cmdlet("Get", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ApiManagementDiagnostic", DefaultParameterSetName = AllDiagnostics)]
    [OutputType(typeof(PsApiManagementDiagnostic))]
    public class GetAzureApiManagementDiagnostic : AzureApiManagementCmdletBase
    {
        private const string FindByDiagnosticId = "GetByDiagnosticId";
        private const string FindByApiDiagnosticId = "GetByApiDiagnosticId";
        private const string AllApiDiagnostics = "GetAllApiDiagnostics";
        private const string AllDiagnostics = "GetAllDiagnostics";

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Instance of PsApiManagementContext. This parameter is required.")]
        [ValidateNotNullOrEmpty]
        public PsApiManagementContext Context { get; set; }

        [Parameter(
            ParameterSetName = FindByDiagnosticId,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing product. If specified will return product-scope policy." +
                          " This parameters is optional.")]
        [Parameter(
            ParameterSetName = FindByApiDiagnosticId,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing product. If specified will return product-scope policy." +
                          " This parameters is optional.")]
        public String DiagnosticId { get; set; }

        [Parameter(
            ParameterSetName = FindByApiDiagnosticId,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing API. If specified will return API-scope policy. This parameters is required.")]
        [Parameter(
            ParameterSetName = AllApiDiagnostics,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing API. If specified will return API-scope policy. This parameters is required.")]
        public String ApiId { get; set; }

        public override void ExecuteApiManagementCmdlet()
        {
            if (ParameterSetName.Equals(AllDiagnostics))
            {
                WriteObject(Client.DiagnosticListTenantLevel(Context), true);
            }
            else if (ParameterSetName.Equals(AllApiDiagnostics))
            {
                WriteObject(Client.DiagnosticListApiLevel(Context, ApiId), true);
            }
            else if (ParameterSetName.Equals(FindByDiagnosticId))
            {
                WriteObject(Client.DiagnosticGetTenantLevel(Context, DiagnosticId));
            }
            else if (ParameterSetName.Equals(FindByApiDiagnosticId))
            {
                WriteObject(Client.DiagnosticGetApiLevel(Context, ApiId, DiagnosticId));
            }
            else
            {
                throw new InvalidOperationException(string.Format("Parameter set name '{0}' is not supported.", ParameterSetName));
            }
        }
    }
}
