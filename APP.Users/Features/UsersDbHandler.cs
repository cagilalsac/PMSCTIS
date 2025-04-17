using APP.Users.Domain;
using CORE.APP.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace APP.Users.Features
{
    /// <summary>
    /// An abstract class that provides a base handler for operations related to the Users database.
    /// </summary>
    public abstract class UsersDbHandler : Handler
    {
        /// <summary>
        /// Gets the database context for accessing the Users database.
        /// </summary>
        protected readonly UsersDb _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersDbHandler"/> class.
        /// </summary>
        /// <param name="db">The database context to be used for database operations.</param>
        protected UsersDbHandler(UsersDb db) : base(new CultureInfo("en-US"))
        {
            _db = db;
        }

        /// <summary>
        /// Creates a signed JWT access token with the specified claims and expiration time.
        /// </summary>
        /// <param name="claims">A list of claims to include in the token.</param>
        /// <param name="expiration">The expiration date and time of the token.</param>
        /// <returns>A signed JWT access token as a string.</returns>
        protected virtual string CreateAccessToken(List<Claim> claims, DateTime expiration)
        {
            // Create signing credentials using the app's symmetric security key and HMAC SHA256 algorithm
            var signingCredentials = new SigningCredentials(AppSettings.SigningKey, SecurityAlgorithms.HmacSha256Signature);

            // Create the JWT token with issuer, audience, claims, and expiration
            var jwtSecurityToken = new JwtSecurityToken(AppSettings.Issuer, AppSettings.Audience, claims, DateTime.Now, expiration, signingCredentials);

            // Write the token to a string
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            return jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
        }

        /// <summary>
        /// Generates a list of claims based on the provided user object.
        /// </summary>
        /// <param name="user">The user for whom to generate claims.</param>
        /// <returns>A list of claims including Name, Role, and Id.</returns>
        protected virtual List<Claim> GetClaims(User user)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("Id", user.Id.ToString()) // Custom claim for the user's ID
            };
        }

        protected virtual string CreateRefreshToken()
        {
            var bytes = new byte[32];

            // Way 1:
            //var randomNumberGenerator = RandomNumberGenerator.Create();
            //randomNumberGenerator.GetBytes(bytes);
            //randomNumberGenerator.Dispose();

            // Way 2:
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(bytes);
            }

            return Convert.ToBase64String(bytes);
        }

        protected virtual ClaimsPrincipal GetPrincipal(string accessToken)
        {
            accessToken = accessToken.StartsWith(JwtBearerDefaults.AuthenticationScheme) ?
                accessToken.Remove(0, JwtBearerDefaults.AuthenticationScheme.Length + 1) : accessToken;
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AppSettings.SigningKey
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = jwtSecurityTokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
            return securityToken is null ? null : principal;
        }
    }
}
