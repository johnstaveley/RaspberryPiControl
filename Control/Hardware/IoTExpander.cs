//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Control.Hardware
//{
    
        
//        public  object @__version__ = "0.0.2";
        
//        public  object I2C_ADDR = 0x18;
        
//        public  object CHIP_ID = 0xE26A;
        
//        public  object CHIP_VERSION = 2;
        
//        public  object REG_CHIP_ID_L = 0xfa;
        
//        public  object REG_CHIP_ID_H = 0xfb;
        
//        public  object REG_VERSION = 0xfc;
        
//        public  object REG_ENC_EN = 0x04;
        
//        public  object BIT_ENC_EN_1 = 0;
        
//        public  object BIT_ENC_MICROSTEP_1 = 1;
        
//        public  object BIT_ENC_EN_2 = 2;
        
//        public  object BIT_ENC_MICROSTEP_2 = 3;
        
//        public  object BIT_ENC_EN_3 = 4;
        
//        public  object BIT_ENC_MICROSTEP_3 = 5;
        
//        public  object BIT_ENC_EN_4 = 6;
        
//        public  object BIT_ENC_MICROSTEP_4 = 7;
        
//        public  object REG_ENC_1_CFG = 0x05;
        
//        public  object REG_ENC_1_COUNT = 0x06;
        
//        public  object REG_ENC_2_CFG = 0x07;
        
//        public  object REG_ENC_2_COUNT = 0x08;
        
//        public  object REG_ENC_3_CFG = 0x09;
        
//        public  object REG_ENC_3_COUNT = 0x0A;
        
//        public  object REG_ENC_4_CFG = 0x0B;
        
//        public  object REG_ENC_4_COUNT = 0x0C;
        
//        public  object REG_CAPTOUCH_EN = 0x0D;
        
//        public  object REG_CAPTOUCH_CFG = 0x0E;
        
//        public  object REG_CAPTOUCH_0 = 0x0F;
        
//        public  object REG_SWITCH_EN_P0 = 0x17;
        
//        public  object REG_SWITCH_EN_P1 = 0x18;
        
//        public  object REG_SWITCH_P00 = 0x19;
        
//        public  object REG_SWITCH_P10 = 0x21;
        
//        public  object REG_USER_FLASH = 0xD0;
        
//        public  object REG_FLASH_PAGE = 0xF0;
        
//        public  object REG_DEBUG = 0xF8;
        
//        public  object REG_P0 = 0x40;
        
//        public  object REG_SP = 0x41;
        
//        public  object REG_DPL = 0x42;
        
//        public  object REG_DPH = 0x43;
        
//        public  object REG_RCTRIM0 = 0x44;
        
//        public  object REG_RCTRIM1 = 0x45;
        
//        public  object REG_RWK = 0x46;
        
//        public  object REG_PCON = 0x47;
        
//        public  object REG_TCON = 0x48;
        
//        public  object REG_TMOD = 0x49;
        
//        public  object REG_TL0 = 0x4a;
        
//        public  object REG_TL1 = 0x4b;
        
//        public  object REG_TH0 = 0x4c;
        
//        public  object REG_TH1 = 0x4d;
        
//        public  object REG_CKCON = 0x4e;
        
//        public  object REG_WKCON = 0x4f;
        
//        public  object REG_P1 = 0x50;
        
//        public  object REG_SFRS = 0x51;
        
//        public  object REG_CAPCON0 = 0x52;
        
//        public  object REG_CAPCON1 = 0x53;
        
//        public  object REG_CAPCON2 = 0x54;
        
//        public  object REG_CKDIV = 0x55;
        
//        public  object REG_CKSWT = 0x56;
        
//        public  object REG_CKEN = 0x57;
        
//        public  object REG_SCON = 0x58;
        
//        public  object REG_SBUF = 0x59;
        
//        public  object REG_SBUF_1 = 0x5a;
        
//        public  object REG_EIE = 0x5b;
        
//        public  object REG_EIE1 = 0x5c;
        
//        public  object REG_CHPCON = 0x5f;
        
//        public  object REG_P2 = 0x60;
        
//        public  object REG_AUXR1 = 0x62;
        
//        public  object REG_BODCON0 = 0x63;
        
//        public  object REG_IAPTRG = 0x64;
        
//        public  object REG_IAPUEN = 0x65;
        
//        public  object REG_IAPAL = 0x66;
        
//        public  object REG_IAPAH = 0x67;
        
//        public  object REG_IE = 0x68;
        
//        public  object REG_SADDR = 0x69;
        
//        public  object REG_WDCON = 0x6a;
        
