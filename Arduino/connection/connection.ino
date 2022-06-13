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
//      delay(500);
//      Serial1.write("\r\nAT+ORGL\r\n");
//      delay(500);
//      Serial1.write("\r\nAT+RESET\r\n");
//      delay(500);
//      Serial1.write("\r\nAT+ROLE=1\r\n");
//      delay(500);
//      Serial1.write("\r\nAT+CMODE=1\r\n");
//      delay(500);
//      Serial1.write("\r\nAT+ROLE=1\r\n");
//      delay(500);
//      Serial1.write("\r\nAT+INQM=1,8,20\r\n");
//      delay(500);
//      Serial1.write("\r\nAT+INQ\r\n");
//      delay(500);
//      delay(100);
//      Serial1.write("AT+RESET\r\n");
//
//      delay(100);
//       serial1Flush();
//       Serial1.flush();


}
bool rssiHasRun = false;
void connect2()
{
      
      
      
}

void serial1Flush(){
  while(Serial1.available() > 0) {
    char t = Serial1.read();
  }
}

String devices[8];
String addresses[8];
int rssi[8];
String positions[3];
int strongestRssi;
int strongestNum;
String INQ = "+INQ:";
String INQ2 = "INQ:";
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
    delay(100);
    Serial1.write(stringData[i]);
  }
}

void breakDownAddress(String address, String positions[])//BREAKS THE INQ DATA INTO ADRESS/SMTH/RSSI
{
  int commaIndex = address.indexOf(',');
  int secondCommaIndex = address.indexOf(',', commaIndex + 1);

  String firstValue = address.substring(0, commaIndex);
  String secondValue = address.substring(commaIndex + 1, secondCommaIndex);
  String thirdValue = address.substring(secondCommaIndex + 1, secondCommaIndex + 5);
  
  positions[0] = firstValue;
  positions[1] = secondValue;
  positions[2] = thirdValue;
}

int k =0;
int temp = -999;
unsigned long StartTime = 0;
unsigned long CurrentTime = 0;
bool commHasRun = false;
bool test = true;
int cmdIndex = 0;
bool executed = false;
int inq = 0;
      String command;
      const char * cmd;
    boolean done = false;
