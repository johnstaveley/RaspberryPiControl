using Common;
using Common.Model;
using Control.Model;
using Microsoft.Azure.Devices;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UI.Model;

namespace UI.Services
{
    /// <summary>
    /// A service to control peripherals connected to an IoT Device
    /// </summary>
    public class ThingService : IThingService, IDisposable
    {
        IAppConfiguration _appConfiguration;
        ServiceClient _serviceClient;
        string _deviceId;

        public ThingService(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration ?? throw new ArgumentNullException(nameof(AppConfiguration));
            _deviceId = _appConfiguration.DeviceId;
            _serviceClient = ServiceClient.CreateFromConnectionString(_appConfiguration.IoTHubConnectionString);
        }

        public void Dispose()
        {
            _serviceClient?.Dispose();
        }

        public async Task<ResponseModel> SendMessage(string method, int number, double value, string message = "")
        {
            var responseModel = new ResponseModel();
            try
            {
                var controlAction = new ControlAction
                {
                    Method = method,
                    Number = number,
                    Value = value,
                    Message = message
                };
                var deviceMethod = new CloudToDeviceMethod(Consts.MethodName);
                var payload = JsonSerializer.Serialize(controlAction);
                deviceMethod.SetPayloadJson(payload);
                var result = await _serviceClient.InvokeDeviceMethodAsync(_deviceId, deviceMethod);
                if (result.Status == 200) {
                    responseModel.Success = true;
                }
                var methodResponse = JsonSerializer.Deserialize<ControlActionResponse>(result.GetPayloadAsJson());
                responseModel.Message = methodResponse.Message;
            }
            catch (Exception exception)
            {
                responseModel.Message = exception.Message + " " + exception.InnerException?.Message;
                responseModel.Success = false;
            }
            responseModel.Success = true;
            return responseModel;

        }
    }
}
