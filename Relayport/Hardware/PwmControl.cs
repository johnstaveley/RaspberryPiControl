//*!
// *  @file Adafruit_PWMServoDriver.cpp
// *
// *  @mainpage Adafruit 16-channel PWM & Servo driver
// *
// *  @section intro_sec Introduction
// *
// *  This is a library for the 16-channel PWM & Servo driver.
// *
// *  Designed specifically to work with the Adafruit PWM & Servo driver.
// *
// *  Pick one up today in the adafruit shop!
// *  ------> https://www.adafruit.com/product/815
// *
// *  These displays use I2C to communicate, 2 pins are required to interface.
// *
// *  Adafruit invests time and resources providing this open source code,
// *  please support Adafruit andopen-source hardware by purchasing products
// *  from Adafruit!
// *
// *  @section author Author
// *
// *  Limor Fried/Ladyada (Adafruit Industries).
// *
// *  @section license License
// *
// *  BSD license, all text above must be included in any redistribution
// */

////#include "Adafruit_PWMServoDriver.h"
////#include <Wire.h>

////#define ENABLE_DEBUG_OUTPUT

//using System;
//using System.Threading;
//using Unosquare.RaspberryIO;
//using Unosquare.RaspberryIO.Abstractions;

//namespace Control
//{

//    public class PwmControl
//    {

//        // REGISTER ADDRESSES
//        public byte PCA9685_MODE1 = 0x00;      /**< Mode Register 1 */
//        public byte PCA9685_MODE2 = 0x01;      /**< Mode Register 2 */
//        public byte PCA9685_SUBADR1 = 0x02;    /**< I2C-bus subaddress 1 */
//        public byte PCA9685_SUBADR2 = 0x03;    /**< I2C-bus subaddress 2 */
//        public byte PCA9685_SUBADR3 = 0x04;    /**< I2C-bus subaddress 3 */
//        public byte PCA9685_ALLCALLADR = 0x05; /**< LED All Call I2C-bus address */
//        public byte PCA9685_LED0_ON_L = 0x06;  /**< LED0 on tick, low byte*/
//        public byte PCA9685_LED0_ON_H = 0x07;  /**< LED0 on tick, high byte*/
//        public byte PCA9685_LED0_OFF_L = 0x08; /**< LED0 off tick, low byte */
//        public byte PCA9685_LED0_OFF_H = 0x09; /**< LED0 off tick, high byte */
//        // etc all 16:  LED15_OFF_H 0x45
//        public byte PCA9685_ALLLED_ON_L = 0xFA;  /**< load all the LEDn_ON registers, low */
//        public byte PCA9685_ALLLED_ON_H = 0xFB;  /**< load all the LEDn_ON registers, high */
//        public byte PCA9685_ALLLED_OFF_L = 0xFC; /**< load all the LEDn_OFF registers, low */
//        public byte PCA9685_ALLLED_OFF_H = 0xFD; /**< load all the LEDn_OFF registers,high */
//        public byte PCA9685_PRESCALE = 0xFE;     /**< Prescaler for PWM output frequency */
//        public byte PCA9685_TESTMODE = 0xFF;     /**< defines the test mode to be entered */

//        // MODE1 bits
//        public byte MODE1_ALLCAL = 0x01;  /**< respond to LED All Call I2C-bus address */
//        public byte MODE1_SUB3 = 0x02;    /**< respond to I2C-bus subaddress 3 */
//        public byte MODE1_SUB2 = 0x04;    /**< respond to I2C-bus subaddress 2 */
//        public byte MODE1_SUB1 = 0x08;    /**< respond to I2C-bus subaddress 1 */
//        public byte MODE1_SLEEP = 0x10;   /**< Low power mode. Oscillator off */
//        public byte MODE1_AI = 0x20;      /**< Auto-Increment enabled */
//        public byte MODE1_EXTCLK = 0x40;  /**< Use EXTCLK pin clock */
//        public byte MODE1_RESTART = 0x80; /**< Restart enabled */
//        // MODE2 bits
//        public byte MODE2_OUTNE_0 = 0x01; /**< Active LOW output enable input */
//        public byte MODE2_OUTNE_1 = 0x02; /**< Active LOW output enable input - high impedience */
//        public byte MODE2_OUTDRV = 0x04; /**< totem pole structure vs open-drain */
//        public byte MODE2_OCH = 0x08;    /**< Outputs change on ACK vs STOP */
//        public byte MODE2_INVRT = 0x10;  /**< Output logic state inverted */

