using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Projects.Features.Tags
{
    public class TagQueryRequest : Request, IRequest<IQueryable<TagQueryResponse>>
    {
    }

    public class TagQueryResponse : QueryResponse
    {
        public string Name { get; set; }
    }

    public class TagQueryHandler : ProjectsDbHandler, IRequestHandler<TagQueryRequest, IQueryable<TagQueryResponse>>
    {
        public TagQueryHandler(ProjectsDb projectsDb) : base(projectsDb)
        {
        }

        public Task<IQueryable<TagQueryResponse>> Handle(TagQueryRequest request, CancellationToken cancellationToken)
        {
            // select Id, Name from Tags
            return Task.FromResult(_projectsDb.Tags.OrderBy(tag => tag.Name).Select(tag => new TagQueryResponse()
            {
                Id = tag.Id,
                Name = tag.Name
            }));
        }
    }
}
