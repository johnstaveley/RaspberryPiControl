using System;
using System.Text.Json.Serialization;

namespace Common.Model
{
    /// <summary>
    /// Class for sending messages from the device to event hubs, and there on to any subscribers
    /// </summary>
    public class DeviceEvent
    {
        public string Event { get; private set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        public DateTime EventDate {get; set;}
        public DeviceEvent(string method)
        {
            EventDate = DateTime.UtcNow;
            Message = "";
            Event = method;
        }
    }
}
