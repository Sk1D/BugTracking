using BugTrackingApp.Domain.Core;
using BugTrackingApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BugTrackingApp.Controllers
{
    [ApiController]
    [Route("api/issue")]
    public class IssueController : Controller
    {
        private IRepository<Issue> _issueRepository;
        public IssueController(IRepository<Issue> repository)
        {
            _issueRepository = repository;
        }
        [HttpGet]
        public IEnumerable<Issue> Get()
        {
            return _issueRepository.GetAll();
        }

        [HttpGet("{id}")]
        public Issue Get(int id)
        {
            var issue = _issueRepository.Get(id);
            return issue;
        }
    }
}
