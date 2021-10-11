namespace UI.Model
{
    public interface IAppConfiguration
    {
        /// <summary>
        /// Username to login with
        /// </summary>
        string ApplicationUserName {get; set;}
        /// <summary>
        /// Password to login with
        /// </summary>
        string ApplicationPassword {get; set;}
        /// <summary>
        /// Container name in blob storage, must be lowercase
        /// </summary>
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
