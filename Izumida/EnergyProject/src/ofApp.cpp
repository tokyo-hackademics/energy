#include "ofApp.h"

//--------------------------------------------------------------
void ofApp::setup(){
    // setting http
    url = "http://192.168.3.38:8080/api/posts";
    ofAddListener(httpUtils.newResponseEvent, this, &ofApp::newResponse);
    httpUtils.start();

    // setting twe-lite
    twelite.setup("/dev/tty.usbserial-AHXMUJD0", 115200);
    buff.clear();
    
    analogVal = 0;
    
    // setting gui
    p_gui = new ofxUICanvas(320, ofGetHeight());
    
    p_gui->addSlider("AnalogValue", 0, 5000, &analogVal, 300, 20);
    p_gui->addSlider("PowerValue", 0, 100, &powerVal, 300, 20);
    p_gui->addSlider("EnergyValue", 0, 10, &energyVal, 300, 20);
    
    p_gui->setVisible(false);
    
    for (int i = 0; i < 300; i++) {
        T_3DROTATE_POS pos = {
            ofRandom(360),
            ofRandom(360),
            ofRandom(100, 200)
        };
        
        boxPos.push_back(pos);
    }
    
    buttonImg.loadImage("button.png");
    buttonRect.set(
        (ofGetWidth() / 2) - ((float)buttonImg.width / 2),
        700 - buttonImg.height / 2,
        buttonImg.width,
        buttonImg.height
    );
    
    ofBackground(0);
    font.loadFont("frabk.ttf", 30);
    
    colorB = 0;
}

//--------------------------------------------------------------
void ofApp::update(){
    if (twelite.isInitialized()) {
        int len = twelite.available();
        if (len != 0) {
            unsigned char* p_buff = new unsigned char[len];
            twelite.readBytes(p_buff, len);
            for (int i = 0; i < len; i++) {
                buff += p_buff[i];
            }
            delete [] p_buff;

            if (buff.c_str()[buff.length() - 1] == '\n') {
                if (buff.c_str()[buff.length() - 2] == '\r') {
                    if (buff.c_str()[0] == ':') {
                        if (buff.length() == 51) {
                            string dataStr = buff.substr(1, 48);
                            // cout << dataStr << std::endl;
                        
                            int a0 = ofHexToInt(dataStr.substr(36, 2));
                            int f = ofHexToInt(dataStr.substr(44, 2));
                            int f0 = f & 0x03;
                        
                            analogVal = (a0 * 4 + f0) * 4;
                            powerVal = analogVal * analogVal / 100000 * 2;
                            energyVal += powerVal / 3600;
                        
                            // cout << a0 << ":" << f << ":" << analogVal << ":" << powerVal << ":" << energyVal << endl;
                        }
                    }
                
                    buff.clear();
                }
            }
        }
    }
    
    colorB = ofMap(sin(ofGetElapsedTimef()), 0, 1, 220, 240);
    rRatio = ofMap(energyVal, 0, 10, 0, 2, true) * ofMap(sin(ofGetElapsedTimef()), 0, 1, 0.95, 1.05);
    sizeRatio = ofMap(energyVal, 0, 10, 0.01, 2, true);
}

//--------------------------------------------------------------
void ofApp::draw(){

    ofPushMatrix();
    ofTranslate(ofGetWidth() / 2, ofGetHeight() / 2);
    
    ofRotateY(ofGetElapsedTimef() * 360 / 30);
    ofRotateX(sin(ofGetElapsedTimef()) * 100 / 20);

    for (vector<T_3DROTATE_POS>::iterator it = boxPos.begin(); it != boxPos.end(); it++) {
        float noise = ofNoise(it->rotateX, it->rotateY, it->r, ofGetElapsedTimef());
        
        ofSetColor(ofColor::fromHsb(
            ofWrap(150 - ofMap(analogVal, 0, 1300, 0, 150, true) + ofMap(noise, 0, 1, -40, 40), 0, 255),
            255,
            ofClamp(colorB + ofMap(noise, 0, 1, -30, 30), 0, 255),
            ofClamp(50 + ofMap(noise, 0, 1, -50, 50), 0, 255)
        ));
        
        float r = it->r * rRatio + ofMap(noise, 0, 1, -20, 20);
        
        float size = 5 * sizeRatio + ofMap(noise, 0, 1, -10, 10);
        
        ofPushMatrix();
        ofRotateX(it->rotateX);
        ofRotateY(it->rotateY);
        ofTranslate(r, 0, 0);
        ofDrawSphere(0, 0, 0, size);
        ofPopMatrix();
        
        float z = sin(ofDegToRad(it->rotateY)) * r;
        float x = cos(ofDegToRad(it->rotateY)) * r;
        float y = sin(ofDegToRad(it->rotateX)) * z;
        
        ofLine(0, 0, 0, x, y, z);
    }
    ofPopMatrix();
    
    ofPushMatrix();

    ofTranslate(ofGetWidth() / 2, ofGetHeight() / 2 + 200);
    ofSetColor(200, 180);
    font.drawStringCentered(ofToString(energyVal, 2) + " mWh", 0, 0);
    
    ofPopMatrix();
    
    ofSetColor(255, 200);
    buttonImg.draw(buttonRect);
}

void ofApp::newResponse(ofxHttpResponse & response) {
    string responseStr = ofToString(response.status) + ": " + (string)response.responseBody;
    std::cout << responseStr << std::endl;
}

void ofApp::sendDataForServer(void) {
    
    ofxJSONElement json;
    json["device_id"]   = 0;
    json["user_name"]   = "izumida";
    json["lat"]         = 35.41;
    json["lng"]         = 139.41;
    json["solar"]       = energyVal;
    
    cout << json.getRawString() << endl;
    
    ofBuffer buff;
    buff.set(json.getRawString());
    
    httpUtils.postData(
                       url,
                       buff,
                       "application/json"
                       );
    
    // reset energy
    energyVal = 0;
}

//--------------------------------------------------------------
void ofApp::keyPressed(int key){
    if (key == 32) {        // 32 is key code of SPACE KEY
        sendDataForServer();
        
    } else if (key == 'm') {
        p_gui->toggleVisible();
        
    }
}

//--------------------------------------------------------------
void ofApp::keyReleased(int key){

}

//--------------------------------------------------------------
void ofApp::mouseMoved(int x, int y ){

}

//--------------------------------------------------------------
void ofApp::mouseDragged(int x, int y, int button){

}

//--------------------------------------------------------------
void ofApp::mousePressed(int x, int y, int button){

}

//--------------------------------------------------------------
void ofApp::mouseReleased(int x, int y, int button){
    if (buttonRect.inside(x, y)) {
        sendDataForServer();
    }
}

//--------------------------------------------------------------
void ofApp::windowResized(int w, int h){

}

//--------------------------------------------------------------
void ofApp::gotMessage(ofMessage msg){

}

//--------------------------------------------------------------
void ofApp::dragEvent(ofDragInfo dragInfo){ 

}
