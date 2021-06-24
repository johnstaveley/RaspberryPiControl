//This is the device: https://thepihut.com/products/4-relay-heavy-duty-stackable-card-for-raspberry-pi?variant=32299393548350
// Convert using https://github.com/unosquare/raspberryio#using-the-spi-bus
// or https://github.com/dotnet/iot/blob/main/Documentation/raspi-i2c.md
using System.Collections.Generic;
using System;
using System.Linq;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace RelayPort
{
    public class FourRelay
    {
        private II2CDevice _i2c;

        public FourRelay(int stack)
        {
            stack = 0x07 ^ stack;
            if (stack < 0 || stack > 7)
            {
                throw new ArgumentException("Invalid stack level");
            }
            _i2c = Pi.I2C.AddDevice(DEVICE_ADDRESS + stack);
        }

        public byte DEVICE_ADDRESS = 0x38;
        public byte RELAY4_INPORT_REG_ADD = 0x00;
        public byte RELAY4_OUTPORT_REG_ADD = 0x01;
        public byte RELAY4_POLINV_REG_ADD = 0x02;
        public byte RELAY4_CFG_REG_ADD = 0x03;

        public List<byte> RelayMaskRemap = new List<byte> {
            0x80,
            0x40,
            0x20,
            0x10
        };

        public List<int> RelayChRemap = new List<int> {
            7,
            6,
            5,
            4
        };

        public byte relayToIO(byte relay)
        {
            byte val = (byte) 0;
            foreach (var i in Enumerable.Range(0, 4))
            {
                if ((relay & 1 << i) != 0)
                {
                    val = (byte) (val + RelayMaskRemap[i]);
                }
            }
            return val;
        }

        public byte IoToRelay(byte iov)
        {
            var val = 0;
            foreach (var i in Enumerable.Range(0, 4))
            {
                if ((iov & RelayMaskRemap[i]) != 0)
                {
                    val = val + (1 << i);
                }
            }
            return (byte) val;
        }

        private byte Check()
        {
            var cfg = _i2c.ReadAddressByte( RELAY4_CFG_REG_ADD );
            if (cfg != 0)
            {
                _i2c.WriteAddressByte(RELAY4_CFG_REG_ADD, 0);
                _i2c.WriteAddressByte(RELAY4_OUTPORT_REG_ADD, 0);
            }
            return _i2c.ReadAddressByte(RELAY4_INPORT_REG_ADD);
        }

        public void Set(byte relay, byte value)
        {
            if (relay < 1 || relay > 4)
            {
                throw new ArgumentException("Invalid relay number");
            }
            var oldVal = Check();
            oldVal = IoToRelay(oldVal);
            if (value == 0)
            {
                oldVal = (byte) (oldVal & ~(1 << relay - 1));
                oldVal = relayToIO(oldVal);
                _i2c.WriteAddressByte(RELAY4_OUTPORT_REG_ADD, oldVal);
            }
            else
            {
                oldVal = (byte) (oldVal | 1 << relay - 1);
                oldVal = relayToIO(oldVal);
                _i2c.WriteAddressByte(RELAY4_OUTPORT_REG_ADD, oldVal);
            }
        }

        public byte Get(byte relay)
        {
            if (relay < 1 || relay > 4)
            {
                throw new ArgumentException("Invalid relay number");
            }
            var val = Check();
            val = IoToRelay(val);
            val = (byte) (val & 1 << relay - 1);
            if (val == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public void SetAll(byte value)
        {
            Set(1, value);
            Set(2, value);
            Set(3, value);
            Set(4, value);
        }
    }
}
