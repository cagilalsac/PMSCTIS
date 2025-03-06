using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Projects
{
    public class ProjectUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, Length(5, 200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(400)]
        public string Url { get; set; }

        [Range(0, double.MaxValue)]
        public double? Version { get; set; }

        public List<int> TagIds { get; set; }
    }

    public class ProjectUpdateHandler : ProjectsDbHandler, IRequestHandler<ProjectUpdateRequest, CommandResponse>
    {
        public ProjectUpdateHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(ProjectUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await _projectsDb.Projects.AnyAsync(p => p.Id != request.Id && p.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
                return Error("Project with the same name exists!");

            // Way 1:
            //var entity = new Project()
            //{
            //    Id = request.Id,
            //    Name = request.Name.Trim(),
            //    Description = request.Description?.Trim(),
            //    Url = request.Url?.Trim(),
            //    Version = request.Version,
            //    TagIds = request.TagIds
            //};
            // Way 2:
            //var entity = await _projectsDb.Projects.FindAsync(request.Id, cancellationToken);
            // Way 3:
            var entity = await _projectsDb.Projects.Include(p => p.ProjectTags).SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Project not found!");

            _projectsDb.ProjectTags.RemoveRange(entity.ProjectTags);

            entity.Name = request.Name.Trim();
            entity.Description = request.Description?.Trim();
            entity.Url = request.Url?.Trim();
            entity.Version = request.Version;
            entity.TagIds = request.TagIds;

            _projectsDb.Projects.Update(entity);
            await _projectsDb.SaveChangesAsync(cancellationToken);

            return Success("Projects updated successfully.", entity.Id);
        }
    }
}
