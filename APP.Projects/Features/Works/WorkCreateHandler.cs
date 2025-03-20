using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.Projects.Features.Works
{
    public class WorkCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(300)]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public int? ProjectId { get; set; }

        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }
    }

    public class WorkCreateHandler : ProjectsDbHandler, IRequestHandler<WorkCreateRequest, CommandResponse>
    {
        public WorkCreateHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public async Task<CommandResponse> Handle(WorkCreateRequest request, CancellationToken cancellationToken)
        {
            if (request.DueDate < request.StartDate)
                return Error("Due date must be later or equal to start date!");
            if (await _projectsDb.Works.AnyAsync(w => w.Name.ToUpper() == request.Name.ToUpper().Trim()))
                return Error("Work with the same name exists!");
            var entity = new Work()
            {
                Description = request.Description?.Trim(),
                DueDate = request.DueDate,
                Name = request.Name?.Trim(),
                ProjectId = request.ProjectId,
                StartDate = request.StartDate
            };
            _projectsDb.Works.Add(entity);
            await _projectsDb.SaveChangesAsync(cancellationToken);
            return Success("Work created successfully.", entity.Id);
        }
    }
}
