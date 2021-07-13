using System;
using Microsoft.Extensions.Configuration;

namespace Control.Model
{
    public class AppConfiguration
    {
        public string IoTHubConnectionString { get; set; }
        public AppConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            IoTHubConnectionString = config["IoTHubConnectionString"];
        }
    }
}
