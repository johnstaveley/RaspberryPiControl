# RaspberryPiControl

This project is to control a 4 relay header board on a Raspberry Pi from a remote web page.

## Setup

![Picture of the Raspberry Pi set up with hardware](https://github.com/johnstaveley/RaspberryPiControl/blob/master/PictureOfRaspberryPiSetup.jpg "Picture of the raspberry pi setup with hardware")

- Stack 4 relay board on top of Raspberry pi
- Servo 1: Attach Black to pin 9, Red to pin 1 and White/Yellow to pin 12
- Servo 2: Attach Black to pin 6, Red to pin 17 and White/Yellow to pin 
	NB: Depending on your servos you may have to connect these to 5V instead
- LEDS: Attach to pins 11, 13, 36
- Switch input: Attach to pin 15
- LCD 1602 with I2C interface. GND Pin 39, 5V Pin 2, SDA Pin 3, SCL Pin 5 such as this one: https://www.amazon.co.uk/gp/product/B07J2Q9LB7/ref=ppx_yo_dt_b_asin_title_o01_s00?ie=UTF8&psc=1
- Connect Pi and relay board to 5V 3A supply
- Mounting components on Din rail is optional but just helps to make things tidy
- In Azure. Iot Hub and go to the following blades:
  - Shared access policy. For device and put in IoTHubConnectionString 
  - IoT Devices. Add your Raspberry Pi
  - Networking. Don't put in IP restrictions if you want your azure function to be able to contact the IoT Hub

## Operation

You can use the IoT Hub explorer to send direct messages to the Raspberry Pi and control its hardware using the 'ControlAction' direct method and following json package:
{
	"Method": SetRelay, GetRelay, SetServo, SetText, SetOutput, GetInput
	"Number": Value of the hardware if applicable
	"Value": 0 or 1 for relays and 0 to 1 for servos
	"Message": When writing to the screen, the message to send
}

e.g. this will turn Relay 4 on:
{
	"Method": "SetRelay",
	"Number": 4,
	"Value": 1,
	"Message": ""
}

this writes a message to the second row (Number) and third character location of the LCD screen:

{
	"Method": "SetText",
	"Number": 2,
	"Value": 3,
    "Message": "Hardware Test"
}

if there is a mistake then a bad request will be returned to you

Setting via desvice twin

You can set the state of LED 1 by changing the desired property:

{
	"properties": {
		"desired": {
			"ledState": 0,
...

## Tech notes

Publish as single file does not seem to work, you get a file not found error

You may have to follow the instructions here to run without sudo: https://raspberrypi.stackexchange.com/questions/40105/access-gpio-pins-without-root-no-access-to-dev-mem-try-running-as-root
sudo apt-get update
sudo apt-get upgrade
sudo chown root.gpio /dev/gpiomem
sudo chmod g+rw /dev/gpiomem

A rapsberry pi can control a maximum of two servos using its PWM channels according to this reference: https://embeddedcircuits.com/raspberry-pi/tutorial/how-to-generate-pwm-signal-from-raspberry-pi