# RaspberryPiControl

This project is to control a 4 relay header board on a Raspberry Pi.
This involved porting some python code to C#.

##Setup

![Picture of the Raspberry Pi set up with hardware](https://github.com/johnstaveley/RaspberryPiControl/blob/master/PictureOfRaspberryPiSetup.jpg "Picture of the raspberry pi setup with hardware")

- Stack 4 relay board on top of Raspberry pi
- Servo 1: Attach Brown to pin 9, Red to pin 2 and Yellow to pin 12
- Connect Pi and relay board to 5V 3A supply
- Mounting components on Din rail is optional but just helps to make things tidy
- In Azure. Iot Hub and go to the following blades:
  - Shared access policy. For device and put in IoTHubConnectionString 
  - IoT Devices. Add your Raspberry Pi
  - Networking. Don't put in IP restrictions if you want your azure function to be able to contact the IoT Hub

##Tech notes

Publish as single file does not seem to work, you get a file not found error
You may have to follow the instructions here to run without sudo: https://raspberrypi.stackexchange.com/questions/40105/access-gpio-pins-without-root-no-access-to-dev-mem-try-running-as-root
sudo apt-get update
sudo apt-get upgrade
sudo chown root.gpio /dev/gpiomem
sudo chmod g+rw /dev/gpiomem