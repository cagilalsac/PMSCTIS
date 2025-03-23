using APP.Users.Domain;
using APP.Users.Features.Skills;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Users
{
    /// <summary>
    /// Represents a request to query user data.
    /// </summary>
    public class UserQueryRequest : Request, IRequest<IQueryable<UserQueryResponse>>
    {
    }

    /// <summary>
    /// Represents the response containing user data for a query.
    /// </summary>
    public class UserQueryResponse : QueryResponse
    {
        /// <summary>
        /// Gets or sets the user's username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the status indicating if the user is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the formatted status of the user's activity (Active/Inactive).
        /// </summary>
        public string IsActiveF { get; set; }

        /// <summary>
        /// Gets or sets the registration date of the user.
        /// </summary>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the formatted registration date of the user.
        /// </summary>
        public string RegistrationDateF { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname of the user.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the full name of the user (Name + Surname).
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user's role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user's role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the list of skill IDs associated with the user.
        /// </summary>
        public List<int> SkillIds { get; set; }

        /// <summary>
        /// Gets or sets the list of skills associated with the user.
        /// </summary>
        public List<SkillQueryResponse> Skills { get; set; }
    }

    /// <summary>
    /// Handles the request to query user data.
    /// </summary>
    public class UserQueryHandler : UsersDbHandler, IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserQueryHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for handling user-related operations.</param>
        public UserQueryHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to query user data.
        /// </summary>
        /// <param name="request">The request containing the user data query parameters.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, with a result of a queryable list of <see cref="UserQueryResponse"/>.</returns>
        public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Users.Include(u => u.Role)
                .Include(u => u.UserSkills)
                .ThenInclude(us => us.Skill)
                .OrderBy(u => u.Name)
                .Select(u => new UserQueryResponse()
                {
                    Id = u.Id,
                    Name = u.Name,
                    FullName = u.Name + " " + u.Surname,
                    IsActive = u.IsActive,
                    IsActiveF = u.IsActive ? "Active" : "Inactive",
                    Password = u.Password,
                    Role = u.Role.Name,
                    Surname = u.Surname,
                    UserName = u.UserName,
                    RegistrationDate = u.RegistrationDate,
                    RegistrationDateF = u.RegistrationDate.HasValue ? u.RegistrationDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                    RoleId = u.RoleId,
                    SkillIds = u.SkillIds,
                    Skills = u.UserSkills.Select(us => new SkillQueryResponse()
                    {
                        Id = us.Skill.Id,
                        Name = us.Skill.Name
                    }).ToList()
                });
            return Task.FromResult(query);
        }
    }
}
