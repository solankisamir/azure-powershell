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
    using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
    using Models;

    [Cmdlet("New", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ApiManagementDiagnostic", DefaultParameterSetName = TenantLevel, SupportsShouldProcess = true)]
    [OutputType(typeof(PsApiManagementDiagnostic))]
    public class NewAzureApiManagementDiagnostic : AzureApiManagementCmdletBase
    {
        private const string TenantLevel = "SetTenantLevel";
        private const string ApiLevel = "SetApiLevel";

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Instance of PsApiManagementContext. This parameter is required.")]
        [ValidateNotNullOrEmpty]
        public PsApiManagementContext Context { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of the logger to push diagnostics to. This parameter is required.")]
        public String LoggerId { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Identifier of the diagnostics entity. This parameter is optional.")]
        [PSArgumentCompleter("applicationinsights", "azuremonitor")]
        public String DiagnosticId { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Specifies for what type of messages sampling settings should not apply. This parameter is optional.")]
        [ValidateSet("allErrors")]
        public String AlwaysLog { get; set; }

        [Parameter(
            ParameterSetName = ApiLevel,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing API. If specified will set API-scope policy. This parameters is required.")]
        public String ApiId { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Sampling Setting of the Diagnostic. This parameter is optional.")]
        public PsApiManagementSamplingSetting SamplingSetting { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Diagnostic setting for incoming/outgoing Http Messsages to the Gateway. This parameter is optional.")]
        public PsApiManagementPipelineDiagnosticSetting FrontEnd { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Diagnostic setting for incoming/outgoing Http Messsages to the Backend. This parameter is optional.")]
        public PsApiManagementPipelineDiagnosticSetting Backend { get; set; }

        public override void ExecuteApiManagementCmdlet()
        {
            var diagnosticId = DiagnosticId ?? "applicationinsights";
            PsApiManagementDiagnostic diagnostic;
            switch (ParameterSetName)
            {
                case TenantLevel:
                    diagnostic = Client.DiagnosticCreateTenantLevel(
                        Context,
                        diagnosticId,
                        LoggerId,
                        AlwaysLog,
                        SamplingSetting,
                        FrontEnd,
                        Backend);
                    break;
                case ApiLevel:
                    diagnostic = Client.DiagnosticCreateApiLevel(
                        Context,
                        ApiId,
                        diagnosticId,
                        LoggerId,
                        AlwaysLog,
                        SamplingSetting,
                        FrontEnd,
                        Backend);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Parameter set name '{0}' is not supported.", ParameterSetName));
            }

            WriteObject(diagnostic);
        }
    }
}
