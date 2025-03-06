using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Projects.Features.Projects
{
    public class ProjectDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class ProjectDeleteHandler : ProjectsDbHandler, IRequestHandler<ProjectDeleteRequest, CommandResponse>
    {
        public ProjectDeleteHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(ProjectDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _projectsDb.Projects.Include(p => p.ProjectTags).SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Project not found!");

            _projectsDb.ProjectTags.RemoveRange(entity.ProjectTags);

            _projectsDb.Projects.Remove(entity);
            await _projectsDb.SaveChangesAsync(cancellationToken);

            return Success("Project deleted successfully.", entity.Id);
        }
    }
}
