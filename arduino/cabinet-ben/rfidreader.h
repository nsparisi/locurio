#ifndef RfidReader_h
#define RfidReader_h

#include <SoftwareSerial.h>

#include <inttypes.h>

#define MAX_TAG_LEN 255
#define MAX_FAIL 1

// Set to 1 to obtain debug output
#define RfidDebugOutput 0

// Timing parameters
#define ResetDelay 50
#define SerialTimeout 150


class RfidReader
{
  private:
    static bool serialInitialized;
    int SerialPin;

    char* currentTag = new char[MAX_TAG_LEN];
    bool tagPresent = false;
    int failCount = 0;
    char* buf = new char[MAX_TAG_LEN];
    const char* friendlyName;
    
    SoftwareSerial* swserial = 0;

  public:
    RfidReader(int serialPin, const char* readerName);

    bool WaitForTag();

    bool GetIsTagPresent();
    const char* GetCurrentTag();
    const char* GetFriendlyName();
};

#endif

