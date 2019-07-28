using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Topic.Subscription
{
    public class Subscriber
    {
        const string SbConnectionString =
           "Endpoint=sb://ajaysrvbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=daFDBDx58nlz4Q1YVdOUjhlFzFGhDr3LFYOA/RDQWO8=";

        public const string SbQueueName = "20190728queue";


        public static QueueClient QueueClient;

        public static async Task Run()
        {
            try
            {
                QueueClient = new QueueClient(SbConnectionString, SbQueueName);

                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };
                QueueClient.RegisterMessageHandler(ReceiveMessagesAsync, messageHandlerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
                await QueueClient.CloseAsync();
            }
        }

        private static async Task ReceiveMessagesAsync(Message message, CancellationToken token)
        {
            try
            {
                Console.WriteLine($"Received message: {Encoding.UTF8.GetString(message.Body)}");

                await QueueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception ex)
            {
                await QueueClient.AbandonAsync(message.SystemProperties.LockToken);
                Console.WriteLine(ex.Message);
            }
        }


        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs.Exception);
            return Task.CompletedTask;
        }
    }
}