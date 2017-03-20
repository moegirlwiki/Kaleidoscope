//-----------------------------------------------------------------------
// <copyright file="ApiParameterAttribute.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ApiParameterAttribute : Attribute
    {
        public string Name { get; }

        public bool IsSensitive { get; }

        public bool IsAggregated { get; }

        internal Type CustomConverter { get; }

        internal bool IsFinalized { get; }

        public ApiParameterAttribute(string name, bool isSensitive = false, 
            bool isAggregated = false, Type customConverter = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IsSensitive = isSensitive;
            IsAggregated = IsAggregated;
            
            // Validate if converter is present
            if (customConverter != null)
            {
                if (typeof(IAggregatedApiParamSerializer).IsAssignableFrom(customConverter))
                {
                    IsFinalized = false;
                }
                else if (typeof(IFinalizedApiParamSerializer).IsAssignableFrom(customConverter))
                {
                    if (IsAggregated) throw new InvalidOperationException("Aggregated property cannot use finalized converter");
                    IsFinalized = true;
                }
                else
                {
                    throw new ArgumentException("Not a valid converter");
                }

                CustomConverter = customConverter;
            }

            // Validate aggreated property
            if (IsAggregated && CustomConverter == null)
            {
                throw new InvalidOperationException("Aggreated property must use custom converter");
            }
        }
    }
}