//        public  object REG_BODCON1 = 0x6b;
        
//        public  object REG_P3M1 = 0x6c;
        
//        public  object REG_P3S = 0xc0;
        
//        public  object REG_P3M2 = 0x6d;
        
//        public  object REG_P3SR = 0xc1;
        
//        public  object REG_IAPFD = 0x6e;
        
//        public  object REG_IAPCN = 0x6f;
        
//        public  object REG_P3 = 0x70;
        
//        public  object REG_P0M1 = 0x71;
        
//        public  object REG_P0S = 0xc2;
        
//        public  object REG_P0M2 = 0x72;
        
//        public  object REG_P0SR = 0xc3;
        
//        public  object REG_P1M1 = 0x73;
        
//        public  object REG_P1S = 0xc4;
        
//        public  object REG_P1M2 = 0x74;
        
//        public  object REG_P1SR = 0xc5;
        
//        public  object REG_P2S = 0x75;
        
//        public  object REG_IPH = 0x77;
        
//        public  object REG_PWMINTC = 0xc6;
        
//        public  object REG_IP = 0x78;
        
//        public  object REG_SADEN = 0x79;
        
//        public  object REG_SADEN_1 = 0x7a;
        
//        public  object REG_SADDR_1 = 0x7b;
        
//        public  object REG_I2DAT = 0x7c;
        
//        public  object REG_I2STAT = 0x7d;
        
//        public  object REG_I2CLK = 0x7e;
        
//        public  object REG_I2TOC = 0x7f;
        
//        public  object REG_I2CON = 0x80;
        
//        public  object REG_I2ADDR = 0x81;
        
//        public  object REG_ADCRL = 0x82;
        
//        public  object REG_ADCRH = 0x83;
        
//        public  object REG_T3CON = 0x84;
        
//        public  object REG_PWM4H = 0xc7;
        
//        public  object REG_RL3 = 0x85;
        
//        public  object REG_PWM5H = 0xc8;
        
//        public  object REG_RH3 = 0x86;
        
//        public  object REG_PIOCON1 = 0xc9;
        
//        public  object REG_TA = 0x87;
        
//        public  object REG_T2CON = 0x88;
        
//        public  object REG_T2MOD = 0x89;
        
//        public  object REG_RCMP2L = 0x8a;
        
//        public  object REG_RCMP2H = 0x8b;
        
//        public  object REG_TL2 = 0x8c;
        
//        public  object REG_PWM4L = 0xca;
        
//        public  object REG_TH2 = 0x8d;
        
//        public  object REG_PWM5L = 0xcb;
        
//        public  object REG_ADCMPL = 0x8e;
        
//        public  object REG_ADCMPH = 0x8f;
        
//        public  object REG_PSW = 0x90;
        
//        public  object REG_PWMPH = 0x91;
        
//        public  object REG_PWM0H = 0x92;
        
//        public  object REG_PWM1H = 0x93;
        
//        public  object REG_PWM2H = 0x94;
        
//        public  object REG_PWM3H = 0x95;
        
//        public  object REG_PNP = 0x96;
        
//        public  object REG_FBD = 0x97;
        
//        public  object REG_PWMCON0 = 0x98;
        
//        public  object REG_PWMPL = 0x99;
        
//        public  object REG_PWM0L = 0x9a;
        
//        public  object REG_PWM1L = 0x9b;
        
//        public  object REG_PWM2L = 0x9c;
        
//        public  object REG_PWM3L = 0x9d;
        
//        public  object REG_PIOCON0 = 0x9e;
        
//        public  object REG_PWMCON1 = 0x9f;
        
//        public  object REG_ACC = 0xa0;
        
//        public  object REG_ADCCON1 = 0xa1;
        
//        public  object REG_ADCCON2 = 0xa2;
        
//        public  object REG_ADCDLY = 0xa3;
        
//        public  object REG_C0L = 0xa4;
        
//        public  object REG_C0H = 0xa5;
        
//        public  object REG_C1L = 0xa6;
        
//        public  object REG_C1H = 0xa7;
        
//        public  object REG_ADCCON0 = 0xa8;
        
//        public  object REG_PICON = 0xa9;
        
//        public  object REG_PINEN = 0xaa;
        
//        public  object REG_PIPEN = 0xab;
        
//        public  object REG_PIF = 0xac;
        
//        public  object REG_C2L = 0xad;
        
//        public  object REG_C2H = 0xae;
        
//        public  object REG_EIP = 0xaf;
        
//        public  object REG_B = 0xb0;
        
//        public  object REG_CAPCON3 = 0xb1;
        
//        public  object REG_CAPCON4 = 0xb2;
        
