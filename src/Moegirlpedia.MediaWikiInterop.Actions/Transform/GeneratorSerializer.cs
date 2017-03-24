//-----------------------------------------------------------------------
// <copyright file="GeneratorSerializer.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Actions.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System.Collections.Generic;

namespace Moegirlpedia.MediaWikiInterop.Actions.Transform
{
    public class GeneratorSerializer : IAggregatedApiParamSerializer
    {
        public List<KeyValuePair<string, string>> SerializeAllFields(object entity)
        {
            if (entity is PurgeInputModel)
            {
                var kvPairs = entity.ToKeyValuePairCollection();
                kvPairs.Add(new KeyValuePair<string, string>("generator", ((PurgeInputModel)entity).Name));
                return kvPairs;
            }

            return new List<KeyValuePair<string, string>>();
        }

        public string SerializeFinalizedFields(object entity)
        {
            return string.Empty;
        }
    }
}
