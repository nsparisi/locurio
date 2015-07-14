#include <Arduino.h>

#include <LedControl.h>
#include <SoftwareSerial.h>

#include "megabrite.h"
#include "rfidreader.h"
#include "lightgroup.h"
#include "reset.h"
#include "tagtypes.h"
#include "abysshandler.h"

#include "tagdatabase.h"

#define PuzzleDebug 1

#define readerCount 16
#define wordCount 11
#define topCount 5

int sideLights[16];

#define logMemory 0

int freeRam() {
  extern int __heap_start, *__brkval;
  int v;
  return (int) &v - (__brkval == 0 ? (int) &__heap_start : (int) __brkval);
}

TagType topExpectedTypes[topCount];

RfidReader* wordPuzzle[wordCount];
RfidReader* topPuzzle[topCount];

LightGroup* wordLeds[wordCount];
LightGroup* topLightSegments[topCount];


LightGroup led0 = LightGroup(0,  0, 1);
LightGroup led1 = LightGroup(0,  2, 3);
LightGroup led2 = LightGroup(0,  4, 5);
LightGroup led3 = LightGroup(1,  0, 1);
LightGroup led4 = LightGroup(1,  2, 3);
LightGroup led5 = LightGroup(1,  4, 5);
LightGroup led6 = LightGroup(2,  0, 1);
LightGroup led7 = LightGroup(2,  2, 3);
LightGroup led8 = LightGroup(2,  4, 5);
LightGroup led9 = LightGroup(3,  0, 1);
LightGroup led10 = LightGroup(3,  2, 3);
LightGroup led11 = LightGroup(4,  0);
LightGroup led12 = LightGroup(4,  1);
LightGroup led13 = LightGroup(4,  2);
LightGroup led14 = LightGroup(4,  3);
LightGroup led15 = LightGroup(4,  4);

RfidReader reader2 = RfidReader(8, 36, "F2");
RfidReader reader0 = RfidReader(7, 36, "F0");
RfidReader reader1 = RfidReader(3, 36, "F1");
RfidReader reader3 = RfidReader(4, 38, "R0");
RfidReader reader4 = RfidReader(5, 38, "R1");
RfidReader reader5 = RfidReader(6, 38, "R2");
RfidReader reader6 = RfidReader(11, 40, "B0");
RfidReader reader7 = RfidReader(12, 40, "B1");
RfidReader reader8 = RfidReader(13, 40, "B2");
RfidReader reader9 = RfidReader(9, 42, "L0");
RfidReader reader10 = RfidReader(10, 42, "L1");
RfidReader reader11 = RfidReader(0, 44, "T1(TC)", true);
RfidReader reader12 = RfidReader(1, 44, "T2(BR)", true);
RfidReader reader13 = RfidReader(2, 44, "T3(BL)", true);
RfidReader reader14 = RfidReader(14, 44, "T4(ML)", true);
RfidReader reader15 = RfidReader(15, 44, "T5(MR)", true);

int brightnessFailure[30] = {200, 400, 600, 800, 900, 1023, 1023, 1023, 1023, 1023, 900, 850, 700, 750, 600, 650, 500, 550, 400, 450, 300, 350, 200, 250, 200, 100, 75, 50, 0};

void feedback_failure()
{
  for(int i=0; i<30; i++)
  {
    MegaBriteInstance.AllLightsRed(brightnessFailure[i]);
    delay(100);
  }
}

void printFreeMem()
{
  if (logMemory)
  {
    Serial.print(F("Free memory is: "));
    Serial.print(freeRam());
    Serial.println(F(" bytes"));
  }
}

void setup(void)
{
  Serial.begin(115200);
  
  printFreeMem();
  
  wordPuzzle[0] = &reader6;	// reader: Back 0
  wordPuzzle[1] = &reader2;	// reader: Front 2
  wordPuzzle[2] = &reader5;	// reader: Right 2
  wordPuzzle[3] = &reader10;	// reader: Left 1
  wordPuzzle[4] = &reader7;	// reader: Back 1
  wordPuzzle[5] = &reader9;	// reader: Left 0
  wordPuzzle[6] = &reader0;	// reader: Front 0
  wordPuzzle[7] = &reader3;	// reader: Right 0
  wordPuzzle[8] = &reader8;	// reader: Back 2
  wordPuzzle[9] = &reader4;	// reader: Right 1
  wordPuzzle[10] = &reader1;	// reader: Front 1
  
  wordLeds[0] = &led6;	        // led: Back 0
  wordLeds[1] = &led2;	        // led: Front 2
  wordLeds[2] = &led5;	        // led: Right 2
  wordLeds[3] = &led10;	        // led: Left 1
  wordLeds[4] = &led7;	        // led: Back 1
  wordLeds[5] = &led9;	        // led: Left 0
  wordLeds[6] = &led0;	        // led: Front 0
  wordLeds[7] = &led3;	        // led: Right 0
  wordLeds[8] = &led8;	        // led: Back 2
  wordLeds[9] = &led4;	        // led: Right 1
  wordLeds[10] = &led1;	        // led: Front 1
  
  topPuzzle[0] = &reader11;
  topPuzzle[1] = &reader15;
  topPuzzle[2] = &reader12;
  topPuzzle[3] = &reader13;
  topPuzzle[4] = &reader14;

  topExpectedTypes[0] = TOP1;
  topExpectedTypes[1] = TOP2;
  topExpectedTypes[2] = TOP3;
  topExpectedTypes[3] = TOP4;
  topExpectedTypes[4] = TOP5;
  
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
  
  topLightSegments[0] = &led11;
  topLightSegments[1] = &led15;
  topLightSegments[2] = &led12;
  topLightSegments[3] = &led13;
  topLightSegments[4] = &led14;
  
  Serial.print(F("Light check"));
  
  MegaBriteInstance.AllLightsBlue();
  delay(1000);
  MegaBriteInstance.AllLightsGreen();
  delay(1000);
  MegaBriteInstance.AllLightsOff();
  
  if (PuzzleDebug)
  {
    TagDatabaseInstance.dumpTagDatabase();
  }
    
  printFreeMem();
}