//        public  object REG_SPCR = 0xb3;
        
//        public  object REG_SPCR2 = 0xcc;
        
//        public  object REG_SPSR = 0xb4;
        
//        public  object REG_SPDR = 0xb5;
        
//        public  object REG_AINDIDS = 0xb6;
        
//        public  object REG_EIPH = 0xb7;
        
//        public  object REG_SCON_1 = 0xb8;
        
//        public  object REG_PDTEN = 0xb9;
        
//        public  object REG_PDTCNT = 0xba;
        
//        public  object REG_PMEN = 0xbb;
        
//        public  object REG_PMD = 0xbc;
        
//        public  object REG_EIP1 = 0xbe;
        
//        public  object REG_EIPH1 = 0xbf;
        
//        public  object REG_USER_FLASH = 0xd0;
        
//        public  object REG_FLASH_PAGE = 0xf0;
        
//        public  object REG_INT = 0xf9;
        
//        public  object MASK_INT_TRIG = 0x1;
        
//        public  object MASK_INT_OUT = 0x2;
        
//        public  object BIT_INT_TRIGD = 0;
        
//        public  object BIT_INT_OUT_EN = 1;
        
//        public  object BIT_INT_PIN_SWAP = 2;
        
//        public  object REG_INT_MASK_P0 = 0x00;
        
//        public  object REG_INT_MASK_P1 = 0x01;
        
//        public  object REG_INT_MASK_P3 = 0x03;
        
//        public  object REG_VERSION = 0xfc;
        
//        public  object REG_ADDR = 0xfd;
        
//        public  object REG_CTRL = 0xfe;
        
//        public  object MASK_CTRL_SLEEP = 0x1;
        
//        public  object MASK_CTRL_RESET = 0x2;
        
//        public  object MASK_CTRL_FREAD = 0x4;
        
//        public  object MASK_CTRL_FWRITE = 0x8;
        
//        public  object MASK_CTRL_ADDRWR = 0x10;
        
//        public  object BIT_ADDRESSED_REGS = new List<object> {
//            REG_P0,
//            REG_P1,
//            REG_P2,
//            REG_P3
//        };
        
//        public  object PIN_MODE_IO = 0b00000;
        
//        public  object PIN_MODE_QB = 0b00000;
        
//        public  object PIN_MODE_PP = 0b00001;
        
//        public  object PIN_MODE_IN = 0b00010;
        
//        public  object PIN_MODE_PU = 0b10000;
        
//        public  object PIN_MODE_OD = 0b00011;
        
//        public  object PIN_MODE_PWM = 0b00101;
        
//        public  object PIN_MODE_ADC = 0b01010;
        
//        public  object MODE_NAMES = ("IO", "PWM", "ADC");
        
//        public  object GPIO_NAMES = ("QB", "PP", "IN", "OD");
        
//        public  object STATE_NAMES = ("LOW", "HIGH");
        
//        public  object IN = PIN_MODE_IN;
        
//        public  object IN_PULL_UP = PIN_MODE_PU;
        
//        public  object IN_PU = PIN_MODE_PU;
        
//        public  object OUT = PIN_MODE_PP;
        
//        public  object PWM = PIN_MODE_PWM;
        
//        public  object ADC = PIN_MODE_ADC;
        
//        public  object HIGH = 1;
        
//        public  object LOW = 0;
        
//        public class PIN {
            
//            public PIN(object port, object pin) {
//                if (getattr(this, "type", null) == null) {
//                    this.type = new List<object> {
//                        PIN_MODE_IO
//                    };
//                }
//                this.mode = null;
//                this.port = port;
//                this.pin = pin;
//                // The PxM1 and PxM2 registers encode GPIO MODE
//                // 0 0 = Quasi-bidirectional
//                // 0 1 = Push-pull
//                // 1 0 = Input-only (high-impedance)
//                // 1 1 = Open-drain
//                this.reg_m1 = new List<object> {
//                    REG_P0M1,
//                    REG_P1M1,
//                    -1,
//                    REG_P3M1
//                }[port];
//                this.reg_m2 = new List<object> {
//                    REG_P0M2,
//                    REG_P1M2,
//                    -1,
//                    REG_P3M2
//                }[port];
//                // The Px input register
//                this.reg_p = new List<object> {
//                    REG_P0,
//                    REG_P1,
//                    -1,
//                    REG_P3
//                }[port];
//                // The PxS Schmitt trigger register
//                this.reg_ps = new List<object> {
//                    REG_P0S,
//                    REG_P1S,
//                    -1,
//                    REG_P3S
//                }[port];
//                this.reg_int_mask_p = new List<object> {
//                    REG_INT_MASK_P0,
//                    REG_INT_MASK_P1,
//                    -1,
//                    REG_INT_MASK_P3
//                }[port];
//            }
//        }
        
