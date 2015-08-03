using ChatRoom.Entities;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Configuration;

namespace ChatRoom.Data.AzureTableRepositories
{
    public class ChatMessageTableEntity : TableEntity
    {
        [IgnoreProperty]
        public string Name
        {
            get
            {
                return this.PartitionKey;
            }
            set
            {
                this.PartitionKey = value;
            }
        }

        public string Message { get; set; }

        public ChatMessageTableEntity(string name)
        {
            this.PartitionKey = name;
            this.RowKey = Guid.NewGuid().ToString();
        }
    }


    public class ChatMessageAzureTableRepository : IChatMessagePersister
    {
        private const string TABLE_NAME = "chatmessages";

        private static CloudStorageAccount _storageAccount;
        private static CloudTableClient _tableClient;
        private static CloudTable _table;

        private bool _initialized;

        public ChatMessageAzureTableRepository()
        {
            _initialized = false;
        }
            
        public void Add(ChatMessage entity)
        {
            if(!_initialized)
            {
                Initialize();
            }

            var tableEntity = new ChatMessageTableEntity(entity.Name)
            {
                Message = entity.Message
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

        public void Delete(ChatMessage entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ChatMessage entity)
        {
            throw new NotImplementedException();
        }
    }
}
