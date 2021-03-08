using BugTrackingApp.Domain.Core;
using BugTrackingApp.Domain.Interfaces;
using BugTrackingApp.Services.Interfaces;
using System.Collections.Generic;

namespace BugTrackingApp.Infrastructure.Business
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;

        public ProjectService(IRepository<Project> repository)
        {
            _projectRepository = repository;
        }
        public void AddProject(Project project)
        {
            _projectRepository.Create(project);
        }

        public void DeleteProject(int id)
        {
            _projectRepository.Delete(id);
        }

        public Project GetProjectById(int id)
        {
            Project project = _projectRepository.Get(id);
            return project;
        }

        public IEnumerable<Project> GetProjects()
        {
            return _projectRepository.GetAll();
        }

        public void UpdateProject(Project project)
        {
            _projectRepository.Update(project);
        }
    }
}
