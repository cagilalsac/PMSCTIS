using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.Users.Features.Users
{
    /// <summary>
    /// Represents a request to create a new user.
    /// </summary>
    public class UserCreateRequest : Request, IRequest<CommandResponse>
    {
        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; } // Inherited ID property, ignored in JSON serialization.

        /// <summary>
        /// Gets or sets the username for the new user.
        /// </summary>
        /// <remarks>
        /// The username is required and must be between 3 and 30 characters in length.
        /// </remarks>
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for the new user.
        /// </summary>
        /// <remarks>
        /// The password is required and must be between 3 and 15 characters in length.
        /// </remarks>
        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets whether the user is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname of the user.
        /// </summary>
        [StringLength(50)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the registration date of the user.
        /// </summary>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the role ID for the user.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the list of skill IDs associated with the user.
        /// </summary>
        public List<int> SkillIds { get; set; }
    }

    /// <summary>
    /// Handles the creation of a new user in the system.
    /// </summary>
    public class UserCreateHandler : UsersDbHandler, IRequestHandler<UserCreateRequest, CommandResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreateHandler"/> class.
        /// </summary>
        /// <param name="db">The database context used for handling user-related operations.</param>
        public UserCreateHandler(UsersDb db) : base(db)
        {
        }

        /// <summary>
        /// Handles the request to create a new user.
        /// </summary>
        /// <param name="request">The request containing the user creation information.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="CommandResponse"/> indicating the success or failure of the user creation operation.</returns>
        public async Task<CommandResponse> Handle(UserCreateRequest request, CancellationToken cancellationToken)
        {
            // Check if a user with the same username or full name already exists
            if (_db.Users.Any(u => u.UserName == request.UserName || (u.Name == request.Name && u.Surname == request.Surname)))
                return Error("User with the same user name or full name exists!");

            // Create a new user entity from the request data
            var user = new User()
            {
                IsActive = request.IsActive,
                Name = request.Name?.Trim(),
                Password = request.Password,
                RoleId = request.RoleId,
                Surname = request.Surname?.Trim(),
                UserName = request.UserName,
                RegistrationDate = request.RegistrationDate
            };

            // Add the new user to the database
            _db.Users.Add(user);

            // Add skills to the user
            if (request.SkillIds != null && request.SkillIds.Any())
            {
                var userSkills = request.SkillIds.Select(skillId => new UserSkill
                {
                    UserId = user.Id,
                    SkillId = skillId
                }).ToList();
                _db.UserSkills.AddRange(userSkills);
            }

            // Save the changes to the database
            await _db.SaveChangesAsync(cancellationToken);

            return Success("User created successfully.", user.Id);
        }
    }
}
