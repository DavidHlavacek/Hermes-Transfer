#include <vector>

const int firstButtonPin = 6;
const int secButtonPin = 7;
int firstBttonState = 0;
int secButtonState = 0;

String command;
const char *cmd;

void setup() {
  Serial.begin(9600);
  Serial1.begin(38400);  //Default Baud for comm
  pinMode(firstButtonPin, INPUT);
  pinMode(secButtonPin, INPUT);
}

void reset();
void setUp();
void search();

void breakDownAddress(String, String[]);
int hexToDec(const char*);

int find(std::vector<int>);
void connect(int);
void loop() {
    firstBttonState = digitalRead(firstButtonPin);
    secButtonState = digitalRead(secButtonPin);
    
    while (Serial.available()) {
        Serial1.write(Serial.read()); //Send command to HC05
    }
    while (Serial1.available()) {
         Serial.write(Serial1.read()); //Output response
    }
    Serial1.flush();
    if (firstBttonState == HIGH) {
        Serial.println("Executing...");
        search();
        delay(800);
        // Serial.println("button1Pressed");
    } else if (secButtonState == HIGH) {
        // Serial.write("Reset button pressed\n");
        // search();
        // delay(800);
        Serial1.write('1');
    }
}

void reset() {
    //reset variables                 //TODO: EMPTY DEVICES LIST!
    // Serial.println("resetStart");
    // Serial.println("AT:");
    Serial1.flush();
    command = "AT\r\n";
    cmd = command.c_str();
    Serial1.write(cmd);
    // while (Serial1.available()) {
    //      Serial.write(Serial1.read()); //Output response
    // }
    Serial1.flush();
    delay(800);
    Serial1.flush();
    // Serial.println("AT+ORGL:");
    command = "AT+ORGL\r\n";
    cmd = command.c_str();
    Serial1.write(cmd);
    // while (Serial1.available()) {
    //      Serial.write(Serial1.read()); //Output response
    // }
    Serial1.flush();
    delay(800);
    Serial1.flush();
    // Serial.println("RESET:");
    command = "AT+RESET\r\n";
    cmd = command.c_str();
    Serial1.write(cmd);
    // while (Serial1.available()) {
    //      Serial.write(Serial1.read()); //Output response
    // }
    Serial1.flush();
    delay(800);
    Serial1.flush();
    // Serial.println("resetEnd");
}

void setUp() {
    reset();
    // Serial.println("setupStart");
    // Serial.println("CMODE:");
    Serial1.flush();
    command = "AT+CMODE=1\r\n";
    cmd = command.c_str();
    Serial1.write(cmd);
    // while (Serial1.available()) {
    //      Serial.write(Serial1.read()); //Output response
    // }
    Serial1.flush();
    delay(800);
    Serial1.flush();
    // Serial.println("ROLE:");
    command = "AT+ROLE=1\r\n";
    cmd = command.c_str();
    Serial1.write(cmd);
    Serial1.flush();
    // while (Serial1.available()) {
    //      Serial.write(Serial1.read()); //Output response
    // }
    Serial1.flush();
    delay(800);
    Serial1.flush();
    // Serial.println("INQM");
    command = "AT+INQM=1,9,48\r\n";
    cmd = command.c_str();
    Serial1.write(cmd);
    // while (Serial1.available()) {
    //      Serial.write(Serial1.read()); //Output response
    // }
    delay(800);
    Serial1.flush();
    command = "AT+INIT\r\n";
    cmd = command.c_str();
    Serial1.write(cmd);
    // while (Serial1.available()) {
    //      Serial.write(Serial1.read()); //Output response
    // }
    delay(800);
    Serial1.flush();
    // Serial.println("setupEnd");
}

void search() {
    setUp();
    // Serial.println("searchStart");
    char byte;
    String line;

    String positions[3];

    std::vector<String> addressList;
    std::vector<String> groupList;
    std::vector<int> rssiList;
    // Serial.println("INQ:");
    command = "AT+INQ\r\n";
    cmd = command.c_str();
    Serial1.write(cmd);
    // while (Serial1.available()) {
    //      Serial.write(Serial1.read()); //Output response
    // }
    delay(1000);
    // Serial.println("searchWhileLoopStart");
    while(Serial1.available()) {       // STORE OUTPUT INTO VARIABLE
        
        byte = Serial1.read();
        // Serial.write(byte);
        line.concat(byte);
        // Serial.println(line);
        delay(20);
        if(byte == '\n' && line.indexOf("INQ")>0) {
            Serial1.flush();
            breakDownAddress(line, positions);
            line.remove(0);

            positions[0].replace(":", ",");
            const char *rssiHex = positions[2].c_str();
            int rssiDec = hexToDec(rssiHex);

            bool isDuplicate = false;
            bool diffRssi = false;
            int i = 0;
            // Serial.println("searchWhileForLoopStart");
            for(i = 0; i<addressList.size(); i++) {         //Go through list
                if(addressList[i] == positions[0]) {        //Check if duplicate
                    isDuplicate = true;
                    if(rssiList[i] != rssiDec) {            //If duplicate check if diff rssi
                    diffRssi = true;
                    }
                    break;
                }
            }
            // Serial.println("searchWhileForLoopEnd");

            if(!isDuplicate) {
                addressList.push_back(positions[0]);
                groupList.push_back(positions[1]);
                rssiList.push_back(rssiDec);
            } else if (isDuplicate && diffRssi) {
                if(rssiDec > rssiList[i]) {
                    rssiList[i] = rssiDec; 
                }
            }
            // for(int i = 0; i < addressList.size(); i++)
            // {
            //     Serial.println(addressList[i] + " " + groupList[i] + " " + rssiList[i]);
            // }
            // Serial.println(positions[0] + positions[1] + rssiDec);
            // Serial1.flush();             //MAYBE NEEDED TO INCLUDE!!!
        }
    }
    Serial1.flush();
    // Serial.println("searchWhileLoopEnd");
    // Serial.println("searchEnd");
    connect(find(rssiList), addressList);

}

void breakDownAddress(String line, String positions[]) {
    int firstColumnIndex = line.indexOf(':');
    int firstCommaIndex = line.indexOf(',');
    int secondCommaIndex = line.indexOf(',', firstCommaIndex + 1);

    String address = line.substring(firstColumnIndex + 1, firstCommaIndex);
    String group = line.substring(firstCommaIndex + 1, secondCommaIndex);
    String rssi = line.substring(secondCommaIndex + 1, secondCommaIndex + 5);

    positions[0] = address;
    positions[1] = group;
    positions[2] = rssi;
}

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

int find(std::vector<int> rssiList) {
    // Serial.println("findStart");
    int strongestRssi = 0;
    int strongestIndex = 0;
    for(int i = 0; i < rssiList.size(); i++)
        {
            if(rssiList[i] > strongestRssi)
            {
                strongestRssi = rssiList[i];
                strongestIndex = i;
            }
        }
    // Serial.println("findEnd");
    return strongestIndex;
}
void connect(int index, std::vector<String> addressList) { 
    
    command = "AT+RNAME?" + addressList[index] + "\r\n";
    cmd = command.c_str();
    Serial1.write(cmd);
    delay(800);
    String name;
    while (Serial1.available()) {     
        char byte = Serial1.read();
        name.concat(byte);
        delay(20); 
    }
    Serial1.flush();
    Serial.println("Connecting to " + name); 

    command = "AT+PAIR=" + addressList[index] + ",15\r\n";
    cmd = command.c_str();
    Serial1.write(cmd);
    delay(800);
    Serial1.flush();
    // Serial.println("connEnd");
}