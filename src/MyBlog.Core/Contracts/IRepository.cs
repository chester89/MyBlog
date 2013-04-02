using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyBlog.Core.Contracts
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        void Add(T newEntity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> Get(Expression<Func<T, bool>> filter);
        IQueryable<T> Take(int count);
        void Transaction(Action action);
    }
}
