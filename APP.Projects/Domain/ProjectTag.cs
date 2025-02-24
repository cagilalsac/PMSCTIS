using CORE.APP.Domain;

namespace APP.Projects.Domain
{
    public class ProjectTag : Entity
    {
        public int ProjectId { get; set; } // foreign key
        public Project Project { get; set; } // navigational property

        public int TagId { get; set; } // foreign key
        public Tag Tag { get; set; } // navigational property
    }
}
