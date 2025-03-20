using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Works
{
    public class WorkUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(300)]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public int? ProjectId { get; set; }
    }

    public class WorkUpdateHandler : ProjectsDbHandler, IRequestHandler<WorkUpdateRequest, CommandResponse>
    {
        public WorkUpdateHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(WorkUpdateRequest request, CancellationToken cancellationToken)
        {
            if (request.DueDate < request.StartDate)
                return Error("Due date must be later or equal to start date!");
            if (await _projectsDb.Works.AnyAsync(w => w.Id != request.Id && w.Name.ToUpper() == request.Name.ToUpper().Trim()))
                return Error("Work with the same name exists!");
            var entity = await _projectsDb.Works.SingleOrDefaultAsync(w => w.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Work not found!");
            entity.Description = request.Description?.Trim();
            entity.DueDate = request.DueDate;
            entity.Name = request.Name?.Trim();
            entity.ProjectId = request.ProjectId;
            entity.StartDate = request.StartDate;
            _projectsDb.Works.Update(entity);
            await _projectsDb.SaveChangesAsync(cancellationToken);
            return Success("Work updated successfully.", entity.Id);
        }
    }
}
