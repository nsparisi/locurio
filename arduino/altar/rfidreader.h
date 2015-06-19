#ifndef RfidReader_h
#define RfidReader_h

#include <inttypes.h>
#include "OldSoftwareSerial.h"

#define NotConnectedPin 41

#define MAX_TAG_LEN 255
#define MAX_FAIL 2

class RfidReader
{
  private:
	int DataPin;
	int ResetPin;

	char* currentTag = new char[MAX_TAG_LEN];
	bool tagPresent = false;
	int failCount = 0;
        char* buf = new char[MAX_TAG_LEN];

	SoftwareSerial* softwareSerialPort;
	
  public:
	RfidReader(int dataPin, int resetPin);
	
        void Setup();
	bool PollForTag();

	bool GetIsTagPresent();
	const char* GetCurrentTag();
};

#endif

