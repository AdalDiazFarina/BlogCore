using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly DbContext Context;
        internal DbSet<T> dBSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.dBSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            dBSet.Add(entity);
        }

        public T Get(int id)
        {
            return dBSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            // Filter
            IQueryable<T> query = dBSet;
            if (filter != null) query = query.Where(filter);

            // Include Properties separados por comas
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);    
                }
            }

            // Order by
            if (orderBy != null) return orderBy(query).ToList();

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            // Filter
            IQueryable<T> query = dBSet;
            if (filter != null) query = query.Where(filter);

            // Include Properties separados por comas
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entityToRemove = dBSet.Find(id);
            //dBSet.Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            dBSet.Remove(entity);
        }
    }
}
