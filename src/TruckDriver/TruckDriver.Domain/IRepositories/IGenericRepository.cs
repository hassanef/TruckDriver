using System.Linq.Expressions;

namespace TruckDriver.Domain.IRepositories
{
    public interface IGenericRepository<T>  where T : class
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
    }
}
