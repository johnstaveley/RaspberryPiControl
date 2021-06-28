using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using RelayPort.Model;
using System;
using System.Device.Pwm.Drivers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Iot.Device.Ssd13xx.Commands.Ssd1306Commands;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace RelayPort
{
    class Program
    {
        static SoftwarePwmChannel servo;
        static FourRelay fourRelay;
        static async Task Main(string[] args)
        {
            Console.WriteLine("Loading settings");
            var configuration = new AppConfiguration();
            if (configuration.IoTHubConnectionString == "CHANGEME")
            {
                Console.WriteLine("Invalid configuration settings");
                return;
            }

            Console.WriteLine("Initialising board");
            Pi.Init<BootstrapWiringPi>();
            servo = new SoftwarePwmChannel(18, 400, 0.5, true);
            servo.Start();

            Console.WriteLine("Setting up IoT Hub");
            var _deviceClient = DeviceClient.CreateFromConnectionString(configuration.IoTHubConnectionString);
            await _deviceClient.SetMethodHandlerAsync("ControlAction", BoardAction, null);
            Console.WriteLine("Setting up board");
            //using I2cDevice i2cDevice = I2cDevice.Create(new I2cConnectionSettings(1, 0x20));
            //using Mcp23008 serialDriver = new Mcp23008(i2cDevice);
            try
            {
                Console.WriteLine("Starting relay");
                fourRelay = new FourRelay(0);
                for (int i = 0; i < 30; i++)
                {
                    Console.WriteLine($"Values of Relays are 1:{fourRelay.Get(1)} 2:{fourRelay.Get(2)} 3:{fourRelay.Get(3)} 4:{fourRelay.Get(4)}");
                    await Task.Delay(5000);
                }
            }
            catch (IOException)
            {
                // Do nothing, there is just no device
                Console.WriteLine("No Device");
            }
            Console.WriteLine("Relay Finished, press key to end");
            Console.ReadKey();
        }

        private static Task<MethodResponse> BoardAction(MethodRequest methodRequest, object userContext)
        {
            try
            {
                var data = methodRequest.DataAsJson;
                ConsoleHelper.WriteGreenMessage($"Received data: {data} from calling method {methodRequest}");
                var controlAction = JsonConvert.DeserializeObject<ControlAction>(data);
                int status = 200;
                string message = "";
                switch (controlAction.Method)
                {
                    case "Servo":
                        ConsoleHelper.WriteGreenMessage($"Setting servo to value {controlAction.Value}");
                        if (controlAction.Value >= 0 && controlAction.Value <=1) {
                            servo.DutyCycle = controlAction.Value;
                        } else
                        {
                            status = 400;
                            message = $"Servo value of {controlAction.Value} is illegal but be between 0 and 1 inclusive";
                        }
                        break;
                    case "Relay":
                        var relay = (int) controlAction.Number;
                        if (relay == -1)
                        {
                            ConsoleHelper.WriteGreenMessage($"Setting all relays to value {controlAction.Value}");
                            fourRelay.SetAll((byte) controlAction.Value);
                        } else if (relay > 0 && relay < 5) {
                            ConsoleHelper.WriteGreenMessage($"Setting relay {controlAction.Number} to value {controlAction.Value}");
                            fourRelay.SetRelay((byte) relay, (byte) controlAction.Value);
                        } else
                        {
                            status = 400;
                            message = $"Relay address {relay} unknown. Must be 1 to 4 or -1 for all relays";
                        }
                        break;
                    default:
                        message = $"Unknown method: {controlAction.Method}. Must be either 'Relay' or 'Servo'";
                        ConsoleHelper.WriteRedMessage(message);
                        status = 400;
                        break;
                }

                // Acknowledge the direct method call with a 200 success message.
                string result = "{\"result\":\"Executed direct method: " + methodRequest.Name + "\",\"message\":\"" + message +"\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), status));
            }
            catch
            {
                // Acknowledge the direct method call with a 400 error message.
                string result = "{\"result\":\"Invalid parameter\"}";
                ConsoleHelper.WriteRedMessage("Direct method failed: " + result);
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 400));
            }

        }

    }
}
