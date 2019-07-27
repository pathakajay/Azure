using System;
using Microsoft.Extensions.Configuration;

namespace AzureStorageQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", true, true);
            StorageQueueSample.Run().GetAwaiter().GetResult();

        }
    }
}