//        // PWM Pin.
//        // 
//        //     Class to store details of a PWM-enabled pin.
//        // 
//        //     
//        public class PWM_PIN
//            : PIN {
            
//            public PWM_PIN(object port, object pin, object channel, object reg_iopwm)
//                : base(port, pin) {
//                this.type.append(PIN_MODE_PWM);
//                this.pwm_channel = channel;
//                this.reg_iopwm = reg_iopwm;
//                this.reg_pwml = new List<object> {
//                    REG_PWM0L,
//                    REG_PWM1L,
//                    REG_PWM2L,
//                    REG_PWM3L,
//                    REG_PWM4L,
//                    REG_PWM5L
//                }[channel];
//                this.reg_pwmh = new List<object> {
//                    REG_PWM0H,
//                    REG_PWM1H,
//                    REG_PWM2H,
//                    REG_PWM3H,
//                    REG_PWM4H,
//                    REG_PWM5H
//                }[channel];
//            }
//        }
        
//        // ADC Pin.
//        // 
//        //     Class to store details of an ADC-enabled pin.
//        // 
//        //     
//        public class ADC_PIN
//            : PIN {
            
//            public ADC_PIN(object port, object pin, object channel)
//                : base(port, pin) {
//                this.type.append(PIN_MODE_ADC);
//                this.adc_channel = channel;
//            }
//        }
        
//        // ADC/PWM Pin.
//        // 
//        //     Class to store details of an ADC/PWM-enabled pin.
//        // 
//        //     
//        public class ADC_OR_PWM_PIN
//            : ADC_PIN, PWM_PIN {
            
//            public ADC_OR_PWM_PIN(
//                object port,
//                object pin,
//                object adc_channel,
//                object pwm_channel,
//                object reg_iopwm)
//                : base(port, pin, adc_channel) {
//                PWM_PIN.@__init__(this, port, pin, pwm_channel, reg_iopwm);
//            }
//        }
        
//        public class IOE {
            
//            public IOE(
//                object i2c_addr = I2C_ADDR,
//                object interrupt_timeout = 1.0,
//                object interrupt_pin = null,
//                object gpio = null,
//                object skip_chip_id_check = false) {
//                this._i2c_addr = i2c_addr;
//                this._i2c_dev = SMBus(1);
//                this._debug = false;
//                this._vref = 3.3;
//                this._timeout = interrupt_timeout;
//                this._interrupt_pin = interrupt_pin;
//                this._gpio = gpio;
//                this._encoder_offset = new List<object> {
//                    0,
//                    0,
//                    0,
//                    0
//                };
//                this._encoder_last = new List<object> {
//                    0,
//                    0,
//                    0,
//                    0
//                };
//                if (this._interrupt_pin != null) {
//                    if (this._gpio == null) {
//                        this._gpio = GPIO;
//                    }
//                    this._gpio.setwarnings(false);
//                    this._gpio.setmode(GPIO.BCM);
//                    this._gpio.setup(this._interrupt_pin, GPIO.IN, pull_up_down: GPIO.PUD_OFF);
//                    this.enable_interrupt_out();
//                }
//                this._pins = new List<object> {
//                    PWM_PIN(1, 5, 5, REG_PIOCON1),
//                    PWM_PIN(1, 0, 2, REG_PIOCON0),
//                    PWM_PIN(1, 2, 0, REG_PIOCON0),
//                    PWM_PIN(1, 4, 1, REG_PIOCON0),
//                    PWM_PIN(0, 0, 3, REG_PIOCON0),
//                    PWM_PIN(0, 1, 4, REG_PIOCON0),
//                    ADC_OR_PWM_PIN(1, 1, 7, 1, REG_PIOCON0),
//                    ADC_OR_PWM_PIN(0, 3, 6, 5, REG_PIOCON0),
//                    ADC_OR_PWM_PIN(0, 4, 5, 3, REG_PIOCON1),
//                    ADC_PIN(3, 0, 1),
//                    ADC_PIN(0, 6, 3),
//                    ADC_OR_PWM_PIN(0, 5, 4, 2, REG_PIOCON1),
//                    ADC_PIN(0, 7, 2),
//                    ADC_PIN(1, 7, 0)
//                };
//                if (!skip_chip_id_check) {
//                    var chip_id = this.i2c_read8(REG_CHIP_ID_H) << 8 | this.i2c_read8(REG_CHIP_ID_L);
//                    if (chip_id != CHIP_ID) {
//                        throw RuntimeError("Chip ID invalid: {:04x} expected: {:04x}.".format(chip_id, CHIP_ID));
//                    }
//                }
//            }
            
