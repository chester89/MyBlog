using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Contracts;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;

namespace MyBlog.Data.Repositories
{
    public abstract class NhRepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly ISession Session;
        protected static bool autoCreateDb;

        static NhRepositoryBase()
        {
            autoCreateDb = false;

            if (autoCreateDb)
            {
                var configuration = NhConfigurationHelper.GetConfiguration();

                new SchemaExport(configuration).Create(true, true);
            }
        }

        protected NhRepositoryBase(ISession session)
        {
            Session = session;
        }

        public virtual T Get(int id)
        {
            return Session.Get<T>(id);
        }

        public virtual void Add(T newEntity)
        {
            Session.Save(newEntity);
        }

        public virtual void Update(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public virtual void Delete(T entity)
        {
            Session.Delete(entity);
            Session.Flush();
        }

        public virtual IQueryable<T> GetAll()
        {
            return Session.Query<T>().AsQueryable();
        }

        public virtual void Transaction(Action action)
        {
            if (Session.Transaction.IsActive)
            {
                action();
                return;
            }
            using (var transaction = Session.BeginTransaction())
            {
                action();
                transaction.Commit();
            }
        }
    }
}
