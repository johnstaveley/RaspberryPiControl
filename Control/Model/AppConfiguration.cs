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
            var assembly = Assembly.GetExecutingAssembly();
            var config = new ConfigurationBuilder()
                .SetBasePath(new FileInfo(assembly.Location).DirectoryName)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(assembly, optional: false)
                .Build();

            IoTHubConnectionString = config["IoTHubConnectionString"];
        }
    }
}
