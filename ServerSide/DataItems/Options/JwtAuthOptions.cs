﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SpotifyExtension.DataItems.Options
{
    public static class JwtAuthOptions
    {
        public const string AuthenticationType = "JwtToken";

        public const string Issuer = "SpotifyExtensionAuthServer";

        public const string Audience = "SpotifyExtensionAuthClient";

        public const string KeyForCreateJwtTokens = "QTkyygq1D7UtcHSjcV6zae8bX3Vso2flXZkSpTN58VxSOLaByC9PbcISlyIO37UUUeIfj9GLiTcNt3wjKYxTV";

        public const int Lifetime = 12 * 60;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KeyForCreateJwtTokens));
        }
    }
}
