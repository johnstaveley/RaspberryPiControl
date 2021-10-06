using System;
using UI.Model;

namespace UI.Services
{
    public interface IIotHubService
    {
        delegate void MyEventHandler(object sender, DeviceEventArgs args);
        event EventHandler OnEventReceived;
    }
}
