using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Common.Model;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using UI.Model;

namespace UI.Services
{
    public class IotHubService : IIotHubService, IDisposable
    {

        IAppConfiguration _appConfiguration;
        BlobContainerClient _storageClient;
        EventProcessorClient _eventProcessor;
        public event EventHandler OnEventReceived = delegate { };

        public IotHubService(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration ?? throw new ArgumentNullException(nameof(AppConfiguration));

            // Create a blob container client that the event processor will use to track its read position in event hub
            _storageClient = new BlobContainerClient(_appConfiguration.BlobStorageConnectionString, _appConfiguration.BlobContainerName);
            _storageClient.CreateIfNotExists();

            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
            _eventProcessor = new EventProcessorClient(_storageClient, consumerGroup, _appConfiguration.EventHubConnectionString, 
                _appConfiguration.EventHubName);

            // Register handlers for processing events and handling errors
            _eventProcessor.ProcessEventAsync += ProcessEventHandler;
            _eventProcessor.ProcessErrorAsync += ProcessErrorHandler;
            _eventProcessor.StartProcessing();
        }

        public async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            var eventString = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
            Console.WriteLine($"\tReceived event: {eventString}");
            var eventData = JsonConvert.DeserializeObject<DeviceEvent>(eventString);
            var deviceEventArgs = new DeviceEventArgs() { EventDate = eventData.EventDate, Message = eventData.Message, Method = eventData.Event };
            OnEventReceived(this, deviceEventArgs);

            // Update checkpoint in the blob storage so that the app receives only new events the next time it's run
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            // Write details about the error to the console window
            Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _eventProcessor?.StopProcessing();
        }
    }
}
