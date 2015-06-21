#ifndef RfidReader_h
#define RfidReader_h

#include <inttypes.h>

#define MAX_TAG_LEN 255
#define MAX_FAIL 2

// Set to 1 to obtain debug output
#define RfidDebugOutput 1

// Timing parameters
#define ResetDelay 50
#define SerialTimeout 150


class RfidReader
{
  private:
    static bool serialInitialized;
    int MultiplexerChannel;
    int ResetPin;

    char* currentTag = new char[MAX_TAG_LEN];
    bool tagPresent = false;
    int failCount = 0;
    char* buf = new char[MAX_TAG_LEN];
    const char* friendlyName;

  public:
    RfidReader(int muxChannel, int resetPin, const char* readerName);

    bool PollForTag();

    bool GetIsTagPresent();
    const char* GetCurrentTag();
    const char* GetFriendlyName();
};

#endif

