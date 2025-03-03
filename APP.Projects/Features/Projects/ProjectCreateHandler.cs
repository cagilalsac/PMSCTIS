using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Projects
{
    public class ProjectCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, Length(5, 200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(400)]
        public string Url { get; set; }

        public double? Version { get; set; }

        //[Required] // to make tag ids not null, which means tag ids must have one or more elements
        public List<int> TagIds { get; set; }
    }

    public class ProjectCreateHandler : 
    {
    }
}
