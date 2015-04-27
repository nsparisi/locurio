// ***********************************
// This is the Abyss sample sketch.
// Similiar to the blink sample sketch, this sketch will turn on and off the 13 LED.
// Send the message "ON" to this arduino from abyss to turn on the LED.
// Send the message "OFF" to turn off.
// ***********************************

// ***********************************
// Setup the XBEE as normal.
// ***********************************
#define XBEE_RX 3
#define XBEE_TX 2
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
// this is the ID of this arduino.
// this arduino will only respond to messages that
// are sent with a header matching this ID.
// ***********************************
const char* MY_ID = "NICK";

int string_index_of(char* text, const char* substring);
bool string_contains(char* text, const char* substring);
bool string_compare(const char* a, const char* b);
void split_buffer_into_head_and_body();
void clear_buffers();
void check_xbee_messages();

void xbee_println(char* message);
bool xbee_available();
char xbee_read();

void loop()
{
    // intercept and handle all XBEE logic
    check_xbee_messages();
}

// ***********************************
// Use this function to send messages back home to Abyss.
// ***********************************
void send_message(const char* message);

// ***********************************
// Abyss messages sent to this arduino will be passed here.
// Add any logic you expect to find from messages.
// ***********************************
void received_message(char* message)
{
    // compare the body with any expected messages.
    if (string_compare(message, "ON"))
    {
        digitalWrite(13, HIGH);
        
        send_message("Thanks for turning me on. :)");
    }
    
    if(string_compare(message, "OFF"))
    {
        digitalWrite(13, LOW);
    }
}

// ***********************************
// Delegate functions
// Implement these functions to handle XBee hooks.
// ***********************************
void xbee_println(char* message)
{
    XBee.println(message);
}

bool xbee_available()
{
    return XBee.available();
}

char xbee_read()
{
    return XBee.read();
}

// ***********************************
// Abyss communication logic. Add to every arduino.
// Performs the following functions:
//   -Read all messages sent to XBEE
//   -Messages are split by newline \n
//   -Message is parsed into header and body
//   -Checks header against this arduino ID
//   -sends message to received_message function
// ***********************************
const char* DELIMITER = "::";
char head[32];
char body[256];
char receive_buffer[256];
char send_buffer[256];
int buffer_index = 0;
bool check_message;

void send_message(const char* message)
{
    // prepare the message
    // clear buffer and add a header
    memset(send_buffer, 0, strlen(send_buffer));
    strncpy(send_buffer, MY_ID, strlen(MY_ID) + 1);
    strncat(send_buffer, DELIMITER, strlen(DELIMITER) + 1);
    strncat(send_buffer, message, strlen(message) + 1);

    // send to abyss
    xbee_println(send_buffer);
}

int string_index_of(char* text, const char* substring)
{
    char *result = strstr(text, substring);
    int position = result - text;
    return position;
}

bool string_contains(char* text, const char* substring)
{
    return string_index_of(text, substring) >= 0;
}

bool string_compare(const char* a, const char* b)
{
    return strcmp(a, b) == 0;
}

void split_buffer_into_head_and_body()
{
    int index = string_index_of(receive_buffer, DELIMITER);

    memcpy(head, receive_buffer, index);
    memcpy(body, receive_buffer + index + strlen(DELIMITER), strlen(receive_buffer) - index);
}

void clear_buffers()
{
    memset(receive_buffer, 0, strlen(receive_buffer));
    memset(head, 0, strlen(head));
    memset(body, 0, strlen(body));
}

void check_xbee_messages()
{  
    check_message = false;
    while (xbee_available())
    {
        // read one character at a time
        char c = xbee_read();

        // newline is the end of the message.
        // don't copy this char to the buffer
        if (c == '\n')
        {
            check_message = true;
            buffer_index = 0;
            break;
        }
        
        // carriage returns are ignored
        else if(c == '\r')
        {
            continue;
        }

        // otherwise add the char to the buffer
        receive_buffer[buffer_index] = c;
        buffer_index++;
    }

    // when we hit a newline
    // handle the contents of the buffer.
    if (check_message)
    {
        if (string_contains(receive_buffer, DELIMITER))
        {
            split_buffer_into_head_and_body();

            if (string_compare(head, MY_ID))
            {
                received_message(body);
            }
        }

        // always clean the buffers when a message it complete
        clear_buffers();
    }
}

// ***********************************
// END abyss logic.
// ***********************************




