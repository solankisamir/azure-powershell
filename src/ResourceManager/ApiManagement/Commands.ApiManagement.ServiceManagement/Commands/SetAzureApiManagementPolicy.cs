﻿//  
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
    using Management.ApiManagement.Models;
    using Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Models;
    using System;
    using System.IO;
    using System.Management.Automation;
    using System.Text;

    [Cmdlet(VerbsCommon.Set, Constants.ApiManagementPolicy, DefaultParameterSetName = TenantLevel)]
    [OutputType(typeof(bool))]
    public class SetAzureApiManagementPolicy : AzureApiManagementCmdletBase
    {
        private const string DefaultFormat = "application/vnd.ms-azure-apim.policy+xml";

        private const string TenantLevel = "SetTenantLevel";
        private const string ProductLevel = "SetProductLevel";
        private const string ApiLevel = "SetApiLevel";
        private const string OperationLevel = "SetOperationLevel";

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Instance of PsApiManagementContext. This parameter is required.")]
        [ValidateNotNullOrEmpty]
        public PsApiManagementContext Context { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Format of the policy. This parameter is optional." +
                          "When using application/vnd.ms-azure-apim.policy+xml, expressions contained within the policy must be XML-escaped." +
                          "When using application/vnd.ms-azure-apim.policy.raw+xml no escaping is necessary." +
                          "Default value is 'application/vnd.ms-azure-apim.policy+xml'.")]
        [Obsolete("This parameter will be deprecated in the future release.")]
        public String Format { get; set; }

        [Parameter(
            ParameterSetName = ProductLevel,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing product. If specified will set product-scope policy. This parameters is required.")]
        public String ProductId { get; set; }

        [Parameter(
            ParameterSetName = ApiLevel,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing API. If specified will set API-scope policy. This parameters is required.")]
        [Parameter(
            ParameterSetName = OperationLevel,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing API. If specified will set API-scope policy. This parameters is required.")]
        public String ApiId { get; set; }

        [Parameter(
            ParameterSetName = OperationLevel,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing operation. If specified with ApiId will set operation-scope policy. This parameters is required.")]
        public String OperationId { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Policy document as a string. This parameter is required if -PolicyFilePath or -PolicyUrl is not specified.")]
        public String Policy { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Policy document file path. This parameter is required if -Policy or -PolicyUrl is not specified.")]
        public String PolicyFilePath { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "The Url where the Policy document is hosted. This parameter is required if -Policy or -PolicyFilePath is not specified.")]
        public String PolicyUrl { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "If specified will write true in case operation succeeds. This parameter is optional. Default value is false.")]
        public SwitchParameter PassThru { get; set; }

        public override void ExecuteApiManagementCmdlet()
        {
            string policyContent;
            string contentFormat;
            if (!string.IsNullOrWhiteSpace(Policy))
            {
                policyContent = Policy;
                contentFormat = PolicyContentFormat.Xml;
            }
            else if (!string.IsNullOrEmpty(PolicyFilePath))
            {
                policyContent = File.ReadAllText(PolicyFilePath);
                contentFormat = PolicyContentFormat.Xml;
            }
            else if (!string.IsNullOrEmpty(PolicyUrl))
            {
                policyContent = PolicyUrl;
                contentFormat = PolicyContentFormat.XmlLink;
            }
            else
            {
                throw new PSInvalidOperationException("Either -Policy or -PolicyFilePath or -PolicyUrl should be specified.");
            }

            string format = Format ?? DefaultFormat;
            switch (ParameterSetName)
            {
                case TenantLevel:
                    Client.PolicySetTenantLevel(Context, format, policyContent, contentFormat);
                    break;
                case ProductLevel:
                    Client.PolicySetProductLevel(Context, format, policyContent, ProductId, contentFormat);
                    break;
                case ApiLevel:
                    Client.PolicySetApiLevel(Context, format, policyContent, ApiId, contentFormat);
                    break;
                case OperationLevel:
                    if (string.IsNullOrWhiteSpace(ApiId))
                    {
                        throw new PSArgumentNullException("ApiId");
                    }
                    Client.PolicySetOperationLevel(Context, format, policyContent, ApiId, OperationId, contentFormat);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Parameter set name '{0}' is not supported.", ParameterSetName));
            }

            if (PassThru)
            {
                WriteObject(true);
            }
        }
    }
}
