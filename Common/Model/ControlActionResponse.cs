using System.Text.Json.Serialization;

namespace Common.Model
{
    public class ControlActionResponse
    {
        [JsonPropertyName("result")]
        public string Result {get; set;}
        [JsonPropertyName("message")]
        public string Message {get; set;}

    }
}
