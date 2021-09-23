using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Control.Model
{
    public class AppConfiguration
    {
        public string IoTHubConnectionString { get; set; }
        public AppConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName)
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            IoTHubConnectionString = config["IoTHubConnectionString"];
        }
    }
}
