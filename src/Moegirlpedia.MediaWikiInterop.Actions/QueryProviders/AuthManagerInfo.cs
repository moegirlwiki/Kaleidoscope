//-----------------------------------------------------------------------
// <copyright file="AuthManagerInfo.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Moegirlpedia.MediaWikiInterop.Actions.QueryProviders
{
    public class AuthManagerInfo
    {
        [JsonProperty("requests")]
        private JObject[] m_rawRequestEntries;
        private List<AuthManagerProviderMetadata> m_requestEntries;

        /// <summary>
        /// Class constructor that create new instance of <see cref="AuthManagerInfo"/>.
        /// </summary>
        public AuthManagerInfo()
        {
            m_requestEntries = new List<AuthManagerProviderMetadata>();
            m_rawRequestEntries = null;
        }

        public bool CanAuthenticateNow { get; set; }
        public bool CanCreateAccounts { get; set; }
        public bool CanLinkAccounts { get; set; }
        public bool HasPreservedState { get; set; }
        public bool HasPrimaryPreservedState { get; set; }
        public string PreservedUsername { get; set; }
        public IReadOnlyList<AuthManagerProviderMetadata> Requests
        {
            get
            {
                if (m_requestEntries.Count < 1) PostParseRequests();
                return m_requestEntries;
            }
        }

        private void PostParseRequests()
        {
            if (m_rawRequestEntries == null) return;
            foreach (var entry in m_rawRequestEntries)
            {
                m_requestEntries.Add(JsonConvert.DeserializeObject<AuthManagerProviderMetadata>(entry.ToString()));
            }
        }

        public class AuthManagerProviderMetadata
        {
            [JsonProperty("required")]
            private string m_requirementString;

            [JsonProperty("fields")]
            private JObject m_rawFields;

            private Requirement m_requirement;
            private Dictionary<string, ProviderEntry> m_fields;

            public AuthManagerProviderMetadata()
            {
                m_fields = new Dictionary<string, ProviderEntry>();
                m_requirement = Requirement.Unknown;
                m_rawFields = null;
                m_requirementString = null;
            }

            public string Id { get; set; }
            public Requirement Required
            {
                get
                {
                    if (m_requirement != Requirement.Unknown) return m_requirement;
                    switch (m_requirementString)
                    {
                        case "primary-required":
                            m_requirement = Requirement.PrimaryRequired;
                            break;
                        case "required":
                            m_requirement = Requirement.Required;
                            break;
                        case "optional":
                            m_requirement = Requirement.Optional;
                            break;
                    }

                    return m_requirement;
                }
            }
            public string Provider { get; set; }
            public string Account { get; set; }
            public IReadOnlyDictionary<string, ProviderEntry> Fields
            {
                get
                {
                    if (m_fields.Count < 1) PostParseFields();
                    return m_fields;
                }
            }

            private void PostParseFields()
            {
                if (m_rawFields == null) return;
                foreach (var field in m_rawFields)
                {
                    if (!m_fields.ContainsKey(field.Key))
                        m_fields.Add(field.Key,
                            JsonConvert.DeserializeObject<ProviderEntry>(field.Value.ToString()));
                }
            }

            public enum Requirement
            {
                Optional = 0,
                Required = 1,
                PrimaryRequired = 2,
                Unknown = 3
            }

            public class ProviderEntry
            {
                public string Type { get; set; }
                public string Label { get; set; }
                public string Help { get; set; }
                public bool Optional { get; set; }
                public bool Sensitive { get; set; }
            }
        }

        public override string ToString()
        {
            return $"Auth={CanAuthenticateNow}, Create={CanCreateAccounts}, " +
                $"Link={CanLinkAccounts}, Preserved={HasPreservedState}, " +
                $"PrimaryPreserved={HasPrimaryPreservedState}, PreservedUsername={PreservedUsername}, " +
                $"RequestsType={string.Join(",", Requests.Select(i => i.Id))}";
        }
    }
}
