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

        private const string QueueName = "samplequeue";


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
            var connectionString = new StorageQueueSample()._appOptions.ConnectionString;

            await CreateQueue();

            await BasicQueueOperation(connectionString);
            await UpdateQueueMessage(connectionString);
            await DequeueQueue(connectionString);

            Console.WriteLine("Press a key to exit.");
            Console.ReadLine();
        }

        private static async Task BasicQueueOperation(string connectionString)
        {
            var queueClient = GetQueueClient(connectionString);
            CloudQueue queue = queueClient.GetQueueReference(QueueName);
            for (int i = 0; i < 5; i++)
            {
                await queue.AddMessageAsync(new CloudQueueMessage("Hello World" + "\t" + i.ToString()));
            }
            Console.WriteLine("Peek at the next message");
            CloudQueueMessage queueMessage = await queue.PeekMessageAsync();
            if (queueMessage != null)
            {
                Console.WriteLine("The peeked message is: {0}", queueMessage.AsString);
            }

            Console.WriteLine("De-queue the next message");
            CloudQueueMessage cloudQueueMessage = await queue.GetMessageAsync();
            if (cloudQueueMessage != null)
            {
                Console.WriteLine("Processing & deleting message with content: {0}", cloudQueueMessage.AsString);
                await queue.DeleteMessageAsync(cloudQueueMessage);
            }
        }

        private static async Task UpdateQueueMessage(string connectionString)
        {
            var queueClient = GetQueueClient(connectionString);
            CloudQueue queue = queueClient.GetQueueReference(QueueName);

            Console.WriteLine("De-queue the next message");
            CloudQueueMessage cloudQueueMessage = await queue.GetMessageAsync();

            if (cloudQueueMessage != null)
            {
                cloudQueueMessage.SetMessageContent("Content Updated");
                await queue.UpdateMessageAsync(cloudQueueMessage, TimeSpan.Zero,
                    MessageUpdateFields.Content | MessageUpdateFields.Visibility);


            }
        }

        public static async Task<bool> CreateQueue()
        {
            string name = "demotest-" + Guid.NewGuid().ToString();
            var connectionString = new StorageQueueSample()._appOptions.ConnectionString;
            var client = GetQueueClient(connectionString);

            // Retrieve a reference to a queue.
            CloudQueue queue = client.GetQueueReference(name);

            try
            {
                // Create the queue if it doesn't already exist.
                await queue.CreateIfNotExistsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return true;

        }


        public static async Task DequeueQueue(string connectionString)
        {
            var queueClient = GetQueueClient(connectionString);
            CloudQueue queue = queueClient.GetQueueReference(QueueName);

            Console.WriteLine("Get the queue length");

            await queue.FetchAttributesAsync();
            int? cachedMessageCount = queue.ApproximateMessageCount;
            Console.WriteLine("Number of messages in queue: {0}", cachedMessageCount);
            QueueRequestOptions options = new QueueRequestOptions();
            OperationContext operationContext = new OperationContext();

            if (cachedMessageCount == null)
            {
                Console.WriteLine("No Message Present in Queue");
                return;
            }

            foreach (CloudQueueMessage message in await queue.GetMessagesAsync(cachedMessageCount.Value,
                new TimeSpan(0, 0, 5, 0), options, operationContext))
            {
                // Process all messages in less than 5 minutes, deleting each message after processing.

                Console.WriteLine("Processing {0} Message", message.AsString);
                await queue.DeleteMessageAsync(message);
            }
        }
        public static CloudQueueClient GetQueueClient(string connectionString)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

            // Create the queue client.
            CloudQueueClient client = account.CreateCloudQueueClient();
            return client;
        }
    }
}
