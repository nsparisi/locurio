#ifndef GenericFeedback_h
#define GenericFeedback_h

#include <inttypes.h>

#include "megabrite.h"

void feedback_error()
{
  Serial.println(F("Error"));
  MegaBriteInstance.AllLightsRed();
  delay(500);
}

void feedback_success()
{
  Serial.println(F("Success"));
  MegaBriteInstance.AllLightsGreen();
  delay(500);
}

void feedback_info()
{
  Serial.println(F("Info"));
  MegaBriteInstance.AllLightsBlue();
  delay(500);
}

void feedback_none()
{
  MegaBriteInstance.AllLightsOff();
  delay(500);
}



void permanent_error()
{
  while (true)
  {
    feedback_error();
    feedback_none();
  }
}

#endif

