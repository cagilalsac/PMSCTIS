using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Projects
{
    public class ProjectCreateRequest : Request, IRequest<CommandResponse>
    {
        // MVC Fluent Validation: third party validation library
        [Required, Length(5, 200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(400)]
        public string Url { get; set; }

        // Way 2:
        [Range(0, double.MaxValue)]
        public double? Version { get; set; }

        //[Required] // to make tag ids not null, which means tag ids must have one or more elements
        public List<int> TagIds { get; set; }
    }

    public class ProjectCreateHandler : ProjectsDbHandler, IRequestHandler<ProjectCreateRequest, CommandResponse>
    {
        public ProjectCreateHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(ProjectCreateRequest request, CancellationToken cancellationToken)
        {
            if (await _projectsDb.Projects.AnyAsync(p => p.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
                return Error("Project with the same name exists!");

            // Way 1: this validation should be made in the request by using Range data annotation
            //if (request.Version < 0)
            //    return Error("Version must be a positive decimal!");

            var entity = new Project()
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                Url = request.Url?.Trim(),
                Version = request.Version,
                TagIds = request.TagIds
            };

            _projectsDb.Projects.Add(entity);
            await _projectsDb.SaveChangesAsync(cancellationToken);

            return Success("Projects created successfully.", entity.Id);
        }
    }
}
