/*
  SoftwareSerial.h - Software serial library
  Copyright (c) 2006 David A. Mellis.  All right reserved.

  This library is free software; you can redistribute it and/or
  modify it under the terms of the GNU Lesser General Public
  License as published by the Free Software Foundation; either
  version 2.1 of the License, or (at your option) any later version.

  This library is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
  Lesser General Public License for more details.

  You should have received a copy of the GNU Lesser General Public
  License along with this library; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

#ifndef MegaBrite_h
#define MegaBrite_h

#include <inttypes.h>
#include <Arduino.h>

#include <Adafruit_NeoPixel.h>  // Adafruit Neopixel Library 1.0.6

/* Ports and Pins

 Direct port access is much faster than digitalWrite.
 You must match the correct port and pin as shown in the table below.

 Arduino Mega Pin   Port        Pin
 13.................PORTB.......7
 12.................PORTB.......6
 11.................PORTB.......5
 10.................PORTB.......4
 9..................PORTH.......6
 8..................PORTH.......5
 7..................PORTH.......4
 6..................PORTH.......3
 5..................PORTE.......3
 4..................PORTG.......5
 3..................PORTE.......5
 2..................PORTE.......4
 1 (TX).............PORTE.......1
 0 (RX).............PORTE.......0
 A7 (Analog)........PORTF.......7
 A6 (Analog)........PORTF.......6
 A5 (Analog)........PORTF.......5
 A4 (Analog)........PORTF.......4
 A3 (Analog)........PORTF.......3
 A2 (Analog)........PORTF.......2
 A1 (Analog)........PORTF.......1
 A0 (Analog)........PORTF.......0

*/
// Defines for use with Arduino functions
#define datapin    4 // DI

#define MAX_BRIGHTNESS 160
#define DIM_BRIGHTNESS 96

#define NumLEDs 64

class MegaBrite
{
  private:
    // Create LED value storage array
    int LEDChannels[NumLEDs][5];

    Adafruit_NeoPixel* strip;
    void SetLight(int channel, int red, int green, int blue);
    
    void SetAllLights(int red, int green, int blue);
    void WriteLEDArray();
    
  public:

    MegaBrite();
    void Setup();

    void AllOff();
    

    void AllLightsOff();
    void AllLightsOn();
    void AllLightsDim();
    void TopLightOnly();
    void AllLightsGreen();
    void AllLightsRed();
    void AllLightsRed(int brightness);
    void AllLightsBlue();
    void Rave();

};

extern MegaBrite MegaBriteInstance;
#endif

