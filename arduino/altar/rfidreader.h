#ifndef RfidReader_h
#define RfidReader_h

#include <inttypes.h>

#define MAX_TAG_LEN 32
#define MAX_FAIL 1

// Set to 1 to obtain debug output
#define RfidDebugOutput 0

// Timing parameters
#define ResetDelay 100
#define SerialTimeout 250

#define TimesUntilEachReset 20


class RfidReader
{
  private:
    static bool serialInitialized;
    int MultiplexerChannel;
    int ResetPin;

    char currentTag[MAX_TAG_LEN];
    bool tagPresent = false;
    int failCount = 0;
    char buf[MAX_TAG_LEN];
    const char* friendlyName;

    bool IsResetPinInverted = false;

    static int ResetCounter;
  public:
    RfidReader(int muxChannel, int resetPin, const char* readerName, bool isResetPinInverted = false);

    bool PollForTag();
    bool PollForTag(bool shouldReset);

    bool GetIsTagPresent();
    const char* GetCurrentTag();
    const char* GetFriendlyName();
    
    void Reset();
    
    void SetMultiplexer();
};

#endif

