#include <ESP8266HTTPClient.h>
#include <ESP8266WiFi.h>
#include <Arduino.h>

char clientName[] = "test";
char Bearer[] = "secret";
char url[] = "http://demo.colours.ai/tabletop/api/update";
char heartbeaturl[] = "http://demo.colours.ai/tabletop/api/heartbeat";
char wifiSSID[] = "2";
char wifiPass[] = "2";



int inputPin = A0;               // choose the input pin (for PIR sensor)
int pirState = LOW;             // we start, assuming no motion detected
int val = 0;                    // variable for reading the pin status
int inputValue = 0;
int disconnected = 0;

int count = 0;

void setup() {

  Serial.begin(115200);                                  //Serial connection
  WiFi.begin(wifiSSID, wifiPass);
  pinMode(inputPin, INPUT);     // declare sensor as input

  Serial.println("******");
  Serial.println(">> Waiting for connection <<");

  while (WiFi.status() != WL_CONNECTED) {  //Wait for the WiFI connection completion
    delay(500);
    Serial.print(".");
    if(WiFi.status() == WL_NO_SSID_AVAIL || WiFi.status() == WL_CONNECT_FAILED) {
      Serial.print(connectionStatus ( WiFi.status() ) );
      return;
    }
  }

  Serial.println("******");

}

void(* resetFunc) (void) = 0; //declare reset function @ address 0


void loop() {

  inputValue = analogRead(inputPin);     // read the input pin

  if(inputValue >= 800) {
    val = HIGH;
  }
  else {
    val = LOW;
  }


  if (val == HIGH) {            // check if the input is HIGH
    //digitalWrite(ledPin, HIGH);  // turn LED ON
    if (pirState == LOW) {
      // we have just turned on
      Serial.println("Motion detected!");
      httpRequest(String(url));

      // We only want to print on the output change, not state
      pirState = HIGH;
    }
  }else {
    //digitalWrite(ledPin, LOW); // turn LED OFF
    if (pirState == HIGH){
      // we have just turned of
      Serial.println("Motion ended!");
      // httpRequest(0);
      // We only want to print on the output change, not state
      pirState = LOW;
    }
  }

   if(WiFi.status() != WL_CONNECTED){
      Serial.println(connectionStatus(WiFi.status()));
      resetFunc();  //call reset
   }

   count++;
   if(count < 600 && count != 30) {
     // Very important to avoid https://github.com/esp8266/Arduino/issues/1634
     delay(500);
   }
   else 
   {
       if(count != 30) {
          count = 0;
       }
       httpRequest(String(heartbeaturl));
       Serial.println("> 100");
   }
}

void httpRequest(String requestUrl) {

 if(WiFi.status() == WL_CONNECTED){   //Check WiFi connection status

   HTTPClient http;    //Declare object of class HTTPClient
   
   Serial.println(requestUrl); 

   http.begin(requestUrl);
   http.addHeader("Content-Type", "application/x-www-form-urlencoded");
   http.addHeader("Authorization", "Bearer " + String(Bearer));

   int httpCode = http.POST("status=" + String(1) + "&name=" + clientName);   //Send the request
   String payload = http.getString();                  //Get the response payload

   Serial.println("httpCode");   //Print HTTP return code

   Serial.println(httpCode);   //Print HTTP return code
   Serial.println(payload);    //Print request response payload

   http.end();  //Close connection

 }else{

    Serial.println("Error in WiFi connection");
    Serial.println(connectionStatus(WiFi.status()));

    resetFunc();  //call reset

 }

}



/********************************************************
/*  WiFi Connection Status                              *
/********************************************************/
String connectionStatus ( int which )
{
    switch ( which )
    {
        case WL_CONNECTED:
            return "Connected";
            break;

        case WL_NO_SSID_AVAIL:
            return "Network not availible";
            break;

        case WL_CONNECT_FAILED:
            return "Wrong password";
            break;

        case WL_IDLE_STATUS:
            return "Idle status";
            break;

        case WL_DISCONNECTED:
            return "Disconnected";
            break;

        default:
            return "Unknown";
            break;
    }
}
