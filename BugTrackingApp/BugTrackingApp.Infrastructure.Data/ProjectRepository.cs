using BugTrackingApp.Domain.Core;
using BugTrackingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BugTrackingApp.Infrastructure.Data
{
    // TODO use sql procedure
    public class ProjectRepository : AdoRepository<Project>, IRepository<Project>
    {
        public ProjectRepository(string connectionString) : base(connectionString) { }

        public void Create(Project value)
        {
            var builder = new SqlQueryBuilder<Project>(value);
            ExecuteCommand(builder.GetInsertCommand());
        }

        public void Delete(int id)
        {
            var project = Get(id);
            if (project != null)
            {
                var builder = new SqlQueryBuilder<Project>(project);
                ExecuteCommand(builder.GetDeleteCommand());
            }
        }

        public IEnumerable<Project> Find(Func<Project, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Project Get(int id)
        {
            using (var command = new SqlCommand("SELECT * FROM Projects WHERE Id = @id"))
            {
                command.Parameters.Add(GetParameter("id", id));
                return GetRecord(command);
            }
        }

        public IEnumerable<Project> GetAll()
        {
            using (var command = new SqlCommand("SELECT * FROM Projects"))
            {
                return GetRecords(command);
            }
        }

        // using Dapper
        //public IEnumerable<Issue> GetAll()
        //{
        //    using (IDbConnection db = new SqlConnection(_connectionString))
        //    {
        //        var data = db.Query<Issue, Project, Issue>(
        //            "SELECT * FROM Issues INNER JOIN Projects ON Issues.ProjectId = Projects.Id",
        //            map: (issue, project) =>
        //            {
        //                issue.Project = project;
        //                return issue;
        //            },
        //            splitOn: "Id"
        //        );
        //        return data;
        //    }
        //}

        public void Update(Project value)
        {
            var builder = new SqlQueryBuilder<Project>(value);
            ExecuteCommand(builder.GetUpdateCommand());
            //using (IDbConnection db = new SqlConnection(_connectionString))
            //{
            //    var sqlQuery = "UPDATE Projects SET Name = @Name WHERE Id = @Id"; //TODO
            //    db.Execute(sqlQuery, value);
            //}
        }

        public override Project PopulateRecord(SqlDataReader reader)
        {
            return new Project
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }
    }
}
