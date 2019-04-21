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
    using System.Globalization;
    using System.Management.Automation;
    using Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Models;
    using Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Properties;

    [Cmdlet("Remove", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ApiManagementDiagnostic", SupportsShouldProcess = true)]
    [OutputType(typeof(bool))]
    public class RemoveAzureApiManagementDiagnostic : AzureApiManagementCmdletBase
    {
        private const string FindByDiagnosticId = "GetByDiagnosticId";
        private const string FindByApiDiagnosticId = "GetByApiDiagnosticId";

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Instance of PsApiManagementContext. This parameter is required.")]
        [ValidateNotNullOrEmpty]
        public PsApiManagementContext Context { get; set; }

        [Parameter(
            ParameterSetName = FindByApiDiagnosticId,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of the API. This parameter is required.")]
        [ValidateNotNullOrEmpty]
        public String ApiId { get; set; }

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
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "If specified will write true in case operation succeeds. This parameter is optional.")]
        public SwitchParameter PassThru { get; set; }

        public override void ExecuteApiManagementCmdlet()
        {
            if (ParameterSetName == FindByDiagnosticId)
            {
                var actionDescription = string.Format(CultureInfo.CurrentCulture, Resources.DiagnosticRemoveDescription, DiagnosticId);
                var actionWarning = string.Format(CultureInfo.CurrentCulture, Resources.DiagnosticRemoveWarning, DiagnosticId);

                // Do nothing if force is not specified and user cancelled the operation
                if (!ShouldProcess(
                        actionDescription,
                        actionWarning,
                        Resources.ShouldProcessCaption))
                {
                    return;
                }

                Client.DiagnosticRemoveTenantLevel(Context, DiagnosticId);
            }
            else if (ParameterSetName == FindByApiDiagnosticId)
            {
                var actionDescription = string.Format(CultureInfo.CurrentCulture, Resources.ApiDiagnosticRemoveDescription, DiagnosticId, ApiId);
                var actionWarning = string.Format(CultureInfo.CurrentCulture, Resources.ApiDiagnosticRemoveWarning, DiagnosticId, ApiId);

                // Do nothing if force is not specified and user cancelled the operation
                if (!ShouldProcess(
                        actionDescription,
                        actionWarning,
                        Resources.ShouldProcessCaption))
                {
                    return;
                }

                Client.DiagnosticRemoveApiLevel(Context, ApiId, DiagnosticId);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Parameter set name '{0}' is not supported.", ParameterSetName));
            }

            if (PassThru.IsPresent)
            {
                WriteObject(true);
            }
        }
    }
}
