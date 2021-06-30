using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using RelayPort.Model;
using System;
using System.Collections.Generic;
using System.Device.Pwm.Drivers;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayPort.Hardware;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace RelayPort
{
    class Program
    {
        static SoftwarePwmChannel _servo1;
        static SoftwarePwmChannel _servo2;
        static FourRelayBoard _fourRelayBoard;

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
            _servo1 = new SoftwarePwmChannel(18, 400, 0.5, true);
            _servo1.Start();
            _servo2 = new SoftwarePwmChannel(13, 400, 0.5, true);
            _servo2.Start();

            Console.WriteLine("Setting up IoT Hub");
            var deviceClient = DeviceClient.CreateFromConnectionString(configuration.IoTHubConnectionString);
            await deviceClient.SetMethodHandlerAsync("ControlAction", BoardAction, null);
            Console.WriteLine("Setting up board");
            Lcd1602 lcd = new Lcd1602();
            lcd.Init();
            lcd.Clear();
            lcd.Write(0, 0, "Hello");
            lcd.Write(0, 1, "     World!");
            try
            {
                Console.WriteLine("Starting Raspberry Pi control, send messages via IoT Explorer to set values");
                _fourRelayBoard = new FourRelayBoard(0);
                for (int i = 0; i < 3000; i++)
                {
                    //Console.WriteLine($"Values of Relays are 1:{_fourRelayBoard.Get(1)} 2:{_fourRelayBoard.Get(2)} 3:{_fourRelayBoard.Get(3)} 4:{_fourRelayBoard.Get(4)}");
                    Console.Write(".");
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
                ConsoleHelper.WriteGreenMessage($"Received: {data} from direct method {methodRequest.Name}");
                var controlAction = JsonConvert.DeserializeObject<ControlAction>(data);
                int status = 200;
                string message = "";
                switch (controlAction.Method)
                {
                    case "SetServo":
                        ConsoleHelper.WriteGreenMessage($"Setting servo to value {controlAction.Value}");
                        if (!(controlAction.Value is >= 0 and <= 1))
                        {
                            status = 400;
                            message = $"Servo value of {controlAction.Value} is illegal, must be between 0 and 1 inclusive";
                        }
                        else
                        {
                            switch (controlAction.Number)
                            {
                                case -1:
                                    _servo1.DutyCycle = controlAction.Value;
                                    _servo2.DutyCycle = controlAction.Value;
                                    break;
                                case 1:
                                    _servo1.DutyCycle = controlAction.Value;
                                    break;
                                case 2:
                                    _servo2.DutyCycle = controlAction.Value;
                                    break;
                                default:
                                    status = 400;
                                    message = $"Servo {controlAction.Number} is illegal, must be either 1 or 2";
                                    break;
                            }
                        }
                        break;
                    case "GetRelay":
                        var getRelayNumber = (int)controlAction.Number;
                        if (getRelayNumber == -1)
                        {
                            ConsoleHelper.WriteGreenMessage($"Getting value of all relays");
                            List<Tuple<int, byte>> responses = new List<Tuple<int, byte>>();
                            foreach (var relayNumber in Enumerable.Range(1, 4))
                            {
                                responses.Add(new Tuple<int, byte>(relayNumber, _fourRelayBoard.Get((byte)relayNumber)));
                            }
                            status = 200;
                            message = string.Join(", ", responses.Select(a => $"Relay {a.Item1} has value {a.Item2}"));
                        }
                        else if (getRelayNumber is > 0 and < 5)
                        {
                            ConsoleHelper.WriteGreenMessage($"Getting relay {controlAction.Number}");
                            var relayResponse = _fourRelayBoard.Get((byte)getRelayNumber);
                            status = 200;
                            message = $"GetRelay - Relay {getRelayNumber} value is {relayResponse}";
                        }
                        else
                        {
                            status = 400;
                            message = $"GetRelay - Relay address {getRelayNumber} unknown. Must be 1 to 4 or -1 for all relays";
                        }
                        break;
                    case "SetRelay":
                        var relay = (int)controlAction.Number;
                        if (relay == -1)
                        {
                            ConsoleHelper.WriteGreenMessage($"Setting all relays to value {controlAction.Value}");
                            var relayResponse = _fourRelayBoard.SetAll((byte)controlAction.Value);
                            if (!relayResponse.Success)
                            {
                                ConsoleHelper.WriteRedMessage(relayResponse.Message);
                            }
                        }
                        else if (relay is > 0 and < 5)
                        {
                            ConsoleHelper.WriteGreenMessage($"Setting relay {controlAction.Number} to value {controlAction.Value}");
                            var relayResponse = _fourRelayBoard.SetRelay((byte)relay, (byte)controlAction.Value);
                            if (!relayResponse.Success)
                            {
                                ConsoleHelper.WriteRedMessage(relayResponse.Message);
                            }
                        }
                        else
                        {
                            status = 400;
                            message = $"SetRelay - Relay address {relay} unknown. Must be 1 to 4 or -1 for all relays";
                        }
                        break;
                    default:
                        message = $"Unknown method: {controlAction.Method}. Must be either 'Relay' or 'Servo'";
                        ConsoleHelper.WriteRedMessage(message);
                        status = 400;
                        break;
                }

                // Acknowledge the direct method call with a 200 success message.
                string result = "{\"result\":\"Executed direct method: " + methodRequest.Name + "\",\"message\":\"" + message + "\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), status));
            }
            catch (Exception exception)
            {
                // Acknowledge the direct method call with a 400 error message.
                string result = "{\"result\":\"Exception: " + exception.Message + "\"}";
                ConsoleHelper.WriteRedMessage("Direct method failed: " + exception.Message);
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 400));
            }

        }

    }
}
