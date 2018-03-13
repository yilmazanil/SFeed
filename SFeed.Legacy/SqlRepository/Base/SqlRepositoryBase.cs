using SFeed.Core.Infrastructue.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SFeed.SqlRepository
{
    public abstract class SqlRepositoryBase<T> : IRepository<T> where T : class
    {
        private SocialFeedEntities dataContext;
        private IDbSet<T> dbSet;


        protected IDbSet<T> DbSet
        {
            get
            {
                if (dbSet != null)
                {
                    return dbSet;
                }
                else
                {
                    dbSet = DbContext.Set<T>();
                    return dbSet;
                }
            }
        }

        protected SocialFeedEntities DbContext
        {
            get { return dataContext ?? (dataContext = new SocialFeedEntities()); }
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = DbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                DbSet.Remove(obj);
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault<T>();
        }

        public bool Any(Expression<Func<T, bool>> where)
        {
            return DbSet.Any(where);
        }

        public void CommitChanges()
        {
            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }

            }
        }
    }
}
