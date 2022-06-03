void setup()
{
 
  Serial.begin(9600);
  Serial1.begin(38400);  //Default Baud for comm
//  Serial.println("The bluetooth gates are open.\n Connect to HC-05 from any other bluetooth device with 1234 as pairing key!.");
//  Serial1.write("AT+CMODE=1\n\r");
//  Serial1.write("AT+ROLE=1\n\r");
//  Serial1.write("AT+INQM=1,9,48\n\r");
//  Serial1.write("AT+INIT\n\r");
//  Serial1.write("AT+INQ\n\r");

}
bool rssiHasRun = false;
void rssi()
{
  delay(100);
  Serial1.write("AT+RESET\n\r");
  delay(100);
  Serial1.write("AT+CMODE=1\n\r");
  delay(100);
  Serial1.write("AT+ROLE=1\n\r");
  delay(100);
  Serial1.write("AT+INQM=1,9,10\n\r");
  delay(100);
  Serial1.write("AT+INIT\n\r");
  delay(100);
}

String devices[8];
String addresses[8];
int rssi[8];
String positions[3];
int strongestRssi;
int strongestNum;
String INQ = "+INQ:";
int counter = 0;

int hexToDec(String hexString)
{
  int a = hexString.charAt(2);
  int b = hexString.charAt(3);


  return a+b;
}
/*
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
*/
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
void loop()
{

//  if(rssiHasRun == false)
//  {
//    rssi();
//    rssiHasRun = true;
//  }

   String content = "";
   char character;


  while(Serial1.available()) {       // STORE OUTPUT INTO VARIABLE
       character = Serial1.read();
       content.concat(character);
       delay(5);                      
  }
  if (Serial.available()){
    Serial1.write(Serial.read());     //SEND COMMAND TO HC05
  }

  
  if(content!="")
  {
          Serial.print((String) "Length: " + content.length() + " CONTENT:" + content); //PRINT OUTPUT OF COMMAND
  }
  
  if(content.startsWith(INQ) && devices[7] == "") { 
    int counter =0;
    
      String newContent = content.substring(5);
      breakDownAddress(newContent,positions);
        for(int j = 0;j <= sizeof(addresses);j++)   
        {
          if(positions[0].equals(addresses[j]))               //CHECKS IF THE ADDRESS IS ALREADY IN THE ARRAY
          {
            counter++;
          }
        }
        if(counter == 0)
        {
          devices[k] = newContent;
          addresses[k] = positions[0];
          rssi[k] = hexToDec(positions[2]);
          Serial.println("Device"+(String)k + " : " + (String)devices[k]);
          k++;
        }
  }

  strongestRssi = rssi[0];                                    //SELECTS THE STRONGEST SIGNAL AND CONNECTS TO THE ADDRESS ASOCIATED WITH IT
  for(int l = 0;l<sizeof(rssi);l++)
  {
    if(rssi[l] > strongestRssi)
    {
      strongestRssi = rssi[l];
      strongestNum = l;
    }
  }
  String command = "AT+PAIR="+addresses[strongestNum]+"\n\r";
  writeString(command);

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