//        public byte PCA9685_I2C_ADDRESS = 0x40;     /**< Default PCA9685 I2C Slave Address */
//        public uint FREQUENCY_OSCILLATOR = 25000000; /**< Int. osc. frequency in datasheet */

//        public int PCA9685_PRESCALE_MIN = 3;   /**< minimum prescale value */
//        public int PCA9685_PRESCALE_MAX = 255; /**< maximum prescale value */

//        private readonly II2CDevice _i2c;
//        public bool IsDebug = true;
//        private byte _i2caddr;

//        private uint _oscillator_freq;

//        /*!
//         *  @brief  Instantiates a new PCA9685 PWM driver chip with the I2C address on a
//         * TwoWire interface
//         */
//        public PwmControl()
//        {
//            _i2caddr = PCA9685_I2C_ADDRESS;
//            _i2c = Pi.I2C.AddDevice(PCA9685_I2C_ADDRESS);
//        }

//        /*!
//         *  @brief  Instantiates a new PCA9685 PWM driver chip with the I2C address on a
//         * TwoWire interface
//         *  @param  addr The 7-bit I2C address to locate this chip, default is 0x40
//         */
//        public PwmControl(byte addr)
//        {
//            _i2caddr = addr;
//            _i2c = Pi.I2C.AddDevice(addr);
//        }

//        /// <summary>
//        /// Setups the I2C interface and hardware and optionally Sets External Clock
//        /// </summary>
//        /// <param name="prescale">prescale - ignored if zero</param>
//        public void Begin(byte prescale)
//        {
//            Reset();
//            if (prescale > 0)
//            {
//                SetExtClk(prescale);
//            }
//            else
//            {
//                // set a default frequency
//                SetPWMFreq(1000);
//            }
//            // set the default internal frequency
//            SetOscillatorFrequency(FREQUENCY_OSCILLATOR);
//        }

//        /// <summary>
//        /// Sends a reset command to the PCA9685 chip over I2C
//        /// </summary>
//        public void Reset()
//        {
//            write8(PCA9685_MODE1, MODE1_RESTART);
//            Thread.Sleep(10000);
//        }

//        /// <summary>
//        /// Puts board into sleep mode
//        /// </summary>
//        public void Sleep()
//        {
//            var awake = read8(PCA9685_MODE1);
//            var sleep = (byte) (awake | MODE1_SLEEP); // set sleep bit high
//            write8(PCA9685_MODE1, sleep);
//            Thread.Sleep(5); // wait until cycle ends for sleep to be active
//        }

//        /// <summary>
//        /// Wakes board from sleep
//        /// </summary>
//        public void Wakeup()
//        {
//            var sleep = read8(PCA9685_MODE1);
//            var wakeup = (byte) (sleep & ~MODE1_SLEEP); // set sleep bit low
//            write8(PCA9685_MODE1, wakeup);
//        }

//        /*!
//         *  @brief  Sets EXTCLK pin to use the external clock
//         *  @param  prescale
//         *          Configures the prescale value to be used by the external clock
//         */
//        public void SetExtClk(byte prescale)
//        {
//            byte oldmode = read8(PCA9685_MODE1);
//            var newmode = (byte) ((oldmode & ~MODE1_RESTART) | MODE1_SLEEP); // sleep
//            write8(PCA9685_MODE1, newmode); // go to sleep, turn off internal oscillator

//            // This sets both the SLEEP and EXTCLK bits of the MODE1 register to switch to
//            // use the external clock.
//            write8(PCA9685_MODE1, (newmode |= MODE1_EXTCLK));

//            write8(PCA9685_PRESCALE, prescale); // set the prescaler

//            Thread.Sleep(5000);
//            // clear the SLEEP bit to start
//            write8(PCA9685_MODE1, (byte) ((newmode & ~MODE1_SLEEP) | MODE1_RESTART | MODE1_AI));

//            if (IsDebug)
//            {
//                Console.Write("Mode now 0x");
//                Console.WriteLine($"{read8(PCA9685_MODE1):X}");
//            }
//        }

//        /*!
//         *  @brief  Sets the PWM frequency for the entire chip, up to ~1.6 KHz
//         *  @param  freq Floating point frequency that we will attempt to match
//         */
//        public void SetPWMFreq(float freq)
//        {
//            if (IsDebug)
//            {
//                Console.Write("Attempting to set freq ");
//                Console.WriteLine(freq);
//            }
//            // Range output modulation frequency is dependant on oscillator
//            if (freq < 1)
//                freq = 1;
//            if (freq > 3500)
//                freq = 3500; // Datasheet limit is 3052=50MHz/(4*4096)

