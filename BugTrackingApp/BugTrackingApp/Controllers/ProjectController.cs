using BugTrackingApp.Domain.Core;
using BugTrackingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BugTrackingApp.Controllers
{
    [ApiController]
    [Route("api/project")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpGet]
        public IEnumerable<Project> Get()
        {
            return _projectService.GetProjects();
        }

        [HttpGet("{id}")]
        public Project Get(int id)
        {
            Project project = _projectService.GetProjectById(id);
            return project;
        }

        [HttpPost]
        public IActionResult Post(Project project)
        {
            if (ModelState.IsValid)
            {
                _projectService.AddProject(project);
                return Ok(project);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put(Project project)
        {
            if (ModelState.IsValid)
            {
                _projectService.UpdateProject(project);
                return Ok(project);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Project project = _projectService.GetProjectById(id);
            if (project != null)
            {
                _projectService.DeleteProject(id);
            }
            return Ok(project);
        }
    }
}
