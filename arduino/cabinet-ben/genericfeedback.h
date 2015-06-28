#ifndef GenericFeedback_h
#define GenericFeedback_h

#include <inttypes.h>


void feedback_error()
{
  Serial.println("Error");
  delay(500);
}

void feedback_success()
{
  Serial.println("Success");
  delay(500);
}

void feedback_info()
{
  Serial.println("Info");
  delay(500);
}

void feedback_none()
{
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

