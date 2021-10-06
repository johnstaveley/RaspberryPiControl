using System;

namespace UI.Model
{
    public class DeviceEventArgs : EventArgs
    {
        public DateTime EventDate { get; set; }
        public string Message { get; set; }
        public string Method { get; set; }
    }
}
