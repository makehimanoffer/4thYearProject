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
       public BitStream bsOut;
       public RakString rs;
       public List<RakNetGUID> clients;
       private static ServerForGame instance;
       String textString = "";
       private ServerForGame()
       {
           Initialize();
       }
       public void Initialize()
       {
           peer = RakPeerInterface.GetInstance();
           SocketDescriptor sd = new SocketDescriptor();
           packet = new Packet();
           peer.Startup(4, sd, 1);
           peer.SetMaximumIncomingConnections(4);
          
           Thread t= new Thread(new ThreadStart(Update));
           t.Start();

       }

       public void Update()
       {
           while (true)
           {
              // textString = "THIS IS A TEXT";
               for (packet = peer.Receive(); packet != null; peer.DeallocatePacket(packet), packet = peer.Receive())
               {
                   switch ((DefaultMessageIDTypes)packet.data[0])
                   {
                       case DefaultMessageIDTypes.ID_REMOTE_DISCONNECTION_NOTIFICATION:
                           textString = "Another client has disconnected.\n";
                           System.Diagnostics.Debug.WriteLine("Another client has disconnected.\n");
                           break;
                       case DefaultMessageIDTypes.ID_REMOTE_CONNECTION_LOST:
                           textString = "Another client has lost the connection.\n";
                           System.Diagnostics.Debug.WriteLine("Another client has lost the connection.\n");
                           break;
                       case DefaultMessageIDTypes.ID_REMOTE_NEW_INCOMING_CONNECTION:
                           textString = "A Connection is incoming";
                           System.Diagnostics.Debug.WriteLine("Another client has connected.\n");
                           break;
                       case DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED:
                           System.Diagnostics.Debug.WriteLine("Our connection request has been accepted.\n");
                           break;
                       case DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                           textString = "A Connection is incoming";
                           System.Diagnostics.Debug.WriteLine("A connection is incoming.\n");
                           break;
                       case DefaultMessageIDTypes.ID_NO_FREE_INCOMING_CONNECTIONS:
                           textString = "A Connection is incoming";
                           System.Diagnostics.Debug.WriteLine("The server is full.\n");
                           break;
                       case DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                           textString = "A Connection is incoming";
                           System.Diagnostics.Debug.WriteLine("We have been disconnected.\n");

                           break;
                       case DefaultMessageIDTypes.ID_CONNECTION_LOST:
                           textString = "A Connection is incoming";
                           System.Diagnostics.Debug.WriteLine("Connection lost.\n");

                           break;
                       default:
                           System.Diagnostics.Debug.WriteLine("Message with identifier %i has arrived.\n", packet.data[0]);
                           break;
                   }
               }
           }
                
                   
                
            
             
            RakPeerInterface.DestroyInstance(peer);


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
       }
    }
}
