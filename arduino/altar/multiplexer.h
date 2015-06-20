#ifndef Multiplexer_h
#define Multiplexer_h

#include <inttypes.h>

class Multiplexer
{
  private:
		static int muxChannelLookupTable[16][4];
		int currentChannel = 0;
                int controlPins[4];
	
  public:
  Multiplexer(int s0, int s1, int s2, int s3);
  
  static Multiplexer Instance;
  void Select(int channel);
  int GetCurrentChannel();
  
};
  
#endif

