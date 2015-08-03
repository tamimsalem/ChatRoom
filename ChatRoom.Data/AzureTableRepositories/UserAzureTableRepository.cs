using ChatRoom.Entities;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

namespace ChatRoom.Data.AzureTableRepositories
{
    public class UserTableEntity : TableEntity
    {
        [IgnoreProperty]
        public string Id
        {
            get
            {
                return this.RowKey;
            }
            set
            {
                this.RowKey = value;
            }
        }
        public string Name { get; set; }

        public UserTableEntity(string Id)
        {
            this.PartitionKey = "none";
            this.RowKey = Id;
        }

        public UserTableEntity()
        {
            this.PartitionKey = "none";
        }
    }


    public class UserAzureTableRepository : IUserPersister
    {
        private const string TABLE_NAME = "onlineusers";

        private static CloudStorageAccount _storageAccount;
        private static CloudTableClient _tableClient;
        private static CloudTable _table;

        private bool _initialized;

        public UserAzureTableRepository()
        {
            _initialized = false;
        }
            
        public void Add(User entity)
        {
            if(!_initialized)
            {
                Initialize();
            }

            var tableEntity = new UserTableEntity(entity.Id)
            {
                Name = entity.Name
            };

            TableOperation insertOperation = TableOperation.Insert(tableEntity);

            _table.Execute(insertOperation);
        }

        private void Initialize()
        {
            string connectionValue = string.Empty;

            if (RoleEnvironment.IsAvailable)
            {
                connectionValue = RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString");
            }
            else
            {
                connectionValue = ConfigurationManager.AppSettings["StorageConnectionString"];
            }

            _storageAccount = CloudStorageAccount.Parse(connectionValue);

            _tableClient = _storageAccount.CreateCloudTableClient();

            _table = _tableClient.GetTableReference(TABLE_NAME);

            _table.CreateIfNotExists();

            _initialized = true;
        }

        public void Delete(User entity)
        {
            if (!_initialized)
            {
                Initialize();
            }

            DynamicTableEntity pattern = new DynamicTableEntity()
            {
                RowKey = entity.Id,
                PartitionKey = "none",
                ETag = "*"
            };

            TableOperation deleteOperation = TableOperation.Delete(pattern);

            _table.Execute(deleteOperation);
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            if (!_initialized)
            {
                Initialize();
            }

            TableQuery<UserTableEntity> query = new TableQuery<UserTableEntity>();

            var result = _table.ExecuteQuery<UserTableEntity>(query);

            return result.Select(u => new User()
            {
                Id = u.Id,
                Name = u.Name
            }).ToList();
        }
    }
}
