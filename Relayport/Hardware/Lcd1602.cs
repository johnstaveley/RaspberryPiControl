using System.Text;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace RelayPort.Hardware
{

    public class Lcd1602
    {
        private readonly II2CDevice _i2c;
        public byte DEVICE_ADDRESS = 0x27;
        public Lcd1602()
        {
            _i2c = Pi.I2C.AddDevice(DEVICE_ADDRESS);
        }

        protected void SendCommand(int comm)
        {
            byte buf;
            // Send bit7-4 firstly
            buf = (byte)(comm & 0xF0);
            buf |= 0x04; // RS = 0, RW = 0, EN = 1
            buf |= 0x08;
            _i2c.Write(buf);
            Thread.Sleep(2);
            buf &= 0xFB; // Make EN = 0
            buf |= 0x08;
            _i2c.Write(buf);

            // Send bit3-0 secondly
            buf = (byte)((comm & 0x0F) << 4);
            buf |= 0x04; // RS = 0, RW = 0, EN = 1
            _i2c.Write(buf);
            Thread.Sleep(2);
            buf &= 0xFB; // Make EN = 0
            _i2c.Write(buf);
        }

        protected void SendData(int data)
        {
            byte buf;
            // Send bit7-4 firstly
            buf = (byte)(data & 0xF0);
            buf |= 0x05; // RS = 1, RW = 0, EN = 1
            buf |= 0x08;
            _i2c.Write(buf);
            Thread.Sleep(2);
            buf &= 0xFB; // Make EN = 0
            buf |= 0x08;
            _i2c.Write(buf);

            // Send bit3-0 secondly
            buf = (byte)((data & 0x0F) << 4);
            buf |= 0x05; // RS = 1, RW = 0, EN = 1
            buf |= 0x08;
            _i2c.Write(buf);
            Thread.Sleep(2);
            buf &= 0xFB; // Make EN = 0
            buf |= 0x08;
            _i2c.Write(buf);
        }

        public void Init()
        {
            SendCommand(0x33); // Must initialize to 8-line mode at first
            Thread.Sleep(2);
            SendCommand(0x32); // Then initialize to 4-line mode
            Thread.Sleep(2);
            SendCommand(0x28); // 2 Lines & 5*7 dots
            Thread.Sleep(2);
            SendCommand(0x0C); // Enable display without cursor
            Thread.Sleep(2);
            SendCommand(0x01); // Clear Screen
        }

        public void Clear()
        {
            SendCommand(0x01); //clear Screen
        }

        public void Write(int x, int y, string str)
        {
            // Move cursor
            int addr = 0x80 + 0x40 * y + x;
            SendCommand(addr);

            byte[] charData = Encoding.ASCII.GetBytes(str);

            foreach (byte b in charData)
            {
                SendData(b);
            }
        }
    }
}
