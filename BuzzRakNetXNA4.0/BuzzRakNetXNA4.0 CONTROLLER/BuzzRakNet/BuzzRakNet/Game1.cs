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
using System.Threading;

namespace BuzzRakNet
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool pressed = false;
        SpriteFont font;
        SpriteFont scoreFont;
        Texture2D MenuImage;
        Texture2D BackgroundImage;
        String quizString="";
        XDocument doc;
        String question = "";
        List<Question> questions;
        int questionNo = 0;
        ServerForGame server;
        String BuzzerAnnouncements = "";
        public static int questionScore = 3;
        public static string currentAnswer = "";
        public static List<SoundEffect> buzzerSounds= new List<SoundEffect>();
        public static bool startGame;
        public Song MenuSong;
        public Song GameSong;
        public bool startSong;
        public static List<bool> playBuzzer= new List<bool>();
        public static List<Texture2D> playerImages= new List<Texture2D>();
        public static List<Texture2D> buzzedOrNotImages = new List<Texture2D>();
        public string Question = "";
        public string AnswerA = "";
        public string AnswerB = "";
        public string AnswerC = "";
        public string AnswerD = "";
        public List<SoundEffect> QuestionSounds = new List<SoundEffect>();
        public List<SoundEffectInstance> QuestionSoundInstances = new List<SoundEffectInstance>();
        public SoundEffect player1Wins;
        public SoundEffect player2Wins;
        public SoundEffect tiedGame;

        public static Texture2D borderBackgroundBuzzers;
        Color BuzzerGreen;
        Color BuzzerPurple = new Color(178, 0, 255);
        Color BuzzerBlue = new Color(0, 38, 255);
        public bool player1Win = false;
        public bool player2Win = false;
        
      // Start the thread
      
        
       
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Window.AllowUserResizing=true;
            graphics.ToggleFullScreen();
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
            startGame = false;
            server=ServerForGame.getInstance();
            server.Initialize();
            BuzzerGreen= new Color(25,214,25);
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
            Question = questions[0].q;
            AnswerA= questions[0].A;
            AnswerB = questions[0].B;
            AnswerC= questions[0].C;
            AnswerD=  questions[0].D;
            

           
            
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
            scoreFont = Content.Load<SpriteFont>("Score");
            buzzerSounds.Add(Content.Load<SoundEffect>("client0"));
            buzzerSounds.Add(Content.Load<SoundEffect>("client1"));
            buzzerSounds.Add(Content.Load<SoundEffect>("client2"));
            buzzerSounds.Add(Content.Load<SoundEffect>("client4"));
            MenuImage = Content.Load<Texture2D>("MenuTITLE");
            BackgroundImage = Content.Load<Texture2D>("BG");
            MenuSong = Content.Load<Song>("FunInABottle");
            GameSong = Content.Load<Song>("MSM");
            MediaPlayer.IsRepeating = true; 
            playerImages.Add(Content.Load<Texture2D>("Player1"));
            playerImages.Add(Content.Load<Texture2D>("Player2"));
            playerImages.Add(Content.Load<Texture2D>("Player3"));
            playerImages.Add(Content.Load<Texture2D>("Player4"));

            buzzedOrNotImages.Add(Content.Load<Texture2D>("Player1Unbuzzed"));
            buzzedOrNotImages.Add(Content.Load<Texture2D>("Player2Unbuzzed"));
            buzzedOrNotImages.Add(Content.Load<Texture2D>("Player1Buzzed"));
            buzzedOrNotImages.Add(Content.Load<Texture2D>("Player2Buzzed"));

            borderBackgroundBuzzers = Content.Load<Texture2D>("borderBackground");
            QuestionSounds.Add(Content.Load<SoundEffect>("Question1"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question2"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question3"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question4"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question5"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question6"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question7"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question8"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question9"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question10"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question11"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question12"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question13"));
            QuestionSounds.Add(Content.Load<SoundEffect>("Question14"));

            QuestionSoundInstances.Add(QuestionSounds[0].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[1].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[2].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[3].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[4].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[5].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[6].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[7].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[8].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[9].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[10].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[11].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[12].CreateInstance());
            QuestionSoundInstances.Add(QuestionSounds[13].CreateInstance());
            player1Wins = Content.Load<SoundEffect>("Player1Wins");
            player2Wins = Content.Load<SoundEffect>("Player2Wins");
            tiedGame = Content.Load<SoundEffect>("TiedGame");
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
            if (newState.IsKeyDown(Keys.Escape))
            {

                this.Exit();
            }
            
            for (int i = 0; i < playBuzzer.Count; i++)
            {
                if (playBuzzer[i] == true)
                {
                    buzzerSounds[i].CreateInstance().Play();
                    playBuzzer[i] = false;
                }

            }
            server.Update();
            handleController();
            if (startGame == true)
            {
                
                
                if (startSong)
                {
                    MediaPlayer.Play(GameSong);
                    startSong = false;
                }

                if (server.questionNo < questions.Count)
                {
                    Question = questions[questionNo].q;
                    AnswerA = "<A> " + questions[questionNo].A;
                    AnswerB = "<B> " + questions[questionNo].B;
                    AnswerC = "<X> " + questions[questionNo].C;
                    AnswerD = "<Y> " + questions[questionNo].D;
                    questionNo = server.questionNo;
                    if (server.nextQuestion == true)
                    {
                        if (questionNo >= 1)
                        {
                            QuestionSoundInstances[questionNo - 1].Stop();
                            QuestionSoundInstances[questionNo].Play();
                        }
                        else
                        {
                            QuestionSoundInstances[questionNo].Play();
                        }

                        server.nextQuestion = false;
                    }
                }
                else
                {
                    startGame = false;
                    if (server.buzzed.Count >= 2)
                    {
                        if (server.clientScore[0] > server.clientScore[1])
                        {
                            //PLAY PLAYER 1 WINS;
                            player1Wins.CreateInstance().Play();
                        }
                        if (server.clientScore[0] < server.clientScore[1])
                        {
                            //PLAY PLAYER 2 WINS;
                            player2Wins.CreateInstance().Play();
                        }
                        if (server.clientScore[0] == server.clientScore[1])
                        {
                            //PLAY It's a Draw;
                            tiedGame.CreateInstance().Play();
                        }
                    }
                    else
                    {
                        player1Wins.CreateInstance().Play();
                    }

                    QuestionSoundInstances[13].Stop();
                }
                
                // Allows the game to exit
                currentAnswer = questions[questionNo].CorrectAnswer;
               
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
                

            }
            else
            {
                if (!startSong)
                {
                    MediaPlayer.Play(MenuSong);
                    MediaPlayer.Volume = 0.2f;
                    startSong = true;
                }
            }


            
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
            if (startGame == true)
            {
               
                spriteBatch.Draw(BackgroundImage, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.AntiqueWhite);
                
                spriteBatch.DrawString(font, Question, new Vector2(50, 100), Color.AntiqueWhite);
                spriteBatch.DrawString(font, AnswerA, new Vector2(50, 150),BuzzerGreen);
                spriteBatch.DrawString(font, AnswerB, new Vector2(50, 200),Color.Red);
                spriteBatch.DrawString(font, AnswerC, new Vector2(50, 250),BuzzerBlue);
                spriteBatch.DrawString(font, AnswerD, new Vector2(50, 300), BuzzerPurple);
                
                spriteBatch.DrawString(font, BuzzerAnnouncements, new Vector2(100, 550), Color.AntiqueWhite);
                spriteBatch.DrawString(font, question, new Vector2(200, 100), Color.AntiqueWhite);

                server.Draw(spriteBatch, font,scoreFont);
            }
            else
            {
                spriteBatch.Draw(MenuImage, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.AntiqueWhite);
            }
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void handleController()
        {
            
            int i;
            if (server.ScoreOutputs.Count == 1)
            {
               i = 0;
            }
            else
            {
                i = 1;
            }
            if (server.ScoreOutputs.Count >= 1 && server.ScoreOutputs[i]!=null)
            {
                server.ScoreOutputs[i] = server.clientScore[i].ToString();
            }
           GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
           if (gamePadState.IsConnected)
           {
               #region back
               if (gamePadState.Buttons.Back == ButtonState.Pressed && pressed==false)
               {
                   startGame = true;
                   pressed = true; ;
                   // do something
                   

                   server.clientBuzzTime.Add(long.MaxValue);
                   server.buzzed.Add(false);
                   server.clientScore.Add(0);
                   server.ScoreOutputs.Add("");
                   server.BuzzedIn.Add("");
                   playBuzzer.Add(false);

               }
               #endregion
               #region start
               if (gamePadState.Buttons.Start == ButtonState.Pressed )
               {
                   playBuzzer[0] = true;
                   //playBuzzer[0] = false;
                   if (server.buzzed[i] == false)
                   {

                       server.buzzed[i] = true;
                       server.BuzzedIn[i] = "Controller 1 buzzed in";
                       
                   }
               }
               
               #endregion
               #region B
               if (gamePadState.Buttons.B == ButtonState.Pressed && server.buzzed[i]==true)
               {
                   if (server.buzzed[i] == true && server.rightAnswer("Answer2"))
                   {
                       server.buzzed[i] = false;
                       server.questionNo++;
                       server.nextQuestion = true;
                       server.clientScore[i] = server.clientScore[i] + Game1.questionScore;
                       server.answerHit = "Controller " + i + " answered red and was right";
                       
                   }
                   else
                   {
                       server.clientBuzzTime[i] = long.MaxValue;
                       server.buzzed[i] = false;
                       server.clientScore[i] = server.clientScore[i] - 1;
                       server.answerHit = "Controller " + 1 + " answered red and was wrong";
                       server.BuzzedIn[i] = "";

                       if (server.buzzed.Count < 2)
                       {
                           server.questionNo++;
                           server.nextQuestion = true;
                           server.answerHit += "Moving on";
                       }
                       else
                       {
                           server.answerHit += "waiting for next fastest buzzer";
                       }
                   }
               }
               #endregion
               #region A
               if (gamePadState.Buttons.A == ButtonState.Pressed && server.buzzed[i] == true)
               {
                   if (server.buzzed[i] == true && server.rightAnswer("Answer1"))
                   {
                       server.buzzed[i] = false;
                       server.questionNo++;
                       server.nextQuestion = true;
                       server.clientScore[i] = server.clientScore[i] + Game1.questionScore;
                       server.answerHit = "Controller " + 1 + " answered red and was right";
                       
                   }
                   else
                   {
                       server.buzzed[i] = false;
                       server.clientBuzzTime[i] = long.MaxValue;
                       
                       server.clientScore[i] = server.clientScore[i] - 1;
                       server.answerHit = "Controller " + 1 + " answered red and was wrong";
                       server.BuzzedIn[i] = "";

                       if (server.buzzed.Count < 2)
                       {
                           server.questionNo++;
                           server.nextQuestion = true;
                           server.answerHit += "Moving on";
                       }
                       else
                       {
                           server.answerHit += "waiting for next fastest buzzer";
                       }
                   }
               }
               #endregion
               #region X
               if (gamePadState.Buttons.X == ButtonState.Pressed && server.buzzed[i] == true)
               {
                   if (server.buzzed[i] == true && server.rightAnswer("Answer3"))
                   {
                       server.questionNo++;
                       server.nextQuestion = true;
                       server.clientScore[i] = server.clientScore[i] + Game1.questionScore;
                       server.answerHit = "Controller " + 1 + " answered red and was right";
                       server.buzzed[i] = false;
                   }
                   else
                   {
                       server.clientBuzzTime[i] = long.MaxValue;
                       server.buzzed[i] = false;
                       server.clientScore[i] = server.clientScore[i] - 1;
                       server.answerHit = "Controller " + 1 + " answered red and was wrong";
                       server.BuzzedIn[i] = "";

                       if (server.buzzed.Count < 2)
                       {
                           server.questionNo++;
                           server.nextQuestion = true;
                           server.answerHit += "Moving on";
                       }
                       else
                       {
                           server.answerHit += "waiting for next fastest buzzer";
                       }
                   }
               }
               #endregion
               #region Y
               if (gamePadState.Buttons.Y == ButtonState.Pressed && server.buzzed[i] == true)
               {
                   if (server.buzzed[i] == true && server.rightAnswer("Answer4"))
                   {
                       server.questionNo++;
                       server.nextQuestion = true;
                       server.clientScore[i] = server.clientScore[i] + Game1.questionScore;
                       server.answerHit = "Controller " + 1 + " answered red and was right";
                       server.buzzed[i] = false;
                   }
                   else
                   {
                       server.clientBuzzTime[i] = long.MaxValue;
                       server.buzzed[i] = false;
                       server.clientScore[i] = server.clientScore[i] - 1;
                       server.answerHit = "Controller " + 1 + " answered red and was wrong";
                       server.BuzzedIn[i] = "";

                       if (server.buzzed.Count < 2)
                       {
                           server.questionNo++;
                           server.nextQuestion = true;
                           server.answerHit += "Moving on";
                       }
                       else
                       {
                           server.answerHit += "waiting for next fastest buzzer";
                       }
                   }
               }
               #endregion
           }
        }
    }
    
}
