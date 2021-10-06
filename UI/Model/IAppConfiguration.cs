namespace UI.Model
{
    public interface IAppConfiguration
    {
        string BlobContainerName { get; set; }
        string BlobStorageConnectionString { get; set; }
        /// <summary>
        /// The device Id as defined in Azure IoTHub
        /// </summary>
        string DeviceId { get; set; }
        string EventHubConnectionString { get; set; }
        string EventHubName { get; set; }
        /// <summary>
        /// Azure IoT Hub SERVICE connection string
        /// </summary>
        string IoTHubConnectionString { get; set; }
    }
}
