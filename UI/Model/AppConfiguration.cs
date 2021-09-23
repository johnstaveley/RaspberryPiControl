using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace UI.Model
{
    public class AppConfiguration
    {
        public string EventHubConnectionString { get; set; }
        /// <summary>
        /// This is a connection string for the IoT Hub that has service connect permissions. This is not a device connection string
        /// </summary>
        public string IoTHubConnectionString { get; set; }
        public string DeviceId { get; set; }
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
            EventHubConnectionString = config["EventHubConnectionString"];
            IoTHubConnectionString = config["IoTHubConnectionString"];
        }
    }
}
