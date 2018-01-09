#include <SPI.h>
#include <Ethernet2.h>

char clientName[] = "tafelvoetbal";
char Bearer[] = "kHZ6ody2nQ9dmcMSCk5m";
char server[] = "demo.colours.ai";    // without http://

// assign a MAC address for the ethernet controller.
// fill in your address here:
byte mac[] = { 0x90, 0xA2, 0xDA, 0x10, 0x23, 0x25 };
// fill in an available IP address on your network here,
// for manual configuration:

// fill in your Domain Name Server address here:
IPAddress myDns(8, 8, 8, 8);

// initialize the library instance:
EthernetClient client;


// IPAddress server(149,210,217,195);



unsigned long lastConnectionTime = 0;             // last time you connected to the server, in milliseconds
const unsigned long postingInterval = 10L * 1000L; // delay between updates, in milliseconds
// the "L" is needed to use long type numbers

//int ledPin = 13;                // choose the pin for the LED
int inputPin = 2;               // choose the input pin (for PIR sensor)
int pirState = LOW;             // we start, assuming no motion detected
int val = 0;                    // variable for reading the pin status


void setup() {
  // start serial port:
  Serial.begin(9600);
  Serial.println("Script gestart");
  Serial.println("***");
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }

 // pinMode(ledPin, OUTPUT);      // declare LED as output
  pinMode(inputPin, INPUT);     // declare sensor as input

  // give the ethernet module time to boot up:
  delay(1000);
  // start the Ethernet connection using a fixed IP address and DNS server:
  if (Ethernet.begin(mac) == 0) {
    Serial.println("Geen ip toegewezen");
  }
  // print the Ethernet board/shield's IP address:
  Serial.print("My IP address: ");
  Serial.println(Ethernet.localIP());

  Serial.println("***");

}

void loop() {
  // if there's incoming data from the net connection.
  // send it out the serial port.  This is for debugging
  // purposes only:
  if (client.available()) {
    char c = client.read();
    Serial.write(c);
  }

  // if ten seconds have passed since your last connection,
  // then connect again and send data:
//  if (millis() - lastConnectionTime > postingInterval) {
//    httpRequest();
//  }

  val = digitalRead(inputPin);  // read input value
  if (val == HIGH) {            // check if the input is HIGH
    //digitalWrite(ledPin, HIGH);  // turn LED ON
    if (pirState == LOW) {
      // we have just turned on
      Serial.println("Motion detected!");
      httpRequest(1);

      // We only want to print on the output change, not state
      pirState = HIGH;
    }
  } else {
    //digitalWrite(ledPin, LOW); // turn LED OFF
    if (pirState == HIGH){
      // we have just turned of
      Serial.println("Motion ended!");
      // httpRequest(0);
      // We only want to print on the output change, not state
      pirState = LOW;
    }
  }

}



// this method makes a HTTP connection to the server:
void httpRequest(int onOrOff) {

  
  // close any connection before send a new request.
  // This will free the socket on the WiFi shield
  client.stop();

  // if there's a successful connection:
  if (client.connect(server, 80)) {
    Serial.println("connecting...");
    // send the HTTP POST request:


    client.println("POST /tabletop/api/update?status="+ String(onOrOff) + "&name=" + clientName  +" HTTP/1.1");
    client.println("Host: "+ String(server) );
    client.println("Cache-Control: no-cache");
    client.println("User-Agent: arduino-ethernet");
    client.println("Content-Type: application/x-www-form-urlencoded;");
    client.println("Content-Length: 0");
    client.println("Accept: */*");
    client.println("Authorization: Bearer " + String(Bearer));
    client.println();



    

//    String PostData = String(13);
//    PostData =  "{ \"status\": " +  String(onOrOff) +", \"clientName\"" + ":\""+ clientName+ "\"}";

//    String PostData = String(13);
//    PostData = "Name=Dion&Status=1";
//    
//    client.println(PostData);
//
//    
//    client.println();


//    client.println("Connection: close");

    

////    client.println("Content-Type: application/json;");
////    client.println("Content-Length: " + PostData.length() );
//
//
////    client.println();
////    client.println(PostData);
////    client.println();  
////     
////    if (client.connected()) {
////    Serial.println();
////    Serial.println("disconnecting.");
////    client.stop();
////    }
////    



    // note the time that the connection was made:
    lastConnectionTime = millis();
  } else {
    // if you couldn't make a connection:
    Serial.println("connection failed");
  }
}

