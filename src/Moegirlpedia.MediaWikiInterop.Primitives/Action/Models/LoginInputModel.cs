//-----------------------------------------------------------------------
// <copyright file="LoginInputModel.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Action.Models
{
    public class LoginInputModel : IApiActionRequest
    {
        [ApiParameter("lgname", true)]
        public string Username { get; set; }

        [ApiParameter("lgpassword", true)]
        public string Password { get; set; }

        [ApiParameter("lgdomain", true)]
        public string Domain { get; set; }

        [ApiParameter("lgtoken", true)]
        public string Token { get; set; }
    }
}
