#include "rfidreader.h"
#include "multiplexer.h"

#include "Arduino.h"

bool RfidReader::serialInitialized = false;

RfidReader::RfidReader(int muxChannel, int resetPin, const char* readerName)
{
  MultiplexerChannel = muxChannel;
  ResetPin = resetPin;
  
  friendlyName = readerName;
  
  // setup the software serial pins
  pinMode(ResetPin, OUTPUT);
  digitalWrite(ResetPin, HIGH);
  
  if (!RfidReader::serialInitialized)
  {
    Serial2.begin(9600);
    Serial2.setTimeout(175);
  }
    
}

bool RfidReader::PollForTag()
{
  // Set the multiplexer pin
  Multiplexer::Instance.Select(MultiplexerChannel);
  
  // First, reset the reader so it will redetect any existing tags.
  digitalWrite(ResetPin, LOW);
  
  // While all the readers are turned off, flush the serial buffer.
  while (Serial2.available())
  {
    Serial2.read();
  }
  
  // Wait 50 ms.
  delay(50);
  
  // Get started again
  digitalWrite(ResetPin, HIGH);
  
  // Create a buffer and a pointer to walk through it.
  char* ptr = buf;
  byte countRead = Serial2.readBytes(buf, 13);
  
  if (countRead > 0)
  {
  Serial.print(friendlyName);
  Serial.print(": Read ");
  Serial.print((int)countRead);
  Serial.print(" bytes: ");
  Serial.println(buf);
  
    // Check if tag is valid
    if (countRead == 13)  // exact length of RFID tag + 'T' prefix
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
