using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories.Interfaces
{
   public interface IRepository<T>:IDisposable
   
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T GetByid(object id);
        Task<T> GetByAsync(object id);
        void Add(T entity);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        Task<T> UpdateAsync(T entity);
        void DeleteAdd(T entity);
        Task<T> DeleteAddAsync(T entity);

    }
}
