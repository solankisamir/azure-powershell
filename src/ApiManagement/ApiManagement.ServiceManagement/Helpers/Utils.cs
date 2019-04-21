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

namespace Microsoft.Azure.Commands.ApiManagement.ServiceManagement
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AutoMapper;
    using Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Models;
    using Microsoft.Azure.Management.ApiManagement.Models;

    public class Utils
    {
        public static string GetApiIdFullPath(string apiId, string apiRevision)
        {
            if (!string.IsNullOrEmpty(apiId) && !string.IsNullOrEmpty(apiRevision))
            {
                return apiId.ApiRevisionIdentifierFullPath(apiRevision);
            }
            else if (!string.IsNullOrEmpty(apiId))
            {
                return $"/apis/{apiId}";
            }

            return null;
        }

        public static string GetApiVersionIdFullPath(string apiVersionId)
        {
            return $"/apiVersionSets/{apiVersionId}";
        }

        public static string GetLoggerIdFullPath(string loggerId)
        {
            return $"/loggers/{loggerId}";
        }

        public static string GetUserIdFullPath(string userId)
        {
            return $"/users/{userId}";
        }

        public static IDictionary<string, object> ToBackendProperties(BackendTlsProperties tlsProperties)
        {
            if (tlsProperties == null)
            {
                return null;
            }

            var psTlsProperties = new Dictionary<string, object>();
            if (tlsProperties.ValidateCertificateChain.HasValue)
            {
                psTlsProperties.Add("skipCertificateChainValidation", !tlsProperties.ValidateCertificateChain.Value);
            }

            if (tlsProperties.ValidateCertificateName.HasValue)
            {
                psTlsProperties.Add("skipCertificateNameValidation", !tlsProperties.ValidateCertificateName.Value);
            }

            return psTlsProperties;
        }

        public static Dictionary<string, IList<string>> HashTableToDictionary(Hashtable table)
        {
            if (table == null)
            {
                return null;
            }

            var result = new Dictionary<string, IList<string>>();
            foreach (var entry in table.Cast<DictionaryEntry>())
            {
                var entryValue = entry.Value as object[];
                if (entryValue == null)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.InvariantCulture,
                            "Invalid input type specified for Key '{0}', expected string[]",
                            entry.Key));
                }
                result.Add(entry.Key.ToString(), entryValue.Select(i => i.ToString()).ToList());
            }

            return result;
        }

        public static IList<X509CertificateName> HashTableToX509CertificateName(Hashtable certificates)
        {
            if (certificates == null)
            {
                return null;
            }

            var result = new List<X509CertificateName>();
            foreach (var keyEntry in certificates.Cast<DictionaryEntry>())
            {
                result.Add(new X509CertificateName()
                {
                    Name = keyEntry.Key.ToString(),
                    IssuerCertificateThumbprint = keyEntry.Value.ToString()
                });
            }

            return result.ToArray();
        }

        public static Hashtable X509CertificateToHashTable(IEnumerable<X509CertificateName> certificates)
        {
            if (certificates == null || !certificates.Any())
            {
                return null;
            }

            var result = new Hashtable();
            foreach (var keyEntry in certificates)
            {
                result.Add(keyEntry.Name, keyEntry.IssuerCertificateThumbprint);
            }

            return result;
        }

        public static Hashtable DictionaryToHashTable(IDictionary<string, IList<string>> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            var result = new Hashtable();
            foreach (var keyEntry in dictionary.Keys)
            {
                var keyValue = dictionary[keyEntry];

                result.Add(keyEntry, keyValue.Cast<object>().ToArray());
            }

            return result;
        }

        public static IList<ParameterContract> ToParameterContract(PsApiManagementParameter[] parameters)
        {
            if (parameters == null || !parameters.Any())
            {
                return null;
            }

            var parameterList = new List<ParameterContract>();
            foreach (var parameter in parameters)
            {
                parameterList.Add(Mapper.Map<ParameterContract>(parameter));
            }

            return parameterList;
        }

        public static AuthenticationSettingsContract ToAuthenticationSettings(PsApiManagementApi psApiManagementApi)
        {
            if (psApiManagementApi == null ||
                string.IsNullOrWhiteSpace(psApiManagementApi.AuthorizationServerId) ||
                string.IsNullOrEmpty(psApiManagementApi.AuthorizationScope))
            {
                return null;
            }

            var settings = new AuthenticationSettingsContract()
            {
                OAuth2 = new OAuth2AuthenticationSettingsContract()
                {
                    AuthorizationServerId = psApiManagementApi.AuthorizationServerId,
                    Scope = psApiManagementApi.AuthorizationScope
                }
            };

            return settings;
        }

        public static SubscriptionKeyParameterNamesContract ToSubscriptionKeyParameterNamesContract(PsApiManagementApi psApiManagementApi)
        {
            if (psApiManagementApi == null ||
                (string.IsNullOrWhiteSpace(psApiManagementApi.SubscriptionKeyHeaderName) &&
                string.IsNullOrEmpty(psApiManagementApi.SubscriptionKeyQueryParamName)))
            {
                return null;
            }

            var subscriptionKeyParameters = new SubscriptionKeyParameterNamesContract()
            {
                Header = psApiManagementApi.SubscriptionKeyHeaderName,
                Query = psApiManagementApi.SubscriptionKeyQueryParamName
            };

            return subscriptionKeyParameters;
        }

        public static PsApiManagementParameter[] ToParameterContract(IList<ParameterContract> parameters)
        {
            if (parameters == null || !parameters.Any())
            {
                return null;
            }

            var parameterList = new List<PsApiManagementParameter>();
            foreach (var parameter in parameters)
            {
                parameterList.Add(Mapper.Map<PsApiManagementParameter>(parameter));
            }

            return parameterList.ToArray();
        }

        public static string TrimApiResourceIdentifier(string armApiId)
        {
            if (string.IsNullOrEmpty(armApiId))
            {
                return null;
            }

            var apiIdArrary = armApiId.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            return apiIdArrary.Last();
        }
    }
}
