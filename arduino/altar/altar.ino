#include <Arduino.h>

#include <LedControl.h>

#include "megabrite.h"
#include "rfidreader.h"
#include "lightgroup.h"

#define MAX_BRIGHTNESS 1023

RfidReader** readers;
RfidReader** wordPuzzle;
RfidReader** topPuzzle;

LightGroup** leds;
LightGroup** topLightSegments;

int sideLights[16];

int readerCount = 16;
int wordCount = 11;
int topCount = 5;

MegaBrite CenterLights = MegaBrite();

void(*resetFunc) (void) = 0;

void allLightsOff()
{
  for (int i = 0; i < 5; i++)
  {
    CenterLights.SetLight(i, 0, 0, 0);
  }
}

void allLightsOn()
{
  for (int i = 0; i < 5; i++)
  {
    CenterLights.SetLight(i, MAX_BRIGHTNESS, MAX_BRIGHTNESS, MAX_BRIGHTNESS);
  }
}

void topLightOnly()
{
  allLightsOff();
  CenterLights.SetLight(4, MAX_BRIGHTNESS, MAX_BRIGHTNESS, MAX_BRIGHTNESS);
}

void allLightsGreen()
{
  for (int i = 0; i < 5; i++)
  {
    CenterLights.SetLight(i, 0, MAX_BRIGHTNESS, 0);
  }
}

void allLightsRed()
{
  for (int i = 0; i < 5; i++)
  {
    CenterLights.SetLight(i, 0, MAX_BRIGHTNESS, 0);
  }
}

void rave()
{
  for (int i = 0; i < 25; i++)
  {
    for (int j = 0; j < 5; j++)
    {
      CenterLights.SetLight(j, MAX_BRIGHTNESS, 0, 0);
    }
    delay(150);
    for (int j = 0; j < 5; j++)
    {
      CenterLights.SetLight(j, 0, MAX_BRIGHTNESS, 0);
    }
    delay(150);
    for (int j = 0; j < 5; j++)
    {
      CenterLights.SetLight(j, 0, 0, MAX_BRIGHTNESS);
    }
    delay(150);
    for (int j = 0; j < 5; j++)
    {
      CenterLights.SetLight(j, MAX_BRIGHTNESS, 0, MAX_BRIGHTNESS);
    }
    delay(150);
    for (int j = 0; j < 5; j++)
    {
      CenterLights.SetLight(j, MAX_BRIGHTNESS, MAX_BRIGHTNESS, 0);
    }
    delay(150);
    for (int j = 0; j < 5; j++)
    {
      CenterLights.SetLight(j, 0, MAX_BRIGHTNESS, MAX_BRIGHTNESS);
    }
    delay(150);
  }
}

void setup(void)
{
  Serial.begin(9600);

  readers = new RfidReader*[readerCount];
  wordPuzzle = new RfidReader*[wordCount];
  topPuzzle = new RfidReader*[topCount];
  
  leds = new LightGroup*[readerCount];
  topLightSegments = new LightGroup*[topCount];

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

  wordPuzzle[0] = readers[6];	// reader: Back 0
  wordPuzzle[1] = readers[2];	// reader: Front 2
  wordPuzzle[2] = readers[5];	// reader: Right 2
  wordPuzzle[3] = readers[10];	// reader: Left 1
  wordPuzzle[4] = readers[7];	// reader: Back 1
  wordPuzzle[5] = readers[9];	// reader: Left 0
  wordPuzzle[6] = readers[0];	// reader: Front 0
  wordPuzzle[7] = readers[3];	// reader: Right 0
  wordPuzzle[8] = readers[8];	// reader: Back 2
  wordPuzzle[9] = readers[4];	// reader: Right 1
  wordPuzzle[10] = readers[1];	// reader: Front 1
  
  topPuzzle[0] = readers[11];
  topPuzzle[1] = readers[12];
  topPuzzle[2] = readers[13];
  topPuzzle[3] = readers[14];
  topPuzzle[4] = readers[15];
  
  sideLights[0] = 1;
  sideLights[1] = 1;
  sideLights[2] = 1;
  sideLights[3] = 0;
  sideLights[4] = 0;
  sideLights[5] = 0;
  sideLights[6] = 3;
  sideLights[7] = 3;
  sideLights[8] = 3;
  sideLights[9] = 2;
  sideLights[10] = 2;
  sideLights[11] = 4;
  sideLights[12] = 4;
  sideLights[13] = 4;
  sideLights[14] = 4;
  sideLights[15] = 4;

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
  
  topLightSegments[0] = leds[11];
  topLightSegments[1] = leds[12];
  topLightSegments[2] = leds[13];
  topLightSegments[3] = leds[14];
  topLightSegments[4] = leds[15];
  
  allLightsOff();
}

void solveTopPuzzle()
{
  int currentBestReader = -1;

  while (currentBestReader < topCount-1)
  {
    currentBestReader = -1;
    
    for (int i=0; i<topCount; i++)
    {
      if (wordPuzzle[1]->PollForTag())
      {
        currentBestReader++;
      }  
      else
      {
        break;
      }
    }
    
    for (int j=0; j<topCount; j++)
    {
      topLightSegments[j]->SetState(j <= currentBestReader);
    }

    Serial.print(currentBestReader);    
  }
}

void solveWordPuzzle()
{
   for (int i = 0; i < wordCount; i++)
  {
    while (!wordPuzzle[i]->PollForTag())
    {
      delay(5);
    }
    
    if (i < (wordCount-1))
    {
      wordPuzzle[i+1]->SetMultiplexer();
    }
    
    for (int i = 0; i < 6; i++)
    {
      for (int j = 0; j < 5; j++)
      {
        CenterLights.SetLight(j, 0, 128 * i, 0);
      }
    }

    delay(1000);

    for (int i = 0; i < 6; i++)
    {
      for (int j = 0; j < 5; j++)
      {
        CenterLights.SetLight(j, 128 * i, 128 * 6, 128 * i);
      }
    }
    
    for (int i=0; i<5; i++)
    {
      CenterLights.SetLight(i, 1023, 1023, 1023);
    }
  }
}

void loop(void)
{
  solveTopPuzzle();
  solveWordPuzzle();
  rave();
  resetFunc();
}
