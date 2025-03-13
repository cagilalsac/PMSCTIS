using APP.Projects.Domain;
using APP.Projects.Features.Projects;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Projects.Features.Works
{
    public class WorkQueryRequest : Request, IRequest<IQueryable<WorkQueryResponse>>
    {
        public string Name { get; set; }
    }

    public class WorkQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateF { get; set; }
        public DateTime DueDate { get; set; }
        public string DueDateF { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ProjectQueryResponse ProjectQueryResponse { get; set; }
    }

    public class WorkQueryHandler : ProjectsDbHandler, IRequestHandler<WorkQueryRequest, IQueryable<WorkQueryResponse>>
    {
        public WorkQueryHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public Task<IQueryable<WorkQueryResponse>> Handle(WorkQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _projectsDb.Works.Include(w => w.Project).OrderByDescending(w => w.DueDate).ThenByDescending(w => w.StartDate).ThenBy(w => w.Name).Select(w => new WorkQueryResponse()
            {
                Name = w.Name,
                Description = w.Description,
                DueDate = w.DueDate,
                DueDateF = w.DueDate.ToString("MM/dd/yyyy HH:mm:ss"),
                StartDate = w.StartDate,
                Id = w.Id,

                // Way 1:
                //StartDateF = w.StartDate.ToString("MM/dd/yyyy HH:mm:ss"),
                // Way 2:
                StartDateF = w.StartDate.ToShortDateString(),

                ProjectId = w.ProjectId,
                ProjectName = w.Project.Name,

                ProjectQueryResponse = w.Project != null ? new ProjectQueryResponse()
                {
                    Description = w.Project.Description,
                    Id = w.Project.Id,
                    Name = w.Project.Name,
                    TagIds = w.Project.TagIds,
                    Url = w.Project.Url,
                    Version = w.Project.Version,
                    VersionF = w.Project.Version.HasValue ? w.Project.Version.Value.ToString("N1") : string.Empty
                } : null
            });

            // Filtering
            // Way 1:
            //if (request.Name != null && request.Name != "")
            // Way 2:
            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(w => w.Name == request.Name);

            return Task.FromResult(query);
        }
    }
}
