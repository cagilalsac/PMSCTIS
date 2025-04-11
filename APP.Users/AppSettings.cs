using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APP.Users
{
    /// <summary>
    /// Represents application-level security settings used for JWT generation and validation.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// The token issuer — identifies the principal that issued the token.
        /// </summary>
        public static string Issuer { get; set; }

        /// <summary>
        /// The intended recipient of the token — usually the application itself.
        /// </summary>
        public static string Audience { get; set; }

        /// <summary>
        /// Token expiration time in minutes.
        /// </summary>
        public static int ExpirationInMinutes { get; set; }

        /// <summary>
        /// The secret key used to sign the JWT tokens.
        /// </summary>
        public static string SecurityKey { get; set; }

        /// <summary>
        /// Gets a symmetric security key derived from the <see cref="SecurityKey"/> property.
        /// Used to sign and validate JWT tokens.
        /// </summary>
        public static SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
    }
}
