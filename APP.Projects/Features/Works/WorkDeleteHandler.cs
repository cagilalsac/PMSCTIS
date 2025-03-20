using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Projects.Features.Works
{
    public class WorkDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class WorkDeleteHandler : ProjectsDbHandler, IRequestHandler<WorkDeleteRequest, CommandResponse>
    {
        public WorkDeleteHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(WorkDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _projectsDb.Works.SingleOrDefaultAsync(w => w.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Work not found!");
            _projectsDb.Works.Remove(entity);
            await _projectsDb.SaveChangesAsync(cancellationToken);
            return Success("Work deleted successfully.", entity.Id);
        }
    }
}
