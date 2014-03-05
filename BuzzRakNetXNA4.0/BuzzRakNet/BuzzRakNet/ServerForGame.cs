using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RakNet;
using System.Threading;


namespace BuzzRakNet
{
    
    class ServerForGame
    {
       public RakPeerInterface peer;
       public Packet packet;
       public DefaultMessageIDTypes temp;
       public BitStream bsOut= new BitStream();
       public RakString rs= new RakString();
       public List<RakNetGUID> clients;
       private static ServerForGame instance;
       String textString = "";
       public int questionNo = 0;
       bool buzzerHit=false;
       BitStream bsIn;
       private ServerForGame()
       {
           
       }
       public void Initialize()
       {
           peer = RakPeerInterface.GetInstance();
           SocketDescriptor sd = new SocketDescriptor(60000, "127.0.0.1");
           packet = new Packet();
           peer.Startup(4, sd, 1);
           peer.SetMaximumIncomingConnections(4);
          
           Thread t= new Thread(new ThreadStart(Update));
           t.Start();

       }

       public void Update()
       {
           
              // textString = "THIS IS A TEXT";
               packet = peer.Receive();
               while(packet != null )
               {


                   bsIn=new BitStream(packet.data,packet.length,false);
                   bsIn.Read(packet.data, packet.length);
	               bsIn.IgnoreBytes(1);
                   if(packet.data[0]==(byte)DefaultMessageIDTypes.BUZZER){
                           questionNo++;
                           buzzerHit=true;
                           bsOut.Write((byte)DefaultMessageIDTypes.BUZZER);
                           textString = "A Buzzer is hit";
                           System.Diagnostics.Debug.WriteLine("Buzzer Hit");
                            
                   }
                   if(packet.data[0]==(byte)DefaultMessageIDTypes.ID_REMOTE_DISCONNECTION_NOTIFICATION){
                           textString = "Another client has disconnected.\n";
                           System.Diagnostics.Debug.WriteLine("Another client has disconnected.\n");
                   }
                 if(packet.data[0]==(byte)DefaultMessageIDTypes.ID_REMOTE_CONNECTION_LOST){
                           textString = "Another client has lost the connection.\n";
                           System.Diagnostics.Debug.WriteLine("Another client has lost the connection.\n");
                 }
                 if(packet.data[0]==(byte)DefaultMessageIDTypes.ID_REMOTE_NEW_INCOMING_CONNECTION){
                           textString = "Another client has connected.\n";
                           System.Diagnostics.Debug.WriteLine("Another client has connected.\n");
                 }
                if(packet.data[0]==(byte)DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED){
                           System.Diagnostics.Debug.WriteLine("Our connection request has been accepted.\n");
                }
               if(packet.data[0]==(byte)DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION){
                           textString = "A Connection is incoming";
                           System.Diagnostics.Debug.WriteLine("A connection is incoming.\n");
               }
                if(packet.data[0]==(byte)DefaultMessageIDTypes.ID_NO_FREE_INCOMING_CONNECTIONS){
                           textString = "The server is full.\n";
                           System.Diagnostics.Debug.WriteLine("The server is full.\n");
                }
                if(packet.data[0]==(byte)DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION){
                           textString = "We have been disconnected.\n";
                           System.Diagnostics.Debug.WriteLine("We have been disconnected.\n");

                }
                if(packet.data[0]==(byte)DefaultMessageIDTypes.ID_CONNECTION_LOST){
                           textString = "Connection lost.\n";
                           System.Diagnostics.Debug.WriteLine("Connection lost.\n");

                }
                //peer.DeallocatePacket(packet);
                packet = peer.Receive(); 
               }
               
                
                   
                
            
             
           // RakPeerInterface.DestroyInstance(peer);


       }

       public static ServerForGame getInstance()
       {
           if (instance == null)
           {
               instance = new ServerForGame();
           }
           return instance;
       }

       public void Draw(SpriteBatch spriteBatch,SpriteFont font)
       {
           spriteBatch.DrawString(font, textString, new Vector2(20, 20), Color.AntiqueWhite);
           if (buzzerHit)
           {
               spriteBatch.DrawString(font, "Buzzer hit", new Vector2(40, 20), Color.AntiqueWhite);
           }
       }
       public int returnQuestionNumber()
       {
           if (buzzerHit == true)
           {
               return 2;
           }
           else
           {
               return 0;
           }
       }
    }
}
