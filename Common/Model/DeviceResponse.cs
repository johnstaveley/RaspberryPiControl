using System;
using System.Text.Json.Serialization;

namespace Common.Model
{
    public class DeviceResponse
    {
        public string Method { get; private set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        public DateTime EventDate {get; set;}
        public DeviceResponse(string method)
        {
            EventDate = DateTime.UtcNow;
            Message = "";
            Method = method;
        }
    }
}
