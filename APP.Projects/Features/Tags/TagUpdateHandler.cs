using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace APP.Projects.Features.Tags
{
    public class TagUpdateRequest : Request, IRequest<CommandResponse>
    {
        // Tag name property - required and with a maximum length of 150 characters,
        // minimum length of 3 characters
        [Required, MaxLength(150), MinLength(3)]
        public string Name { get; set; }
    }

    public class TagUpdateHandler : ProjectsDbHandler, IRequestHandler<TagUpdateRequest, CommandResponse>
    {
        public TagUpdateHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(TagUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await _projectsDb.Tags.AnyAsync(t => t.Id != request.Id && t.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
                return Error("Tag with the same name exists!");

            // Way 1:
            //Tag tag = await _projectsDb.Tags.FindAsync(request.Id, cancellationToken);
            // Way 2:
            Tag tag = await _projectsDb.Tags.SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (tag is null)
                return Error("Tag not found!");

            tag.Name = request.Name.Trim();

            // Way 1: does not update relational data
            //_projectsDb.Entry(tag).State = EntityState.Modified;
            // Way 2:
            //_projectsDb.Update(tag);
            // Way 3:
            _projectsDb.Tags.Update(tag);

            await _projectsDb.SaveChangesAsync(cancellationToken);

            return Success("Tag updated successfully.", tag.Id);
        }
    }
}
