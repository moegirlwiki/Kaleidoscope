﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class ApiResponseAttribute : Attribute
    {
        public string Name { get; }

        public ApiResponseAttribute(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
