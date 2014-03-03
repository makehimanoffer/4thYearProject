#include "HelloWorldScene.h"
#include "SimpleAudioEngine.h"  


USING_NS_CC;
using namespace RakNet;



enum GameMessages
{
	WELCOME=ID_USER_PACKET_ENUM+1,
	UPDATE=ID_USER_PACKET_ENUM+2,
	MY_TURN=ID_USER_PACKET_ENUM+3,
	GREEN=ID_USER_PACKET_ENUM+4,
	RED=ID_USER_PACKET_ENUM+5,
	BLUE=ID_USER_PACKET_ENUM+6,
	PURPLE=ID_USER_PACKET_ENUM+7,
	BUZZER=ID_USER_PACKET_ENUM + 8

	
};
CCScene* HelloWorld::scene()
{
	// 'scene' is an autorelease object
	CCScene *scene = CCScene::create();

	// 'layer' is an autorelease object
	HelloWorld *layer = HelloWorld::create();

	// add layer as a child to scene
	scene->addChild(layer);

	// return the scene
	return scene;
}

// on "init" you need to initialize your instance
bool HelloWorld::init()
{
	//////////////////////////////
	// 1. super init first
	if ( !CCLayer::init() )
	{
		return false;
	}

	

	CCSize visibleSize = CCDirector::sharedDirector()->getVisibleSize();
	CCPoint origin = CCDirector::sharedDirector()->getVisibleOrigin();

	/////////////////////////////
	// 2. add a menu item with "X" image, which is clicked to quit the program
	//    you may modify it.

	// add a "close" icon to exit the progress. it's an autorelease object
	/*
	CCMenuItemImage *pCloseItem = CCMenuItemImage::create(
		"ButtonNormal.png",
		"ButtonHit.png",
		this,
		menu_selector(HelloWorld::menuCloseCallback));

	pCloseItem->setPosition(ccp(origin.x +320 ,
		origin.y + 240));

	// create menu, it's an autorelease object
	CCMenu* pMenu = CCMenu::create(pCloseItem, NULL);
	pMenu->setPosition(CCPointZero);
	this->addChild(pMenu, 1);
	*/
	
	CCMenuItemImage *BuzzerButtonObject = CCMenuItemImage::create(
		"BuzzerButtonSmaller.png",
		"BuzzerButtonSmallerHit.png",
		this,
		menu_selector(HelloWorld::BuzzerButton));

	BuzzerButtonObject->setPosition(ccp(origin.x +320 ,
		origin.y+ 395));
	
	//this->addChild(BuzzerButtonObject, 1);
	CCMenuItemImage *greenButtonObject = CCMenuItemImage::create(
		"greenButtonSmaller.png",
		"greenButtonSmallerHit.png",
		this,
		menu_selector(HelloWorld::greenButton));

	greenButtonObject->setPosition(ccp(origin.x +320 ,
		origin.y+ 285));
	CCMenuItemImage *redButtonObject = CCMenuItemImage::create(
		"redButtonSmaller.png",
		"redButtonSmallerHit.png",
		this,
		menu_selector(HelloWorld::redButton));

	redButtonObject->setPosition(ccp(origin.x +320 ,
		origin.y+ 205));

	CCMenuItemImage *blueButtonObject = CCMenuItemImage::create(
		"blueButtonSmaller.png",
		"blueButtonSmallerHit.png",
		this,
		menu_selector(HelloWorld::blueButton));

	blueButtonObject->setPosition(ccp(origin.x +320 ,
		origin.y+ 125));
	CCMenuItemImage *purpleButtonObject = CCMenuItemImage::create(
		"purpleButtonSmaller.png",
		"purpleButtonSmallerHit.png",
		this,
		menu_selector(HelloWorld::purpleButton));

	purpleButtonObject->setPosition(ccp(origin.x +320 ,
		origin.y+ 45));
	
	//this->addChild(greenButtonObject, 1);
	CCMenu* pMenu = CCMenu::create(greenButtonObject, NULL);
	pMenu->addChild(BuzzerButtonObject,1,1);
	pMenu->addChild(redButtonObject,1,1);
	pMenu->addChild(blueButtonObject,1,1);
	pMenu->addChild(purpleButtonObject,1,1);
	pMenu->setPosition(CCPointZero);
	this->addChild(pMenu, 1);
	connected=false;
	peer = RakNet::RakPeerInterface::GetInstance();
	RakNet::SocketDescriptor sd;
	peer->Startup(1,&sd,1);
	CCLOG("Starting the client.\n");
	peer->Connect("127.0.0.1", SERVER_PORT, 0,0);
	// add "HelloWorld" splash screen"
	//  CCSprite* pSprite = CCSprite::create("HelloWorld.png");

	// position the sprite on the center of the screen
	// pSprite->setPosition(ccp(visibleSize.width/2 + origin.x, visibleSize.height/2 + origin.y));

	// add the sprite as a child to this layer
	//this->addChild(pSprite, 0);
	//this->schedule( schedule_selector( HelloWorld::update ), 1.0 / 20 );
	green=blue=red=purple=buzzer=false;
	this->schedule( schedule_selector(HelloWorld::update) );
	return true;
}

