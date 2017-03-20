using System;
using System.Reflection;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ApiParameterAttribute : Attribute
    {
        public string Name { get; }

        public bool IsSensitive { get; }

        internal Type CustomConverter { get; }

        internal bool IsFinalized { get; }

        public ApiParameterAttribute(string name, bool isSensitive = false, Type customConverter = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IsSensitive = isSensitive;
            
            // Validate if converter is present
            if (customConverter != null)
            {
                if (typeof(IMultipleApiParamSerializer).IsAssignableFrom(customConverter))
                {
                    IsFinalized = false;
                }
                else if (typeof(IFinalizedApiParamSerializer).IsAssignableFrom(customConverter))
                {
                    IsFinalized = true;
                }
                else
                {
                    throw new ArgumentException("Not a valid converter");
                }

                CustomConverter = customConverter;
            }
        }
    }
}