//            // Read a single (8bit) register from the device.
//            public virtual object i2c_read8(object reg) {
//                var msg_w = i2c_msg.write(this._i2c_addr, new List<object> {
//                    reg
//                });
//                var msg_r = i2c_msg.read(this._i2c_addr, 1);
//                this._i2c_dev.i2c_rdwr(msg_w, msg_r);
//                return msg_r.ToList()[0];
//            }
            
//            // Write a single (8bit) register to the device.
//            public virtual object i2c_write8(object reg, object value) {
//                var msg_w = i2c_msg.write(this._i2c_addr, new List<object> {
//                    reg,
//                    value
//                });
//                this._i2c_dev.i2c_rdwr(msg_w);
//            }
            
//            // Get a pin definition from its index.
//            public virtual object get_pin(object pin) {
//                if (pin < 1 || pin > this._pins.Count) {
//                    throw ValueError("Pin should be in range 1-14.");
//                }
//                return this._pins[pin - 1];
//            }
            
//            // Enable switch counting on a pin.
//            public virtual object setup_switch_counter(object pin, object mode = IN_PU) {
//                var io_pin = this.get_pin(pin);
//                if (!(0, 1).Contains(io_pin.port)) {
//                    throw ValueError("Pin {} does not support switch counting.".format(pin));
//                }
//                if (!new List<object> {
//                    IN,
//                    IN_PU
//                }.Contains(mode)) {
//                    throw ValueError("Pin mode should be one of IN or IN_PU");
//                }
//                this.set_mode(pin, mode, schmitt_trigger: true);
//                var sw_reg = new List<object> {
//                    REG_SWITCH_EN_P0,
//                    REG_SWITCH_EN_P1
//                }[io_pin.port];
//                this.set_bit(sw_reg, io_pin.pin);
//            }
            
//            // Read the switch count value on a pin.
//            public virtual object read_switch_counter(object pin) {
//                var io_pin = this.get_pin(pin);
//                if (!(0, 1).Contains(io_pin.port)) {
//                    throw ValueError("Pin {} does not support switch counting.".format(pin));
//                }
//                var sw_reg = new List<object> {
//                    REG_SWITCH_P00,
//                    REG_SWITCH_P10
//                }[io_pin.port] + io_pin.pin;
//                var value = this.i2c_read8(sw_reg);
//                // The switch counter is 7-bit
//                // The most significant bit encodes the current GPIO state
//                return Tuple.Create(value & 0x7f, (value & 0x80) == 0x80);
//            }
            
//            // Clear the switch count value on a pin to 0.
//            public virtual object clear_switch_counter(object pin) {
//                var io_pin = this.get_pin(pin);
//                if (!(0, 1).Contains(io_pin.port)) {
//                    throw ValueError("Pin {} does not support switch counting.".format(pin));
//                }
//                var sw_reg = new List<object> {
//                    REG_SWITCH_P00,
//                    REG_SWITCH_P10
//                }[io_pin.port] + io_pin.pin;
//                this.i2c_write8(sw_reg, 0);
//            }
            
//            // Set up a rotary encoder.
//            public virtual object setup_rotary_encoder(
//                object channel,
//                object pin_a,
//                object pin_b,
//                object pin_c = null,
//                object count_microsteps = false) {
//                channel -= 1;
//                this.set_mode(pin_a, PIN_MODE_PU, schmitt_trigger: true);
//                this.set_mode(pin_b, PIN_MODE_PU, schmitt_trigger: true);
//                if (pin_c != null) {
//                    this.set_mode(pin_c, PIN_MODE_OD);
//                    this.output(pin_c, 0);
//                }
//                this.i2c_write8(new List<object> {
//                    REG_ENC_1_CFG,
//                    REG_ENC_2_CFG,
//                    REG_ENC_3_CFG,
//                    REG_ENC_4_CFG
//                }[channel], pin_a | pin_b << 4);
//                this.change_bit(REG_ENC_EN, channel * 2 + 1, count_microsteps);
//                this.set_bit(REG_ENC_EN, channel * 2);
//            }
            