void HelloWorld::update(float dt){
	
	
		for (packet=peer->Receive();packet;peer->DeallocatePacket(packet), packet=peer->Receive())
		{
			if(red){
				CCLOG("RED PRESSED");
				bsOut.Write((MessageID)RED);
				peer->Send(&bsOut,HIGH_PRIORITY,RELIABLE_ORDERED,0,packet->systemAddress,false);
				red=false;

			}
			if(green){
				bsOut.Write((MessageID)GREEN);
				peer->Send(&bsOut,HIGH_PRIORITY,RELIABLE_ORDERED,0,packet->systemAddress,false);
				green=false;
			}
			if(blue){
				bsOut.Write((MessageID)BLUE);
				peer->Send(&bsOut,HIGH_PRIORITY,RELIABLE_ORDERED,0,packet->systemAddress,false);
				blue=false;
			}
			if(purple){
				bsOut.Write((MessageID)PURPLE);
				peer->Send(&bsOut,HIGH_PRIORITY,RELIABLE_ORDERED,0,packet->systemAddress,false);
				
				purple=false;
			}
			if(buzzer){
				bsOut.Write((MessageID)BUZZER);
				peer->Send(&bsOut,HIGH_PRIORITY,RELIABLE_ORDERED,0,packet->systemAddress,false);
				buzzer=false;
			}

			RakNet::BitStream bsIn(packet->data,packet->length,false);
			bsIn.IgnoreBytes(sizeof(MessageID));
			bsIn.Read(int_message);
			switch (packet->data[0])
			{
			case WELCOME:
				CCLOG("Server said I'm client number 10d", int_message);
				
				bsOut.Write((MessageID)UPDATE);
				peer->Send(&bsOut,HIGH_PRIORITY,RELIABLE_ORDERED,0,packet->systemAddress,false);
				break;
			case ID_CONNECTION_REQUEST_ACCEPTED:
				connected = true;
				
				break;
			case UPDATE:
				// report the server's new counter value
				
				CCLOG("Server said we are now at 10d",int_message);
				break;
			case MY_TURN:
				CCLOG("My Turn. Sending message.\n");
				
				bsOut.Write((MessageID)UPDATE);
				peer->Send(&bsOut,HIGH_PRIORITY,RELIABLE_ORDERED,0,packet->systemAddress,false);
				break;
			default:
				CCLOG("Message with identifier %i has arrived.\n", packet->data[0]);
				break;
			}
			bsOut.Reset();
		}
	

	//RakPeerInterface::DestroyInstance(peer);
	
}



void HelloWorld::menuCloseCallback(CCObject* pSender)
{
#if (CC_TARGET_PLATFORM == CC_PLATFORM_WINRT) || (CC_TARGET_PLATFORM == CC_PLATFORM_WP8)
	CCMessageBox("You pressed the close button. Windows Store Apps do not implement a close button.","Alert");
#else
	//CCLOG("Starting the client.\n");
	peer->Connect("127.0.0.1", SERVER_PORT, 0,0);
	//CCMessageBox("Starting The Client","Alert");
	// CCDirector::sharedDirector()->end();
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
	exit(0);
#endif
#endif
}

void HelloWorld::BuzzerButton(CCObject* pSender)
{
	CocosDenshion::SimpleAudioEngine::sharedEngine()->playEffect(
"buzzer.wav"); 
	buzzer=true;
	
				
}

void HelloWorld::greenButton(CCObject* pSender)
{
	CocosDenshion::SimpleAudioEngine::sharedEngine()->playEffect(
"green.wav"); 
	buzzer=true;
	
}

void HelloWorld::redButton(CCObject* pSender)
{
	CocosDenshion::SimpleAudioEngine::sharedEngine()->playEffect(
"red.wav"); 
	red=true;
	
	
}
void HelloWorld::blueButton(CCObject* pSender)
{
	CocosDenshion::SimpleAudioEngine::sharedEngine()->playEffect(
"blue.wav"); 
	blue=true;
	
}
void HelloWorld::purpleButton(CCObject* pSender)
{
	CocosDenshion::SimpleAudioEngine::sharedEngine()->playEffect(
"purple.wav"); 
	purple=true;
}
