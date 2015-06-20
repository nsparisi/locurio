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
  
/*0	Top 0
1	Top 1
2	Top 2
3	Front 1
4	Right 0
5	Right 1
6	Right 2
7	Front 0
8	Front 2
9	Left 0
10	Left 1
11	Back 0
12	Back 1
13	Back 2
14	Top 3
15	Top 4*/

  readers[0] = new RfidReader(7,36,"Front 0"); 
  readers[1] = new RfidReader(4,38,"Right 0"); 
  readers[2] = new RfidReader(11,40,"Back 0"); 
  readers[3] = new RfidReader(9,42,"Left 0"); 
  readers[4] = new RfidReader(0,44,"Top 0");
  
  leds[0] = new LightGroup(0,0,1);
  leds[1] = new LightGroup(1,0,1);
  leds[2] = new LightGroup(2,0,1);
  leds[3] = new LightGroup(3,0,1);
  leds[4] = new LightGroup(4,0,1);
}



void loop(void) 
{
  
  for (int i=0; i<readerCount; i++)
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
