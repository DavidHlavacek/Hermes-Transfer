void setup()
{
 
  Serial.begin(9600);
  Serial1.begin(38400);  //Default Baud for comm
//  Serial.println("The bluetooth gates are open.\n Connect to HC-05 from any other bluetooth device with 1234 as pairing key!.");
//    delay(100);
//    Serial1.write("AT+CMODE=4\n\r");
//    delay(100);
//  Serial1.write("AT+CMODE=1\n\r");
//  Serial1.write("AT+ROLE=1\n\r");
//  Serial1.write("AT+INQM=1,9,48\n\r");
//  Serial1.write("AT+INIT\n\r");
//  Serial1.write("AT+INQ\n\r");

}
bool rssiHasRun = false;
void connect2()
{
//  delay(100);
//  String command = "AT+CMODE=3\n\r";
//  writeString(command);
//  delay(100);
}

String devices[8];
String addresses[8];
int rssi[8];
String positions[3];
int strongestRssi;
int strongestNum;
String INQ = "+INQ:";
String INQ2 = "\n+INQ:";
int counter = 0;

// int hexToDec(String hexString)
// {
//   int a = hexString.charAt(2);
//   int b = hexString.charAt(3);


//   return a+b;
// }

int hexToDec(const char *hex) //Converts hexadecimal to signed decimal(kinda) 
{
    uint16_t value;
    for (value = 0; *hex; hex++) {
        value <<= 4;
        if (*hex >= '0' && *hex <= '9')
            value |= *hex - '0';
        else if (*hex >= 'A' && *hex <= 'F')
            value |= *hex - 'A' + 10;
        else if (*hex >= 'a' && *hex <= 'f')
            value |= *hex - 'a' + 10;
        else
            break;
    }
    return value;
}

void writeString(String stringData) { // Used to serially push out a String with Serial.write()
  
  for (int i = 0; i < stringData.length(); i++)
  {
    Serial1.write(stringData[i]);
  }
}

void breakDownAddress(String address, String positions[])//BREAKS THE INQ DATA INTO ADRESS/SMTH/RSSI
{
  int commaIndex = address.indexOf(',');
  int secondCommaIndex = address.indexOf(',', commaIndex + 1);

  String firstValue = address.substring(0, commaIndex);
  String secondValue = address.substring(commaIndex + 1, secondCommaIndex);
  String thirdValue = address.substring(secondCommaIndex + 1);
  
  positions[0] = firstValue;
  positions[1] = secondValue;
  positions[2] = thirdValue;
}

