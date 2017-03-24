//-----------------------------------------------------------------------
// <copyright file="TokenQueryProvider.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Actions.Query;
using System;
using System.Collections.Generic;

namespace Moegirlpedia.MediaWikiInterop.Actions.QueryProviders
{
    [QueryProvider("tokens", "meta", typeof(Tokens))]
    public class TokenQueryProvider : IQueryProvider<Tokens>
    {
        public TokenTypes Types { get; set; }

        public List<KeyValuePair<string, string>> GetParameters()
        {
            var tokenTypes = new List<TokenTypes>();
            foreach (TokenTypes type in Enum.GetValues(typeof(TokenTypes)))
                AddIfSet(tokenTypes, Types, type);

            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("type", string.Join("|", tokenTypes).ToLower())
            };
        }

        private static void AddIfSet(
            List<TokenTypes> types, TokenTypes selection, TokenTypes match)
        {
            if ((selection & match) == match) types.Add(match);
        }

        [Flags]
        public enum TokenTypes
        {
            Csrf,
            CreateAccount,
            Login,
            Patrol,
            Rollback,
            UserRights,
            Watch
        }
    }
}
