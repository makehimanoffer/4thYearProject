using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RakNet;
using System.Xml.Linq;

namespace BuzzRakNet
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RakPeerInterface peer;
        Packet packet;
        SpriteFont font;
        String quizString="";
        XDocument doc;
        String question = "";
        List<Question> questions;
        int questionNo = 0;
        
        DefaultMessageIDTypes temp;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            peer = RakPeerInterface.GetInstance();
            SocketDescriptor sd= new SocketDescriptor();
            packet=new Packet();

            peer.Startup(1, sd, 1);
            peer.Connect("127.0.0.1", 60000, "", 0);

            
            XDocument doc = XDocument.Load("C:/Users/David/Documents/GitHub/4thYearProjectCode/BuzzRakNetXNA4.0/BuzzRakNet/BuzzRakNetContent/QuizQuestions.xml");
            questions = new List<Question>();
            questions = (from question in doc.Descendants("Item")
                         select new Question()
                         {
                             q = question.Element("Question").Value,
                             A = question.Element("Answer1").Value,
                             B = question.Element("Answer2").Value,
                             C = question.Element("Answer3").Value,
                             D = question.Element("Answer4").Value,
                             CorrectAnswer = question.Element("CorrectAnswer").Value
                         }).ToList();
            quizString = "Question: "+ questions[0].q + "\n";
            quizString += "A: " + questions[0].A + "\n";
            quizString += "B: " + questions[0].B + "\n";
            quizString += "C: " + questions[0].C + "\n";
            quizString += "D: " + questions[0].D + "\n";

            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("QuizFont");
            
           

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            
            KeyboardState newState = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (newState.IsKeyDown(Keys.A))
            {
                if (questions[questionNo].CorrectAnswer.Equals("Answer1"))
                {
                    question = "CORRECT";
                }
            }
            if (newState.IsKeyDown(Keys.B))
            {
                if (questions[questionNo].CorrectAnswer.Equals("Answer2"))
                {
                    question = "CORRECT";
                }
            }
            if (newState.IsKeyDown(Keys.C))
            {
                if (questions[questionNo].CorrectAnswer.Equals("Answer3"))
                {
                    question = "CORRECT";
                }
            }
            if (newState.IsKeyDown(Keys.D))
            {
                if (questions[questionNo].CorrectAnswer.Equals("Answer4"))
                {
                    question = "CORRECT";
                }
            }
                

            


            /*
            // TODO: Add your update logic here
            while (true)
            {
                for (packet = peer.Receive();packet!=null ; peer.DeallocatePacket(packet), packet = peer.Receive())
                {
                    switch (packet.data[0])
                    {
                        case (int)DefaultMessageIDTypes.ID_REMOTE_DISCONNECTION_NOTIFICATION:
                            System.Diagnostics.Debug.WriteLine("Another client has disconnected.\n");
                            break;
                        case (int)DefaultMessageIDTypes.ID_REMOTE_CONNECTION_LOST:
                            System.Diagnostics.Debug.WriteLine("Another client has lost the connection.\n");
                            break;
                        case (int)DefaultMessageIDTypes.ID_REMOTE_NEW_INCOMING_CONNECTION:
                            System.Diagnostics.Debug.WriteLine("Another client has connected.\n");
                            break;
                        case (int)DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED:
                            System.Diagnostics.Debug.WriteLine("Our connection request has been accepted.\n");
                            break;
                        case (int)DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                            System.Diagnostics.Debug.WriteLine("A connection is incoming.\n");
                            break;
                        case (int)DefaultMessageIDTypes.ID_NO_FREE_INCOMING_CONNECTIONS:
                            System.Diagnostics.Debug.WriteLine("The server is full.\n");
                            break;
                        case (int)DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:

                            System.Diagnostics.Debug.WriteLine("We have been disconnected.\n");

                            break;
                        case (int)DefaultMessageIDTypes.ID_CONNECTION_LOST:
                            System.Diagnostics.Debug.WriteLine("Connection lost.\n");

                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("Message with identifier %i has arrived.\n", packet.data[0]);
                            break;
                    }
                }

                
                   
                
            }
             */
            //RakPeerInterface.DestroyInstance(peer);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, quizString, new Vector2(100,200 ), Color.AntiqueWhite);
            spriteBatch.DrawString(font, question, new Vector2(200, 100), Color.AntiqueWhite);
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