//            // Read the step count from a rotary encoder.
//            public virtual object read_rotary_encoder(object channel) {
//                channel -= 1;
//                var last = this._encoder_last[channel];
//                var reg = new List<object> {
//                    REG_ENC_1_COUNT,
//                    REG_ENC_2_COUNT,
//                    REG_ENC_3_COUNT,
//                    REG_ENC_4_COUNT
//                }[channel];
//                var value = this.i2c_read8(reg);
//                if (value & 0b10000000) {
//                    value -= 256;
//                }
//                if (last > 64 && value < -64) {
//                    this._encoder_offset[channel] += 256;
//                }
//                if (last < -64 && value > 64) {
//                    this._encoder_offset[channel] -= 256;
//                }
//                this._encoder_last[channel] = value;
//                return this._encoder_offset[channel] + value;
//            }
            
//            // Set the specified bits (using a mask) in a register.
//            public virtual object set_bits(object reg, object bits) {
//                if (BIT_ADDRESSED_REGS.Contains(reg)) {
//                    foreach (var bit in Enumerable.Range(0, 8)) {
//                        if (bits & 1 << bit) {
//                            this.i2c_write8(reg, 0b1000 | bit & 0b111);
//                        }
//                    }
//                } else {
//                    var value = this.i2c_read8(reg);
//                    time.sleep(0.001);
//                    this.i2c_write8(reg, value | bits);
//                }
//            }
            
//            // Set the specified bit (nth position from right) in a register.
//            public virtual object set_bit(object reg, object bit) {
//                this.set_bits(reg, 1 << bit);
//            }
            
//            // Clear the specified bits (using a mask) in a register.
//            public virtual object clr_bits(object reg, object bits) {
//                if (BIT_ADDRESSED_REGS.Contains(reg)) {
//                    foreach (var bit in Enumerable.Range(0, 8)) {
//                        if (bits & 1 << bit) {
//                            this.i2c_write8(reg, 0b0000 | bit & 0b111);
//                        }
//                    }
//                } else {
//                    var value = this.i2c_read8(reg);
//                    time.sleep(0.001);
//                    this.i2c_write8(reg, value & ~bits);
//                }
//            }
            
//            // Clear the specified bit (nth position from right) in a register.
//            public virtual object clr_bit(object reg, object bit) {
//                this.clr_bits(reg, 1 << bit);
//            }
            
//            // Returns the specified bit (nth position from right) from a register.
//            public virtual object get_bit(object reg, object bit) {
//                return this.i2c_read8(reg) & 1 << bit;
//            }
            
//            // Toggle one register bit on/off.
//            public virtual object change_bit(object reg, object bit, object state) {
//                if (state) {
//                    this.set_bit(reg, bit);
//                } else {
//                    this.clr_bit(reg, bit);
//                }
//            }
            
//            // Enable the IOE interrupts.
//            public virtual object enable_interrupt_out(object pin_swap = false) {
//                this.set_bit(REG_INT, BIT_INT_OUT_EN);
//                this.change_bit(REG_INT, BIT_INT_PIN_SWAP, pin_swap);
//            }
            
//            // Disable the IOE interrupt output.
//            public virtual object disable_interrupt_out() {
//                this.clr_bit(REG_INT, BIT_INT_OUT_EN);
//            }
            
//            // Get the IOE interrupt state.
//            public virtual object get_interrupt() {
//                if (this._interrupt_pin != null) {
//                    return this._gpio.input(this._interrupt_pin) == 0;
//                } else {
//                    return this.get_bit(REG_INT, BIT_INT_TRIGD);
//                }
//            }
            
//            // Clear the interrupt flag.
//            public virtual object clear_interrupt() {
//                this.clr_bit(REG_INT, BIT_INT_TRIGD);
//            }
            
//            // Enable/disable the input interrupt on a specific pin.
//            // 
//            //         :param pin: Pin from 1-14
//            //         :param enabled: True/False for enabled/disabled
//            // 
//            //         
//            public virtual object set_pin_interrupt(object pin, object enabled) {
//                var io_pin = this.get_pin(pin);
//                this.change_bit(io_pin.reg_int_mask_p, io_pin.pin, enabled);
//            }
            
//            // Attach an event handler to be run on interrupt.
//            // 
//            //         :param callback: Callback function to run: callback(pin)
//            // 
//            //         
//            public virtual object on_interrupt(object callback) {
//                if (this._interrupt_pin != null) {
//                    this._gpio.add_event_detect(this._interrupt_pin, this._gpio.FALLING, callback: callback, bouncetime: 1);
//                }
//            }
            
