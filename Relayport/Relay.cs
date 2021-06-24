//This is the device: https://thepihut.com/products/4-relay-heavy-duty-stackable-card-for-raspberry-pi?variant=32299393548350
//   or here: https://shieldslist.com/raspberry-pi-shield/4-relay-heavy-duty-stackable-card-for-raspberry-pi/
// Converted from python code here: https://github.com/SequentMicrosystems/4relay-rpi/blob/master/python/4relay/lib4relay/__init__.py
// Instruction manual for the hardware is here: https://cdn.shopify.com/s/files/1/0176/3274/files/4-RELAY-UsersGuide.pdf?v=1596363564
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

        /// <summary>
        /// Creates a new instance of the class to control the 4 relay header board
        /// </summary>
        /// <param name="stack">Get the stack number from the jumper settings, see manual</param>
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

        public byte RelayToIO(byte relay)
        {
            byte value = (byte) 0;
            foreach (var index in Enumerable.Range(0, 4))
            {
                if ((relay & 1 << index) != 0)
                {
                    value = (byte) (value + RelayMaskRemap[index]);
                }
            }
            return value;
        }

        public byte IoToRelay(byte iov)
        {
            var value = 0;
            foreach (var index in Enumerable.Range(0, 4))
            {
                if ((iov & RelayMaskRemap[index]) != 0)
                {
                    value = value + (1 << index);
                }
            }
            return (byte) value;
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

        /// <summary>
        /// Sets the value of a specified relay
        /// </summary>
        /// <param name="relay">The number of the relay from 1 to 4</param>
        /// <param name="value">The value to set 1 = on and 0 = off</param>
        public void SetRelay(byte relay, byte value)
        {
            if (relay < 1 || relay > 4)
            {
                throw new ArgumentException("Invalid relay number");
            }
            var oldValue = Check();
            oldValue = IoToRelay(oldValue);
            if (value == 0)
            {
                oldValue = (byte) (oldValue & ~(1 << relay - 1));
                oldValue = RelayToIO(oldValue);
                _i2c.WriteAddressByte(RELAY4_OUTPORT_REG_ADD, oldValue);
            }
            else
            {
                oldValue = (byte) (oldValue | 1 << relay - 1);
                oldValue = RelayToIO(oldValue);
                _i2c.WriteAddressByte(RELAY4_OUTPORT_REG_ADD, oldValue);
            }
        }

        /// <summary>
        /// Gets the on/off status of a relay
        /// </summary>
        /// <param name="relay">The number of the relay to query from 1 to 4</param>
        /// <returns>1 for on and 0 for off</returns>
        public byte Get(byte relay)
        {
            if (relay < 1 || relay > 4)
            {
                throw new ArgumentException("Invalid relay number");
            }
            var value = Check();
            value = IoToRelay(value);
            value = (byte) (value & 1 << relay - 1);
            if (value == 0)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// Sets all the relays to a specific value
        /// </summary>
        /// <param name="value"></param>
        public void SetAll(byte value)
        {
            SetRelay(1, value);
            SetRelay(2, value);
            SetRelay(3, value);
            SetRelay(4, value);
        }
    }
}
