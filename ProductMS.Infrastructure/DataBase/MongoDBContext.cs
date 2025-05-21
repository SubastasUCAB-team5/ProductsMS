using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProductMS.Domain.Entities;

namespace ProductMS.Infrastructure.DataBase
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDb:ConnectionString"];
            var dbName = configuration["MongoDb:Database"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }

        public IMongoCollection<ProductReadModel> Products => _database.GetCollection<ProductReadModel>("Products");
    }
}
