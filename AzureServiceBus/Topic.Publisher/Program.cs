using System;

namespace Topic.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Publisher.Run().GetAwaiter().GetResult();
            Console.WriteLine("Hello World!");
        }
    }
}
