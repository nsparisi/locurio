#include <SoftwareSerial.h>

int ledPin = 10;
int data;

SoftwareSerial XBee(11, 12);  // Rx, Tx

void setup()
{
  pinMode( ledPin, OUTPUT );
  Serial.begin(9600);
  Serial.println("Begin");
  
  XBee.begin(9600);
}

void loop()
{
  if (XBee.available())
  { 
    data = XBee.read();
    if ( data == '1' ) {
      analogWrite( ledPin, 255 );
    }
    else if ( data == '0' ) {
      analogWrite( ledPin, 0 );
    }
      
    Serial.println(data);
  }
  
}

