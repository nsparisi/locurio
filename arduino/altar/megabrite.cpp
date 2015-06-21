#include "megabrite.h"
#include "Arduino.h"

MegaBrite::MegaBrite()
{
  Setup();
  AllOff();
}

// Set pins to outputs and initial states
void MegaBrite::Setup()
{
  pinMode(datapin, OUTPUT);
  pinMode(latchpin, OUTPUT);
  pinMode(enablepin, OUTPUT);
  pinMode(clockpin, OUTPUT);

  digitalWrite(latchpin, LOW);
  digitalWrite(enablepin, LOW);
}

void MegaBrite::SendPacket()
{
  if (SB_CommandMode == B01)
  {
    SB_RedCommand = 127;
    SB_GreenCommand = 110;
    SB_BlueCommand = 110;
  }

  SB_CommandPacket = SB_CommandMode & B11;
  SB_CommandPacket = (SB_CommandPacket << 10)  | (SB_BlueCommand & 1023);
  SB_CommandPacket = (SB_CommandPacket << 10)  | (SB_RedCommand & 1023);
  SB_CommandPacket = (SB_CommandPacket << 10)  | (SB_GreenCommand & 1023);

  for (int j = 0; j < 32; j++)
  {
    if ((SB_CommandPacket >> (31 - j)) & 1)
    {
      DATPORT |= (1 << DATPIN);
    }
    else
    {
      DATPORT &= ~(1 << DATPIN);
    }
    CLKPORT |= (1 << CLKPIN);
    CLKPORT &= ~(1 << CLKPIN);
  }
}

void MegaBrite::Latch()
{
  delayMicroseconds(1);
  LATPORT |= (1 << LATPIN);
  ENAPORT |= (1 << ENAPIN);
  delayMicroseconds(1);
  ENAPORT &= ~(1 << ENAPIN);
  LATPORT &= ~(1 << LATPIN);
}

void MegaBrite::WriteLEDArray()
{
  SB_CommandMode = B00; // Write to PWM control registers

  for (int i = 0; i < NumLEDs; i++) {
    SB_RedCommand = LEDChannels[i][0];
    SB_GreenCommand = LEDChannels[i][1];
    SB_BlueCommand = LEDChannels[i][2];
    SendPacket();
  }

  Latch();

  SB_CommandMode = B01; // Write to current control registers

  for (int z = 0; z < NumLEDs; z++)
  {
    SendPacket();
  }

  Latch();
}

void MegaBrite::AllOff()
{
  for (int i = 0; i < NumLEDs; i++)
  {
    LEDChannels[i][0] = 0;
    LEDChannels[i][1] = 0;
    LEDChannels[i][2] = 0;
  }
  WriteLEDArray();
}

void MegaBrite::SetLight(int channel, int red, int green, int blue)
{
  LEDChannels[channel][0] = red;
  LEDChannels[channel][1] = green;
  LEDChannels[channel][2] = blue;

  WriteLEDArray();
}
