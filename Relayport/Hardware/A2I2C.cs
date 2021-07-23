//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using Unosquare.RaspberryIO.Abstractions;

//namespace Control.Hardware {
    
//    public class A2I2C {

//        /// <summary>
//        /// The I2C device instance to be controlled. 
//        /// </summary>
//        private II2CDevice _i2C;


//        public object ADS1x15_DEFAULT_ADDRESS = 0x48;
        
//        public object ADS1x15_POINTER_CONVERSION = 0x00;
        
//        public object ADS1x15_POINTER_CONFIG = 0x01;
        
//        public object ADS1x15_POINTER_LOW_THRESHOLD = 0x02;
        
//        public object ADS1x15_POINTER_HIGH_THRESHOLD = 0x03;
        
//        public object ADS1x15_CONFIG_OS_SINGLE = 0x8000;
        
//        public object ADS1x15_CONFIG_MUX_OFFSET = 12;
        
//        public object ADS1x15_CONFIG_GAIN = new Dictionary<object, object> {
//            {
//                2 / 3,
//                0x0000},
//            {
//                1,
//                0x0200},
//            {
//                2,
//                0x0400},
//            {
//                4,
//                0x0600},
//            {
//                8,
//                0x0800},
//            {
//                16,
//                0x0A00}};
        
//        public object ADS1x15_CONFIG_MODE_CONTINUOUS = 0x0000;
        
//        public object ADS1x15_CONFIG_MODE_SINGLE = 0x0100;
        
//        public object ADS1015_CONFIG_DR = new Dictionary<object, object> {
//            {
//                128,
//                0x0000},
//            {
//                250,
//                0x0020},
//            {
//                490,
//                0x0040},
//            {
//                920,
//                0x0060},
//            {
//                1600,
//                0x0080},
//            {
//                2400,
//                0x00A0},
//            {
//                3300,
//                0x00C0}};
        
//        public object ADS1115_CONFIG_DR = new Dictionary<object, object> {
//            {
//                8,
//                0x0000},
//            {
//                16,
//                0x0020},
//            {
//                32,
//                0x0040},
//            {
//                64,
//                0x0060},
//            {
//                128,
//                0x0080},
//            {
//                250,
//                0x00A0},
//            {
//                475,
//                0x00C0},
//            {
//                860,
//                0x00E0}};
        
//        public object ADS1x15_CONFIG_COMP_WINDOW = 0x0010;
        
//        public object ADS1x15_CONFIG_COMP_ACTIVE_HIGH = 0x0008;
        
//        public object ADS1x15_CONFIG_COMP_LATCHING = 0x0004;
        
//        public object ADS1x15_CONFIG_COMP_QUE = new Dictionary<object, object> {
//            {
//                1,
//                0x0000},
//            {
//                2,
//                0x0001},
//            {
//                4,
//                0x0002}};
        
//        public object ADS1x15_CONFIG_COMP_QUE_DISABLE = 0x0003;
        
//        // Base functionality for ADS1x15 analog to digital converters.
//        public class ADS1x15
//            : object {
            
//            public ADS1x15(byte address = ADS1x15_DEFAULT_ADDRESS, object i2c = null, Hashtable kwargs) {
//                if (i2c == null) {
//                    i2c = I2C;
//                }
//                this._device = i2c.get_i2c_device(address, kwargs);
//            }
            
//            // Retrieve the default data rate for this ADC (in samples per second).
//            //         Should be implemented by subclasses.
//            //         
//            public virtual object _data_rate_default() {
//                throw new NotImplementedException("Subclasses must implement _data_rate_default!");
//            }
            
//            // Subclasses should override this function and return a 16-bit value
//            //         that can be OR'ed with the config register to set the specified
//            //         data rate.  If a value of None is specified then a default data_rate
//            //         setting should be returned.  If an invalid or unsupported data_rate is
//            //         provided then an exception should be thrown.
//            //         
//            public virtual object _data_rate_config(object data_rate) {
//                throw new NotImplementedException("Subclass must implement _data_rate_config function!");
//            }
            
//            // Subclasses should override this function that takes the low and high
//            //         byte of a conversion result and returns a signed integer value.
//            //         
//            public virtual object _conversion_value(object low, object high) {
//                throw new NotImplementedException("Subclass must implement _conversion_value function!");
//            }
            
