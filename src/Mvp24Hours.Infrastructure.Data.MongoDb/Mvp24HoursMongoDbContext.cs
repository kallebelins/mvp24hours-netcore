using MongoDB.Driver;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data.MongoDb
{
    public class Mvp24HoursMongoDbContext
    {
        public string DatabaseName { get; private set; }
        public string ConnectionString { get; private set; }
        public bool IsSSL { get; private set; }

        public virtual IMongoDatabase Database { get; private set; }

        public Mvp24HoursMongoDbContext(string databaseName)
            :this(databaseName, ConfigurationHelper.GetSettings("ConnectionStrings:MongoDbContext"), (bool)ConfigurationHelper.GetSettings("ConnectionStrings:MongoDbSSL").ToBoolean(false))
        {
        }

        public Mvp24HoursMongoDbContext(string databaseName, string connectionString, bool isSSL = false)
        {
            if (!databaseName.HasValue())
            {
                throw new ArgumentNullException("Database name is required.");
            }

            if (!connectionString.HasValue())
            {
                throw new ArgumentNullException("ConnectionString is required.");
            }

            DatabaseName = databaseName;
            ConnectionString = connectionString;
            IsSSL = isSSL;

            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
                if (IsSSL)
                {
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                }
                var mongoClient = new MongoClient(settings);
                Database = mongoClient.GetDatabase(DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to connect to server.", ex);
            }
        }

        public IMongoCollection<TEntity> Set<TEntity>()
        {
            return Set<TEntity>(typeof(TEntity).Name);
        }

        public IMongoCollection<TEntity> Set<TEntity>(string name)
        {
            return Database.GetCollection<TEntity>(name);
        }
    }
}
