#ifndef TagType_h
#define TagType_h

#include <inttypes.h>

enum TagType {
  MASTER = 128,
  TOP1 = 0,
  TOP2 = 1,
  TOP3 = 2,
  TOP4 = 3,
  TOP5 = 4,
  WAND = 64,
  UNKNOWN_VALID = 192,
  INVALID = 254,
  NO_TAG = 253
};

#endif

