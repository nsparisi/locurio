#ifndef LightGroup_h
#define LightGroup_h

#include <LedControl.h>
#include <inttypes.h>


class LightGroup
{
  private:  
	int Row;
	int Columns[8];
	int Count;
	bool CurrentState;
	
  public:
	LightGroup(int row, int column);
	LightGroup(int row, int column1, int column2);
	LightGroup(int row, int column1, int column2, int column3);
	
      void On();
	void Off();
	void SetState(bool state);

        static LedControl ledController;
        
};

#endif

