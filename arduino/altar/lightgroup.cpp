#include "lightgroup.h"
#include "Arduino.h"

LedControl LightGroup::ledController = LedControl(22, 26, 24, 1); //din clk load 1

LightGroup::LightGroup(int row, int column)
{
  Count = 1;
  Row = row;
  Columns[0] = column;

  ledController.shutdown(0, false);
  CurrentState = false;
  Off();
}

LightGroup::LightGroup(int row, int column1, int column2)
{
  Count = 2;
  Row = row;
  Columns[0] = column1;
  Columns[1] = column2;

  ledController.shutdown(0, false);
  ledController.setIntensity(0, 15);
  CurrentState = false;
  Off();
}

LightGroup::LightGroup(int row, int column1, int column2, int column3)
{
  Count = 3;
  Row = row;
  Columns[0] = column1;
  Columns[1] = column2;
  Columns[2] = column3;

  ledController.shutdown(0, false);
  CurrentState = false;
  Off();
}

void LightGroup::On()
{
  SetState(true);
}

void LightGroup::Off()
{
  SetState(false);
}

void LightGroup::SetState(bool state)
{
  CurrentState = state;
  for (int i = 0; i < Count; i++)
  {
    /*   Serial.print("setting led :");
       Serial.print(Row);
       Serial.print(" - ");
       Serial.print(Columns[i]);
       Serial.print(" [ ");
       Serial.print(i);
       Serial.println(state ? "true" : "false");
      */
    ledController.setLed(0, Row, Columns[i], state);
  }
}
