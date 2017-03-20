//-----------------------------------------------------------------------
// <copyright file="StandardKeyValuePairSerializer.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Transform
{
    public static class StandardKeyValuePairSerializer
    {
        private static ConditionalWeakTable<Type, PropertyInfo[]> m_propMetadataCache
            = new ConditionalWeakTable<Type, PropertyInfo[]>();

        public static List<KeyValuePair<string, string>> ToKeyValuePairCollection(this object entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            // Intermediate type
            var cType = entity.GetType();

            // Check if we have property information stored
            PropertyInfo[] properties = null;

            // Intermediate Key-Value pair
            var kvPairs = new List<KeyValuePair<string, string>>();

            // Query all properties with attribute
            if (!m_propMetadataCache.TryGetValue(cType, out properties))
            {
                var propertiesQuery = cType.GetProperties().Where(p => p.IsDefined(typeof(ApiParameterAttribute)));
                properties = propertiesQuery.ToArray();
                m_propMetadataCache.Add(cType, properties);
            }

            // Call path is determined, so it is not necessary to check nullref here
            // Serialize properties
            foreach (var property in properties)
            {
                // Null check
                var pValue = property.GetValue(entity);
                if (pValue == null) continue;

                // Attribute
                var pAttrib = property.GetCustomAttribute<ApiParameterAttribute>();

                // Aggreated property
                if (pAttrib.IsAggregated)
                {
                    var serializer = (IAggregatedApiParamSerializer)Activator.CreateInstance(pAttrib.CustomConverter);
                    kvPairs.AddRange(serializer.SerializeAllFields(pValue));
                    continue;
                }

                // With customer converter
                if (pAttrib.CustomConverter != null)
                {
                    var serializer = (IFinalizedApiParamSerializer)Activator.CreateInstance(pAttrib.CustomConverter);
                    kvPairs.Add(new KeyValuePair<string, string>(pAttrib.Name, serializer.SerializeFinalizedFields(pValue)));
                    continue;
                }

                // Finalized property
                string serValue = null;
                switch (pValue)
                {
                    case bool bValue:
                        serValue = bValue ? "1" : "0";
                        break;
                    case string sValue:
                        serValue = sValue;
                        break;
                    case int iValue:
                        serValue = iValue.ToString("D");
                        break;
                    case List<int> liValue:
                        serValue = string.Join("|", liValue.Select(i => i.ToString("D")));
                        break;
                    case List<string> lsValue:
                        serValue = string.Join("|", lsValue);
                        break;
                }

                kvPairs.Add(new KeyValuePair<string, string>(pAttrib.Name, serValue));
            }

            return kvPairs;
        }
    }
}
