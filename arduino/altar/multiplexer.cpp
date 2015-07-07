#include "multiplexer.h"
#include "Arduino.h"

Multiplexer Multiplexer::Instance = Multiplexer(28, 30, 32, 34);

// based on code from http://bildr.org/2011/02/cd74hc4067-arduino/

int Multiplexer::muxChannelLookupTable[16][4] = {
  {0, 0, 0, 0}, //channel 0
  {1, 0, 0, 0}, //channel 1
  {0, 1, 0, 0}, //channel 2
  {1, 1, 0, 0}, //channel 3
  {0, 0, 1, 0}, //channel 4
  {1, 0, 1, 0}, //channel 5
  {0, 1, 1, 0}, //channel 6
  {1, 1, 1, 0}, //channel 7
  {0, 0, 0, 1}, //channel 8
  {1, 0, 0, 1}, //channel 9
  {0, 1, 0, 1}, //channel 10
  {1, 1, 0, 1}, //channel 11
  {0, 0, 1, 1}, //channel 12
  {1, 0, 1, 1}, //channel 13
  {0, 1, 1, 1}, //channel 14
  {1, 1, 1, 1} //channel 15
};

Multiplexer::Multiplexer(int s0, int s1, int s2, int s3)
{
  controlPins[0] = s0;
  controlPins[1] = s1;
  controlPins[2] = s2;
  controlPins[3] = s3;

  pinMode(s0, OUTPUT);
  pinMode(s1, OUTPUT);
  pinMode(s2, OUTPUT);
  pinMode(s3, OUTPUT);

  Select(0);
}

void Multiplexer::Select(int channel)
{
  Serial.print("Setting mux channel to ");
  Serial.println(channel);
  
  if (channel < 0 || channel > 16)
  {
    channel = 0;
  }

  //loop through the 4 segments
  for (int i = 0; i < 4; i ++) {
    digitalWrite(controlPins[i], muxChannelLookupTable[channel][i]);
  }

  currentChannel = channel;

}

int Multiplexer::GetCurrentChannel()
{
  return currentChannel;
}
