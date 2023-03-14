using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using system_backend.Models.Dtos;

namespace system_backend.Repository.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        Task CreateAsync(T entity);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task RemoveAsync(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
