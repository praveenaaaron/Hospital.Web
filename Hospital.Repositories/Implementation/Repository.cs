using Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories.Implementation
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbset = _context.Set<T>();
        }
        public  void Add(T entity)
        {
            dbset.Add(entity);
        }
        public async Task<T>AddAsync(T entity)
        {
            dbset.Add(entity);
            return entity;
        }
        public void Delete (T entity)
        {
        if(_context.Entry(entity).State==EntityState.Detached)
            {
                dbset.Attach(entity);
            }
            dbset.Remove(entity);
         }
        public async Task<T>DeleteAsync(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbset.Attach(entity);
            }
            dbset.Remove(entity);
            return entity;
        }
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  
        }
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed= true;
        }
        
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }
        public T GetById(object id)
        {
            return dbset.Find(id);
        }
        public async Task<T> GetByIdAsync(object id)
        {
            return await dbset.FindAsync(id);       
        }
        public void Update(T entity)
        {
            dbset.Attach(entity);
            _context.Entry(entity).State= EntityState.Modified;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            dbset.Attach(entity);
_context.Entry(entity).State= EntityState.Modified;
            return entity;
        }

      
    }

}