//            float prescaleval = ((_oscillator_freq / (freq * 4096.0F)) + 0.5F) - 1F;
//            if (prescaleval < PCA9685_PRESCALE_MIN)
//                prescaleval = PCA9685_PRESCALE_MIN;
//            if (prescaleval > PCA9685_PRESCALE_MAX)
//                prescaleval = PCA9685_PRESCALE_MAX;
//            byte prescale = (byte) prescaleval;

//            if (IsDebug)
//            {
//                Console.Write("Final pre-scale: ");
//                Console.WriteLine(prescale);
//            }

//            byte oldmode = read8(PCA9685_MODE1);
//            var newmode = (byte) ((oldmode & ~MODE1_RESTART) | MODE1_SLEEP); // sleep
//            write8(PCA9685_MODE1, newmode);                             // go to sleep
//            write8(PCA9685_PRESCALE, prescale); // set the prescaler
//            write8(PCA9685_MODE1, oldmode);
//            Thread.Sleep(5000);
//            // This sets the MODE1 register to turn on auto increment.
//            write8(PCA9685_MODE1, (byte) (oldmode | MODE1_RESTART | MODE1_AI));

//            if (IsDebug)
//            {
//                Console.Write("Mode now 0x");
//                Console.WriteLine($"{read8(PCA9685_MODE1):X}");
//            }
//        }

//        /*!
//         *  @brief  Sets the output mode of the PCA9685 to either
//         *  open drain or push pull / totempole.
//         *  Warning: LEDs with integrated zener diodes should
//         *  only be driven in open drain mode.
//         *  @param  totempole Totempole if true, open drain if false.
//         */
//        public void SetOutputMode(bool totempole)
//        {
//            byte oldmode = read8(PCA9685_MODE2);
//            byte newmode;
//            if (totempole)
//            {
//                newmode = (byte) (oldmode | MODE2_OUTDRV);
//            }
//            else
//            {
//                newmode = (byte) (oldmode & ~MODE2_OUTDRV);
//            }
//            write8(PCA9685_MODE2, newmode);
//            if (IsDebug)
//            {
//                Console.Write("Setting output mode: ");
//                Console.Write(totempole ? "totempole" : "open drain");
//                Console.Write(" by setting MODE2 to ");
//                Console.WriteLine(newmode);
//            }
//        }

//        /*!
//         *  @brief  Reads set Prescale from PCA9685
//         *  @return prescale value
//         */
//        public uint ReadPrescale()
//        {
//            return read8(PCA9685_PRESCALE);
//        }

//        /*!
//         *  @brief  Gets the PWM output of one of the PCA9685 pins
//         *  @param  num One of the PWM output pins, from 0 to 15
//         *  @return requested PWM output value
//         */
//        public byte GetPwm(byte num)
//        {
//            //_i2c->requestFrom((int)_i2caddr, PCA9685_LED0_ON_L + 4 * num, (int)4);
//            //return _i2c->read();
//            return _i2c.ReadAddressByte((byte) PCA9685_LED0_ON_L + 4 * num);
//        }

//        /*!
//         *  @brief  Sets the PWM output of one of the PCA9685 pins
//         *  @param  num One of the PWM output pins, from 0 to 15
//         *  @param  on At what point in the 4096-part cycle to turn the PWM output ON
//         *  @param  off At what point in the 4096-part cycle to turn the PWM output OFF
//         */
//        public void SetPwm(byte num, UInt16 on, UInt16 off)
//        {
//            if (IsDebug)
//            {
//                Console.Write("Setting PWM ");
//                Console.Write(num);
//                Console.Write(": ");
//                Console.Write(on);
//                Console.Write("->");
//                Console.WriteLine(off);
//            }

//            //_i2c->beginTransmission(_i2caddr);
//            //_i2c->write(PCA9685_LED0_ON_L + 4 * num);
//            //_i2c->write(on);
//            //_i2c->write(on >> 8);
//            //_i2c->write(off);
//            //_i2c->write(off >> 8);
//            //_i2c->endTransmission();
//            byte addr = (byte) (PCA9685_LED0_ON_L + ((byte) 4) * num);
//            byte[] data = {addr, (byte) on, (byte) (on >> 8), (byte) off, (byte) (off >> 8) };
//            _i2c.Write(data);
//        }

