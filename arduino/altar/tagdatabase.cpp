#include "tagdatabase.h"
#include "tagtypes.h"
#include "genericfeedback.h"
#include "reset.h"

TagDatabase TagDatabase::Instance = TagDatabase();

#define tagDebugPrint(...) \
  do { if (TagDatabaseDebugOutput) Serial.print(__VA_ARGS__); } while (0)

#define tagDebugPrintln(...) \
  do { if (TagDatabaseDebugOutput) Serial.println(__VA_ARGS__); } while (0)


bool TagDatabase::isDatabaseCreated()
{
  return database.databaseCreatedFlag == VALID_DATABASE_CREATED_FLAG;
}

void TagDatabase::createDatabase()
{
  tagDebugPrintln(F("Creating database."));
  database.tagCount = 0;
  database.databaseCreatedFlag = VALID_DATABASE_CREATED_FLAG;

  for (int i = 0; i < MAX_TAGS; i++)
  {
    for (int j = 0; j < TAG_LENGTH; j++)
    {
      database.tagStorage[i].tagName[j] = '?';
    }
    database.tagStorage[i].tagName[TAG_LENGTH] = '\0';
    database.tagStorage[i].type = INVALID;
  }

  // Add well known tag IDs to the database by default (the tags from the keychain,
  // as well as the tag on the end of the magic wand)
  addTag(MASTER_TAG_ID, MASTER);
  addTag(("00001E91EA65"), TOP1);
  addTag(("00001E5EFEBE"), TOP2);
  addTag(("00001EAE8838"), TOP3);
  addTag(("00001E64542E"), TOP4);
  addTag(("00001E5386CB"), TOP5);
  addTag(("00001E45174C"), WAND);
  addTag(("6A008FD64A79"), WAND);
  addTag(("00001E8CAB39"), DEBUG);
  saveToEEPROM();
}

void TagDatabase::addTag(const char* tagId, TagType tagType, bool saveDatabase)
{
  tagDebugPrint(F("Adding tag: "));
  tagDebugPrint((tagId));
  tagDebugPrint(F(" with type "));
  tagDebugPrintln(((int)tagType));

  if (database.tagCount == MAX_TAGS)
  {
    // Hang the altar while blinking red/off/red/off, since the database is full.
    permanent_error();
  }
  else
  {
    database.tagStorage[database.tagCount].type = tagType;
    strncpy(database.tagStorage[database.tagCount].tagName, tagId, 12);
    database.tagStorage[database.tagCount].tagName[TAG_LENGTH] = '\0';
    database.tagCount++;
  }

  if (saveDatabase)
  {
    saveToEEPROM();
  }
}

void TagDatabase::saveToEEPROM()
{
  EEPROM.updateBlock(DATABASE_ADDRESS, database);
}

void TagDatabase::readDatabase()
{
  EEPROM.readBlock(DATABASE_ADDRESS, database);

  if (!isDatabaseCreated())
  {
    createDatabase();
  }
}

TagDatabase::TagDatabase()
{
  readDatabase();
  if (!isDatabaseCreated())
  {
    createDatabase();
  }
}

void TagDatabase::enterEnrollMode(RfidReader* sourceReader)
{
  isInEnrollmentMode = true;
  tagDebugPrintln(F("Entering enrollment mode."));
  feedback_info();
  sourceReader->WaitForNoTag();
  delay(2000);
  feedback_none();

  tagDebugPrintln(F("Waiting for source tag."));

  sourceReader->WaitForValidTag();
  TagType knownTagType = getTagType(sourceReader->GetCurrentTag());

  tagDebugPrint((int)knownTagType);
  tagDebugPrintln(F(" - source tag type"));

  feedback_success();
  sourceReader->WaitForNoTag();
  delay(2000);
  feedback_none();

  tagDebugPrintln(F("Waiting for destination tag."));
  while (!sourceReader->PollForTag(true))
  {
    delay(10);
  }

  const char* newTagId = sourceReader->GetCurrentTag();

  tagDebugPrintln(newTagId);

  changeOrAddTag(newTagId, knownTagType, true);

  tagDebugPrintln(F("Tag added.  Rebooting..."));

  feedback_info();
  delay(500);
  feedback_success();
  delay(5000);

  Reset::resetFunc();
}

void TagDatabase::changeOrAddTag(const char* tagId, TagType tagType, bool saveDatabase)
{
  for (int i = 0; i < database.tagCount; i++)
  {
    if (strncmp(tagId, database.tagStorage[i].tagName, 12) == 0)
    {
      database.tagStorage[i].type = tagType;
      if (saveDatabase)
      {
        saveToEEPROM();
      }
      return;
    }
  }
  addTag(tagId, tagType, saveDatabase);
}

bool TagDatabase::isTagOfType(const char* tagId, TagType expectedType, bool acceptUnidentified)
{
  TagType type = getTagType(tagId);

  if ((type == expectedType) ||
      (acceptUnidentified && type == UNKNOWN_VALID))
  {
    return true;
  }

  return false;
}

TagType TagDatabase::getTagType(const char* tagId)
{
  Serial.println(F("Getting tag type"));
  Serial.println(tagId);
  Serial.println((int)tagId[TAG_LENGTH]);
  Serial.println((int)tagId[0]);
  Serial.println((int)tagId[TAG_LENGTH - 1]);

  if (tagId[TAG_LENGTH] == '\0' || tagId[TAG_LENGTH] == 13)
  {
    for (int i = 0; i < database.tagCount; i++)
    {
      Serial.print(F("Comparing \""));
      Serial.print(tagId);
      Serial.print(F("\" to \""));
      Serial.println(database.tagStorage[i].tagName);

      if (strncmp(tagId, database.tagStorage[i].tagName, 12) == 0)
      {
        return database.tagStorage[i].type;
      }
    }
    return UNKNOWN_VALID;
  }
  return INVALID;
}

void TagDatabase::dumpTagDatabase()
{
  Serial.print(F("Dumping "));
  Serial.print(database.tagCount);
  Serial.println(F(" tags."));

  for (int i = 0; i < database.tagCount; i++)
  {
    Serial.print(F("Tag "));
    Serial.print(i);
    Serial.print(F(": Tag "));
    Serial.print(database.tagStorage[i].tagName);
    Serial.print(F(" ("));
    Serial.print((int)database.tagStorage[i].type);
    Serial.println(F(")"));
  }
}

