using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Control.Hardware
{

    public abstract class LedBackPack
    {
        /// <summary>
        /// The I2C device instance to be controlled. 
        /// </summary>
        private readonly II2CDevice _i2C;

        protected int[] displaybuffer = new int[8]; // Raw display data

        protected const int LED_ON = 1;  //< GFX color of lit LED segments (single-color displays)
        protected const int LED_OFF = 0; //< GFX color of unlit LED segments (single-color displays)

        protected const byte HT16K33_BLINK_CMD = 0x80;       //< I2C register for BLINK setting
        protected const byte HT16K33_BLINK_DISPLAYON = 0x01; //< I2C value for steady on
        protected const byte HT16K33_BLINK_OFF = 0;          //< I2C value for steady off
        protected const byte HT16K33_BLINK_2HZ = 1;          //< I2C value for 2 Hz blink
        protected const byte HT16K33_BLINK_1HZ = 2;          //< I2C value for 1 Hz blink
        protected const byte HT16K33_BLINK_HALFHZ = 3;       //< I2C value for 0.5 Hz blink

        protected const byte HT16K33_CMD_BRIGHTNESS = 0xE0; //< I2C register for BRIGHTNESS setting

        protected const int SEVENSEG_DIGITS = 5; //< # Digits in 7-seg displays, plus NUL end


        public void SetBrightness(byte b)
        {
            if (b > 15) b = 15;
            _i2C.Write((byte)(HT16K33_CMD_BRIGHTNESS | b));
        }

        public void BlinkRate(byte b)
        {
            if (b > 3) b = 0; // turn off if not sure
            _i2C.Write((byte)(HT16K33_BLINK_CMD | HT16K33_BLINK_DISPLAYON | (b << 1)));
        }

        public LedBackPack(int address)
        {
            _i2C = Pi.I2C.AddDevice(address);
        }

        public void Begin()
        {
            _i2C.Write(0x21); // turn on oscillator

            // internal RAM powers up with garbage/random values.
            // ensure internal RAM is cleared before turning on display
            // this ensures that no garbage pixels show up on the display
            // when it is turned on.
            Clear();
            WriteDisplay();
            BlinkRate(HT16K33_BLINK_OFF);
            SetBrightness(15); // max brightness
        }

        public void WriteDisplay()
        {
            _i2C.Write((byte)0x00); // start at address $00

            for (byte i = 0; i < 8; i++)
            {
                _i2C.Write((byte)(displaybuffer[i] & 0xFF));
                _i2C.Write((byte)(displaybuffer[i] >> 8));
            }
        }

        public void Clear()
        {
            for (byte i = 0; i < 8; i++)
            {
                displaybuffer[i] = 0;
            }
        }

        protected void Swap(short a, short b)
        {
            short t = a;
            a = b;
            b = t;
        }

    }
}
