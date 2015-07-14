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
#define clockpin   7 // CI
#define enablepin  6 // EI
#define latchpin   5 // LI
#define datapin    4 // DI

// Defines for direct port access
#define CLKPORT PORTH
#define ENAPORT PORTH
#define LATPORT PORTE
#define DATPORT PORTG
#define CLKPIN  4
#define ENAPIN  3
#define LATPIN  3
#define DATPIN  5

#define MAX_BRIGHTNESS 1023

#define NumLEDs 4

class MegaBrite
{
  private:
    // Variables for communication
    unsigned long SB_CommandPacket;
    int SB_CommandMode;
    int SB_BlueCommand;
    int SB_RedCommand;
    int SB_GreenCommand;

    // Create LED value storage array
    int LEDChannels[NumLEDs][5];

    void SendPacket();
    void Latch();

  public:
    static MegaBrite Instance;

    MegaBrite();
    void Setup();

    void WriteLEDArray();
    void AllOff();
    void SetLight(int channel, int red, int green, int blue);

    void AllLightsOff();
    void AllLightsOn();
    void TopLightOnly();
    void AllLightsGreen();
    void AllLightsRed();
    void AllLightsRed(int brightness);
    void AllLightsBlue();
    void Rave();

};

#endif

