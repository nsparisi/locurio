#ifndef GenericFeedback_h
#define GenericFeedback_h

#include <inttypes.h>

#include "megabrite.h"

void feedback_error()
{
  Serial.println("Error");
  MegaBrite::Instance.AllLightsRed();
  delay(500);
}

void feedback_success()
{
  Serial.println("Success");
  MegaBrite::Instance.AllLightsGreen();
  delay(500);
}

void feedback_info()
{
  Serial.println("Info");
  MegaBrite::Instance.AllLightsBlue();
  delay(500);
}

void feedback_none()
{
  MegaBrite::Instance.AllLightsOff();
  delay(500);
}

void permanent_error()
{
  while(true)
  {
    feedback_error();
    feedback_none();
  }
}

#endif

