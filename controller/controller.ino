#include <Keyboard.h>
void setup() {
  Keyboard.begin();
  Serial.begin(9600);
  pinMode(2, INPUT_PULLUP);
  pinMode(3, INPUT_PULLUP);
  pinMode(4, INPUT_PULLUP);
  pinMode(5, INPUT_PULLUP);
  pinMode(6, INPUT_PULLUP);
  Serial.println("Start");

}
  bool isPressed2;
  bool isPressed3;
  bool isPressed4;
  bool isPressed5;
  bool isPressed6;

// the loop function runs over and over again forever
void loop() {
  
  if (digitalRead(2) == LOW && !isPressed2)
  {
    Serial.println("pin 2");
    Keyboard.press('w');
    isPressed2 = true;

  }
  if (digitalRead(3) == LOW && !isPressed3)
  {
    Serial.println("pin 3");
    Keyboard.press('d');
    isPressed3 = true;

  }
  if (digitalRead(4) == LOW && !isPressed4)
  {
    Serial.println("pin 4");
    Keyboard.press('k');
    isPressed4 = true;
  }
  if (digitalRead(5) == LOW && !isPressed5)

  {
    Serial.println("pin 5");
    Keyboard.press('a');
    isPressed5 = true;

  }
  if (digitalRead(6) == LOW && !isPressed6)
  {
    Serial.println("pin 6");
    Keyboard.press('s');
    isPressed6 = true;
  }

  //   if (digitalRead(2) == LOW && !isPressed2)
  // {
  //   Serial.println("pin 2");
  //   Keyboard.press('w');
  //   isPressed2 = true;

  // }
  // if (digitalRead(3) == LOW && !isPressed3)
  // {
  //   Serial.println("pin 3");
  //   Keyboard.press('d');
  //   isPressed3 = true;

  // }
  // if (digitalRead(4) == LOW && !isPressed4)
  // {
  //   Serial.println("pin 4");
  //   Keyboard.press('k');
  //   isPressed4 = true;
  // }
  // if (digitalRead(5) == LOW && !isPressed5)

  // {
  //   Serial.println("pin 5");
  //   Keyboard.press('a');
  //   isPressed5 = true;

  // }
  // if (digitalRead(6) == LOW && !isPressed6)
  // {
  //   Serial.println("pin 6");
  //   Keyboard.press('s');
  //   isPressed6 = true;
  // }

  if (digitalRead(2) == HIGH)
  {
    Keyboard.release('w');
    isPressed2 = false;
  }
  if (digitalRead(3) == HIGH)
  {
    Keyboard.release('d');
    isPressed3 = false;
  }
  if (digitalRead(4) == HIGH)
  {
    Keyboard.release('k');
    isPressed4 = false;
  }
  if (digitalRead(5) == HIGH)
  {
    Keyboard.release('a');
    isPressed5 = false;
  }
  if (digitalRead(6) == HIGH)
  {
    Keyboard.release('s');
    isPressed6 = false;
  }

  // if (digitalRead(2) == LOW)
  // {
  //   Serial.println("pin 2");
  //   Keyboard.press('w');
  //   Keyboard.release('w');
  //
  //
  // }
  // if (digitalRead(3) == LOW)
  // {
  //   Serial.println("pin 3");
  //   Keyboard.press('d');
  //   Keyboard.release('d');
  //
  //
  // }
  // // if (digitalRead(4) == LOW)
  // // {
  // //   Serial.println("pin 4");
  // //   Keyboard.press('k');
  // //   Keyboard.release('k');
  // //
  // //
  // // }
  // if (digitalRead(4) == LOW && !isPressed4)
  // {
  //   Serial.println("pin 4");
  //   Keyboard.press('k');
  //   Keyboard.release('k');
  //   isPressed4 = true;
  //
  // }
  // if (digitalRead(5) == LOW)
  //
  // {
  //   Serial.println("pin 5");
  //   Keyboard.press('a');
  //   Keyboard.release('a');
  //
  //
  // }
  // if (digitalRead(6) == LOW)
  // {
  //   Serial.println("pin 6");
  //   Keyboard.press('s');
  //   Keyboard.release('s');
  //
  //
  // }
  //
  // if (digitalRead(4) == HIGH)
  // {
  //   isPressed4 = false;
  // }
}
