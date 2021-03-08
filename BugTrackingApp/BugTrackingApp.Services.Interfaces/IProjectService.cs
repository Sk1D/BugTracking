using BugTrackingApp.Domain.Core;
using System.Collections.Generic;

namespace BugTrackingApp.Services.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjects();
        Project GetProjectById(int id);
        void AddProject(Project project);
        void UpdateProject(Project project);
        void DeleteProject(int id);
    }
}