int k =0;
int temp = -999;
unsigned long StartTime = 0;
unsigned long CurrentTime = 0;
bool commHasRun = false;
bool test = false;
void loop()
{
  
    if(test) {
      String command = "AT+PAIR="+addresses[strongestNum]+",15\n\r";
      writeString((String)"AT+PAIR="+addresses[strongestNum]+",15\n\r");
      writeString(command);
      test = false;
    }

//  if(rssiHasRun == false)
//  {
//    connect2();
//    rssiHasRun = true;
//  }

   String content = "";
   char character;


  while(Serial1.available()) {       // STORE OUTPUT INTO VARIABLE
       character = Serial1.read();
       content.concat(character);
       delay(20);         
       Serial.println("test2");             
  } 
  if (Serial.available()){
    Serial1.write(Serial.read());     //SEND COMMAND TO HC05
  } 

//  delay(100);
  int j = 0;
  if(content!="")
  {
    Serial.print((String) "Length: " + content.length() + " CONTENT:" + content); //PRINT OUTPUT OF COMMAND
  }
  bool diffRssi = false;
  if((content.startsWith(INQ) || content.startsWith(INQ2)) && devices[7] == "") { 
    StartTime = millis();
    CurrentTime = millis();
    int counter = 0;
      int enterIndex = content.indexOf('\n');
      String newContent = content.substring(5, enterIndex);
      breakDownAddress(newContent,positions);
      Serial.println((String) "0Device"+ k + " : " + devices[k]);
          Serial.println((String) "0Address"+ k + " : " + addresses[k]);
          Serial.println((String) "0Rssi"+ k + " : " + rssi[k]);
      if(positions[2].indexOf("7FFF")<1) {
        for(j = 0;j <= 8;j++)   
        {
          Serial.println("test3");
          if(positions[0].indexOf(addresses[j])>0) {             //CHECKS IF THE ADDRESS IS ALREADY IN THE ARRAY
            counter++;
            const char *rssiHex1 = positions[2].c_str(); 
            if(!(hexToDec(rssiHex1) == (rssi[j]))) {
              diffRssi = true;
            }
          }
        }
        if(counter == 0) {
          devices[k] = newContent;
          positions[0].replace(":", ",");
          addresses[k] = positions[0];
          const char *rssiHex2 = positions[2].c_str(); 
          rssi[k] = hexToDec(rssiHex2);
          Serial.println((String) "Device"+ k + " : " + devices[k]);
          Serial.println((String) "Address"+ k + " : " + addresses[k]);
          Serial.println((String) "Rssi"+ k + " : " + rssi[k]);
          k++;
        } else if (counter != 0 && diffRssi) {
          const char *rssiHex2 = positions[2].c_str(); 
          rssi[j] = hexToDec(rssiHex2);
          Serial.println((String) "Device"+ j + " : " + devices[j]);
          Serial.println((String) "Address"+ j + " : " + addresses[j]);
          Serial.println((String) "Rssi"+ j + " : " + rssi[j]);          
        }
    }
  }

  unsigned long ElapsedTime = CurrentTime - StartTime;
  strongestRssi = 0;
  //SELECTS THE STRONGEST SIGNAL AND CONNECTS TO THE ADDRESS ASOCIATED WITH IT
  
  if((ElapsedTime > 100000 && !commHasRun) || (devices[7]!="" && !commHasRun)) {
    commHasRun = true;
    int l;
    for(l = 0;l<8;l++)
    {
      Serial.println("YOYOYO");
      Serial.println(addresses[l]);
      Serial.println(rssi[l]);
      if(rssi[l] > 0) {
        if(rssi[l] > strongestRssi)
        {
          strongestRssi = rssi[l];
          strongestNum = l;
        }
      }
    }
//    while(true) {
      Serial.println((String)"Device: " +addresses[strongestNum]);
      delay(250);
      String command = "AT+PAIR="+addresses[strongestNum]+",15\n\r";
      writeString((String)"AT+PAIR="+addresses[strongestNum]+",15\n\r");
      writeString(command);
      
      Serial.println("hello?");
      test = true;
      
//    }
  }
  
//  if(devices[8]!="")                                        //CODE TO BE DELETED (PROBABLY)
//  {
//    for(int x = 0; x < 9; x++)
//    {
//      String sa[3]; 
//      int r=0, t=0;
//      for (int i=0; i < devices[x].length(); i++)
//      {
//       if(devices[x].charAt(i) == ',') 
//        { 
//          sa[t] = devices[x].substring(r, i); 
//          r=(i+1); 
//          t++; 
//        }
//      }
//      Serial.println(hexToDec(sa[2]));
//      if(hexToDec(sa[2])>temp){
//        temp = hexToDec(sa[2]);
//        devices[0] = sa[0];
//        }    
//    }
//  }
// String command = "";
//  if(devices[8]!=""){
//    Serial.println(devices[0]);
//    String address; 
//      int r=0, t=0;
//      for (int i=0; i < devices[0].length(); i++)
//      { 
//       if(devices[0].charAt(i) == ':') 
//        { 
//          address += devices[0].substring(r, i);
//          address += ',';
//          r=(i+1); 
//          t++; 
//        }
//      }
//      command = "AT+PAIR="+address+"\n\r";
//      writeString(command);
//  }
}
