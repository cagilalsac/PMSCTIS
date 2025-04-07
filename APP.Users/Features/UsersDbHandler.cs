using APP.Users.Domain;
using CORE.APP.Features;
using System.Globalization;
using System.Security.Claims;

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

        protected virtual string CreateAccessToken(List<Claim> claims, DateTime expiration)
        {
            var signingKey = 
        }
    }
}
