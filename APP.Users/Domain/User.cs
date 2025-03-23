﻿using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Users.Domain
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        /// <remarks>
        /// The username is required and must be between 3 and 30 characters long.
        /// </remarks>
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        /// <remarks>
        /// The password is required and must be between 3 and 15 characters long.
        /// </remarks>
        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active.
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
        /// Gets or sets the date when the user registered.
        /// </summary>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the role ID associated with the user.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the role assigned to the user.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Gets or sets the list of user skills.
        /// </summary>
        public List<UserSkill> UserSkills { get; set; } = new List<UserSkill>();

        /// <summary>
        /// Gets or sets a list of skill IDs associated with the user.
        /// </summary>
        /// <remarks>
        /// This property is not mapped to the database and is used for easier manipulation of skills.
        /// </remarks>
        [NotMapped]
        public List<int> SkillIds
        {
            get => UserSkills.Select(us => us.SkillId).ToList();
            set => UserSkills = value.Select(v => new UserSkill() { SkillId = v }).ToList();
        }
    }
}
