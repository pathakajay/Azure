using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Topic.Subscription
{
    class Program
    {
        static void Main(string[] args)
        {

            Subscriber.Run().GetAwaiter().GetResult();

        }

       
    }
}
