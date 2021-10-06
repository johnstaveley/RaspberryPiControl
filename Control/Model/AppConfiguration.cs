using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Control.Model
{
    public class AppConfiguration
    {
        /// <summary>
        /// The device Id as defined in Azure IoTHub
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// Azure IoT Hub DEVICE connection string
        /// </summary>
        public string IoTHubConnectionString { get; set; }
        public AppConfiguration()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var config = new ConfigurationBuilder()
                .SetBasePath(new FileInfo(assembly.Location).DirectoryName)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(assembly, optional: false)
                .Build();
            DeviceId = config["DeviceId"];
            IoTHubConnectionString = config["IoTHubConnectionString"];
        }
    }
}
