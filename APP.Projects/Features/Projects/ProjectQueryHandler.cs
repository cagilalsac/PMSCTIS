using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Projects.Features.Projects
{
    public class ProjectQueryRequest : Request, IRequest<IQueryable<ProjectQueryResponse>>
    {
    }

    public class ProjectQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public double? Version { get; set; }
        public string VersionF { get; set; }
    }

    public class ProjectQueryHandler : ProjectsDbHandler, IRequestHandler<ProjectQueryRequest, IQueryable<ProjectQueryResponse>>
    {
        public ProjectQueryHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public Task<IQueryable<ProjectQueryResponse>> Handle(ProjectQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_projectsDb.Projects.OrderBy(p => p.Name).ThenByDescending(p => p.Version).Select(p => new ProjectQueryResponse() 
            // optional: AutoMapper can be used for mapping operations
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Url = p.Url,
                Version = p.Version,
                VersionF = p.Version.HasValue ? p.Version.Value.ToString("N1") : string.Empty
            }));
        }
    }
}
