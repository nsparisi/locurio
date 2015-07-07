#ifndef Reset_h
#define Reset_h

#include <inttypes.h>

class Reset
{
    // Pointer to the beginning of program memory;  this has the same effect as a reset.
  public:
    static void(*resetFunc) (void);
};
#endif