//            // Wait for the IOE to finish writing non-volatile memory.
//            public virtual object _wait_for_flash() {
//                var t_start = time.time();
//                while (this.get_interrupt()) {
//                    if (time.time() - t_start > this._timeout) {
//                        throw RuntimeError("Timed out waiting for interrupt!");
//                    }
//                    time.sleep(0.001);
//                }
//                t_start = time.time();
//                while (!this.get_interrupt()) {
//                    if (time.time() - t_start > this._timeout) {
//                        throw RuntimeError("Timed out waiting for interrupt!");
//                    }
//                    time.sleep(0.001);
//                }
//            }
            
//            // Set the IOE i2c address.
//            public virtual object set_i2c_addr(object i2c_addr) {
//                this.set_bit(REG_CTRL, 4);
//                this.i2c_write8(REG_ADDR, i2c_addr);
//                this._i2c_addr = i2c_addr;
//                time.sleep(0.25);
//                // self._wait_for_flash()
//                this.clr_bit(REG_CTRL, 4);
//            }
            
//            // Set the ADC voltage reference.
//            public virtual object set_adc_vref(object vref) {
//                this._vref = vref;
//            }
            
//            // Get the ADC voltage reference.
//            public virtual object get_adc_vref() {
//                return this._vref;
//            }
            
//            // Get the IOE chip ID.
//            public virtual object get_chip_id() {
//                return this.i2c_read8(REG_CHIP_ID_H) << 8 | this.i2c_read8(REG_CHIP_ID_L);
//            }
            
//            public virtual object _pwm_load() {
//                // Load new period and duty registers into buffer
//                var t_start = time.time();
//                this.set_bit(REG_PWMCON0, 6);
//                while (this.get_bit(REG_PWMCON0, 6)) {
//                    time.sleep(0.001);
//                    if (time.time() - t_start >= this._timeout) {
//                        throw RuntimeError("Timed out waiting for PWM load!");
//                    }
//                }
//            }
            
//            // Set PWM settings.
//            // 
//            //         PWM is driven by the 24MHz FSYS clock by default.
//            // 
//            //         :param divider: Clock divider, one of 1, 2, 4, 8, 16, 32, 64 or 128
//            // 
//            //         
//            public virtual object set_pwm_control(object divider) {
//                try {
//                    var pwmdiv2 = new Dictionary<object, object> {
//                        {
//                            1,
//                            0b000},
//                        {
//                            2,
//                            0b001},
//                        {
//                            4,
//                            0b010},
//                        {
//                            8,
//                            0b011},
//                        {
//                            16,
//                            0b100},
//                        {
//                            32,
//                            0b101},
//                        {
//                            64,
//                            0b110},
//                        {
//                            128,
//                            0b111}}[divider];
//                } catch (KeyError) {
//                    throw ValueError("A clock divider of {}".format(divider));
//                }
//                this.i2c_write8(REG_PWMCON1, pwmdiv2);
//            }
            
//            // Set the PWM period.
//            // 
//            //         The period is the point at which the PWM counter is reset to zero.
//            // 
//            //         The PWM clock runs at FSYS with a divider of 1/1.
//            // 
//            //         Also specifies the maximum value that can be set in the PWM duty cycle.
//            // 
//            //         
//            public virtual object set_pwm_period(object value) {
//                value |= 0xffff;
//                this.i2c_write8(REG_PWMPL, value & 0xff);
//                this.i2c_write8(REG_PWMPH, value >> 8);
//                this._pwm_load();
//            }
            
//            // Get the current mode of a pin.
//            public virtual object get_mode(object pin) {
//                return this._pins[pin - 1].mode;
//            }
            
