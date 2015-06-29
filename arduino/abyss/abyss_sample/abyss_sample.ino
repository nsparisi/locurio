// ***********************************
// Setup the XBEE as normal.
// ***********************************
#define XBEE_RX 2
#define XBEE_TX 3
#include <SoftwareSerial.h>
SoftwareSerial XBee(XBEE_RX, XBEE_TX); // RX, TX

void setup()
{
  // Set up the XBee port at 9600 baud. Make sure the baud rate 
  // matches the config setting of your XBee (this can be done in X-CTU).
  XBee.begin(9600);
  
  //set pin 13 as output since the board comes with an LED attached here
  pinMode(13, OUTPUT);   	
}

// ***********************************
// Import Abyss
// Define an IAbyssHandler to take care of the binding between arduino and xbee.
// ***********************************
#include <Abyss.h>
void send_message(const char* message);
class AbyssHandler : public IAbyssHandler
{
    public:        
    
        // send a message over the desired serial 
        // connection ending with a newline
        virtual void xbee_println(char* message)
        {
            XBee.println(message);
        }
        
        // tests if any unread data is available 
        // from the serial receive
        virtual bool xbee_available()
        {
            return XBee.available();
        }
        
        // reads one byte of data from the serial connection
        virtual char xbee_read()
        {
            return XBee.read();
        }
        
        // this is called when a message is sent 
        // from Abyss to this unique arduino.
        virtual void received_message(char* message)
        {
            if (Abyss::string_compare(message, "ON"))
            {
                digitalWrite(13, HIGH);
  
                send_message("Thanks for turning me on. :)");
            }
            
            if(Abyss::string_compare(message, "OFF"))
            {
                digitalWrite(13, LOW);
            }
        }
};

// ***********************************
// Create Abyss and handler objects.
// Define a unique ID.
// This arduino will only respond to messages that are sent with a header matching this ID.
// ***********************************
const char* MY_ID = "NICK";
AbyssHandler* handler = new AbyssHandler();
Abyss* abyss = new Abyss(MY_ID, handler);

// use abyss->send_message to 
// send messages back to abyss
void send_message(const char* message)
{
    abyss->send_message(message);
}

void loop()
{
    // intercept and handle all abyss 
    // logic coming in through XBEE
    abyss->check_xbee_messages();
}