void debugReaders()
{

  while (true)
  {

    for (int i = 0; i < 3; i++)
    {
      MegaBriteInstance.AllLightsBlue();
      delay(500);
      MegaBriteInstance.AllLightsOff();
      delay(100);
    }

    for (int i = 0; i < topCount; i++)
    {
      while (!(topPuzzle[i]->PollForTag(true)))
      {
        for (int j = 0; j <= i; j++)
        {
          MegaBriteInstance.AllLightsRed();
          delay(250);
          MegaBriteInstance.AllLightsOff();
          delay(250);
        }
        delay(300);
      }

      for (int j = 0; j <= i; j++)
      {
        MegaBriteInstance.AllLightsGreen();
        delay(250);
        MegaBriteInstance.AllLightsOff();
        delay(250);
      }

      delay(250);
      MegaBriteInstance.AllLightsOn();
      delay(100);
      MegaBriteInstance.AllLightsOff();
      delay(250);
    }


    for (int i = 0; i < wordCount; i++)
    {
      while (!(wordPuzzle[i]->PollForTag(true)))
      {
        for (int j = 0; j <= i; j++)
        {
          MegaBriteInstance.AllLightsRed();
          delay(250);
          MegaBriteInstance.AllLightsOff();
          delay(250);
        }
        delay(300);
      }

      for (int j = 0; j <= i; j++)
      {
        MegaBriteInstance.AllLightsGreen();
        delay(250);
        MegaBriteInstance.AllLightsOff();
        delay(250);
      }

      delay(250);
      MegaBriteInstance.AllLightsOn();
      delay(100);
      MegaBriteInstance.AllLightsOff();
      delay(250);
    }
  }
}


void solveTopPuzzle()
{
  MegaBriteInstance.TopLightOnly();

  int currentBestReader = -1;
  bool topPuzzleStarted = false;

  while (currentBestReader < (topCount - 1))
  {
    currentBestReader = -1;

    bool failure = false;
    for (int i = 0; i < topCount && !failure; i++)
    {
      
      if (PuzzleDebug)
      {
        Serial.println(F("Start with top count:"));
        Serial.println(topCount);
        Serial.print(F("Checking reader #"));
        Serial.println(i);
      }

      if (topPuzzle[i]->PollForTag(true))
      {
        if (topPuzzle[i]->GetCurrentTagType() == DEBUG)
        {
          debugReaders();
        }
        
        if (topPuzzle[i]->GetCurrentTagType() == topExpectedTypes[i])
        {
          currentBestReader++;
          if (!topPuzzleStarted)
          {
            topPuzzleStarted = true;
            abyss->send_message("TOPSTART");
          }
        }
        else
        {
          failure = true;
        }
      }
      else
      {
        failure=true;
      }
    }

    for (int j = 0; j < topCount; j++)
    {
      topLightSegments[j]->SetState(j <= currentBestReader);
    }

    if (PuzzleDebug)
    {
      Serial.print("Current best reader:  ");
      Serial.println(currentBestReader);
    }
  }
}

// 8 seconds
#define TIMEOUT 8000

bool solveWordPuzzle()
{
  for (int i = 0; i < wordCount; i++)
  {
    printFreeMem();   
       
    // Flip all the other reset lines low before starting on a new reader, in an attempt
    // to minimize interference.
    
    for (int j=0; j<wordCount; j++)
    {
      wordPuzzle[j]->Shutdown();
    }
   
    unsigned long startingTimestamp = millis();
      
    MegaBriteInstance.AllLightsOn();

    while (!wordPuzzle[i]->PollForTag(true) || wordPuzzle[i]->GetCurrentTagType() != WAND)
    {
      if (i > 0 && (millis() - startingTimestamp) > TIMEOUT)
      {
        feedback_failure();
        
        for (int c=0; c<wordCount; c++)
        {
          wordPuzzle[c]->ClearTag();
        } 
        
        abyss->send_message("WORDFAILURE");
        
        return false;
      }
    }
 
    if (i == 0)
    {
      // first word solved;  notify abyss
      abyss->send_message("WORDBEGIN");
    }

    abyss->send_message("WORDTAGPRESENT");

    wordLeds[i]->On();

    if (i < (wordCount - 1))
    {
      wordPuzzle[i + 1]->SetMultiplexer();
    }
 
    MegaBriteInstance.AllLightsGreen();
    delay(500);
  }
  
  return true;
}

void loop(void)
{
  abyss->send_message("POWERON");
  
  solveTopPuzzle();
  abyss->send_message("TOPSOLVED");
  
  while (!solveWordPuzzle())
  {}

  abyss->send_message("WORDSOLVED");

  while (1) 
  {
      MegaBriteInstance.Rave();
  };
}
