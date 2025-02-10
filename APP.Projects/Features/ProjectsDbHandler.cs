using APP.Projects.Domain;
using CORE.APP.Features;
using System.Globalization;

namespace APP.Projects.Features
{
    public abstract class ProjectsDbHandler : Handler
    {
        protected readonly ProjectsDb _projectsDb;

        public ProjectsDbHandler(ProjectsDb projectsDb) : base(new CultureInfo("en-US"))
        {
            _projectsDb = projectsDb;
        }
    }
}
