using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Tags
{
    public class TagCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
    }

    public class TagCreateHandler : ProjectsDbHandler, IRequestHandler<TagCreateRequest, CommandResponse>
    {
        public TagCreateHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(TagCreateRequest request, CancellationToken cancellationToken)
        {
            // TODO: add name check!

            Tag tag = new Tag()
            {
                Name = request.Name.Trim()
            };
            _projectsDb.Tags.Add(tag);
            await _projectsDb.SaveChangesAsync(cancellationToken); // unit of work

            // don't do this way, use the Success and Error methods in the Handler base class
            //return new CommandResponse(true, "Tag created successfully.", tag.Id);
            return Success("Tag created successfully.", tag.Id);
        }
    }
}