//            // Perform an ADC read with the provided mux, gain, data_rate, and mode
//            //         values.  Returns the signed integer result of the read.
//            //         
//            public virtual object _read(object mux, object gain, object data_rate, object mode) {
//                var config = ADS1x15_CONFIG_OS_SINGLE;
//                // Specify mux value.
//                config |= (mux & 0x07) << ADS1x15_CONFIG_MUX_OFFSET;
//                // Validate the passed in gain and then set it in the config.
//                if (!ADS1x15_CONFIG_GAIN.Contains(gain)) {
//                    throw new ArgumentException(("Gain must be one of: 2/3, 1, 2, 4, 8, 16");
//                }
//                config |= ADS1x15_CONFIG_GAIN[gain];
//                // Set the mode (continuous or single shot).
//                config |= mode;
//                // Get the default data rate if none is specified (default differs between
//                // ADS1015 and ADS1115).
//                if (data_rate == null) {
//                    data_rate = this._data_rate_default();
//                }
//                // Set the data rate (this is controlled by the subclass as it differs
//                // between ADS1015 and ADS1115).
//                config |= this._data_rate_config(data_rate);
//                config |= ADS1x15_CONFIG_COMP_QUE_DISABLE;
//                // Send the config value to start the ADC conversion.
//                // Explicitly break the 16-bit value down to a big endian pair of bytes.
//                this._device.writeList(ADS1x15_POINTER_CONFIG, new List<object> {
//                    config >> 8 & 0xFF,
//                    config & 0xFF
//                });
//                // Wait for the ADC sample to finish based on the sample rate plus a
//                // small offset to be sure (0.1 millisecond).
//                time.sleep(1.0 / data_rate + 0.0001);
//                // Retrieve the result.
//                var result = this._device.readList(ADS1x15_POINTER_CONVERSION, 2);
//                return this._conversion_value(result[1], result[0]);
//            }
            
//            // Perform an ADC read with the provided mux, gain, data_rate, and mode
//            //         values and with the comparator enabled as specified.  Returns the signed
//            //         integer result of the read.
//            //         
//            public virtual object _read_comparator(
//                object mux,
//                object gain,
//                object data_rate,
//                object mode,
//                object high_threshold,
//                object low_threshold,
//                object active_low,
//                object traditional,
//                object latching,
//                int num_readings) {
//                Debug.Assert(num_readings == 1 || num_readings == 2 || num_readings == 4);
//                Console.WriteLine("Num readings must be 1, 2, or 4!");
//                // Set high and low threshold register values.
//                this._device.writeList(ADS1x15_POINTER_HIGH_THRESHOLD, new List<object> {
//                    high_threshold >> 8 & 0xFF,
//                    high_threshold & 0xFF
//                });
//                this._device.writeList(ADS1x15_POINTER_LOW_THRESHOLD, new List<object> {
//                    low_threshold >> 8 & 0xFF,
//                    low_threshold & 0xFF
//                });
//                // Now build up the appropriate config register value.
//                var config = ADS1x15_CONFIG_OS_SINGLE;
//                // Specify mux value.
//                config |= (mux & 0x07) << ADS1x15_CONFIG_MUX_OFFSET;
//                // Validate the passed in gain and then set it in the config.
//                if (!ADS1x15_CONFIG_GAIN.Contains(gain)) {
//                    throw ArgumentException("Gain must be one of: 2/3, 1, 2, 4, 8, 16");
//                }
//                config |= ADS1x15_CONFIG_GAIN[gain];
//                // Set the mode (continuous or single shot).
//                config |= mode;
//                // Get the default data rate if none is specified (default differs between
//                // ADS1015 and ADS1115).
//                if (data_rate == null) {
//                    data_rate = this._data_rate_default();
//                }
//                // Set the data rate (this is controlled by the subclass as it differs
//                // between ADS1015 and ADS1115).
//                config |= this._data_rate_config(data_rate);
//                // Enable window mode if required.
//                if (!traditional) {
//                    config |= ADS1x15_CONFIG_COMP_WINDOW;
//                }
//                // Enable active high mode if required.
//                if (!active_low) {
//                    config |= ADS1x15_CONFIG_COMP_ACTIVE_HIGH;
//                }
//                // Enable latching mode if required.
//                if (latching) {
//                    config |= ADS1x15_CONFIG_COMP_LATCHING;
//                }
//                // Set number of comparator hits before alerting.
//                config |= ADS1x15_CONFIG_COMP_QUE[num_readings];
//                // Send the config value to start the ADC conversion.
//                // Explicitly break the 16-bit value down to a big endian pair of bytes.
//                this._device.writeList(ADS1x15_POINTER_CONFIG, new List<object> {
//                    config >> 8 & 0xFF,
//                    config & 0xFF
//                });
//                // Wait for the ADC sample to finish based on the sample rate plus a
//                // small offset to be sure (0.1 millisecond).
//                time.sleep(1.0 / data_rate + 0.0001);
//                // Retrieve the result.
//                var result = this._device.readList(ADS1x15_POINTER_CONVERSION, 2);
//                return this._conversion_value(result[1], result[0]);
//            }
            