//            // Set a pin output mode.
//            // 
//            //         :param mode: one of the supplied IN, OUT, PWM or ADC constants
//            // 
//            //         
//            public virtual object set_mode(object pin, object mode, object schmitt_trigger = false, object invert = false) {
//                var io_pin = this.get_pin(pin);
//                if (io_pin.mode == mode) {
//                    return;
//                }
//                var gpio_mode = mode & 0b11;
//                var io_mode = mode >> 2 & 0b11;
//                var initial_state = mode >> 4;
//                if (io_mode != PIN_MODE_IO && !io_pin.type.Contains(mode)) {
//                    throw ValueError("Pin {} does not support {}!".format(pin, MODE_NAMES[io_mode]));
//                }
//                io_pin.mode = mode;
//                if (this._debug) {
//                    Console.WriteLine("Setting pin {pin} to mode {mode} {name}, state: {state}".format(pin: pin, mode: MODE_NAMES[io_mode], name: GPIO_NAMES[gpio_mode], state: STATE_NAMES[initial_state]));
//                }
//                if (mode == PIN_MODE_PWM) {
//                    this.set_bit(io_pin.reg_iopwm, io_pin.pwm_channel);
//                    this.change_bit(REG_PNP, io_pin.pwm_channel, invert);
//                    this.set_bit(REG_PWMCON0, 7);
//                } else if (io_pin.type.Contains(PIN_MODE_PWM)) {
//                    this.clr_bit(io_pin.reg_iopwm, io_pin.pwm_channel);
//                }
//                var pm1 = this.i2c_read8(io_pin.reg_m1);
//                var pm2 = this.i2c_read8(io_pin.reg_m2);
//                // Clear the pm1 and pm2 bits
//                pm1 |= 255 - (1 << io_pin.pin);
//                pm2 |= 255 - (1 << io_pin.pin);
//                // Set the new pm1 and pm2 bits according to our gpio_mode
//                pm1 |= gpio_mode >> 1 << io_pin.pin;
//                pm2 |= (gpio_mode & 0b1) << io_pin.pin;
//                this.i2c_write8(io_pin.reg_m1, pm1);
//                this.i2c_write8(io_pin.reg_m2, pm2);
//                // Set up Schmitt trigger mode on inputs
//                if (new List<object> {
//                    PIN_MODE_PU,
//                    PIN_MODE_IN
//                }.Contains(mode)) {
//                    this.change_bit(io_pin.reg_ps, io_pin.pin, schmitt_trigger);
//                }
//                // 5th bit of mode encodes default output pin state
//                this.i2c_write8(io_pin.reg_p, initial_state << 3 | io_pin.pin);
//            }
            
//            // Read the IO pin state.
//            // 
//            //         Returns a 12-bit ADC reading if the pin is in ADC mode
//            //         Returns True/False if the pin is in any other input mode
//            //         Returns None if the pin is in PWM mode
//            // 
//            //         :param adc_timeout: Timeout (in seconds) for an ADC read (default 1.0)
//            // 
//            //         
//            public virtual object input(object pin, object adc_timeout = 1) {
//                var io_pin = this.get_pin(pin);
//                if (io_pin.mode == PIN_MODE_ADC) {
//                    if (this._debug) {
//                        Console.WriteLine("Reading ADC from pin {}".format(pin));
//                    }
//                    this.clr_bits(REG_ADCCON0, 0x0f);
//                    this.set_bits(REG_ADCCON0, io_pin.adc_channel);
//                    this.i2c_write8(REG_AINDIDS, 0);
//                    this.set_bit(REG_AINDIDS, io_pin.adc_channel);
//                    this.set_bit(REG_ADCCON1, 0);
//                    this.clr_bit(REG_ADCCON0, 7);
//                    this.set_bit(REG_ADCCON0, 6);
//                    // Wait for the ADCF conversion complete flag to be set
//                    var t_start = time.time();
//                    while (!this.get_bit(REG_ADCCON0, 7)) {
//                        time.sleep(0.01);
//                        if (time.time() - t_start >= adc_timeout) {
//                            throw RuntimeError("Timeout waiting for ADC conversion!");
//                        }
//                    }
//                    var hi = this.i2c_read8(REG_ADCRH);
//                    var lo = this.i2c_read8(REG_ADCRL);
//                    return (hi << 4 | lo) / 4095.0 * this._vref;
//                } else {
//                    if (this._debug) {
//                        Console.WriteLine("Reading IO from pin {}".format(pin));
//                    }
//                    var pv = this.get_bit(io_pin.reg_p, io_pin.pin);
//                    return pv ? HIGH : LOW;
//                }
//            }
            
//            // Write an IO pin state or PWM duty cycle.
//            // 
//            //         :param value: Either True/False for OUT, or a number between 0 and PWM period for PWM.
//            // 
//            //         
//            public virtual object output(object pin, object value) {
//                var io_pin = this.get_pin(pin);
//                if (io_pin.mode == PIN_MODE_PWM) {
//                    if (this._debug) {
//                        Console.WriteLine("Outputting PWM to pin: {pin}".format(pin: pin));
//                    }
//                    this.i2c_write8(io_pin.reg_pwml, value & 0xff);
//                    this.i2c_write8(io_pin.reg_pwmh, value >> 8);
//                    this._pwm_load();
//                } else if (value == LOW) {
//                    if (this._debug) {
//                        Console.WriteLine("Outputting LOW to pin: {pin}".format(pin: pin));
//                    }
//                    this.clr_bit(io_pin.reg_p, io_pin.pin);
//                } else if (value == HIGH) {
//                    if (this._debug) {
//                        Console.WriteLine("Outputting HIGH to pin: {pin}".format(pin: pin));
//                    }
//                    this.set_bit(io_pin.reg_p, io_pin.pin);
//                }
//            }
//        }
//    }
//}

//}
