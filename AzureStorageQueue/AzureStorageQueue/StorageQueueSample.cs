using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureStorageQueue
{
    public class StorageQueueSample
    {
        private readonly AppOptions _appOptions;
/*
        private const string QueueName = "samplequeue";
*/

        public StorageQueueSample()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", true, true);

            var settingsCache = builder.Build();
            _appOptions = new AppOptions();
            settingsCache.Bind(_appOptions);
        }
        public static async Task Run()
        {


            await CreateQueue();


            Console.WriteLine("Press a key to exit.");
            Console.ReadLine();
        }


        public static async Task<bool> CreateQueue()
        {
            string name = "demotest-" + Guid.NewGuid().ToString();
            var connectionString = new StorageQueueSample()._appOptions.ConnectionString;
            var client = GetQueueClient(connectionString);
            CloudQueue queue = client.GetQueueReference(name);

            try
            {
                await queue.CreateIfNotExistsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return true;

        }

        public static CloudQueueClient GetQueueClient(string connectionString)
        {
      
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient client = account.CreateCloudQueueClient();
            return client;
        }
    }
}