//        /*!
//         *   @brief  Helper to set pin PWM output. Sets pin without having to deal with
//         * on/off tick placement and properly handles a zero value as completely off and
//         * 4095 as completely on.  Optional invert parameter supports inverting the
//         * pulse for sinking to ground.
//         *   @param  num One of the PWM output pins, from 0 to 15
//         *   @param  val The number of ticks out of 4096 to be active, should be a value
//         * from 0 to 4095 inclusive.
//         *   @param  invert If true, inverts the output, defaults to 'false'
//         */
//        public void SetPin(byte num, UInt16 val, bool invert)
//        {
//            // Clamp value between 0 and 4095 inclusive.
//            val = Math.Min(val, (UInt16) 4095);
//            if (invert)
//            {
//                if (val == 0)
//                {
//                    // Special value for signal fully on.
//                    SetPwm(num, (UInt16) 4096, 0);
//                }
//                else if (val == 4095)
//                {
//                    // Special value for signal fully off.
//                    SetPwm(num, 0, (UInt16) 4096);
//                }
//                else
//                {
//                    SetPwm(num, 0, (UInt16) (4095 - val));
//                }
//            }
//            else
//            {
//                if (val == 4095)
//                {
//                    // Special value for signal fully on.
//                    SetPwm(num, 4096, 0);
//                }
//                else if (val == 0)
//                {
//                    // Special value for signal fully off.
//                    SetPwm(num, 0, 4096);
//                }
//                else
//                {
//                    SetPwm(num, 0, val);
//                }
//            }
//        }

//        /*!
//         *  @brief  Sets the PWM output of one of the PCA9685 pins based on the input
//         * microseconds, output is not precise
//         *  @param  num One of the PWM output pins, from 0 to 15
//         *  @param  Microseconds The number of Microseconds to turn the PWM output ON
//         */
//        public void WriteMicroseconds(byte num, UInt16 microseconds)
//        {
//            if (IsDebug)
//            {
//                Console.Write("Setting PWM Via Microseconds on output");
//                Console.Write(num);
//                Console.Write(": ");
//                Console.Write(microseconds);
//                Console.WriteLine("->");
//            }

//            double pulse = microseconds;
//            double pulselength;
//            pulselength = 1000000; // 1,000,000 us per second

//            // Read prescale
//            var prescale = ReadPrescale();

//            if (IsDebug)
//            {
//                Console.Write(prescale);
//                Console.WriteLine(" PCA9685 chip prescale");
//            }

//            // Calculate the pulse for PWM based on Equation 1 from the datasheet section
//            // 7.3.5
//            prescale += 1;
//            pulselength *= prescale;
//            pulselength /= _oscillator_freq;

//            if (IsDebug)
//            {
//                Console.Write(pulselength);
//                Console.WriteLine(" us per bit");
//            }

//            pulse /= pulselength;

//            if (IsDebug)
//            {
//                Console.Write(pulse);
//                Console.WriteLine(" pulse for PWM");
//            }

//            SetPwm(num, 0, (UInt16) pulse);
//        }

//        /*!
//         *  @brief  Getter for the internally tracked oscillator used for freq
//         * calculations
//         *  @returns The frequency the PCA9685 thinks it is running at (it cannot
//         * introspect)
//         */
//        uint getOscillatorFrequency()
//        {
//            return _oscillator_freq;
//        }

//        /*!
//         *  @brief Setter for the internally tracked oscillator used for freq
//         * calculations
//         *  @param freq The frequency the PCA9685 should use for frequency calculations
//         */
//        void SetOscillatorFrequency(uint freq)
//        {
//            _oscillator_freq = freq;
//        }

//        /******************* Low level I2C interface */
//        byte read8(byte addr)
//        {
//            return _i2c.ReadAddressByte(addr);
//            //_i2c->beginTransmission(_i2caddr);
//            //_i2c->write(addr);
//            //_i2c->endTransmission();

//            //_i2c->requestFrom((uint)_i2caddr, (uint)1);
//            //return _i2c->read();
//        }

//        void write8(byte addr, byte d)
//        {
//            _i2c.WriteAddressWord(addr, d);
//            //_i2c->beginTransmission(_i2caddr);
//            //_i2c->write(addr);
//            //_i2c->write(d);
//            //_i2c->endTransmission();
//        }

//    }
//}