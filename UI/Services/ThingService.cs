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
        ServiceClient _serviceClient;
        string _deviceId;

        public ThingService()
        {
            var configuration = new AppConfiguration();
            if (configuration.DeviceId == "CHANGEME")
            {
                throw new Exception("Invalid IoT Device configuration settings");
            }
            _deviceId = configuration.DeviceId;
            if (configuration.IoTHubConnectionString == "CHANGEME")
            {
                throw new Exception("Invalid IoT Hub configuration settings");
            }
            _serviceClient = ServiceClient.CreateFromConnectionString(configuration.IoTHubConnectionString);
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
                var methodResponse = JsonSerializer.Deserialize<MethodResponse>(result.GetPayloadAsJson());
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
