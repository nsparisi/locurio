#ifndef Abyss_h
#define Abyss_h

#include "Arduino.h"

// ***********************************
// IAbyssHandler interface.
// ***********************************
class IAbyssHandler
{
  public:
    virtual ~IAbyssHandler() {}
    virtual void xbee_println(char* message) = 0;
    virtual bool xbee_available() = 0;
    virtual char xbee_read() = 0;
    virtual void received_message(char* message) = 0;
};

// ***********************************
// Abyss class.
// Handles all communication between the arduino and Abyss.
// ***********************************
class Abyss
{
  public:
    Abyss(const char* id, IAbyssHandler* handler);
    void check_xbee_messages();
    void send_message(const char* message);

    // string helpers
    static int string_index_of(char* text, const char* substring);
    static bool string_contains(char* text, const char* substring);
    static bool string_compare(const char* a, const char* b);

  private:
    const char* DELIMITER = "::";
    const char* my_id;
    IAbyssHandler* my_handler;
    char head[32];
    char body[64];
    char receive_buffer[64];
    char send_buffer[64];
    int buffer_index = 0;
    bool check_message;

    void split_buffer_into_head_and_body();
    void clear_buffers();
};

#endif

