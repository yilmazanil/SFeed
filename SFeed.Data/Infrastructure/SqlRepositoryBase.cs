using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SFeed.Data.Infrastructure
{
    public abstract class SqlRepositoryBase<T>: ISqlRepository<T> where T : class
    {
        private SFeedEntities dataContext;
        private readonly IDbSet<T> dbSet;

        protected SFeedEntities DbContext
        {
            get { return dataContext ?? (dataContext = new SFeedEntities()); }
        }

        protected SqlRepositoryBase()
        {
            dbSet = DbContext.Set<T>();
        }

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
            }
        }
    }
}
