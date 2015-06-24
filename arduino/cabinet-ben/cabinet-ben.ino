#include <SoftwareSerial.h>
#include "RfidReader.h"

#define PIN_DOOR_MAGNETS 4
#define PIN_REED_SWITCHES 8
#define PIN_BLACKLIGHT 9
#define PIN_RFID_1 5
#define PIN_RFID_2 6
#define PIN_RFID_3 7

RfidReader* readers[3];

void setup() 
{
  pinMode(PIN_BLACKLIGHT, OUTPUT );
  pinMode(PIN_REED_SWITCHES, INPUT_PULLUP );
  pinMode(PIN_DOOR_MAGNETS, OUTPUT );
  pinMode(PIN_RFID_1, INPUT);
  pinMode(PIN_RFID_2, INPUT);
  pinMode(PIN_RFID_3, INPUT);
  
  readers[0] = new RfidReader(PIN_RFID_1, "Reader 1");
  readers[1] = new RfidReader(PIN_RFID_2, "Reader 2");
  readers[2] = new RfidReader(PIN_RFID_3, "Reader 3");
      
  Serial.begin(9600);
  
  pinMode(12, OUTPUT);
  noTone(12);
  
 // digitalWrite(PIN_DOOR_MAGNETS, HIGH);
}

bool oldState = false;
unsigned long lastTimeChange = 0;

int currentRfidReader = 0;
unsigned long minTimeDelta = 1000;

#define NOTE_C4 262
#define NOTE_G3 196
#define NOTE_B3 247

void loop() 
{
        bool isDoorOpen = digitalRead(PIN_REED_SWITCHES);
        if (isDoorOpen != oldState && ((millis() - lastTimeChange) > minTimeDelta))
        {
          oldState = isDoorOpen;
          digitalWrite(PIN_BLACKLIGHT, isDoorOpen ? LOW : HIGH);
        }
    
    /*if (currentRfidReader < 3)
    {
      if (readers[currentRfidReader]->WaitForTag())
      {
            tone(12, 131, 500);

        currentRfidReader++;
        if (currentRfidReader < 2)
        {
          Serial.print("TAG DETECTED");
        }
        else
        {
          Serial.print("OPENING DOOR");
          digitalWrite(PIN_DOOR_MAGNETS, LOW);
          
          for (int i=0; i<10; i++)
          {
                        tone(12, 131, 100);
                        delay(500);
                        noTone(12);
                        delay(500);
          }
        }
      }
    }*/
    

  
  /*delay(5000);
  Serial.print("Turning on magnets");
  digitalWrite(PIN_DOOR_MAGNETS, HIGH);
  
  delay(5000);
  Serial.print("Turning off magnets");
  digitalWrite(PIN_DOOR_MAGNETS, LOW);
  */

}

