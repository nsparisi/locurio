#ifndef Multiplexer_h
#define Multiplexer_h

#include <inttypes.h>

#define MultiplexerDebug 0

class Multiplexer
{
  private:
    int currentChannel = 0;
    static int controlPins[4];

  public:
    Multiplexer(int s0, int s1, int s2, int s3);

    void Select(int channel);
    int GetCurrentChannel();

};

extern Multiplexer MultiplexerInstance;
#endif

