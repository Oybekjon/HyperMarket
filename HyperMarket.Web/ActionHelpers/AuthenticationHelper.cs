using HyperMarket.Queries.User.Login;
using HyperMarket.Web.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HyperMarket.Web.ActionHelpers
{
    public class AuthenticationHelper
    {
        private readonly AuthenticationSettings Settings;

        public AuthenticationHelper(AuthenticationSettings settings)
        {
            Settings = settings;
        }
        public AuthToken ToToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                            issuer: Settings.Issuer,
                            audience: Settings.Audience,
                            notBefore: now,
                            claims: identity.Claims,
                            expires: now.Add(TimeSpan.FromMinutes(Settings.Lifetime)),
                            signingCredentials: new SigningCredentials(
                                Settings.GetSymmetricSecurityKey(),
                                SecurityAlgorithms.HmacSha256
                            )
                        );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new AuthToken
            {
                AccessToken = encodedJwt,
                Email = identity.Name
            };

            return response;
        }

        public ClaimsIdentity ToClaimsIdentity(LoginResult loginResult)
        {
            Guard.NotNull(loginResult, nameof(loginResult));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginResult.Email ?? ""),
                new Claim(ClaimTypes.MobilePhone, loginResult.Phone ?? ""),
                new Claim(ClaimTypes.GivenName, loginResult.FirstName ?? ""),
                new Claim(ClaimTypes.Surname, loginResult.LastName ?? ""),
                new Claim(ClaimTypes.Name, loginResult.UserIdentifier ?? "")
            };

            if (loginResult.Permissions != null)
            {
                foreach (var perm in loginResult.Permissions)
                {
                    claims.Add(new Claim(ClaimTypes.Role, perm));
                }
            }

            if (loginResult.StorePermissions != null)
            {
                foreach (var pair in loginResult.StorePermissions)
                {
                    claims.Add(new Claim(ClaimTypes.Role, $"{pair.Value}/{pair.Key}"));
                }
            }

            var claimsIdentity = new ClaimsIdentity(
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );

            return claimsIdentity;
        }
    }
}
