#ifndef abysshandler_h
#define abysshandler_h

#include "abyss.h"

#define XBEE_RX 3
#define XBEE_TX 2
#include <SoftwareSerial.h>
SoftwareSerial XBee(XBEE_RX, XBEE_TX); // RX, TX

class AbyssHandler : public IAbyssHandler
{
  private:

    bool isInitialized = false;
  public:

    // send a message over the desired serial
    // connection ending with a newline
    virtual void xbee_println(char* message)
    {
      if (!isInitialized)
      {
        XBee.begin(9600);
      }
      XBee.println(message);
    }

    // tests if any unread data is available
    // from the serial receive
    virtual bool xbee_available()
    {
      if (!isInitialized)
      {
        XBee.begin(9600);
      }
      return XBee.available();
    }

    // reads one byte of data from the serial connection
    virtual char xbee_read()
    {
      if (!isInitialized)
      {
        XBee.begin(9600);
      }
      return XBee.read();
    }

    // this is called when a message is sent
    // from Abyss to this unique arduino.
    virtual void received_message(char* message)
    {
    }
};

// ***********************************
// Create Abyss and handler objects.
// Define a unique ID.
// This arduino will only respond to messages that are sent with a header matching this ID.
// ***********************************
const char* MY_ID = "ALTAR";
AbyssHandler* handler = new AbyssHandler();
Abyss* abyss = new Abyss(MY_ID, handler);

#endif
