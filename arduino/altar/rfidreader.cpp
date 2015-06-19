#include "rfidreader.h"
#include "Arduino.h"

RfidReader::RfidReader(int dataPin, int resetPin)
{
  DataPin = dataPin; 
  ResetPin = resetPin;
  
  // setup the software serial pins
  pinMode(DataPin, INPUT);
  pinMode(ResetPin, OUTPUT);
  pinMode(NotConnectedPin, OUTPUT);
  
  digitalWrite(ResetPin, HIGH);
}

void RfidReader::Setup()
{
  softwareSerialPort = new SoftwareSerial(DataPin, NotConnectedPin);
  softwareSerialPort->begin(9600);
}

bool RfidReader::PollForTag()
{

  // First, reset the reader so it will redetect any existing tags.
  digitalWrite(ResetPin, LOW);
  delay(50);
  digitalWrite(ResetPin, HIGH);
  
  // Pull bytes out of the software serial channel
  char currentByte = -1;
  
  // Create a buffer and a pointer to walk through it.
  char* ptr = buf;
  
  // Don't wait longer than 160ms for a tag to appear.
  // (value determined by experimentation with ID-20)
  unsigned long start = millis();
  // 160 was original value here
  while ((int)currentByte != 2 && ((millis() - start) < 160))
  {
     currentByte = softwareSerialPort->read();  
  }
  
  if (currentByte == 2) //start character of sequence
  {
    *ptr = 'T';
    
    // only accept letters or numbers;  terminate sequence on anything else.
    while ((*ptr >= 'A' && *ptr <= 'Z') || (*ptr >= '0' && *ptr <= '9'))
    {
      ptr++;
      *ptr = softwareSerialPort->read();
      Serial.println((int)(*ptr));
    }  
    
    // null terminate our string, since we only save letters and numbers.
    *ptr = '\0';
    
    // Check if tag is valid
    if (ptr-buf == 13)  // exact length of RFID tag + 'T' prefix
    {
      // TODO:  validate checksum
      
      // Reset failure counter to 7 tries.
      failCount = MAX_FAIL;
      tagPresent = true;
	  
      // Save the buffer to the currentTag field.
      strcpy(currentTag, buf);
    }
    else
    {
      // Don't panic yet - sometimes tags fail to read, so we wait for failCount '
      // to reach zero before marking the tag as truly gone.
      failCount--;
    }
  }
  else
  {
    if (failCount > 0)
    {
      failCount--;
    }
    else
    {
      tagPresent = false;
    }   
  }
  
/*  Serial.print(DataPin);
  Serial.print(" : ");
if (tagPresent)
{
  Serial.print(currentTag);
}
else
{
  Serial.print("NO");
}
Serial.println("");
*/
  return tagPresent;
}

bool RfidReader::GetIsTagPresent()
{
  return tagPresent;
}

const char* RfidReader::GetCurrentTag()
{
	if (tagPresent)
	{
		return (const char*)currentTag;
	}
	else
	{
		return 0;
	}
}
