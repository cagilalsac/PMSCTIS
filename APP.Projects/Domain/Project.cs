using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Projects.Domain
{
    public class Project : Entity
    {
        [Required, Length(5, 200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(400)]
        public string Url { get; set; }

        public double? Version { get; set; } // float no1 = 1.2f, decimal no2 = 3.4m

        public List<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>(); // navigational property

        [NotMapped]
        public List<int> TagIds 
        { 
            get => ProjectTags.Select(pt => pt.TagId).ToList(); 
            set => ProjectTags = value.Select(v => new ProjectTag() { TagId = v }).ToList(); 
        }
    }
}
