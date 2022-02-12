using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {

        
        public CatalogContext(IConfiguration configuration)
        {
            var databaseName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
            var collectionName = configuration.GetValue<string>("DatabaseSettings:CollectionName");

            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(databaseName);

            Products = database.GetCollection<Product>(collectionName);
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}