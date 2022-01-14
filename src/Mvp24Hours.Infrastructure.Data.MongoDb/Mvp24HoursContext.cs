//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using MongoDB.Driver;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data.MongoDb
{
    /// <summary>
    /// 
    /// </summary>
    public class Mvp24HoursContext : IDisposable
    {
        public string DatabaseName { get; private set; }
        public string ConnectionString { get; private set; }
        public bool EnableTls { get; private set; }
        public bool EnableTransaction { get; private set; }

        public virtual IMongoDatabase Database { get; private set; }
        public MongoClient MongoClient { get; private set; }

        public IClientSessionHandle Session { get; private set; }

        private bool _isTransactionAsync;


        public Mvp24HoursContext(string databaseName)
            : this(databaseName,
                  ConfigurationHelper.GetSettings("ConnectionStrings:MongoDbContext"),
                  (bool)ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:MongoDb:EnableTls").ToBoolean(false),
                  (bool)ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:MongoDb:EnableTransaction").ToBoolean(false))
        {
        }

        public Mvp24HoursContext(string databaseName, string connectionString)
            : this(databaseName,
                  connectionString,
                  (bool)ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:MongoDb:EnableTls").ToBoolean(false),
                  (bool)ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:MongoDb:EnableTransaction").ToBoolean(false))
        {
        }

        public Mvp24HoursContext(string databaseName, string connectionString, bool enableTls)
            : this(databaseName,
                  connectionString,
                  enableTls,
                  (bool)ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:MongoDb:EnableTransaction").ToBoolean(false))
        {
        }

        public Mvp24HoursContext(string databaseName, string connectionString, bool enableTls, bool enableTransaction)
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
            EnableTls = enableTls;
            EnableTransaction = enableTransaction;

            try
            {
                MongoClientSettings settings = MongoClientSettings.FromConnectionString(ConnectionString);
                if (EnableTls)
                {
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                }
                MongoClient = new MongoClient(settings);
                Database = MongoClient.GetDatabase(DatabaseName);
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

        public void StartSession(CancellationToken cancellationToken = default)
        {
            if (Session != null)
            {
                throw new ArgumentException("Session has already started.");
            }
            Session = MongoClient.StartSession(cancellationToken: cancellationToken);
            if (EnableTransaction)
            {
                Session.StartTransaction();
            }
        }

        public void SaveChanges(CancellationToken cancellationToken = default)
        {
            if (Session != null && Session.IsInTransaction)
            {
                Session.CommitTransaction(cancellationToken);
            }
        }

        public void Rollback(CancellationToken cancellationToken = default)
        {
            if (Session != null && Session.IsInTransaction)
            {
                Session.AbortTransaction(cancellationToken);
            }
        }

        public async Task StartSessionAsync(CancellationToken cancellationToken = default)
        {
            if (Session != null)
            {
                throw new ArgumentException("Session has already started.");
            }
            Session = await MongoClient.StartSessionAsync(cancellationToken: cancellationToken);
            if (EnableTransaction)
            {
                Session.StartTransaction();
                this._isTransactionAsync = true;
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (Session != null && Session.IsInTransaction)
            {
                await Session.CommitTransactionAsync(cancellationToken);
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (Session != null && Session.IsInTransaction)
            {
                await Session.AbortTransactionAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            while (Session != null && Session.IsInTransaction)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }

            if (Session != null)
            {
                if (Session.IsInTransaction)
                {
                    if (this._isTransactionAsync)
                    {
                        Session.CommitTransactionAsync();
                    }
                    else
                    {
                        Session.CommitTransaction();
                    }
                }
                Session.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
