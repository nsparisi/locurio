#ifndef TagDatabase_h
#define TagDatabase_h

#include "tagtypes.h"

#include "EEPROMex.h"
#include "EEPROMVar.h"

#include "rfidreader.h"

#include <inttypes.h>

#define MASTER_TAG_ID "00001E8F1C8D"

#define DATABASE_ADDRESS 0

#define TAG_LENGTH 12

#define TagDatabaseDebugOutput 1

#define MAX_TAGS 48

// We'll define a number (based on the Locurio address :) that is highly
// unlikely to occur by chance.  If we don't find it in the data structure,
// it's a sign we need to recreate the DB from scratch.

#define VALID_DATABASE_CREATED_FLAG 619200

struct Tag {
  char tagName[TAG_LENGTH + 1];
  TagType type;
};

struct Database {
  int tagCount;
  Tag tagStorage[MAX_TAGS];
  unsigned long databaseCreatedFlag;
};

class RfidReader;

class TagDatabase
{
  private:
    Database database;

    bool isDatabaseCreated();
    void readDatabase();
    void addTag(const char* tagId, TagType tagType, bool saveDatabase = false);
    void changeOrAddTag(const char* tagId, TagType tagType, bool saveDatabase = false);
    void saveToEEPROM();

  public:
    static TagDatabase Instance;
    TagDatabase();
    bool isInEnrollmentMode = false;
    void enterEnrollMode(RfidReader* sourceReader);
    bool isTagOfType(const char* tagId, TagType expectedType, bool acceptUnidentified = false);
    TagType getTagType(const char* tagId);

    void createDatabase();
    void dumpTagDatabase();
};

#endif

