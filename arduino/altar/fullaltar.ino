#include <Arduino.h>

#include <LedControl.h>

#include "megabrite.h"
#include "rfidreader.h"
#include "lightgroup.h"

RfidReader** readers;
LightGroup** leds;

int readerCount = 5;
// create SoftwareSerial objects
MegaBrite CenterLights = MegaBrite();

void setup(void) {
  Serial.begin(9600);

readers = new RfidReader*[readerCount];
leds = new LightGroup*[readerCount];

readers[0] = new RfidReader(44,42); //35,34);    //front    
leds[0] = new LightGroup(2,0,1);
/*
leds[0] = new LightGroup(0,0,1);

readers[1] = new RfidReader(29,27);    //right
leds[1] = new LightGroup(1,0,1);

readers[2] = new RfidReader(44,42);    //back      

readers[3] = new RfidReader(45,43);    //left
leds[3] = new LightGroup(3,0,1);

readers[4] = new RfidReader(28,26);    //top
leds[4] = new LightGroup(4,0,1);

for (int i=0; i<readerCount; i++)
{
  readers[i]->Setup();
}
  */
  
  readers[0]->Setup();
}






void loop(void) 
{
  
  for (int i=0; i<1; i++)
  {
    Serial.print(i);
    leds[i]->SetState(readers[i]->PollForTag());
    Serial.print(readers[i]->GetIsTagPresent() ? "+" : "-");
    if (readers[i]->GetIsTagPresent())
    {
      CenterLights.SetLight(i, 255,255,255);
    }
    else
    {
      CenterLights.SetLight(i,0,0,0);
    }
  }
  
  for (int i=0; i<1; i++)
  {
    leds[i]->SetState(true);
    delay(50);
    leds[i]->SetState(false);
    delay(50);
  }
  
  Serial.println();

}
