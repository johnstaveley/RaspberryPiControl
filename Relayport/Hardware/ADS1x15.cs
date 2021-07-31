using System;
using System.Collections.Generic;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Control.Hardware
{

    public class ADS1x15
    {

        /// <summary>
        /// The I2C device instance to be controlled. 
        /// </summary>
        private II2CDevice _i2C;

        private const byte ADS1x15_DEFAULT_ADDRESS = 0x48;

        private byte ADS1x15_POINTER_CONVERSION = 0x00;

        private byte ADS1x15_POINTER_CONFIG = 0x01;

        private byte ADS1x15_POINTER_lowThreshold = 0x02;

        private byte ADS1x15_POINTER_highThreshold = 0x03;

        private int ADS1x15_CONFIG_OS_SINGLE = 0x8000;

        private int ADS1x15_CONFIG_MUX_OFFSET = 12;

        private Dictionary<double, int> ADS1x15_CONFIG_GAIN = new Dictionary<double, int> {
            {2 / 3, 0x0000},
            {1, 0x0200},
            {2, 0x0400},
            {4, 0x0600},
            {8, 0x0800},
            {16, 0x0A00}};

        private int ADS1x15_CONFIG_MODE_CONTINUOUS = 0x0000;

        private int ADS1x15_CONFIG_MODE_SINGLE = 0x0100;

        /// <summary>
        /// Config for data rates
        /// </summary>
        private Dictionary<int, int> ADS1115_CONFIG_DR = new Dictionary<int, int> {
            {8, 0x0000},
            {16, 0x0020},
            {32, 0x0040},
            {64, 0x0060},
            {128, 0x0080},
            {250, 0x00A0},
            {475, 0x00C0},
            {860, 0x00E0}};

        private byte ADS1x15_CONFIG_COMP_WINDOW = 0x0010;

        private byte ADS1x15_CONFIG_COMP_ACTIVE_HIGH = 0x0008;

        private byte ADS1x15_CONFIG_COMP_LATCHING = 0x0004;

        private Dictionary<int, byte> ADS1x15_CONFIG_COMP_QUE = new Dictionary<int, byte> {
            {1, 0x0000},
            {2, 0x0001},
            {4, 0x0002}};

        private int ADS1x15_CONFIG_COMP_QUE_DISABLE = 0x0003;

        // Base functionality for ADS1x15 analog to digital converters.
        public ADS1x15(byte address = ADS1x15_DEFAULT_ADDRESS)
        {
            _i2C = Pi.I2C.GetDeviceById(address);
        }

        /// <summary>
        /// Retrieve the default data rate for this ADC (in samples per second). 
        /// </summary>
        /// <returns></returns>
        public virtual int GetDataRateDefault()
        {
            return 128;
        }

        /// <summary>
        /// Gets config for data rate or throws exception if not found
        /// </summary>
        /// <param name="dataRate"></param>
        /// <returns>16-bit value that can be OR'ed with the config register to set the specified data rate</returns>
        public int GetDataRateConfig(int dataRate)
        {
            if (!ADS1115_CONFIG_DR.ContainsKey(dataRate))
            {
                throw new ArgumentException($"{nameof(dataRate)} must be one of: 8, 16, 32, 64, 128, 250, 475, 860");
            }
            return ADS1115_CONFIG_DR[dataRate];
        }

        public int GetConversionValue(byte low, byte high)
        {
            // Convert to 16-bit signed value.
            var value = (((high & 0xFF) << 8) | (low & 0xFF));
            // Check for sign bit and turn into a negative value if set.
            if ((byte)(value & 0x8000) != 0)
            {
                value -= 1 << 16;
            }
            return value;
        }

        /// <summary>
        /// Perform an ADC read with the provided mux, gain, dataRate, and mode values
        /// </summary>
        /// <param name="mux"></param>
        /// <param name="gain"></param>
        /// <param name="dataRate"></param>
        /// <param name="mode"></param>
        /// <returns>signed integer result of the read.</returns>
        public int Read(int mux, double gain, int? dataRate, int mode)
        {
            var config = ADS1x15_CONFIG_OS_SINGLE;
            // Specify mux value.
            config |= (mux & 0x07) << ADS1x15_CONFIG_MUX_OFFSET;
            // Validate the passed in gain and then set it in the config.
            if (!ADS1x15_CONFIG_GAIN.ContainsKey(gain))
            {
                throw new ArgumentException("Gain must be one of: 2/3, 1, 2, 4, 8, 16");
            }
            config |= ADS1x15_CONFIG_GAIN[gain];
            // Set the mode (continuous or single shot).
            config |= mode;
            // Get the default data rate if none is specified (default differs between ADS1015 and ADS1115).
            dataRate ??= GetDataRateDefault();
            // Set the data rate (this is controlled by the subclass as it differs between ADS1015 and ADS1115).
            config |= GetDataRateConfig(dataRate.Value);
            config |= ADS1x15_CONFIG_COMP_QUE_DISABLE;
            // Send the config value to start the ADC conversion.
            // Explicitly break the 16-bit value down to a big endian pair of bytes.
            //_i2C.writeList(ADS1x15_POINTER_CONFIG, new List<object> {config >> 8 & 0xFF, config & 0xFF});
            _i2C.Write(new [] { ADS1x15_POINTER_CONFIG, (byte)(config >> 8 & 0xFF), (byte)(config & 0xFF) });

            // Wait for the ADC sample to finish based on the sample rate plus a
            // small offset to be sure (0.1 millisecond).
            //Thread.Sleep(1.0 / dataRate + 0.0001);
            Thread.Sleep(1 + (int)(1.0 / dataRate * 0.001));
            // Retrieve the result.
            //var result = _i2C.readList(ADS1x15_POINTER_CONVERSION, 2);
            var result0 = _i2C.ReadAddressByte(ADS1x15_POINTER_CONVERSION);
            var result1 = _i2C.ReadAddressByte(ADS1x15_POINTER_CONVERSION + 1);
            return GetConversionValue(result1, result0);
        }

        /// <summary>
        /// Perform an ADC read with the provided mux, gain, dataRate, and values and with the comparator enabled as specified.  Returns the 
        /// </summary>
        /// <param name="mux"></param>
        /// <param name="gain"></param>
        /// <param name="dataRate"></param>
        /// <param name="mode"></param>
        /// <param name="highThreshold"></param>
        /// <param name="lowThreshold"></param>
        /// <param name="activeLow"></param>
        /// <param name="traditional"></param>
        /// <param name="latching"></param>
        /// <param name="numReadings"></param>
        /// <returns>Signed integer result of the read.</returns>
        public virtual int ReadComparator(int mux, double gain, int? dataRate, int mode, int highThreshold, int lowThreshold, bool activeLow, bool traditional, bool latching, int numReadings)
        {
            if (!(numReadings == 1 || numReadings == 2 || numReadings == 4))
            {
                throw new ArgumentException($"{nameof(numReadings)} must be 1, 2, or 4!");
            }
            if (!ADS1x15_CONFIG_GAIN.ContainsKey(gain))
            {
                throw new ArgumentException("Gain must be one of: 2/3, 1, 2, 4, 8, 16");
            }
            // Set high and low threshold register values.
            //_i2C.writeList(ADS1x15_POINTER_highThreshold, new List<byte> {highThreshold >> 8 & 0xFF, highThreshold & 0xFF});
            _i2C.Write(new [] { ADS1x15_POINTER_highThreshold, (byte)(highThreshold >> 8 & 0xFF), (byte)(highThreshold & 0xFF) });
            //i2C.writeList(ADS1x15_POINTER_lowThreshold, new List<byte> {lowThreshold >> 8 & 0xFF, lowThreshold & 0xFF});
            _i2C.Write(new [] { ADS1x15_POINTER_lowThreshold, (byte)(lowThreshold >> 8 & 0xFF), (byte)(lowThreshold & 0xFF) });
            // Now build up the appropriate config register value.
            var config = ADS1x15_CONFIG_OS_SINGLE;
            // Specify mux value.
            config |= (mux & 0x07) << ADS1x15_CONFIG_MUX_OFFSET;
            // Validate the passed in gain and then set it in the config.
            config |= ADS1x15_CONFIG_GAIN[gain];
            // Set the mode (continuous or single shot).
            config |= mode;
            // Get the default data rate if none is specified (default differs between
            // ADS1015 and ADS1115).
            dataRate ??= GetDataRateDefault();
            // Set the data rate (this is controlled by the subclass as it differs
            // between ADS1015 and ADS1115).
            config |= GetDataRateConfig(dataRate.Value);
            // Enable window mode if required.
            if (!traditional)
            {
                config |= ADS1x15_CONFIG_COMP_WINDOW;
            }
            // Enable active high mode if required.
            if (!activeLow)
            {
                config |= ADS1x15_CONFIG_COMP_ACTIVE_HIGH;
            }
            // Enable latching mode if required.
            if (latching)
            {
                config |= ADS1x15_CONFIG_COMP_LATCHING;
            }
            // Set number of comparator hits before alerting.
            config |= ADS1x15_CONFIG_COMP_QUE[numReadings];
            // Send the config value to start the ADC conversion. Explicitly break the 16-bit value down to a big endian pair of bytes.
            //            _i2C.writeList(ADS1x15_POINTER_CONFIG, new List<object> {config >> 8 & 0xFF, config & 0xFF});
            _i2C.Write(new [] { ADS1x15_POINTER_CONFIG, (byte)(config >> 8 & 0xFF), (byte)(config & 0xFF) });
            // Wait for the ADC sample to finish based on the sample rate plus a small offset to be sure (0.1 millisecond).
            //Thread.Sleep(1.0 / dataRate + 0.0001);
            Thread.Sleep(1 + (int)(1.0 / dataRate * 0.001));
            // Retrieve the result.
            //var result = _i2C.readList(ADS1x15_POINTER_CONVERSION, 2);
            //            return _conversion_value(result[1], result[0]);
            var result0 = _i2C.ReadAddressByte(ADS1x15_POINTER_CONVERSION);
            var result1 = _i2C.ReadAddressByte(ADS1x15_POINTER_CONVERSION + 1);
            return GetConversionValue(result1, result0);
        }

        /// <summary>
        /// Read a single ADC channel and return the ADC value 
        /// </summary>
        /// <param name="channel">within 0-3</param>
        /// <param name="gain"></param>
        /// <param name="dataRate">Optional</param>
        /// <returns>signed integer </returns>
        public int ReadAdc(byte channel, double gain = 1, int? dataRate = null)
        {
            if (0 < channel || channel > 3)
            {
                throw new ArgumentException($"{nameof(channel)} must be between 0 and 3");
            }
            // Perform a single shot read and set the mux value to the channel plus the highest bit (bit 3) set.
            return Read(channel + 0x04, gain, dataRate, ADS1x15_CONFIG_MODE_SINGLE);
        }

        /// <summary>
        /// Read the difference between two ADC channels
        /// </summary>
        /// <param name="differential">must be one of 0 = Channel 0 minus channel 1, 1 = Channel 0 minus channel 3, 2 = Channel 1 minus channel 3, 3 = Channel 2 minus channel 3</param>
        /// <param name="gain"></param>
        /// <param name="dataRate"></param>
        /// <returns>the ADC value as a signed integer result</returns>
        public int ReadAdcDifference(int differential, double gain = 1, int? dataRate = null)
        {
            if (0 < differential || differential > 3)
            {
                throw new ArgumentException($"{nameof(differential)} must be between 0 and 3");
            }
            // Perform a single shot read using the provided differential value
            // as the mux value (which will enable differential mode).
            return Read(differential, gain, dataRate, ADS1x15_CONFIG_MODE_SINGLE);
        }

        /// <summary>
        /// Start continuous ADC conversions on the specified channel. Then call the GetLastResult() to read the most recent conversion result. Call StopAdc() to stop conversions.
        /// </summary>
        /// <param name="channel">0-3</param>
        /// <param name="gain"></param>
        /// <param name="dataRate"></param>
        /// <returns>initial conversion result</returns>
        public object StartAdc(int channel, double gain = 1, int? dataRate = null)
        {
            if (0 < channel || channel > 3)
            {
                throw new ArgumentException($"{nameof(channel)} must be between 0 and 3");
            }
            // Start continuous reads and set the mux value to the channel plus the highest bit (bit 3) set.
            return Read(channel + 0x04, gain, dataRate, ADS1x15_CONFIG_MODE_CONTINUOUS);
        }

        /// <summary>
        /// Start continuous ADC conversions between two ADC channels. Then call the GetLastResult() function continuously to read the most recent conversion result.  Call StopAdc() to stop conversions.
        /// </summary>
        /// <param name="differential">must be one of: 0 = Channel 0 minus channel 1, 1 = Channel 0 minus channel 3, 2 = Channel 1 minus channel 3, 3 = Channel 2 minus channel 3</param>
        /// <param name="gain"></param>
        /// <param name="dataRate"></param>
        /// <returns>an initial conversion result</returns>
        public virtual object StartAdcDifference(int differential, double gain = 1, int? dataRate = null)
        {
            if (0 < differential || differential > 3)
            {
                throw new ArgumentException($"{nameof(differential)} must be between 0 and 3");
            }
            // Perform a single shot read using the provided differential value
            // as the mux value (which will enable differential mode).
            return Read(differential, gain, dataRate, ADS1x15_CONFIG_MODE_CONTINUOUS);
        }

        /// <summary>
        /// Start continuous ADC conversions on the specified channel with the comparator enabled. When enabled the comparator to will check if the ADC value is within the highThreshold &
        /// lowThreshold value and trigger the ALERT pin. Then call the GetLastResult() function continuously to read the most recent conversion result. Call StopAdc() to stop conversions.
        /// </summary>
        /// <param name="channel">0-3</param>
        /// <param name="highThreshold">signed 16 bit int</param>
        /// <param name="lowThreshold">signed 16 bit int</param>
        /// <param name="gain"></param>
        /// <param name="dataRate"></param>
        /// <param name="activeLow">Boolean that indicates if ALERT is pulled low or high when active/triggered. Default is true</param>
        /// <param name="traditional">Boolean that indicates if the comparator is in traditional mode where it fires when the value is within the threshold,
        ///                          or in window mode where it fires when the value is _outside_ the threshold range.  Default is true</param>
        /// <param name="latching">Boolean that indicates if the alert should be held until GetLastResult() is called to read the value and clear
        ///                       the alert.  Default is false, non-latching.</param>
        /// <param name="numReadings">The number of readings that match the comparator before triggering the alert.  Can be 1, 2, or 4.  Default is 1.</param>
        /// <returns>initial conversion result</returns>
        public virtual object StartAdcComparator(byte channel, int highThreshold, int lowThreshold, double gain = 1, int? dataRate = null, bool activeLow = true, bool traditional = true,
            bool latching = false, int numReadings = 1)
        {
            if (0 < channel || channel > 3)
            {
                throw new ArgumentException($"{nameof(channel)} must be between 0 and 3");
            }

            // Start continuous reads with comparator and set the mux value to the channel plus the highest bit (bit 3) set.
            return ReadComparator(channel + 0x04, gain, dataRate, ADS1x15_CONFIG_MODE_CONTINUOUS, highThreshold, lowThreshold, activeLow, traditional, latching, numReadings);
        }

        /// <summary>
        /// Start continuous ADC conversions between two channels with the comparator enabled. See startAdcDifference for valid differential parameter values and their meaning.
        /// When enabled the comparator to will check if the ADC value is within the highThreshold & lowThreshold value (both should be signed 16-bit integers) and trigger the ALERT pin.
        /// </summary>
        /// <param name="differential"></param>
        /// <param name="highThreshold"></param>
        /// <param name="lowThreshold"></param>
        /// <param name="gain"></param>
        /// <param name="dataRate"></param>
        /// <param name="activeLow">Boolean that indicates if ALERT is pulled low or high when active/triggered.  Default is true, active low.</param>
        /// <param name="traditional">Boolean that indicates if the comparator is in traditional mode where it fires when the value is within the threshold, or in window mode where it fires when the value is _outside_ the threshold range.  Default is true, traditional mode.</param>
        /// <param name="latching">Boolean that indicates if the alert should be held until getLastResult() is called to read the value and clear the alert. Default is false, non-latching.</param>
        /// <param name="numReadings">The number of readings that match the comparator before triggering the alert.  Can be 1, 2, or 4.  Default is 1.</param>
        /// <returns>Will return an initial conversion result, then call the getLastResult() function continuously to read the most recent conversion result. Call StopAdc() to stop conversions.</returns>
        public virtual object StartAdcDifferenceComparator(int differential, int highThreshold, int lowThreshold, double gain = 1, int? dataRate = null, bool activeLow = true, bool traditional = true,
            bool latching = false, int numReadings = 1)
        {
            if (0 < differential || differential > 3)
            {
                throw new ArgumentException($"{nameof(differential)} must be between 0 and 3");
            }
            // Start continuous reads with comparator and set the mux value to the channel plus the highest bit (bit 3) set.
            return ReadComparator(differential, gain, dataRate, ADS1x15_CONFIG_MODE_CONTINUOUS, highThreshold, lowThreshold, activeLow, traditional, latching, numReadings);
        }

         
        /// <summary>
        /// Stop all continuous ADC conversions (either normal or difference mode).
        /// </summary>         
        public void StopAdc()
        {
            // Set the config register to its default value of 0x8583 to stop
            // continuous conversions.
            var config = 0x8583;
            //_i2C.writeList(ADS1x15_POINTER_CONFIG, new List<object> {config >> 8 & 0xFF, config & 0xFF});
            _i2C.Write(new [] { ADS1x15_POINTER_CONFIG, (byte)(config >> 8 & 0xFF), (byte)(config & 0xFF) });
        }

        /// <summary>
        /// Read the last conversion result when in continuous conversion mode. 
        /// </summary>
        /// <returns>Will return a signed integer value.</returns>
        public int GetLastResult()
        {
            // Retrieve the conversion register value, convert to a signed int, and
            // return it.
            //var result = _i2C.readList(ADS1x15_POINTER_CONVERSION, 2);
            //return _conversion_value(result[1], result[0]);
            var result0 = _i2C.ReadAddressByte(ADS1x15_POINTER_CONVERSION);
            var result1 = _i2C.ReadAddressByte(ADS1x15_POINTER_CONVERSION + 1);
            return GetConversionValue(result1, result0);
        }
    }

}

