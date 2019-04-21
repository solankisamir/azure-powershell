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
    using System;
    using System.Linq;
    using System.Management.Automation;

    [Cmdlet("Set", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ApiManagementDiagnostic", DefaultParameterSetName = ExpandedParameterSet)]
    [OutputType(typeof(PsApiManagementDiagnostic), ParameterSetName = new[] { ExpandedParameterSet, ByInputObjectParameterSet })]
    public class SetAzureApiManagementDiagnostic : AzureApiManagementCmdletBase
    {
        #region Parameter Set Names

        protected const string ExpandedParameterSet = "ExpandedParameter";
        protected const string ByInputObjectParameterSet = "ByInputObject";

        #endregion

        [Parameter(
            ParameterSetName = ExpandedParameterSet,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Instance of PsApiManagementContext. This parameter is required.")]
        [ValidateNotNullOrEmpty]
        public PsApiManagementContext Context { get; set; }        

        [Parameter(
            ParameterSetName = ExpandedParameterSet,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing Diagnostic. This parameter is required.")]
        [ValidateNotNullOrEmpty]
        public String DiagnosticId { get; set; }

        [Parameter(
            ParameterSetName = ByInputObjectParameterSet,
            ValueFromPipeline = true,
            Mandatory = true,
            HelpMessage = "Instance of PsApiManagementDiagnostic. This parameter is required.")]
        [ValidateNotNullOrEmpty]
        public PsApiManagementDiagnostic InputObject { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Identifier of existing API. This parameter is optional.")]
        [ValidateNotNullOrEmpty]
        public String ApiId { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Identifier of the logger to push diagnostics to. This parameter is required.")]
        public String LoggerId { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Specifies for what type of messages sampling settings should not apply. This parameter is optional.")]
        [ValidateSet("allErrors")]
        public String AlwaysLog { get; set; }

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

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "If specified then instance of" +
                          " Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Models.PsApiManagementDiagnostic type " +
                          "representing the set Diagnostic.")]
        public SwitchParameter PassThru { get; set; }

        public override void ExecuteApiManagementCmdlet()
        {
            string resourcegroupName;
            string serviceName;
            string diagnosticId;

            if (ParameterSetName.Equals(ByInputObjectParameterSet))
            {
                resourcegroupName = InputObject.ResourceGroupName;
                serviceName = InputObject.ServiceName;
                diagnosticId = InputObject.DiagnosticId;
            }
            else
            {
                resourcegroupName = Context.ResourceGroupName;
                serviceName = Context.ServiceName;
                diagnosticId = DiagnosticId;
            }

            PsApiManagementDiagnostic diagnostic;
            if (string.IsNullOrEmpty(ApiId))
            {
                diagnostic = Client.DiagnosticSetTenantLevel(
                    resourcegroupName,
                    serviceName,
                    diagnosticId,
                    LoggerId,
                    AlwaysLog,
                    SamplingSetting,
                    FrontEnd,
                    Backend,                    
                    InputObject);
            }
            else
            {
                diagnostic = Client.DiagnosticSetApiLevel(
                    resourcegroupName,
                    serviceName,
                    ApiId,
                    diagnosticId,
                    LoggerId,
                    AlwaysLog,
                    SamplingSetting,
                    FrontEnd,
                    Backend,
                    InputObject);
            }

            if (PassThru.IsPresent)
            {
                WriteObject(diagnostic);
            }
        }
    }
}
