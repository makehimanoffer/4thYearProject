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
using System.Net;


namespace BuzzRakNet
{
    
    class ServerForGame
    {
       public RakPeerInterface peer;
       public Packet packet;
       public DefaultMessageIDTypes temp;
       public BitStream bsOut= new BitStream();
       public RakString rs= new RakString();
       
       private static ServerForGame instance;
       String textString = "";
       public int questionNo = 0;
       bool buzzerHit=false;
       BitStream bsIn;
       String debug = "";
       int currentTime=0;
       
      
       public List<RakNetGUID> clients= new List<RakNetGUID>();
       public List<long> clientBuzzTime = new List<long>();
       public List<bool> buzzed = new List<bool>();
       public List<int> clientScore = new List<int>();
       public List<String> ScoreOutputs = new List<string>();
       public String answerHit = "";
       public List<int> averagePingOfClients = new List<int>();
       public List<string> BuzzedIn = new List<string>();
       public bool buttonPushed = false;
       public bool nextQuestion = true;
       private ServerForGame()
       {
           
       }
       public void Initialize()
       {
           peer = RakPeerInterface.GetInstance();
           
           SocketDescriptor sd = new SocketDescriptor(60000,"172.30.9.222" );
           packet = new Packet();
           peer.Startup(4, sd, 1);
           peer.SetMaximumIncomingConnections(10);
           
           

       }
       
