using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AzureStorageQueue
{
   public class StorageQueueSample
    {
        private   readonly IConfigurationRoot settingsCache;
        private   AppOptions appOptions;

        public StorageQueueSample()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", true, true);

            settingsCache = builder.Build();
            appOptions = new AppOptions();
            settingsCache.Bind(appOptions);
        }
        public static async Task Run()
        {
            StorageQueueSample queueSample= new StorageQueueSample();

            
            Console.WriteLine("Press a key to exit.");
            Console.ReadLine();
        }
    }
}
