using Control.Hardware;
using Control.Model;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.Pwm.Drivers;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace Control
{
    class Program
    {
        static SoftwarePwmChannel _servo1;
        static FourRelayBoard _fourRelayBoard;
        static PwmDriver _pwmDriver;
        static Lcd1602 _lcd;
        static bool ledState1;
        static System.Device.Gpio.GpioController _controller;
        static int _inputPin;
        static int _outputPin1;
        static int _outputPin2;
        static int _outputPin3;

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
            _servo1 = new SoftwarePwmChannel(18, 400, 0.9, true);
            _servo1.Start();
            // Allow servos to move to new position and then stop them. This removes jitter
            Thread.Sleep(1000);
            _servo1.DutyCycle = 0;
            
            _outputPin1 = 17; // board pin 11
            _outputPin2 = 27; // board pin 13
            _outputPin3 = 16; // board pin 36
            _inputPin = 22; // board pin 15
            _controller = new();
            _controller.OpenPin(_outputPin1, PinMode.Output);
            _controller.OpenPin(_outputPin2, PinMode.Output);
            _controller.OpenPin(_outputPin3, PinMode.Output);
            _controller.OpenPin(_inputPin, PinMode.Input);
            _controller.Write(_outputPin1, PinValue.Low);
            _controller.Write(_outputPin2, PinValue.Low);
            _controller.Write(_outputPin3, PinValue.Low);

            Console.WriteLine("Setting up Pwm Driver");
            _pwmDriver = new PwmDriver();
            _pwmDriver.ResetDevice();
            _pwmDriver.IsDebug = true;
            Thread.Sleep(1000);
            //Console.WriteLine("Pwm rate");
            //_pwmDriver.SetPwmUpdateRate(400);
            Console.WriteLine("Set all true");
            _pwmDriver.SetAllCall(true);

            Console.WriteLine("Setting up IoT Hub");
            var deviceClient = DeviceClient.CreateFromConnectionString(configuration.IoTHubConnectionString);
            await deviceClient.SetMethodHandlerAsync("ControlAction", BoardAction, null);
            var twin = await deviceClient.GetTwinAsync();
            Console.WriteLine("Successfully got twin for the device", ConsoleColor.Green);
            //if (twin != null) Console.WriteLine(twin.ToString(), ConsoleColor.Green);
            await StartListeningForDesiredPropertyChanges(deviceClient);
            Console.WriteLine("Setting up board");
            _lcd = new Lcd1602();
            _lcd.Init();
            _lcd.Clear();
            _lcd.Write(0, 0, "Pi Control");
            _lcd.Write(0, 1, "Started");
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
                    case "GetInput":
                        var inputResult = _controller.Read(_inputPin);
                        status = 200;
                        message = $"GetInput: Value is {inputResult}";
                        break;
                    case "SetOutput":
                        if (!(controlAction.Number is 1 or 2 or 3))
                        {
                            status = 400;
                            message = "SetOutput Number must be 1 to 3 to indicate which output to target";
                        }
                        if (!(controlAction.Value is 0 or 1))
                        {
                            status = 400;
                            message = $"SetOutput value of {controlAction.Value} is illegal, must be either 0 or 1";
                        }
                        var outputNumber = (int) controlAction.Number;
                        var outputValue = (int) controlAction.Value;
                        switch (outputNumber)
                        {
                            case 1:
                                _controller.Write(_outputPin1, outputValue);
                                break;
                            case 2:
                                _controller.Write(_outputPin2, outputValue);
                                break;
                            case 3:
                                _controller.Write(_outputPin3, outputValue);
                                break;
                        }
                        break;
                    case "SetText":
                        if (controlAction.Number == 0) controlAction.Number = 1;
                        if (!(controlAction.Number is 1 or 2))
                        {
                            status = 400;
                            message = "SetText Number must be 1 or 2 to indicate either the first or second line of the display";
                        } else
                        {
                            var xCoordinate = (int) controlAction.Value;
                            var displayMessage = controlAction.Message
                                .PadLeft(controlAction.Message.Length + xCoordinate, ' ')
                                .PadRight(20 - controlAction.Message.Length - xCoordinate, ' ');
                            _lcd.Write(0, (int) controlAction.Number - 1, displayMessage);
                        }
                        break;
                    case "SetPwm":
                        if (!(controlAction.Value is >= 0 and <= 1))
                        {
                            status = 400;
                            message = $"Pwm value of {controlAction.Value} is illegal, must be between 0 and 1 inclusive";
                        }
                        else
                        {
                            var pwmMessage = controlAction.Number == -1 ? "All" : controlAction.Number.ToString();
                            ConsoleHelper.WriteGreenMessage($"Setting pwm {pwmMessage} to value {controlAction.Value}");
                            switch (controlAction.Number)
                            {
                                case -1:
                                    // All
                                    _pwmDriver.SetDutyCycle(PwmDriver.PwmChannel.C0, controlAction.Value * 100);
                                    Thread.Sleep(1000);
                                    var value1 = _pwmDriver.GetPwmOn(PwmDriver.PwmChannel.C0);
                                    ConsoleHelper.WriteColorMessage($"Pwm 0 value:{value1}", ConsoleColor.Yellow);
                                    break;
                                default:
                                    status = 400;
                                    message = $"Pwm {controlAction.Number} is illegal, must be either 1 or 2";
                                    break;
                            }
                        }
                        break;
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
                                    // All
                                    _servo1.DutyCycle = controlAction.Value;
                                    break;
                                case 1:
                                    _servo1.DutyCycle = controlAction.Value;
                                    break;
                                default:
                                    status = 400;
                                    message = $"Servo {controlAction.Number} is illegal, must be either 1 or 2";
                                    break;
                            }
                            // Give the servos time to get to their new position. and then stop them, this stops jitter
                            Thread.Sleep(1000);
                            _servo1.DutyCycle = 0;
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

        private static async Task DesiredPropertyUpdateCallback(TwinCollection desiredProperties, object userContext)
        {
            try
            {
                ConsoleHelper.WriteGreenMessage("Received desired property update:");
                ConsoleHelper.WriteGreenMessage(desiredProperties.ToJson(Formatting.Indented));
                ApplyDesiredProperties(desiredProperties);

            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteRedMessage($"* ERROR * {ex.Message}");
            }
        }

        private static async Task StartListeningForDesiredPropertyChanges(DeviceClient deviceClient)
        {
            try
            {
                await deviceClient.SetDesiredPropertyUpdateCallbackAsync(callback: DesiredPropertyUpdateCallback, userContext: deviceClient);
                Console.WriteLine("Now listening for desired property updates");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteRedMessage($"* ERROR * {ex.Message}");
            }
        }

        private static void ApplyDesiredProperties(TwinCollection desiredProperties)
        {
            try
            {
                if (desiredProperties == null)
                {
                    return;
                }
                if (desiredProperties.Contains("ledState") && desiredProperties["ledState"] != null)
                {
                    var propertyValue = (string) desiredProperties["ledState"].ToString();
                    ledState1 = int.Parse(propertyValue) == 1;
                    Console.WriteLine($"Setting LED1 to {ledState1}");
                    _controller.Write(_outputPin1, ledState1 ? PinValue.High : PinValue.Low);
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteRedMessage($"* ERROR * {ex.Message}");
            }
        }
    }
}
