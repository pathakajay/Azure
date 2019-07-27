using System;

namespace AzureStorageQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            StorageQueueSample.Run().GetAwaiter().GetResult();

        }
    }
}
