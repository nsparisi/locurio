#include "rfidreader.h"
#include "multiplexer.h"

#include "Arduino.h"

bool RfidReader::serialInitialized = false;
int RfidReader::ResetCounter = 0;

RfidReader::RfidReader(int muxChannel, int resetPin, const char* readerName, bool isResetPinInverted, HardwareSerial* targetSerialPort)
{
  this->usesMultiplexer = true;
  this->usesResetPin = true;
  this->serialPort = targetSerialPort;
  this->IsResetPinInverted = isResetPinInverted;
  this->MultiplexerChannel = muxChannel;
  this->ResetPin = resetPin;
  this->friendlyName = readerName;

  // setup the software serial pins
  pinMode(ResetPin, OUTPUT);
  digitalWrite(ResetPin, isResetPinInverted ? LOW : HIGH);
  
  serialPort->begin(9600);
  serialPort->setTimeout(SerialTimeout);
}

RfidReader::RfidReader(const char* readerName, HardwareSerial* targetSerialPort)
{
  this->usesMultiplexer = false;
  this->usesResetPin = false;
  this->IsResetPinInverted = false;
  this->MultiplexerChannel = 0;
  this->ResetPin = 0;
  
  this->serialPort = targetSerialPort;

  this->friendlyName = readerName;
  
  serialPort->begin(9600);
  serialPort->setTimeout(SerialTimeout);
}

 void Initialize()
{
} 

void RfidReader::SetMultiplexer()
{
  if (usesMultiplexer)
  {
    #ifndef NO_MULTIPLEXER
    // Set the multiplexer pin
    Multiplexer::Instance.Select(MultiplexerChannel);
    #endif
    
    while (serialPort->available())
    {
      serialPort->read();
    }
  }
}

void RfidReader::Reset()
{
  if (usesResetPin)
  {
    // First, reset the reader so it will redetect any existing tags.
    digitalWrite(ResetPin, IsResetPinInverted ? HIGH : LOW);
    
    if (RfidDebugOutput)
    {
      Serial.println("Reset!");
    }
    
    while (serialPort->available())
    {
      serialPort->read();
    }
  
    // Wait 50 ms.
    delay(ResetDelay);
  
    // Get started again
    digitalWrite(ResetPin, IsResetPinInverted ? LOW : HIGH);
  
    delay(50);
  }
}

bool RfidReader::PollForTag()
{
  if (RfidReader::ResetCounter == 0)
  {
    RfidReader::ResetCounter = TimesUntilEachReset;
    Reset();
  }
  else
  {
    RfidReader::ResetCounter--;
  }
  
  return PollForTag(false);
}

bool RfidReader::PollForTag(bool shouldReset)
{
  SetMultiplexer();
  if (shouldReset)
  {
    Reset();
  }
  
  byte countRead = 0;

  if (serialPort->find("\x02"))
  {
    if (RfidDebugOutput)
    {
      Serial.println("Found start byte 0x02");
    }
   
    countRead = serialPort->readBytes(buf, 13);
  }
  
  if (countRead > 0 && countRead < MAX_TAG_LEN - 1)
  {
    // Null terminate the string
    buf[countRead] = '\0';

    if (RfidDebugOutput)
    {
      Serial.print(friendlyName);
      Serial.print(": Read ");
      Serial.print((int)countRead);
      Serial.print(" bytes: ");

      for (int i = 0; i < countRead; i++)
      {
        Serial.print((int)buf[i]);
        Serial.print(",");
      }
      Serial.println(buf);
    }

    // Check if tag is valid
    if (countRead == 13)  // exact length of RFID tag + 0x02 prefix
    {
      currentTag[countRead - 1] = '\0';

      bool isLetter = true;
      for (int i = 0; i < 12; i++)
      {
        isLetter &= ((buf[i] >= 'A' && buf[i] <= 'Z') || (buf[i] >= '0' && buf[i] <= '9'));
      }

      if (isLetter)
      {
        if (RfidDebugOutput)
        {
          Serial.println("All valid tag characters!");
        }
        // TODO:  validate checksum

        // Reset failure counter to 7 tries.
        failCount = MAX_FAIL;
        tagPresent = true;

        // Save the buffer to the currentTag field, without the 0x02 prefix
        strcpy(currentTag, buf);
      }
      else
      {
        if (RfidDebugOutput)
        {
          Serial.println("Tag read, but characters were invalid");
        }
        failCount--;
      }
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

  if (tagPresent)
  {
    currentTagType = TagDatabase::Instance.getTagType(currentTag);
    Serial.print(currentTagType);
    Serial.println(" was detected tag type.");
    if (currentTagType == MASTER && !TagDatabase::Instance.isInEnrollmentMode)
    {
      TagDatabase::Instance.enterEnrollMode(this);
    }
  }
  else
  {
    currentTagType = NO_TAG;
  }
  
  
  return tagPresent;
}

bool RfidReader::GetIsTagPresent()
{
  return tagPresent;
}

TagType RfidReader::GetCurrentTagType()
{
  return currentTagType;
}

void RfidReader::WaitForValidTag()
{
  bool validTagFound = false;
  while (!validTagFound)
  {
    while (!PollForTag(true))
    {
      delay(10);
    }
    if (currentTagType == NO_TAG || currentTagType == INVALID || currentTagType == UNKNOWN_VALID)
    {
      if (RfidDebugOutput)
      {
        Serial.println("Invalid tag detected;  remaining in WaitForValidTag()");
      }
    }
    else
    {
      validTagFound=true;
    }
  }
}

void RfidReader::WaitForNoTag()
{
  while (PollForTag(true))
  {
    delay(10);
  }
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

const char* RfidReader::GetFriendlyName()
{
  return friendlyName;
}
