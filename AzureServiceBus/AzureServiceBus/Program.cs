using System;

namespace AzureServiceBus
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SampleAzureServiceBus.Run().GetAwaiter().GetResult();
            Console.WriteLine("Hello World!");
        }
    }
}