//            // Read a single ADC channel and return the ADC value as a signed integer
//            //         result.  Channel must be a value within 0-3.
//            //         
//            public virtual object read_adc(object channel, object gain = 1, object data_rate = null) {
//                Debug.Assert(0 <= channel <= 3);
//                Debug.Assert("Channel must be a value within 0-3!");
//                // Perform a single shot read and set the mux value to the channel plus
//                // the highest bit (bit 3) set.
//                return this._read(channel + 0x04, gain, data_rate, ADS1x15_CONFIG_MODE_SINGLE);
//            }
            
//            // Read the difference between two ADC channels and return the ADC value
//            //         as a signed integer result.  Differential must be one of:
//            //           - 0 = Channel 0 minus channel 1
//            //           - 1 = Channel 0 minus channel 3
//            //           - 2 = Channel 1 minus channel 3
//            //           - 3 = Channel 2 minus channel 3
//            //         
//            public virtual object read_adc_difference(object differential, object gain = 1, object data_rate = null) {
//                Debug.Assert(0 <= differential <= 3);
//                Debug.Assert("Differential must be a value within 0-3!");
//                // Perform a single shot read using the provided differential value
//                // as the mux value (which will enable differential mode).
//                return this._read(differential, gain, data_rate, ADS1x15_CONFIG_MODE_SINGLE);
//            }
            
//            // Start continuous ADC conversions on the specified channel (0-3). Will
//            //         return an initial conversion result, then call the get_last_result()
//            //         function to read the most recent conversion result. Call stop_adc() to
//            //         stop conversions.
//            //         
//            public virtual object start_adc(object channel, object gain = 1, object data_rate = null) {
//                Debug.Assert(0 <= channel <= 3);
//                Debug.Assert("Channel must be a value within 0-3!");
//                // Start continuous reads and set the mux value to the channel plus
//                // the highest bit (bit 3) set.
//                return this._read(channel + 0x04, gain, data_rate, ADS1x15_CONFIG_MODE_CONTINUOUS);
//            }
            
//            // Start continuous ADC conversions between two ADC channels. Differential
//            //         must be one of:
//            //           - 0 = Channel 0 minus channel 1
//            //           - 1 = Channel 0 minus channel 3
//            //           - 2 = Channel 1 minus channel 3
//            //           - 3 = Channel 2 minus channel 3
//            //         Will return an initial conversion result, then call the get_last_result()
//            //         function continuously to read the most recent conversion result.  Call
//            //         stop_adc() to stop conversions.
//            //         
//            public virtual object start_adc_difference(object differential, object gain = 1, object data_rate = null) {
//                Debug.Assert(0 <= differential <= 3);
//                Debug.Assert("Differential must be a value within 0-3!");
//                // Perform a single shot read using the provided differential value
//                // as the mux value (which will enable differential mode).
//                return this._read(differential, gain, data_rate, ADS1x15_CONFIG_MODE_CONTINUOUS);
//            }
            
//            // Start continuous ADC conversions on the specified channel (0-3) with
//            //         the comparator enabled.  When enabled the comparator to will check if
//            //         the ADC value is within the high_threshold & low_threshold value (both
//            //         should be signed 16-bit integers) and trigger the ALERT pin.  The
//            //         behavior can be controlled by the following parameters:
//            //           - active_low: Boolean that indicates if ALERT is pulled low or high
//            //                         when active/triggered.  Default is true, active low.
//            //           - traditional: Boolean that indicates if the comparator is in traditional
//            //                          mode where it fires when the value is within the threshold,
//            //                          or in window mode where it fires when the value is _outside_
//            //                          the threshold range.  Default is true, traditional mode.
//            //           - latching: Boolean that indicates if the alert should be held until
//            //                       get_last_result() is called to read the value and clear
//            //                       the alert.  Default is false, non-latching.
//            //           - num_readings: The number of readings that match the comparator before
//            //                           triggering the alert.  Can be 1, 2, or 4.  Default is 1.
//            //         Will return an initial conversion result, then call the get_last_result()
//            //         function continuously to read the most recent conversion result.  Call
//            //         stop_adc() to stop conversions.
//            //         
//            public virtual object start_adc_comparator(
//                object channel,
//                object high_threshold,
//                object low_threshold,
//                object gain = 1,
//                object data_rate = null,
//                object active_low = true,
//                object traditional = true,
//                object latching = false,
//                object num_readings = 1) {
//                Debug.Assert(0 <= channel <= 3);
//                Debug.Assert("Channel must be a value within 0-3!");
//                // Start continuous reads with comparator and set the mux value to the
//                // channel plus the highest bit (bit 3) set.
//                return this._read_comparator(channel + 0x04, gain, data_rate, ADS1x15_CONFIG_MODE_CONTINUOUS, high_threshold, low_threshold, active_low, traditional, latching, num_readings);
//            }
            
