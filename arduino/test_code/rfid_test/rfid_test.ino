// 2 55 53 48 48 55 51 54 54 51 57 53 57 13 10 3 


//-------------------------------------------------------------------------------------------------------------
#include <SoftwareSerial.h>
SoftwareSerial RFID(5, 6); // RX and TX
int data1 = 0;
int ok = -1;
int yes = 9;
int no = 13;
// use first sketch in http://wp.me/p3LK05-3Gk to get your tag numbers
int tag1[14] = {2,52,48,48,48,56,54,66,49,52,70,51,56,3};
int tag2[14] = {2,55,53,48,48,55,51,54,54,51,57,53,57,13};
int newtag[14] = {0,0,0,0,0,0,0,0,0,0,0,0,0,0}; // used for read comparisons

void setup()
{
  RFID.begin(9600); // start serial to RFID reader
  Serial.begin(9600); // start serial to PC 
  pinMode(yes, OUTPUT); // for status LEDs
  pinMode(no, OUTPUT);
}

boolean comparetag(int tag1[14], int tag2[14])
{
  boolean matched = true;

  for (int index = 0 ; index < 14 ; index++)
  {
    if (tag1[index] != tag2[index])
      matched = false;
  }

  return matched;
}

void checkmytags() // compares each tag against the tag just read
{
  ok = 0; // this variable helps decision-making,
  // if it is 1 we have a match, zero is a read but no match,
  // -1 is no read attempt made
  if (comparetag(newtag, tag1) == true)
  {
    ok++;
  }
  if (comparetag(newtag, tag2) == true)
  {
    ok++;
  }
}

void readTags()
{
  ok = -1;
  if (RFID.available() > 0) 
  {
    // read tag numbers
    delay(100); // needed to allow time for the data to come in from the serial buffer.
    for (int z = 0 ; z < 14 ; z++) // read the rest of the tag
    {
      data1 = RFID.read();
      newtag[z] = data1;
    }
    RFID.flush(); // stops multiple reads
    // do the tags match up?
    checkmytags();
  }
  // now do something based on tag type
  if (ok > 0) // if we had a match
  {
    Serial.println("Accepted");
    digitalWrite(no, HIGH);
    delay(1000);
    digitalWrite(no, LOW);
    ok = -1;
  }
  else if (ok == 0) // if we didn't have a match
  {
    Serial.print("Rejected:");
    // I (Phil) added this to print out the tag ID if there is no match
    for(int x=0; x<14; x++)
      Serial.print(" " + String(newtag[x]));
    Serial.println(); 
    digitalWrite(no, HIGH);
    delay(1000);
    digitalWrite(no, LOW);
    ok = -1;
  }
}

void loop()
{
  digitalWrite(yes, HIGH);  //only for light testing, remove for rfid testing
  readTags();
}
// ------------------------------------------------------------------------------------



