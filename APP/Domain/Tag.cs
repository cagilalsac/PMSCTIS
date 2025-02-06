using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class Tag : Entity
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
    }
}
