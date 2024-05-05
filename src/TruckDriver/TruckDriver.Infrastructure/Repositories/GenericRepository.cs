using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;
using TruckDriver.Domain.IRepositories;
using TruckDriver.Infrastructure.Services.Contract;

namespace TruckDriver.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Container _container;

        public GenericRepository(ICosmosContainerService container)
        {
            _container = container.GetContainer();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                if(predicate is null)
                    throw new ArgumentNullException(nameof(predicate), "predicate in reposity is null!");

                using FeedIterator<T> queryIterator = _container.GetItemLinqQueryable<T>()
                                                                .Where(predicate)
                                                                .ToFeedIterator<T>();

                if(queryIterator is null)
                    throw new ArgumentNullException(nameof(queryIterator), "queryIterator in reposity is null!");

                var results = new List<T>();
                while (queryIterator.HasMoreResults)
                {
                    var response = await queryIterator.ReadNextAsync();
                    results.AddRange(response);
                }

                return results;
            }
            catch (Exception)
            {
                //Log Exception
                throw new InvalidOperationException("Query from cosmosdb failed!");
            }
        }
    }
}
