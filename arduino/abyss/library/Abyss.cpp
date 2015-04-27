#include "Arduino.h"
#include "Abyss.h"

Abyss::Abyss(const char* id, IAbyssHandler* handler)
{
    my_id = id;
    my_handler = handler;
}

void Abyss::send_message(const char* message)
{
    // prepare the message
    // clear buffer and add a header
    memset(send_buffer, 0, strlen(send_buffer));
    strncpy(send_buffer, my_id, strlen(my_id) + 1);
    strncat(send_buffer, DELIMITER, strlen(DELIMITER) + 1);
    strncat(send_buffer, message, strlen(message) + 1);

    // send to abyss
    my_handler->xbee_println(send_buffer);
}

int Abyss::string_index_of(char* text, const char* substring)
{
    char *result = strstr(text, substring);
    int position = result - text;
    return position;
}

bool Abyss::string_contains(char* text, const char* substring)
{
    return string_index_of(text, substring) >= 0;
}

bool Abyss::string_compare(const char* a, const char* b)
{
    return strcmp(a, b) == 0;
}

void Abyss::split_buffer_into_head_and_body()
{
    int index = string_index_of(receive_buffer, DELIMITER);

    memcpy(head, receive_buffer, index);
    memcpy(body, receive_buffer + index + strlen(DELIMITER), strlen(receive_buffer) - index);
}

void Abyss::clear_buffers()
{
    memset(receive_buffer, 0, strlen(receive_buffer));
    memset(head, 0, strlen(head));
    memset(body, 0, strlen(body));
}

void Abyss::check_xbee_messages()
{  
    check_message = false;
    while (my_handler->xbee_available())
    {
        // read one character at a time
        char c = my_handler->xbee_read();

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

            if (string_compare(head, my_id))
            {
                my_handler->received_message(body);
            }
        }

        // always clean the buffers when a message it complete
        clear_buffers();
    }
}

