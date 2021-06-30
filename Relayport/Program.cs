﻿using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using RelayPort.Model;
using System;
using System.Device.Pwm.Drivers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace RelayPort
{
    class Program
    {
        static SoftwarePwmChannel _servo;
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
            _servo = new SoftwarePwmChannel(18, 400, 0.5, true);
            _servo.Start();

            Console.WriteLine("Setting up IoT Hub");
            var deviceClient = DeviceClient.CreateFromConnectionString(configuration.IoTHubConnectionString);
            await deviceClient.SetMethodHandlerAsync("ControlAction", BoardAction, null);
            Console.WriteLine("Setting up board");
            //using I2cDevice i2cDevice = I2cDevice.Create(new I2cConnectionSettings(1, 0x20));
            //using Mcp23008 serialDriver = new Mcp23008(i2cDevice);
            try
            {
                Console.WriteLine("Starting relay");
                _fourRelayBoard = new FourRelayBoard(0);
                for (int i = 0; i < 30; i++)
                {
                    Console.WriteLine($"Values of Relays are 1:{_fourRelayBoard.Get(1)} 2:{_fourRelayBoard.Get(2)} 3:{_fourRelayBoard.Get(3)} 4:{_fourRelayBoard.Get(4)}");
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
                        if (controlAction.Value is >= 0 and <= 1) {
                            _servo.DutyCycle = controlAction.Value;
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
                            var relayResponse = _fourRelayBoard.SetAll((byte) controlAction.Value);
                            if (!relayResponse.Success)
                            {
                                ConsoleHelper.WriteRedMessage(relayResponse.Message);
                            }
                        } else if (relay is > 0 and < 5) {
                            ConsoleHelper.WriteGreenMessage($"Setting relay {controlAction.Number} to value {controlAction.Value}");
                            var relayResponse = _fourRelayBoard.SetRelay((byte) relay, (byte) controlAction.Value);
                            if (!relayResponse.Success)
                            {
                                ConsoleHelper.WriteRedMessage(relayResponse.Message);
                            }
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
