using System;
using System.Collections.Generic;

namespace BugTrackingApp.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Create(T value);
        void Update(T value);
        void Delete(int id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
    }
}
