using System;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus
{
    class Program
    {
    

        static void Main(string[] args)
        {
            SampleAzureServiceBus.Run().GetAwaiter().GetResult();
            Console.WriteLine("Hello World!");
        }
    }
}
