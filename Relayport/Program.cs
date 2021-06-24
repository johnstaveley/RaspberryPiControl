using System;
using System.IO;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace RelayPort
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Relay!!");
            Pi.Init<BootstrapWiringPi>();
            try
            {
                //foreach (var device in Pi.I2C.Devices)
                //{
                //    Console.WriteLine($"Registered I2C Device: {device.DeviceId}");
                //}
                Console.WriteLine("Starting relay");
                var relay = new FourRelay(0);
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                    relay.Set(1, 1);
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                    relay.Set(2, 1);
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                    relay.Set(3, 1);
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                    relay.Set(4, 1);
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                    relay.SetAll(0);
                    Console.WriteLine($"Values of Relays are 1:{relay.Get(1)} 2:{relay.Get(2)} 3:{relay.Get(3)} 4:{relay.Get(4)}");
                    Thread.Sleep(1000);
                }
            }
            catch (IOException)
            {
                // Do nothing, there is just no device
                Console.WriteLine("No Device");
            }
            Console.WriteLine("Relay Finished. Press a key to end");
            Console.ReadKey();
        }
    }
}
