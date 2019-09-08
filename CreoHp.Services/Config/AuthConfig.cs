using System;
using System.Security.Cryptography;
using CreoHp.Dto.Users;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace CreoHp.Services.Config
{
    public class AuthConfig
    {
        RsaSecurityKey _securityKey;
        string _issuerUrl;

        public string IssuerUrl
        {
            get => _issuerUrl;
            set
            {
                _issuerUrl = value;
                Issuer = new Uri(IssuerUrl).Host;
            }
        }

        public Rsa Rsa { get; set; }

        [JsonIgnore]
        public RsaSecurityKey SecurityKey => Rsa.D == null
            ? null
            : _securityKey ?? (_securityKey = new RsaSecurityKey(Rsa.Parameters));

        [JsonIgnore] public string Issuer { get; private set; }

        [JsonIgnore] public string Audience => Issuer;

        public SignUpDto InitialAdmin { get; set; }
    }

    public class Rsa
    {
        public string D { get; set; }
        public string DP { get; set; }
        public string DQ { get; set; }
        public string Exponent { get; set; }
        public string InverseQ { get; set; }
        public string Modulus { get; set; }
        public string P { get; set; }
        public string Q { get; set; }

        [JsonIgnore]
        public RSAParameters Parameters => new RSAParameters
        {
            D = Convert.FromBase64String(D),
            DP = Convert.FromBase64String(DP),
            DQ = Convert.FromBase64String(DQ),
            Exponent = Convert.FromBase64String(Exponent),
            InverseQ = Convert.FromBase64String(InverseQ),
            Modulus = Convert.FromBase64String(Modulus),
            P = Convert.FromBase64String(P),
            Q = Convert.FromBase64String(Q)
        };
    }
}