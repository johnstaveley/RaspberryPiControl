# Raspberry Pi Control

This project is to control various hardware on a Raspberry Pi from a remote web page.
Hardware includes:
* A 4 relay header board to control a lamp and solenoid
* LCD Display LCD1602
* PWM Devices hub can control up to 16 servos including continuous and angular servos

## Device Setup

![Picture of the Raspberry Pi set up with hardware](https://github.com/johnstaveley/RaspberryPiControl/blob/master/PictureOfRaspberryPiSetup.jpg "Picture of the raspberry pi setup with hardware")

- Stack 4 relay board on top of Raspberry pi
- Servo 1: Attach Black to pin 9, Red to pin 1 and White/Yellow to pin 12
	NB: Depending on your servos you may have to connect these to 5V instead
- LEDS: Attach to pins 11, 13, 36
- Switch input: Attach to pin 15
- LCD 1602 with I2C interface. GND Pin 39, 5V Pin 2, SDA Pin 3, SCL Pin 5 such as this one: https://www.amazon.co.uk/gp/product/B07J2Q9LB7/ref=ppx_yo_dt_b_asin_title_o01_s00?ie=UTF8&psc=1
- Connect Pi and relay board to 5V 3A supply
- Mounting components on Din rail is optional but just helps to make things tidier
- In Azure. Create Iot Hub in standard tier and go to the following blades:
  - Shared access policy. For DEVICE and put in IoTHubConnectionString 
  - IoT Devices. Add your Raspberry Pi, give device name e.g. RaspberryPi4
  - Networking. Don't put in IP restrictions if you want your azure function to be able to contact the IoT Hub
- In order to get the program to run when the Pi is started, do the following:
  - sudo nano /etc/rc.local
  - Just before the Exit command at the end, put in bash /home/pi/Control/Control &
  - Press Ctrl + S then Ctrl + X
  - restart the raspberry pi

## Blazor App Setup

- In Azure. 
  - Create blob storage and copy the connection string

 appsettings.json
- ApplicationUserName / ApplicationPassword - Make up a username/password combination
- BlobContainerName - e.g. pieventhub
- BlobStorageConnectionString - The connection string from above Azure
- DeviceId - device name as configured above e.g. RaspberryPi4
- EventHubConnectionString - Taken from IoTHub -> built in endpoints - Event hub-compatible endpoint
- EventHubName - Taken from ??
- IoTHubConnectionString - Take from device, shared access policy

## Operation

You can use the IoT Hub explorer to send direct messages to the Raspberry Pi and control its hardware using the 'ControlAction' direct method and following json package:
{
	"Method": SetRelay, GetRelay, SetServo, SetText, SetOutput, GetInput, SetPwm
	"Number": Value of the hardware if applicable. -1 to 15 for Pwm
	"Value": 0 or 1 for relays and 0 to 1 for servos and pwm
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