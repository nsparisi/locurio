#include <Arduino.h>

#include <LedControl.h>

#include "megabrite.h"
#include "rfidreader.h"
#include "lightgroup.h"

RfidReader** readers;
LightGroup** leds;
int sideLights[16];

int readerCount = 16;


MegaBrite CenterLights = MegaBrite();

void setup(void) 
{
  Serial.begin(9600);

  readers = new RfidReader*[readerCount];
  leds = new LightGroup*[readerCount];

  readers[0] = new RfidReader(7, 36, "Front 0");
  readers[1] = new RfidReader(3, 36, "Front 1");
  readers[2] = new RfidReader(8, 36, "Front 2");
  readers[3] = new RfidReader(4, 38, "Right 0");
  readers[4] = new RfidReader(5, 38, "Right 1");
  readers[5] = new RfidReader(6, 38, "Right 2");
  readers[6] = new RfidReader(11, 40, "Back 0");
  readers[7] = new RfidReader(12, 40, "Back 1");
  readers[8] = new RfidReader(13, 40, "Back 2");
  readers[9] = new RfidReader(9, 42, "Left 0");
  readers[10] = new RfidReader(10, 42, "Left 1");
  readers[11] = new RfidReader(0, 44, "Top 0");
  readers[12] = new RfidReader(1, 44, "Top 1");
  readers[13] = new RfidReader(2, 44, "Top 2");
  readers[14] = new RfidReader(14, 44, "Top 3");
  readers[15] = new RfidReader(15, 44, "Top 4");

  sideLights[0] = 4;
  sideLights[1] = 4;
  sideLights[2] = 4;
  sideLights[3] = 0;
  sideLights[4] = 0;
  sideLights[5] = 0;
  sideLights[6] = 3;
  sideLights[7] = 3;
  sideLights[8] = 3;
  sideLights[9] = 2;
  sideLights[10] = 2;
  sideLights[11] = 5;
  sideLights[12] = 5;
  sideLights[13] = 5;
  sideLights[14] = 5;
  sideLights[15] = 5;
  

  leds[0] = new LightGroup(0,  0, 1);
  leds[1] = new LightGroup(0,  2, 3);
  leds[2] = new LightGroup(0,  4, 5);
  leds[3] = new LightGroup(1,  0, 1);
  leds[4] = new LightGroup(1,  2, 3);
  leds[5] = new LightGroup(1,  4, 5);
  leds[6] = new LightGroup(2,  0, 1);
  leds[7] = new LightGroup(2,  2, 3);
  leds[8] = new LightGroup(2,  4, 5);
  leds[9] = new LightGroup(3,  0, 1);
  leds[10] = new LightGroup(3,  2, 3);
  leds[11] = new LightGroup(4,  0);
  leds[12] = new LightGroup(4,  1);
  leds[13] = new LightGroup(4,  2);
  leds[14] = new LightGroup(4,  3);
  leds[15] = new LightGroup(4,  4);

  for (int i = 0; i < 5; i++)
  {
    CenterLights.SetLight(i, 768, 768, 768);
  }
}

void loop(void)
{
  for (int i = 0; i < readerCount; i++)
  {
    Serial.println(i);
    bool oldState = readers[i]->GetIsTagPresent();
    bool newState = readers[i]->PollForTag();


    if (newState != oldState)
    {
      leds[i]->SetState(newState);

      if (newState)
      {
        Serial.print("Tag arrived:  ");
        Serial.print(readers[i]->GetFriendlyName());
        Serial.print(" [");
        Serial.print(readers[i]->GetCurrentTag());
        Serial.println("]");

        CenterLights.SetLight(sideLights[i], 0,768,0);
      }
      else
      {
        Serial.print("Tag departed:  ");
        Serial.print(readers[i]->GetFriendlyName());
        Serial.println(" []");
        CenterLights.SetLight(sideLights[i], 768, 768, 768);
      }
    }
  }
}
