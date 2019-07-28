using System;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBus.Common
{
    public class QueueHelper
    {
        public const string ServiceBusConnectionString =
            "Endpoint=sb://ajaysrvbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=daFDBDx58nlz4Q1YVdOUjhlFzFGhDr3LFYOA/RDQWO8=";

        private const string QueueName = "20190728queue";

        private static IQueueClient _queueClient;

        public static IQueueClient GetQueueClient()
        {
            if (_queueClient == null)
            {
                _queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            }


            return _queueClient;
        }
    }
}
