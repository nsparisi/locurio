#include <SoftwareSerial.h>

// Reed switch pin used for sensing front door closure
#define REED_SWITCHES 3

// Predefined by the Evil Mad Scientist Relay Shield's configuration
// Output pin for controlling the back door latch magnets
#define DOOR_MAGNETS 4 



// Output pin for activating the blacklight
#define BLACKLIGHT 10

// blacklight status, allows for easy toggling 
// This is only being used for RFID testing, so the wand can toggle the blacklight on and off
boolean blacklight_status = false;


/****************************************************/
/*                  XBee Variables                  */
/****************************************************/
// RX and TX pins for the XBee.  The XBee is plugged directly into the Arduino's RX and TX lines
// The defined constants are never used, but are placed here as a reminder of which pins are used
#define XBEE_RX 1
#define XBEE_TX 0

int Xbee_data;

/****************************************************/
/*                  RFID Variables                  */
/****************************************************/
// Input pins from the three RFID readers
#define RFID_PIN_1 5
#define RFID_PIN_2 6
#define RFID_PIN_3 7

SoftwareSerial RFID(RFID_PIN_1, 9); // RX and TX

int RFID_data = 0;  // data from the RFID reader is 
int ok = -1;  // a status indicator for tag reads
int newtag[14] = {0,0,0,0,0,0,0,0,0,0,0,0,0,0}; // used for read comparisons

/****************************************************/
/*                 Known RFID Tags                  */
/****************************************************/
int tag1[14] = {2,52,48,48,48,56,54,66,49,52,70,51,56,3};  // I think this sample came with the RFID code I copied
int tag2[14] = {2,55,53,48,48,55,51,54,54,51,57,53,57,13};  // I think this is our actual tag



void setup() {
  pinMode( BLACKLIGHT, OUTPUT );
  pinMode(13, OUTPUT);
  
  // XBee did not work if I begin its serial stream before RFID.  Have not tested RFID yet.
  // I tested the RFID and it didn't work unless I commented out the XBee code.  It seems that
  // whichever one comes last will work.  Does software serial only support one port?
  // Turns out this is a limitation of the Software Serial library.  Supposedly it can only 
  // read from one port at a time.  I would think this means I could cycle through them.  I tried
  // this by begining the streams in the loop every time, but that doesn't seem to work, as it causes 
  // 
  
  Serial.begin(9600);
  RFID.begin(9600); 
}

void loop() {

  if (Serial.available()) { 
    Xbee_data = Serial.read();

    if ( Xbee_data == '1' ) {
      digitalWrite( BLACKLIGHT, HIGH );
      blacklight_status = true;
      digitalWrite(13, HIGH);
    }
    else if ( Xbee_data == '0' ) {
      digitalWrite( BLACKLIGHT, LOW );
      blacklight_status = false;
      digitalWrite(13, LOW);
    }
  }
  
  readTags();  
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
      RFID_data = RFID.read();
      newtag[z] = RFID_data;
    }
    RFID.flush(); // stops multiple reads
    // do the tags match up?
    checkmytags();
  }
  // now do something based on tag type
  if (ok > 0) // if we had a match
  {
    Serial.println("Accepted");
    toggleBlacklight();
    delay(1000);
    ok = -1;
  }
  else if (ok == 0) // if we didn't have a match
  {
    Serial.print("Rejected:");
    // I (Phil) added this to print out the tag ID if there is no match
    for(int x=0; x<14; x++)
      Serial.print(" " + String(newtag[x]));
    ok = -1;
  }
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

void toggleBlacklight() {
  blacklight_status = !blacklight_status;

  if( blacklight_status )
    digitalWrite( BLACKLIGHT, HIGH );
  else
    digitalWrite( BLACKLIGHT, LOW );

}


