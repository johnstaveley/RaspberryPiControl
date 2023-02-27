using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace UI.Model
{
    public class AppConfiguration : IAppConfiguration
    {
        /// <summary>
        /// Username to login with
        /// </summary>
        public string ApplicationUserName {get; set;}
        /// <summary>
        /// Password to login with
        /// </summary>
        public string ApplicationPassword {get; set;}
        /// <summary>
        /// Container name in blob storage, must be lowercase
        /// </summary>
        public string BlobContainerName { get; set; }
        public string BlobStorageConnectionString { get; set; }
        /// <summary>
        /// The device Id as defined in Azure IoTHub
        /// </summary>
        public string DeviceId { get; set; }
        public string EventHubConnectionString { get; set; }
        public string EventHubName { get; set; }
        /// <summary>
        /// Azure IoT Hub SERVICE connection string
        /// </summary>
        public string IoTHubConnectionString { get; set; }
        public AppConfiguration()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var config = new ConfigurationBuilder()
                .SetBasePath(new FileInfo(assembly.Location).DirectoryName)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(assembly, optional: true)
                .Build();
            ApplicationUserName = config["ApplicationUserName"];
            ApplicationPassword = config["ApplicationPassword"];
            BlobContainerName = config["BlobContainerName"];
            BlobStorageConnectionString = config["BlobStorageConnectionString"];
            DeviceId = config["DeviceId"];
            EventHubConnectionString = config["EventHubConnectionString"];
            EventHubName = config["EventHubName"];
            IoTHubConnectionString = config["IoTHubConnectionString"];
            if (DeviceId == "CHANGEME")
            {
                throw new Exception("Invalid IoT DeviceId configuration settings");
            }
            if (IoTHubConnectionString == "CHANGEME")
            {
                throw new Exception("Invalid IoT Hub configuration settings");
            }

        }
    }
}
