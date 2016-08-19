#include "megabrite.h"
#include "Arduino.h"

MegaBrite MegaBriteInstance;

MegaBrite::MegaBrite()
{

}

// Set pins to outputs and initial states
void MegaBrite::Setup()
{
  pinMode(datapin, OUTPUT);

  strip = new Adafruit_NeoPixel(NumLEDs, datapin, NEO_GRB + NEO_KHZ800);
  strip->begin();
  strip->show();
}

void MegaBrite::WriteLEDArray()
{
  for (int i = 0; i < NumLEDs; i++) {
    strip->setPixelColor(i, LEDChannels[i][0], LEDChannels[i][1], LEDChannels[i][2]);
  }

  strip->show();
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

void MegaBrite::SetAllLights(int red, int green, int blue)
{
  for (int i=0; i < NumLEDs; i++)
  {
    if (i % 2 == 0)
    {
       LEDChannels[i][0] = red;
       LEDChannels[i][1] = green;
        LEDChannels[i][2] = blue;
    }
    else

    {
      LEDChannels[i][0] =0;
      LEDChannels[i][1] = 0;
      LEDChannels[i][2] = 0;
    }
  }
  WriteLEDArray();
}

void MegaBrite::AllLightsOff()
{

    MegaBriteInstance.SetAllLights(0, 0, 0);
}

void MegaBrite::AllLightsOn()
{
    SetAllLights(MAX_BRIGHTNESS, MAX_BRIGHTNESS, MAX_BRIGHTNESS);
 
}

void MegaBrite::AllLightsDim()
{
    SetAllLights(DIM_BRIGHTNESS, DIM_BRIGHTNESS, DIM_BRIGHTNESS);
  
}

void MegaBrite::TopLightOnly()
{
  AllLightsOff();
}

void MegaBrite::AllLightsGreen()
{
    SetAllLights(0, MAX_BRIGHTNESS, 0);
}

void MegaBrite::AllLightsRed(int brightness)
{
    SetAllLights((int)(((float)brightness)/4.0), 0, 0);
}

void MegaBrite::AllLightsRed()
{
  AllLightsRed(MAX_BRIGHTNESS);  
}

void MegaBrite::AllLightsBlue()
{
    SetAllLights(0, 0, MAX_BRIGHTNESS);
}

void MegaBrite::Rave()
{
  for (int i = 0; i < 25; i++)
  {
      SetAllLights(MAX_BRIGHTNESS, 0, 0);
    delay(150);
      SetAllLights(0, MAX_BRIGHTNESS, 0);
    delay(150);
      SetAllLights(0, 0, MAX_BRIGHTNESS);
    delay(150);
      SetAllLights(MAX_BRIGHTNESS, 0, MAX_BRIGHTNESS);
    delay(150);
      SetAllLights(MAX_BRIGHTNESS, MAX_BRIGHTNESS, 0);
    delay(150);
    
      SetAllLights(0, MAX_BRIGHTNESS, MAX_BRIGHTNESS);
    delay(150);
  }
}