void loop()
{

    if(!done) {
  //  delay(500);
  //      String command = "\r\nAT+INQM?\r\n";
  //      const char * cmd = command.c_str();
  //      Serial1.write(cmd);
  //      delay(500);
  //    if(test) {
  ////      String command = "AT+PAIR="+addresses[strongestNum]+",15\n\r";
  ////      writeString((String)"AT+PAIR="+addresses[strongestNum]+",15\n\r");
  ////      writeString(command);
  //      test = false;
  //        delay(500);
  //      Serial1.write("AT+ORGL\n\r");
  //      delay(500);
  //      Serial1.write("AT+RESET\n\r");
  //      delay(500);
  //      Serial1.write("AT+ROLE=1\n\r");
  //      delay(500);
  //      Serial1.write("AT+CMODE=1\n\r");
  //      delay(500);
  //      Serial1.write("AT+ROLE=1\n\r");
  //      delay(500);
  //      Serial1.write("AT+INQM=1,8,20\n\r");
  //      delay(500);
  //      Serial1.write("AT+INQ\n\r");
  //      delay(500);
  //    }
    if(cmdIndex < 12)
    {
        Serial.println(cmdIndex);
        if(cmdIndex == 0 && !executed){
          Serial.println("TEST0");
          delay(100);
          command = "AT\r\n";
          cmd = command.c_str();
           
          Serial1.write(cmd);
          executed = true;
        }
        else if(cmdIndex == 1 && !executed){
          Serial.println("TEST1");
          delay(100);
          command = "AT\r\n";
          cmd = command.c_str();
           
          Serial1.write(cmd);
          executed = true;
        }
        else if(cmdIndex == 2 && !executed){
          Serial.println("TEST2");
          delay(100);
          command = "AT+ORGL\r\n";
          cmd = command.c_str();
           
          Serial1.write(cmd);
          executed = true;
  
        }
        else if(cmdIndex == 3 && !executed){
          delay(100);
          command = "AT\r\n";
          cmd = command.c_str();
           
          Serial1.write(cmd);
          executed = true;
  
        }
        else if(cmdIndex == 4 && !executed){
  //        delay(100);
  //        command = "AT+RESET\r\n";
  //        cmd = command.c_str();
  //         
  //        Serial1.write("AT+RESET\r\n");
  //        if (Serial1.available()){
  //          Serial1.write("AT+RESET\r\n");     //SEND COMMAND TO HC05
  //          executed = true;
//            delay(100);
//            Serial1.write("AT+RESET\r\n");
//      
//            delay(100);
            cmdIndex++;
  //        }
  
          
  
        }
        else if(cmdIndex == 5 && !executed){
          delay(100);
          command = "AT+CMODE=1\r\n";
          cmd = command.c_str();
           
          Serial1.write("AT+CMODE=1\r\n");
          executed = true;
        }
        else if(cmdIndex == 6 && !executed){
          delay(100);
          command = "AT+ROLE=1\r\n";
          cmd = command.c_str();
           
          Serial1.write(cmd);
          executed = true;
        }
        else if(cmdIndex == 7 && !executed){
          delay(100);
          Serial.println("TEST7");
          command = "AT+INQM=1,8,20\r\n";
          cmd = command.c_str();
           
          Serial1.write(cmd);
  //        Serial1.write("AT+INQM=1,8,20\r\n");
          executed = true;
   
        }
        else if(cmdIndex == 8 && !executed){ 
          delay(100);
          command = "AT+INIT\r\n";
          cmd = command.c_str();
           
          Serial1.write(cmd);
          executed = true;
  
        }
        else if(cmdIndex == 9  && !executed){
          delay(100);
          Serial.println("TEST9");
          command = "AT+INQM=1,8,20\r\n";
          cmd = command.c_str();
           
          Serial1.write(cmd);
  //        Serial1.write("AT+INQM=1,8,20\r\n");
          executed = true;
  
        }
        else if(cmdIndex == 10 && !executed){
          delay(100);
          command = "AT+CMODE?\r\n";
          cmd = command.c_str();
           
          Serial1.write(cmd);
          executed = true;
  
        }
        else if(cmdIndex == 11 && !executed){
          delay(100);
          command = "AT+CMODE?\r\n";
          cmd = command.c_str();
           
          Serial1.write(cmd);
          executed = true;
  
        }
    }
  
     String content = "";
     char character;
      if (Serial.available()){
      Serial1.write(Serial.read());     //SEND COMMAND TO HC05
    }
    boolean firstplus = false;
    while(Serial1.available()) {       // STORE OUTPUT INTO VARIABLE
  //       Serial.println("STOOPID");
         character = Serial1.read();
         if(character == '+'){firstplus = true;}
         if(firstplus && character == '+')
         {
          break;
         }
         Serial.write(character);
         content.concat(character);
         delay(20);         
    }
    if(inq < 8 && cmdIndex > 11)
     {
  //        delay(2000);
            Serial.println("CMON");
          delay(100);
  //         
          command = "AT+INQ\r\n";
          cmd = command.c_str();
          Serial1.write(cmd);
          inq++;
     }
   
     
    
  
  
  //  delay(100);
    int j = 0;
    if(content!="")
    {
      Serial.print((String) "Length: " + content.length() + " CONTENT:" + content); //PRINT OUTPUT OF COMMAND
      executed = false;
      cmdIndex++;
  //    if(cmdIndex > 11) {inq++;}
      command = "";
      cmd = "";
    }
    bool diffRssi = false;
  
    if((content.startsWith(INQ) || content.startsWith(INQ2)) && devices[7] == "") { 
      StartTime = millis();
      CurrentTime = millis();
      int counter = 0;
        int enterIndex = content.indexOf('\n');
        String newContent = content.substring(4, enterIndex);
        breakDownAddress(newContent,positions);
        Serial.println((String) "0Device"+ k + " : " + devices[k]);
            Serial.println((String) "0Address"+ k + " : " + addresses[k]);
            Serial.println((String) "0Rssi"+ k + " : " + rssi[k]);
          positions[0].replace(":",",");
          for(j = 0;j < 8;j++)   
          {
            if(positions[0]!=""){
              Serial.println("test3");
              if(positions[0].equals(addresses[j])) {             //CHECKS IF THE ADDRESS IS ALREADY IN THE ARRAY
                counter++;
                const char *rssiHex1 = positions[2].c_str(); 
                if(!(hexToDec(rssiHex1) == (rssi[j]))) {
                  diffRssi = true;
                  rssi[j] = hexToDec(rssiHex1);
                }
              }
            }
          }
          if(counter == 0) {
            devices[k] = newContent;
            addresses[k] = positions[0];
            const char *rssiHex2 = positions[2].c_str(); 
            rssi[k] = hexToDec(rssiHex2);
            Serial.println((String) "Device"+ k + " : " + devices[k]);
            Serial.println((String) "Address"+ k + " : " + addresses[k]);
            Serial.println((String) "Rssi"+ k + " : " + rssi[k]);
            k++;
          } 
    }
          CurrentTime = millis();

    unsigned long ElapsedTime = CurrentTime - StartTime;
    strongestRssi = 0;
    //SELECTS THE STRONGEST SIGNAL AND CONNECTS TO THE ADDRESS ASOCIATED WITH IT
    
    if((ElapsedTime > 10000 && !commHasRun)) {
      commHasRun = true;
      int l;
      for(l = 0;l<8;l++)
      {
        Serial.println("YOYOYO");
        Serial.println(addresses[l]);
        Serial.println(rssi[l]);
  
          if(rssi[l] > strongestRssi)
          {
            strongestRssi = rssi[l];
            strongestNum = l;
          }
        
      }
      if(!done) {
  //    while(true) {
        Serial.println((String)"aTRONGEST Device: " + addresses[strongestNum] + " " + strongestNum);
        delay(100);
        String command = "AT+PAIR="+addresses[strongestNum]+",15\n\r";
        const char * cmd = command.c_str();
//        serial1Flush();
//        Serial1.flush();
        Serial1.write(cmd);
        delay(100);
        writeString((String)"AT+PAIR="+addresses[strongestNum]+",15\n\r");
        delay(100);
        writeString(command);
        
        Serial.println("hello?");
        done = true;
      }
        
        
        
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
  
    content = "";
    }
}