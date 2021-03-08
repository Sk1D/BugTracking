using BugTrackingApp.Domain.Core;
using BugTrackingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace BugTrackingApp.Infrastructure.Data
{
    public class IssueRepository : AdoRepository<Issue>, IRepository<Issue>
    {
        public IssueRepository(string connectionString) : base(connectionString) { }
        public void Create(Issue value)
        {
            var builder = new SqlQueryBuilder<Issue>(value);
            ExecuteCommand(builder.GetInsertCommand());
            // если мы хотим получить id добавленного пользователя
            // var sqlQuery = "INSERT INTO Products (Name, Age) VALUES(@Name, @Age); SELECT CAST(SCOPE_IDENTITY() as int)";
            //  int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
            // user.Id = userId.Value;
        }

        public void Delete(int id)
        {
            var issue = Get(id);
            if (issue != null)
            {
                var builder = new SqlQueryBuilder<Issue>(issue);
                ExecuteCommand(builder.GetDeleteCommand());
            }
        }

        public IEnumerable<Issue> Find(Func<Issue, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Issue Get(int id)
        {
            using (var command = new SqlCommand("SELECT * FROM Issues WHERE Id = @id"))
            {
                command.Parameters.Add(GetParameter("id", id));
                return GetRecord(command);
            }
        }

        public IEnumerable<Issue> GetAll()
        {
            using (var command = new SqlCommand("SELECT * FROM Issues"))
            {
                return GetRecords(command);
            }
        }

        public void Update(Issue value)
        {
            var builder = new SqlQueryBuilder<Issue>(value);
            ExecuteCommand(builder.GetUpdateCommand());
        }

        public override Issue PopulateRecord(SqlDataReader reader)
        {
            return new Issue
            {

                Id = reader.GetInt32(0),
                // Project = reader.GetString(1),
                Summary = reader["summary"]?.ToString(),
                Title = reader["title"]?.ToString()
            };
        }
    }
}
