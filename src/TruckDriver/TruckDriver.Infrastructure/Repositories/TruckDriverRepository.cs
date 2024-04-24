using Microsoft.Azure.Cosmos;
using TruckDriver.Domain.IRepositories;
using TruckDriver.Domain.Models;

namespace TruckDriver.Infrastructure.Repositories
{
    public class TruckDriverRepository(Container container) : ITruckDriverRepository
    {
        private readonly Container _container = container; 
        private const string LocationParameter = "@location";
        private const string QueryTemplate = "SELECT * FROM c WHERE LOWER(c.location) = LOWER({0})";

        public async Task<IEnumerable<Driver>> GetAsync(string location)
        {
            var query = new QueryDefinition(string.Format(QueryTemplate, LocationParameter))
                                                    .WithParameter(LocationParameter, location);

            var queryResultSetIterator = _container.GetItemQueryIterator<Driver>(query);
            if (queryResultSetIterator is null)
                throw new ArgumentNullException(nameof(queryResultSetIterator), "Created query in Azure cosmosdb is null!");

            var drivers = new List<Driver>();
            while (queryResultSetIterator.HasMoreResults)
            {
                var response = await queryResultSetIterator.ReadNextAsync();
                drivers.AddRange(response);
            }

            return drivers;
        }
    }
}
