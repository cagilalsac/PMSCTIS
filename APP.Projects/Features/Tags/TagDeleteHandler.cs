using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Projects.Features.Tags
{
    public class TagDeleteRequest : Request, IRequest<CommandResponse>
    { 
    }

    public class TagDeleteHandler : ProjectsDbHandler, IRequestHandler<TagDeleteRequest, CommandResponse>
    {
        public TagDeleteHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(TagDeleteRequest request, CancellationToken cancellationToken)
        {
            Tag tag = await _projectsDb.Tags.SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (tag is null)
                return Error("Tag not found!");

            // Way 1:
            //_projectsDb.Entry(tag).State = EntityState.Deleted;
            // Way 2:
            //_projectsDb.Remove(tag);
            // Way 3:
            _projectsDb.Tags.Remove(tag);

            await _projectsDb.SaveChangesAsync(cancellationToken);

            return Success("Tag deleted successfully.");
        }
    }
}