       public void Update()
       {
           
              // textString = "THIS IS A TEXT";
          
               packet = peer.Receive();
               while (packet != null)
               {


                   bsIn = new BitStream(packet.data, packet.length, false);
                   bsIn.Read(packet.data, packet.length);
                   bsIn.IgnoreBytes(1);
                   #region ColorValues
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.RED)
                   {
                       for (int i = 0; i < clients.Count; i++)
                       {
                           if (packet.systemAddress == peer.GetSystemAddressFromGuid(clients[i]) && buzzed[i] == true && clients[i] == packet.guid && i == getClientWithLowestSytemTime(clientBuzzTime))
                           {
                                   if (rightAnswer("Answer2"))
                                   {
                                       questionNo++;
                                       nextQuestion = true;
                                       clientScore[i] = clientScore[i] + Game1.questionScore;
                                       answerHit = "Client " + i + " answered red and was right";
                                       buzzed[i] = false;
                                       BuzzedIn[i] = "";
                                       
                                   }
                                   else
                                   {
                                       clientBuzzTime[i] = long.MaxValue;
                                       buzzed[i] = false;
                                       clientScore[i] = clientScore[i] - 1;
                                       answerHit = "Client " + i + " answered red and was wrong";
                                       BuzzedIn[i] = "";
                                       
                                       if (clients.Count < 2)
                                       {
                                           questionNo++;
                                           nextQuestion = true;
                                           answerHit += "Moving on";
                                       }
                                       else
                                       {
                                           answerHit += "waiting for next fastest buzzer";
                                       }
                                   }
                               }
                               else
                               {
                                   

                               }
                           
                          
                       }
                       bsOut.Reset();
                       bsOut.Write((byte)DefaultMessageIDTypes.UPDATE);
                       peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, '0', packet.systemAddress, false);


                   }
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.GREEN)
                   {
                       for (int i = 0; i < clients.Count; i++)
                       {
                           if (packet.systemAddress == peer.GetSystemAddressFromGuid(clients[i]) && buzzed[i] == true && clients[i] == packet.guid && i == getClientWithLowestSytemTime(clientBuzzTime))
                           {
                                   if (rightAnswer("Answer1"))
                                   {
                                       questionNo++;
                                       clientScore[i] = clientScore[i] + Game1.questionScore;
                                       answerHit = "Client " + i + " answered green and was right";
                                       buzzed[i] = false;
                                       nextQuestion = true;
                                       BuzzedIn[i] = "";
                                       bsOut.Reset();
                                       bsOut.Write((byte)DefaultMessageIDTypes.UPDATE);
                                   }
                                   else
                                   {
                                       clientBuzzTime[i] = long.MaxValue;
                                       answerHit = "Client " + i + " answered green and was wrong\n";
                                       buzzed[i] = false;
                                       BuzzedIn[i] = "";
                                       clientScore[i] = clientScore[i] - 1;
                                       if (clients.Count < 2)
                                       {
                                           questionNo++;
                                           nextQuestion = true;
                                           answerHit += "Moving on";
                                       }
                                       else
                                       {
                                           answerHit += "waiting for next fastest buzzer";
                                       }
                                   }
                               
                               
                           }

                       }
                       bsOut.Reset();
                       bsOut.Write((byte)DefaultMessageIDTypes.UPDATE);
                                       
                       peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, '0', packet.systemAddress, false);


                   }
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.BLUE)
                   {
                       for (int i = 0; i < clients.Count; i++)
                       {
                           if (packet.systemAddress == peer.GetSystemAddressFromGuid(clients[i]) && buzzed[i] == true && clients[i] == packet.guid && i == getClientWithLowestSytemTime(clientBuzzTime))
                           {
                                   if (rightAnswer("Answer3"))
                                   {
                                       nextQuestion = true;
                                       questionNo++;
                                       clientScore[i] = clientScore[i] + Game1.questionScore;
                                       answerHit = "Client " + i + " answered blue and was right";
                                       bsOut.Reset();
                                       BuzzedIn[i] = "";
                                       buzzed[i] = false;
                                      
                                   }
                                   else
                                   {
                                       clientBuzzTime[i] = long.MaxValue;
                                       answerHit = "Client " + i + " answered blue and was wrong\n";
                                       bsOut.Reset();
                                       buzzed[i] = false;
                                       clientScore[i] = clientScore[i] - 1;
                                       BuzzedIn[i] = "";
                                       
                                       if (clients.Count < 2)
                                       {
                                           questionNo++;
                                           nextQuestion = true;
                                           answerHit += "Moving on";
                                       }
                                       else
                                       {
                                           answerHit += "waiting for next fastest buzzer";
                                       }
                                   }
                               
                               
                           }
                       }
                       bsOut.Reset();
                       bsOut.Write((byte)DefaultMessageIDTypes.UPDATE);
                                       
                       peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, '0', packet.systemAddress, false);


                   }

                   if (packet.data[0] == (byte)DefaultMessageIDTypes.PURPLE)
                   {
                       for (int i = 0; i < clients.Count; i++)
                       {
                           if (packet.systemAddress == peer.GetSystemAddressFromGuid(clients[i]) && buzzed[i] == true && clients[i] == packet.guid && i == getClientWithLowestSytemTime(clientBuzzTime))
                           {
                               
                               
                                   if (rightAnswer("Answer4"))
                                   {
                                       questionNo++;
                                       nextQuestion = true;
                                       clientScore[i] = clientScore[i] + Game1.questionScore;
                                       answerHit = "Client " + i + " answered purple and was right";
                                       bsOut.Reset();
                                       buzzed[i] = false;
                                       BuzzedIn[i] = "";
                                       
                                   }
                                   else
                                   {
                                       clientBuzzTime[i] = long.MaxValue;
                                       answerHit = "Client " + i + " answered purple and was wrong\n";
                                       BuzzedIn[i] = "";
                                       bsOut.Reset();
                                       clientScore[i] = clientScore[i] - 1;
                                       buzzed[i] = false;
                                      
                                       if (clients.Count < 2)
                                       {
                                           questionNo++;
                                           nextQuestion = true;
                                           answerHit += "Moving on";
                                       }
                                       else
                                       {
                                           answerHit += "waiting for next fastest buzzer";
                                       }
                                   }
                               
                               
                           }
                       }
                       bsOut.Reset();
                       bsOut.Write((byte)DefaultMessageIDTypes.UPDATE);
                                       
                       peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, '0', packet.systemAddress, false);


                   }
                   #endregion
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.BUZZER)
                   {
                       
                       if (Game1.startGame == false)
                       {
                           Game1.startGame = true;
                           bsOut.Reset();
                           bsOut.Write((byte)DefaultMessageIDTypes.UPDATE);
                           peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, '0', packet.systemAddress, false);

                       }
                       else
                       {
                           //questionNo++;
                           debug = "A BUZZER HAS BEEN HIT by client " + packet.systemAddress + "\n";
                           bsOut.Reset();

                           for (int i = 0; i < clients.Count; i++)
                           {
                               // std::cout << "  To: " << i+1 << " - " << clients[i].g << std::endl;
                               // use the GetSystemAddressFromGUID to convert the stored GUIDs into addresses
                               
                               if (packet.systemAddress == peer.GetSystemAddressFromGuid(clients[i]) && clients[i]==packet.guid)
                               {
                                   
                                   
                                   debug += "Sending back to: " + peer.GetSystemAddressFromGuid(clients[i]) + "\n";
                                   Game1.playBuzzer[0] = true;


                                   if (buzzed[i] == false)
                                   {

                                       clientBuzzTime[i] = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                                       debug += "at time " + clientBuzzTime[i];

                                       buzzed[i] = true;
                                       BuzzedIn[i] = "Buzzer at Client " + i + " buzzed in\n";
                                   }
                                   
                                   bsOut.Reset();
                                   buttonPushed = false;
                                   bsOut.Write((byte)DefaultMessageIDTypes.UPDATE);
                                   peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, '0', peer.GetSystemAddressFromGuid(clients[i]), false);

                               }
                               else
                               {
                                   bsOut.Reset();
                                   buttonPushed = false;
                                   bsOut.Write((byte)DefaultMessageIDTypes.UPDATE);
                                   peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, '0', packet.systemAddress, false);


                               }


                           }
                           
                           textString = "A Buzzer is hit";
                           System.Diagnostics.Debug.WriteLine("Buzzer Hit");
                       }

                   }
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.ID_REMOTE_DISCONNECTION_NOTIFICATION)
                   {
                       textString = "Another client has disconnected.\n";
                       System.Diagnostics.Debug.WriteLine("Another client has disconnected.\n");
                   }
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.ID_REMOTE_CONNECTION_LOST)
                   {
                       textString = "Another client has lost the connection.\n";
                       System.Diagnostics.Debug.WriteLine("Another client has lost the connection.\n");
                   }
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.ID_REMOTE_NEW_INCOMING_CONNECTION)
                   {
                       textString = "Another client has connected.\n";
                       System.Diagnostics.Debug.WriteLine("Another client has connected.\n");


                   }
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED)
                   {
                       System.Diagnostics.Debug.WriteLine("Our connection request has been accepted.\n");
                   }
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION)
                   {

                       textString = "A Connection is incoming";
                       clients.Add(packet.guid);
                       clientBuzzTime.Add(long.MaxValue);
                       buzzed.Add(false);
                       clientScore.Add(0);
                       ScoreOutputs.Add("");
                       BuzzedIn.Add("");
                       Game1.playBuzzer.Add(false);
                       averagePingOfClients.Add(peer.GetAveragePing(clients[clients.Count - 1]));
                       System.Diagnostics.Debug.WriteLine("A connection is incoming.\n");
                       bsOut.Reset();
                       bsOut.Write((byte)DefaultMessageIDTypes.UPDATE);
                       bsOut.Write(clients.Count - 1);
                       //time(&currentTime);
                       //bsOut.Write(currentTime);
                       peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, '0', packet.systemAddress, false);

                   }
                   #region GenericPacketHandling
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.ID_NO_FREE_INCOMING_CONNECTIONS)
                   {
                       textString = "The server is full.\n";
                       System.Diagnostics.Debug.WriteLine("The server is full.\n");
                   }
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION)
                   {
                       textString = "We have been disconnected.\n";
                       System.Diagnostics.Debug.WriteLine("We have been disconnected.\n");

                   }
                   #endregion
                   #region CONNECTIONLOST
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.ID_CONNECTION_LOST)
                   {
                       textString = "Connection lost.\n";
                       System.Diagnostics.Debug.WriteLine("Connection lost.\n");
                       for (int i = 0; i < clients.Count; i++)
                       {
                           if (peer.GetSystemAddressFromGuid(clients[i]) == packet.systemAddress)
                           {
                               textString += "Client " + i + "has been removed from game";
                               clients.RemoveAt(i);
                               clientBuzzTime.RemoveAt(i);
                               clientScore.RemoveAt(i);
                               ScoreOutputs.RemoveAt(i);
                               buzzed.RemoveAt(i);
                               BuzzedIn.RemoveAt(i);

                           }
                       }

                   }
                   #endregion
                   #region UPDATE
                   if (packet.data[0] == (byte)DefaultMessageIDTypes.UPDATE)
                   {
                       //printf("Updating");
                       bsOut.Reset();
                       bsOut.Write((byte)DefaultMessageIDTypes.UPDATE);
                       for (int i = 0; i < clients.Count; i++)
                       {
                           // std::cout << "  To: " << i+1 << " - " << clients[i].g << std::endl;
                           ScoreOutputs[i] = clientScore[i].ToString();
                       }
                           // use the GetSystemAddressFromGUID to convert the stored GUIDs into addresses
                           peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, '0', packet.systemAddress, false);

                       


                   }
                   #endregion

                   peer.DeallocatePacket(packet);
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

       public void Draw(SpriteBatch spriteBatch,SpriteFont font,SpriteFont scoreFont)
       {
          
           //spriteBatch.DrawString(font, debug, new Vector2(300, 70), Color.AntiqueWhite);
           spriteBatch.DrawString(font, answerHit, new Vector2(200, 500), Color.AntiqueWhite);
          
           for (int i = 0; i < clients.Count; i++)
           {
               spriteBatch.DrawString(scoreFont, ScoreOutputs[i], new Vector2(1200,250+( i * 100)), Color.AntiqueWhite);
               //spriteBatch.DrawString(font, BuzzedIn[i], new Vector2(520, 500+ (i * 50)), Color.AntiqueWhite);
               spriteBatch.Draw(Game1.playerImages[i], new Rectangle(1000, 250 + (i * 100), 200, 100), Color.AntiqueWhite);
               
               
               if (buzzed.Count <= 2)
               {
                   if (buzzed[i] == false)
                   {
                       spriteBatch.Draw(Game1.buzzedOrNotImages[i], new Rectangle(520 + (i * 100), 550, 100, 100), Color.AntiqueWhite);
                   } if (buzzed[i] == true)
                   {
                       spriteBatch.Draw(Game1.buzzedOrNotImages[i + 2], new Rectangle(520 + (i * 100), 550, 100, 100), Color.AntiqueWhite);

                   }

               }
           }
          
       }
       public int returnQuestionNumber()
       {
          
               return questionNo;
           
          
       }
       public int getClientWithLowestSytemTime(List<long> list)
       {
           long temp = long.MaxValue;
           int clientVal=100;
           for (int i = 0; i < list.Count; i++)
           {
               if (temp >= list[i])
               {
                   temp = list[i];
                   clientVal = i;
               }
           }
           return clientVal;
       }
       public bool rightAnswer(String answerVal)
       {
           if (answerVal.Equals(Game1.currentAnswer))
           {
               return true;
           }
           else
           {
               return false;
           }
       }
    }
}
