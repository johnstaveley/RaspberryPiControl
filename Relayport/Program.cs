using System;
using System.IO;
using System.Threading;
using Unosquare.RaspberryIO;
using System.Device.Gpio;
using System.Device.Pwm.Drivers;
using Unosquare.WiringPi;

namespace RelayPort
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Relay!!");
            Pi.Init<BootstrapWiringPi>();
            var pwmController = new SoftwarePwmChannel(18, 400, 0.5, true);
            pwmController.Start();
            try
            {
                Console.WriteLine("Starting relay");
                var relay = new FourRelay(0);
                for (int i = 0; i < 100; i++)
                {
                    pwmController.DutyCycle = i / 100D;
                    Thread.Sleep(100);
                }
                for (int i = 0; i < 100; i++)
                {
                    pwmController.DutyCycle = (100 - i) /  100D;
                    Thread.Sleep(100);
                }

                for (int i = 0; i < 10; i++)
                {
                    pwmController.DutyCycle = 0.0;
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                    relay.SetRelay(1, 1);
                    pwmController.DutyCycle = 0.25;
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                    relay.SetRelay(2, 1);
                    pwmController.DutyCycle = 0.5;
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                    relay.SetRelay(3, 1);
                    pwmController.DutyCycle = 0.75;
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                    relay.SetRelay(4, 1);
                    pwmController.DutyCycle = 1.0;
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                    relay.SetAll(0);
                    pwmController.DutyCycle = 0.5;
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
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
        
    }
}
