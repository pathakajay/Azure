using System;
using System.Text;
using System.Threading.Tasks;
using AzureServiceBus.Common;
using Microsoft.Azure.ServiceBus;

namespace Topic.Publisher
{
    public class Publisher
    {
        public static async Task Run()
        {
            await SendMessagesAsync(90);
        }

        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    // Create a new message to send to the queue
                    string messageBody = $"Message {DateTime.Now.ToString("yy-mm-dd hh:mm:ss tt")}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the queue
                    await QueueHelper.GetQueueClient().SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
            finally
            {
                await QueueHelper.GetQueueClient().CloseAsync();
            }
        }
    }
}