//            // Start continuous ADC conversions between two channels with
//            //         the comparator enabled.  See start_adc_difference for valid differential
//            //         parameter values and their meaning.  When enabled the comparator to will
//            //         check if the ADC value is within the high_threshold & low_threshold value
//            //         (both should be signed 16-bit integers) and trigger the ALERT pin.  The
//            //         behavior can be controlled by the following parameters:
//            //           - active_low: Boolean that indicates if ALERT is pulled low or high
//            //                         when active/triggered.  Default is true, active low.
//            //           - traditional: Boolean that indicates if the comparator is in traditional
//            //                          mode where it fires when the value is within the threshold,
//            //                          or in window mode where it fires when the value is _outside_
//            //                          the threshold range.  Default is true, traditional mode.
//            //           - latching: Boolean that indicates if the alert should be held until
//            //                       get_last_result() is called to read the value and clear
//            //                       the alert.  Default is false, non-latching.
//            //           - num_readings: The number of readings that match the comparator before
//            //                           triggering the alert.  Can be 1, 2, or 4.  Default is 1.
//            //         Will return an initial conversion result, then call the get_last_result()
//            //         function continuously to read the most recent conversion result.  Call
//            //         stop_adc() to stop conversions.
//            //         
//            public virtual object start_adc_difference_comparator(
//                object differential,
//                object high_threshold,
//                object low_threshold,
//                object gain = 1,
//                object data_rate = null,
//                object active_low = true,
//                object traditional = true,
//                object latching = false,
//                object num_readings = 1) {
//                Debug.Assert(0 <= differential <= 3);
//                Debug.Assert("Differential must be a value within 0-3!");
//                // Start continuous reads with comparator and set the mux value to the
//                // channel plus the highest bit (bit 3) set.
//                return this._read_comparator(differential, gain, data_rate, ADS1x15_CONFIG_MODE_CONTINUOUS, high_threshold, low_threshold, active_low, traditional, latching, num_readings);
//            }
            
//            // Stop all continuous ADC conversions (either normal or difference mode).
//            //         
//            public virtual object stop_adc() {
//                // Set the config register to its default value of 0x8583 to stop
//                // continuous conversions.
//                var config = 0x8583;
//                this._device.writeList(ADS1x15_POINTER_CONFIG, new List<object> {
//                    config >> 8 & 0xFF,
//                    config & 0xFF
//                });
//            }
            
//            // Read the last conversion result when in continuous conversion mode.
//            //         Will return a signed integer value.
//            //         
//            public virtual object get_last_result() {
//                // Retrieve the conversion register value, convert to a signed int, and
//                // return it.
//                var result = this._device.readList(ADS1x15_POINTER_CONVERSION, 2);
//                return this._conversion_value(result[1], result[0]);
//            }
//        }
        
//        // ADS1115 16-bit analog to digital converter instance.
//        public class ADS1115
//            : ADS1x15 {
            
//            public ADS1115(Hashtable kwargs, params object [] args)
//                : base(kwargs) {
//            }
            
//            public virtual object _data_rate_default() {
//                // Default from datasheet page 16, config register DR bit default.
//                return 128;
//            }
            
//            public virtual object _data_rate_config(object data_rate) {
//                if (!ADS1115_CONFIG_DR.Contains(data_rate)) {
//                    throw ArgumentException("Data rate must be one of: 8, 16, 32, 64, 128, 250, 475, 860");
//                }
//                return ADS1115_CONFIG_DR[data_rate];
//            }
            
//            public virtual object _conversion_value(object low, object high) {
//                // Convert to 16-bit signed value.
//                var value = (high & 0xFF) << 8 | low & 0xFF;
//                // Check for sign bit and turn into a negative value if set.
//                if ((value & 0x8000) != 0) {
//                    value -= 1 << 16;
//                }
//                return value;
//            }
//        }
        
//        // ADS1015 12-bit analog to digital converter instance.
//        public class ADS1015
//            : ADS1x15 {
            
//            public ADS1015(Hashtable kwargs, params object [] args)
//                : base(kwargs) {
//            }
            
//            public virtual object _data_rate_default() {
//                // Default from datasheet page 19, config register DR bit default.
//                return 1600;
//            }
            
//            public virtual object _data_rate_config(object data_rate) {
//                if (!ADS1015_CONFIG_DR.Contains(data_rate)) {
//                    throw ArgumentException("Data rate must be one of: 128, 250, 490, 920, 1600, 2400, 3300");
//                }
//                return ADS1015_CONFIG_DR[data_rate];
//            }
            
//            public virtual object _conversion_value(object low, object high) {
//                // Convert to 12-bit signed value.
//                var value = (high & 0xFF) << 4 | (low & 0xFF) >> 4;
//                // Check for sign bit and turn into a negative value if set.
//                if ((value & 0x800) != 0) {
//                    value -= 1 << 12;
//                }
//                return value;
//            }
//        }
//    }
//}
