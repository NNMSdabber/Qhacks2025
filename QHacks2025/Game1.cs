using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace QHacks2025
{
    public class Game1 : Game
    {
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;

        public const Keys W_KEY = Keys.W;
        public const Keys A_KEY = Keys.A;
        public const Keys S_KEY = Keys.S;
        public const Keys D_KEY = Keys.D;
        public const Keys SELECT_KEY = Keys.K;

        public const int MAX_ARROWS = 4;
        public const int RIGHT = 0;
        public const int LEFT = 1;
        public const int UP = 2;
        public const int DOWN = 3;

        public const int MENU = 0;
        public const int GAME_PLAY = 1;
        public const int END = 2;
        public const int SELECT = 3;
        public const int RESULTS = 4;
        private const int DIALOG = 5;

        private const int SONIC_LEVEL_DATA_IDX = 0;
        private const int ICIRRUS_LEVEL_DATA_IDX = 1;
        private const int CHUG_JUG_LEVEL_DATA_IDX = 2;
        
        public const int NUM_COLL = 5;
        
        private const byte ANIM_COLS_MOVE = 3;
        private const byte ANIM_ROWS_MOVE = 3;
        private const byte TOTAL_FRAMES_MOVE = 9;
        private const byte START_FRAME = 0;
        private const byte IDLE_FRAME = 0;
        private const int ANIM_DURATION_MOVE = 900;
        private Vector2[] amyPosList = new Vector2[5];
        private Vector2 amyPos;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public int gameplayState = MENU;
        private int currLevel = SONIC_LEVEL_DATA_IDX;
        public static Random rng = new Random();

        private float yCord = 10;
        
        private Button startBtn;

        private int curMenuNum = SONIC_LEVEL_DATA_IDX;

        private Button sonicBtn;
        private Button icirrusBtn;
        private Button chugBtn;
        private Button backToStartBtn;

        private Texture2D amyFaceImg;
        private Texture2D sonicFaceImg;

        public static Texture2D bg;
        private Rectangle bgRec = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
        private Color bgColor = new Color(255, 255, 255, 255);

        public Texture2D gridBg;
        private Rectangle gridBgRec = new Rectangle(((SCREEN_WIDTH - 500) / 2 - 250) + 500, SCREEN_HEIGHT /2 - 250, 500, 500);

        public static Texture2D[] arrowImg = new Texture2D[MAX_ARROWS];
        private LinkedList<Arrow> arrows = new LinkedList<Arrow>();

        public static Texture2D barImg;
        
        private Arrow[][] levelData = new Arrow[][] { new Arrow[28], new Arrow[15], new Arrow[5]};

        public static SpriteFont labelFont;
        
        public static SpriteFont titleFont;

        private Rectangle collisionRec = new Rectangle(50,SCREEN_HEIGHT-100,400,30);

        public KeyboardState kb = new KeyboardState();
        public KeyboardState prevkb = new KeyboardState();

        private int score = 0;
        private int streak = 0;
        private int highStreak = 0;

        public MouseState mouse;
        public MouseState mousePrev;

        public static Texture2D amySpriteSheet;
        private Animation amyAnim;
        
        public static Texture2D buttonImg; 
        
        public static Texture2D collImg;

        private Rectangle[] collRects =
        {
            new Rectangle(50, 0, 1, SCREEN_HEIGHT), 
            new Rectangle(150, 0, 1, SCREEN_HEIGHT),
            new Rectangle(250, 0, 1, SCREEN_HEIGHT),
            new Rectangle(350, 0, 1, SCREEN_HEIGHT),
            new Rectangle(450, 0, 1, SCREEN_HEIGHT)
        };
        
        private Rectangle[] pathRects =
        {
            new Rectangle(52, 0, 98, SCREEN_HEIGHT), 
            new Rectangle(152, 0, 98, SCREEN_HEIGHT),
            new Rectangle(252, 0, 98, SCREEN_HEIGHT),
            new Rectangle(352, 0, 98, SCREEN_HEIGHT),
        };

        private static Song menuMusic;
        private static Song sonic;
        private static Song pokemon;
        private static Song chugJugWithYou;

        // Dialog
        private int dgNum = 0;
        private string dialogStr = "";
        private Texture2D dialogImg;
        private Rectangle dialogRect = new Rectangle(50,50,200,200);
        Vector2 dialogPos = new Vector2(300,50);
        private SoundEffect[] dialogFx = new SoundEffect[25];
        private SoundEffectInstance[] dialogFxInstance = new SoundEffectInstance[25];

        private SoundEffect[] feedback = new SoundEffect[2]; 
        
        private Texture2D[] backgrounds = new Texture2D[3];

        private Timer timer = new Timer(Timer.INFINITE_TIMER,true);


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
        /// 

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();

            IsMouseVisible = true;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bg = Content.Load<Texture2D>("Images/bg");
            
            gridBg = Content.Load<Texture2D>("Images/gridBG");

            arrowImg[RIGHT] = Content.Load<Texture2D>("Images/Arrows/RightArrow");
            arrowImg[LEFT] = Content.Load<Texture2D>("Images/Arrows/LeftArrow");
            arrowImg[UP] = Content.Load<Texture2D>("Images/Arrows/ArrowUp");
            arrowImg[DOWN] = Content.Load<Texture2D>("Images/Arrows/ArrowDown");
            
            amyFaceImg = Content.Load<Texture2D>("Images/Characters/AmyFace");
            sonicFaceImg = Content.Load<Texture2D>("Images/Characters/SonicFace");

            sonic = Content.Load<Song>("Audio/Music/sonic");
            pokemon = Content.Load<Song>("Audio/Music/icirrus");
            chugJugWithYou = Content.Load<Song>("Audio/Music/chug_jug");

            barImg = Content.Load<Texture2D>("Images/bar");
            
            amySpriteSheet = Content.Load<Texture2D>("Images/Characters/amy");
            
            buttonImg = Content.Load<Texture2D>("Images/button");
            
            labelFont = Content.Load<SpriteFont>("Fonts/LabelFont");
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");

            menuMusic = Content.Load<Song>("Audio/Music/geometry");
            MediaPlayer.Play(menuMusic);

            timer.Activate();
            
            startBtn = new Button(buttonImg, 600, 350, "Start");
            
            sonicBtn = new Button(buttonImg, 100, 350, "Windy Hill");
            icirrusBtn = new Button(buttonImg, 500, 350, "Icirrus City");
            chugBtn = new Button(buttonImg, 900, 350, "Chug Jug");
            backToStartBtn = new Button(buttonImg,500,400,"Press green to return to menu");

            timer = new Timer(Timer.INFINITE_TIMER,true);
            
            amyPosList[0] = new Vector2(gridBgRec.X + 175 ,  gridBgRec.Y + 175);
            amyPosList[1] = new Vector2(amyPosList[0].X - 200, amyPosList[0].Y);
            amyPosList[2] = new Vector2(amyPosList[0].X, amyPosList[0].Y - 200);
            amyPosList[3] = new Vector2(amyPosList[0].X + 200, amyPosList[0].Y);
            amyPosList[4] = new Vector2(amyPosList[0].X, amyPosList[0].Y + 200);

            backgrounds[SONIC_LEVEL_DATA_IDX] = Content.Load<Texture2D>("Images/sonic");
            backgrounds[ICIRRUS_LEVEL_DATA_IDX] = Content.Load<Texture2D>("Images/blackandwhite");
            backgrounds[CHUG_JUG_LEVEL_DATA_IDX] = Content.Load<Texture2D>("Images/chugjug");
            
            for (int i = 0; i < 30; i++)
            {
                arrows.AddLast(new Arrow(new Vector2(100,0-200*i),8,rng.Next(0,4)));
            }
            
            amyAnim = new Animation(amySpriteSheet, ANIM_COLS_MOVE, ANIM_ROWS_MOVE,
                TOTAL_FRAMES_MOVE, START_FRAME, IDLE_FRAME, Animation.ANIMATE_FOREVER, ANIM_DURATION_MOVE, amyPos, true);
            amyAnim.Activate(true);
            
            collImg = new Texture2D(graphics.GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.White;
            }
            collImg.SetData(data);

            for (int i = 0; i < dialogFx.Length; i++)
            {
                dialogFx[i] = Content.Load<SoundEffect>("Audio/Dialog/"+i);
            }
            
            feedback[0] = Content.Load<SoundEffect>("Audio/SFX/Good");
            feedback[1] = Content.Load<SoundEffect>("Audio/SFX/Perfect");
             
            SetUpSonic();
            SetUpPokemon();
            SetUpChugJug();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (collImg != null)
            {
                collImg.Dispose();
                collImg = null;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            prevkb = kb;
            kb = Keyboard.GetState();

            mousePrev = mouse;
            mouse = Mouse.GetState();

            timer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            
            yCord = (float)(25 * Math.Sin(0.0025*timer.GetTimePassed()));
            
            switch (gameplayState) 
            {
                case MENU:
                    
                    if (kb.IsKeyDown(SELECT_KEY) && !prevkb.IsKeyDown(SELECT_KEY))
                    {
                        MediaPlayer.Stop();
                        dialogFxInstance[0] = dialogFx[0].CreateInstance();
                        dialogFxInstance[0].Play();
                        dialogImg = amyFaceImg;
                        gameplayState = DIALOG;
                    }
                    bgColor.R += 1;
                    bgColor.G += 2;
                    bgColor.B += 3;
                    break;

                case SELECT:

                    if (kb.IsKeyDown(D_KEY) && !prevkb.IsKeyDown(D_KEY))
                    {
                        curMenuNum += 1;
                        if (curMenuNum > 2)
                        {
                            curMenuNum = SONIC_LEVEL_DATA_IDX;
                        }
                    }
                    else if (kb.IsKeyDown(A_KEY) && !prevkb.IsKeyDown(A_KEY))
                    {
                        curMenuNum -= 1;
                        if (curMenuNum < 0)
                        {
                            curMenuNum = CHUG_JUG_LEVEL_DATA_IDX;
                        }
                    }

                    if (kb.IsKeyDown(SELECT_KEY) && !prevkb.IsKeyDown(SELECT_KEY))
                    {
                        gameplayState = GAME_PLAY;
                        currLevel = curMenuNum;
                        MediaPlayer.Stop();
                        switch (currLevel)
                        {
                            case SONIC_LEVEL_DATA_IDX:
                                MediaPlayer.Play(sonic);
                                SetUpSonic();
                                break;
                            case ICIRRUS_LEVEL_DATA_IDX:
                                MediaPlayer.Play(pokemon);
                                SetUpPokemon();
                                break;
                            case CHUG_JUG_LEVEL_DATA_IDX:
                                MediaPlayer.Play(chugJugWithYou);
                                SetUpChugJug();
                                break;
                        }
                    }
                    
                    if (sonicBtn.rec.Contains(mouse.Position.ToVector2()))
                    {
                        if (sonicBtn.CheckIfClicked(mouse, mousePrev))
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(sonic);
                            gameplayState = GAME_PLAY;
                            currLevel = SONIC_LEVEL_DATA_IDX;
                        }
                    }
                    else if (icirrusBtn.rec.Contains(mouse.Position.ToVector2()))
                    {
                        if (icirrusBtn.CheckIfClicked(mouse, mousePrev))
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(pokemon);
                            gameplayState = GAME_PLAY;
                            currLevel = ICIRRUS_LEVEL_DATA_IDX;
                        }
                    }
                    else if (chugBtn.rec.Contains(mouse.Position.ToVector2()))
                    {
                        if (chugBtn.CheckIfClicked(mouse, mousePrev))
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(chugJugWithYou);
                            gameplayState = GAME_PLAY;
                            currLevel = CHUG_JUG_LEVEL_DATA_IDX;
                        }
                    }
                    
                    break;

                case GAME_PLAY:
                    if (streak > highStreak)
                        highStreak = streak;
                    foreach (Arrow arrow in levelData[currLevel])
                    {
                        arrow.Update(gameTime);

                        if (arrow.GetArrowRect().Y > SCREEN_HEIGHT && arrow.GetIsAvailable() == true)
                        {
                            streak = 0;
                        }
                        
                       if(kb.IsKeyDown(W_KEY) && !prevkb.IsKeyDown(W_KEY) && arrow.GetDirection() == UP)
                       {
                           amyAnim.TranslateTo((int)amyPosList[1].X, (int)amyPosList[1].Y);
                           if (arrow.GetIsAvailable())
                           {
                               if (arrow.GetArrow2Rect().Y > collisionRec.Y && arrow.GetArrow2Rect().Y < collisionRec.Y + collisionRec.Height)
                               {
                                   score += 100;
                                   
                                   streak++;
                                   arrow.SetIsAvailable(false);
                                   feedback[0].CreateInstance().Play();
                               }
                               else if (arrow.GetArrowRect().Intersects(collisionRec))
                               {
                                   score += 50;
                                   
                                   streak++;
                                   arrow.SetIsAvailable(false);
                                   feedback[1].CreateInstance().Play();
                               }
                               else
                               {
                                   streak = 0;
                               }
                           }
                            
                       }
                       else if (kb.IsKeyDown(A_KEY) && !prevkb.IsKeyDown(A_KEY) && arrow.GetDirection() == LEFT)
                       {
                           amyAnim.TranslateTo((int)amyPosList[2].X, (int)amyPosList[2].Y);

                           if (arrow.GetIsAvailable())
                           {
                               if (arrow.GetArrow2Rect().Y > collisionRec.Y && arrow.GetArrow2Rect().Y < collisionRec.Y + collisionRec.Height)
                               {
                                   score += 100;
                                   
                                   streak++;
                                   arrow.SetIsAvailable(false);
                                   feedback[0].CreateInstance().Play();
                               }
                               else if (arrow.GetArrowRect().Intersects(collisionRec))
                               {
                                   score += 50;
                                   
                                   streak++;
                                   arrow.SetIsAvailable(false);
                                   feedback[1].CreateInstance().Play();
                               }
                           }
                        }
                       else if (kb.IsKeyDown(S_KEY) && !prevkb.IsKeyDown(S_KEY) && arrow.GetDirection() == DOWN)
                       {
                           if (arrow.GetIsAvailable())
                           {
                               if (arrow.GetArrow2Rect().Y > collisionRec.Y && arrow.GetArrow2Rect().Y < collisionRec.Y + collisionRec.Height)
                               {
                                   score += 100;
                                   
                                   streak++;
                                   arrow.SetIsAvailable(false);
                                   feedback[0].CreateInstance().Play();
                               }
                               else if (arrow.GetArrowRect().Intersects(collisionRec))
                               {
                                   score += 50;
                                   
                                   streak++;
                                   arrow.SetIsAvailable(false);
                                   feedback[1].CreateInstance().Play();
                               }
                           }

                        }
                       else if (kb.IsKeyDown(D_KEY) && !prevkb.IsKeyDown(D_KEY) && arrow.GetDirection() == RIGHT)
                       {

                           if (arrow.GetIsAvailable())
                           {
                               if (arrow.GetArrow2Rect().Y > collisionRec.Y && arrow.GetArrow2Rect().Y < collisionRec.Y + collisionRec.Height)
                               {
                                   score += 100;
                                   
                                   streak++;
                                   arrow.SetIsAvailable(false);
                                   feedback[0].CreateInstance().Play();
                               }
                               else if (arrow.GetArrowRect().Intersects(collisionRec))
                               {
                                   score += 50;
                                   
                                   streak++;
                                   arrow.SetIsAvailable(false);
                                   feedback[1].CreateInstance().Play();
                               }
                           }
                       }
                       
                       if(kb.IsKeyDown(W_KEY))
                       {
                           amyAnim.TranslateTo((int)amyPosList[2].X, (int)amyPosList[2].Y);

                       }
                       else if (kb.IsKeyDown(A_KEY))
                       {
                           amyAnim.TranslateTo((int)amyPosList[1].X, (int)amyPosList[1].Y);

                        }
                       else if (kb.IsKeyDown(S_KEY))
                       {
                           amyAnim.TranslateTo((int)amyPosList[4].X, (int)amyPosList[4].Y);
                        }
                       else if (kb.IsKeyDown(D_KEY))
                       {
                           amyAnim.TranslateTo((int)amyPosList[3].X, (int)amyPosList[3].Y);
                       }
                       else
                       {
                           amyAnim.TranslateTo((int)amyPosList[0].X, (int)amyPosList[0].Y);
                       }
                    }

                    if(streak >= 10)
                    {
                        score += 150;
                        streak = 0;
                    }

                    if (levelData[SONIC_LEVEL_DATA_IDX][27].GetArrowRect().Y > SCREEN_HEIGHT)
                    {
                        SetUpSonic();
                    }
                    if (levelData[ICIRRUS_LEVEL_DATA_IDX][14].GetArrowRect().Y > SCREEN_HEIGHT)
                    {
                        SetUpPokemon(); 
                    }
                    if (levelData[CHUG_JUG_LEVEL_DATA_IDX][3].GetArrowRect().Y > SCREEN_HEIGHT)
                    {
                        SetUpChugJug();
                    }

                    if(MediaPlayer.State != MediaState.Playing)
                    {
                        if (highStreak >= 2)
                        {
                            dgNum = 7;
                            dialogFxInstance[7] = dialogFx[7].CreateInstance();
                            dialogFxInstance[7].Play();
                            
                        }
                            
                        else if (highStreak >= 4)
                        {
                            dgNum = 13;
                            dialogFxInstance[13] = dialogFx[13].CreateInstance();
                            dialogFxInstance[13].Play();
                        }
                        else if (highStreak >= 6)
                        {
                            dgNum = 19;
                            dialogFxInstance[19] = dialogFx[10].CreateInstance();
                            dialogFxInstance[19].Play();
                        }
                        gameplayState = DIALOG;
                    }
                    
                    amyAnim.Update(gameTime);
                    
                    bgColor.R += 1;
                    bgColor.G += 2;
                    bgColor.B += 3;
                    break;

                case END:
                    if(kb.IsKeyDown(SELECT_KEY) && !prevkb.IsKeyDown(SELECT_KEY))
                    {
                        gameplayState = MENU;
                        Reset();
                        MediaPlayer.Play(menuMusic);
                    }

                break;
                
                case DIALOG:
                    if (kb.IsKeyDown(SELECT_KEY) && !prevkb.IsKeyDown(SELECT_KEY))
                    {
                        gameplayState = SELECT;
                    }
                    switch (dgNum)
                    {
                        case 0:
                            dialogStr = "Sonic I need to tell you something...";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[0].State == SoundState.Stopped)
                            {
                                dialogFxInstance[1] = dialogFx[1].CreateInstance();
                                dialogFxInstance[1].Play();
                                dgNum = 1;
                            }
                            
                            
                            
                            break; 
                        case 1:
                            dialogStr = "Whats up amy?";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[1].State == SoundState.Stopped)
                            {
                                dialogFxInstance[2] = dialogFx[2].CreateInstance();
                                dialogFxInstance[2].Play();
                                dgNum = 2;
                            }
                            
                            break; 
                        case 2:
                            dialogStr = "I- Id love you sonic";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[2].State == SoundState.Stopped)
                            {
                                dialogFxInstance[3] = dialogFx[3].CreateInstance();
                                dialogFxInstance[3].Play();
                                dgNum = 3;
                            }
                            
                            break; 
                        case 3:
                            dialogStr = "Ughhh no way! I only date hedgehogs \nwho can dance";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[3].State == SoundState.Stopped)
                            {
                                dialogFxInstance[4] = dialogFx[4].CreateInstance();
                                dialogFxInstance[4].Play();
                                dgNum = 4;
                            }
                            
                            break;
                        case 4:
                            dialogStr = "B-bb but I can dance I promise.";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[4].State == SoundState.Stopped)
                            {
                                dialogFxInstance[5] = dialogFx[5].CreateInstance();
                                dialogFxInstance[5].Play();
                                dgNum = 5;
                            }
                            
                            break; 
                        case 5:
                            dialogStr = "Prove it. Finish one of my songs \nand show me some moves";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[5].State == SoundState.Stopped)
                            {
                                dialogFxInstance[6] = dialogFx[6].CreateInstance();
                                dialogFxInstance[6].Play();
                                dgNum = 6;
                            }
                            
                            break; 
                        case 6:
                            dialogStr = "Ok Sonic i'll show you";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[6].State == SoundState.Stopped)
                            {
                                gameplayState = SELECT;
                            }
                            break;
                        case 7:
                            dialogStr = "WOW AMY I CANT BELIEVE IT \nYOU CAN DANCE!";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[7].State == SoundState.Stopped)
                            {
                                dialogFxInstance[8] = dialogFx[8].CreateInstance();
                                dialogFxInstance[8].Play();
                                dgNum = 8;
                            }
                            break;
                        case 8:
                            dialogStr = "I told you I could dance Sonic";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[8].State == SoundState.Stopped)
                            {
                                dialogFxInstance[9] = dialogFx[9].CreateInstance();
                                dialogFxInstance[9].Play();
                                dgNum = 9;
                            }
                            break;
                        case 9:
                            dialogStr = "So will you be mine?";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[9].State == SoundState.Stopped)
                            {
                                dialogFxInstance[10] = dialogFx[10].CreateInstance();
                                dialogFxInstance[10].Play();
                                dgNum = 10;
                            }
                            break;
                        case 10:
                            dialogStr = "Of course!";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[10].State == SoundState.Stopped)
                            {
                                dialogFxInstance[11] = dialogFx[11].CreateInstance();
                                dialogFxInstance[11].Play();
                                dgNum = 11;
                            }
                            break;
                        case 11:
                            dialogStr = "I love you Amy";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[11].State == SoundState.Stopped)
                            {
                                dialogFxInstance[12] = dialogFx[12].CreateInstance();
                                dialogFxInstance[12].Play();
                                dgNum = 12;
                            }
                            break;
                        case 12:
                            dialogStr = "I love you too Sonic";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[12].State == SoundState.Stopped)
                            {
                                gameplayState = END;
                            }
                            break;
                        case 13:
                            dialogStr = "Hmmm...";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[13].State == SoundState.Stopped)
                            {
                                dialogFxInstance[14] = dialogFx[14].CreateInstance();
                                dialogFxInstance[14].Play();
                                dgNum = 14;
                            }
                            break;
                        case 14:
                            dialogStr = "I think you're going to need to \npractice if you want to be mine";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[14].State == SoundState.Stopped)
                            {
                                dialogFxInstance[15] = dialogFx[15].CreateInstance();
                                dialogFxInstance[15].Play();
                                dgNum = 15;
                            }
                            break;
                        case 15:
                            dialogStr = "Why don't you lock in and get better";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[15].State == SoundState.Stopped)
                            {
                                dialogFxInstance[16] = dialogFx[16].CreateInstance();
                                dialogFxInstance[16].Play();
                                dgNum = 16;
                            }
                            break;
                        case 16:
                            dialogStr = "B- bb but..";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[16].State == SoundState.Stopped)
                            {
                                dialogFxInstance[17] = dialogFx[17].CreateInstance();
                                dialogFxInstance[17].Play();
                                dgNum = 17;
                            }
                            break;
                        case 17:
                            dialogStr = "I don't think this is going \nto work out.. Bye amy.";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[17].State == SoundState.Stopped)
                            {
                                dialogFxInstance[18] = dialogFx[18].CreateInstance();
                                dialogFxInstance[18].Play();
                                dgNum = 18;
                            }
                            break;
                        case 18:
                            dialogStr = "Sonic.. Sonic??";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[18].State == SoundState.Stopped)
                            {
                                gameplayState = END;
                            }
                            break;
                        case 19:
                            dialogStr = "Hmmm...";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[19].State == SoundState.Stopped)
                            {
                                dialogFxInstance[20] = dialogFx[20].CreateInstance();
                                dialogFxInstance[20].Play();
                                dgNum = 20;
                            }
                            break;
                        case 20:
                            dialogStr = "I think you're going to need to \npractice if you want to be mine";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[20].State == SoundState.Stopped)
                            {
                                dialogFxInstance[21] = dialogFx[21].CreateInstance();
                                dialogFxInstance[21].Play();
                                dgNum = 21;
                            }
                            break;
                        case 21:
                            dialogStr = "Why don't you lock in and get better";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[21].State == SoundState.Stopped)
                            {
                                dialogFxInstance[22] = dialogFx[22].CreateInstance();
                                dialogFxInstance[22].Play();
                                dgNum = 22;
                            }
                            break;
                        case 22:
                            dialogStr = "B- bb but..";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[22].State == SoundState.Stopped)
                            {
                                dialogFxInstance[23] = dialogFx[23].CreateInstance();
                                dialogFxInstance[23].Play();
                                dgNum = 23;
                            }
                            break;
                        case 23:
                            dialogStr = "I don't think this is going to work out.. \nWhy dont you ask Shadow instead.";
                            dialogImg = sonicFaceImg;
                            if (dialogFxInstance[23].State == SoundState.Stopped)
                            {
                                dialogFxInstance[24] = dialogFx[24].CreateInstance();
                                dialogFxInstance[24].Play();
                                dgNum = 24;
                            }
                            break;
                        case 24:
                            dialogStr = "Sonic.. Sonic??";
                            dialogImg = amyFaceImg;
                            if (dialogFxInstance[24].State == SoundState.Stopped)
                            {
                                gameplayState = END;
                            }
                            break;
                        
                    }
                    break;

            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void SetUpPokemon()
        {
            levelData[ICIRRUS_LEVEL_DATA_IDX][0] = new Arrow(new Vector2(150, 0), 5, LEFT);
            levelData[ICIRRUS_LEVEL_DATA_IDX][1] = new Arrow(new Vector2(50, -150), 5, DOWN);

            levelData[ICIRRUS_LEVEL_DATA_IDX][2] = new Arrow(new Vector2(250, -250), 5, RIGHT);
            levelData[ICIRRUS_LEVEL_DATA_IDX][3] = new Arrow(new Vector2(350, -350), 5, UP);

            levelData[ICIRRUS_LEVEL_DATA_IDX][4] = new Arrow(new Vector2(150, -500), 5, LEFT);
            levelData[ICIRRUS_LEVEL_DATA_IDX][5] = new Arrow(new Vector2(250, -575), 5, RIGHT);

            levelData[ICIRRUS_LEVEL_DATA_IDX][6] = new Arrow(new Vector2(350, -700), 5, UP);
            levelData[ICIRRUS_LEVEL_DATA_IDX][7] = new Arrow(new Vector2(50, -800), 5, DOWN);

            levelData[ICIRRUS_LEVEL_DATA_IDX][8] = new Arrow(new Vector2(150, -900), 5, LEFT);
            levelData[ICIRRUS_LEVEL_DATA_IDX][9] = new Arrow(new Vector2(350, -1000), 5, UP);

            levelData[ICIRRUS_LEVEL_DATA_IDX][10] = new Arrow(new Vector2(50, -1200), 5, DOWN);
            levelData[ICIRRUS_LEVEL_DATA_IDX][11] = new Arrow(new Vector2(250, -1270), 5, RIGHT);

            levelData[ICIRRUS_LEVEL_DATA_IDX][12] = new Arrow(new Vector2(50, -1370), 5, DOWN);
            levelData[ICIRRUS_LEVEL_DATA_IDX][13] = new Arrow(new Vector2(250, -1390), 5, RIGHT);
            levelData[ICIRRUS_LEVEL_DATA_IDX][14] = new Arrow(new Vector2(250, -1480), 5, RIGHT);
        }

        private void SetUpSonic()
        {
            levelData[SONIC_LEVEL_DATA_IDX][0] = new Arrow(new Vector2(50,0),7,DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][1] = new Arrow(new Vector2(150,-100), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][2] = new Arrow(new Vector2(50, -250), 7, DOWN);

            levelData[SONIC_LEVEL_DATA_IDX][3] = new Arrow(new Vector2(50, -400), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][4] = new Arrow(new Vector2(150, -450), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][5] = new Arrow(new Vector2(250, -470), 7, RIGHT);

            levelData[SONIC_LEVEL_DATA_IDX][6] = new Arrow(new Vector2(50, -650), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][7] = new Arrow(new Vector2(150, -700), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][8] = new Arrow(new Vector2(250, -750), 7, RIGHT);

            levelData[SONIC_LEVEL_DATA_IDX][9] = new Arrow(new Vector2(50, -950), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][10] = new Arrow(new Vector2(150, -950), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][11] = new Arrow(new Vector2(250, -1000), 7, RIGHT);

            levelData[SONIC_LEVEL_DATA_IDX][12] = new Arrow(new Vector2(150, -1250), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][13] = new Arrow(new Vector2(150, -1250), 7, RIGHT);
            levelData[SONIC_LEVEL_DATA_IDX][14] = new Arrow(new Vector2(50, -1300), 7, DOWN);

            levelData[SONIC_LEVEL_DATA_IDX][15] = new Arrow(new Vector2(50, -1400), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][16] = new Arrow(new Vector2(150, -1500), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][17] = new Arrow(new Vector2(50, -1550), 7, DOWN);

            levelData[SONIC_LEVEL_DATA_IDX][18] = new Arrow(new Vector2(50, -1800), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][19] = new Arrow(new Vector2(150, -1850), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][20] = new Arrow(new Vector2(250, -1850), 7, RIGHT);

            levelData[SONIC_LEVEL_DATA_IDX][21] = new Arrow(new Vector2(50, -2050), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][22] = new Arrow(new Vector2(150, -2050), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][23] = new Arrow(new Vector2(250, -2100), 7, RIGHT);

            levelData[SONIC_LEVEL_DATA_IDX][24] = new Arrow(new Vector2(150, -2300), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][25] = new Arrow(new Vector2(250, -2300), 7, RIGHT);
            levelData[SONIC_LEVEL_DATA_IDX][26] = new Arrow(new Vector2(50, -2450), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][27] = new Arrow(new Vector2(350, -2450), 7, UP);
        }

        public void SetUpChugJug()
        {
            for (int i = 0; i < 5; i++)
            {
                int num = rng.Next(0, 4);
                switch (num)
                {
                    case 0:
                        levelData[CHUG_JUG_LEVEL_DATA_IDX][i] = new Arrow(new Vector2(250, -40*4*i), 6, num);
                        break;
                    case 1:
                        levelData[CHUG_JUG_LEVEL_DATA_IDX][i] = new Arrow(new Vector2(150, -40*4*i), 6, num);
                        break;
                    case 2:
                        levelData[CHUG_JUG_LEVEL_DATA_IDX][i] = new Arrow(new Vector2(350, -40*4*i), 6, num);
                        break;
                    case 3:
                        levelData[CHUG_JUG_LEVEL_DATA_IDX][i] = new Arrow(new Vector2(50, -40*4*i), 6, num);
                        break;
                }
                
            }
        }

        public void Reset()
        {
            dgNum = 0;
            score = 0;
            
            SetUpSonic();
            SetUpPokemon();
            SetUpChugJug();
            MediaPlayer.Play(menuMusic);
            dialogFxInstance = new SoundEffectInstance[25];
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(bg, bgRec, bgColor);

            switch (gameplayState)
            {
                case MENU:
                    startBtn.DrawButton(spriteBatch, Color.Purple);
                    spriteBatch.DrawString(titleFont, "Dance Date Revolution", new Vector2(20,yCord), Color.Red);
                    break;

                case SELECT:
                    switch (curMenuNum)
                    {
                        case SONIC_LEVEL_DATA_IDX:
                            spriteBatch.Draw(backgrounds[SONIC_LEVEL_DATA_IDX],new Rectangle(0,0,SCREEN_WIDTH,SCREEN_HEIGHT),Color.White);
                            sonicBtn.DrawButton(spriteBatch, Color.Black);
                            break;
                        case ICIRRUS_LEVEL_DATA_IDX:
                            spriteBatch.Draw(backgrounds[ICIRRUS_LEVEL_DATA_IDX],new Rectangle(0,0,SCREEN_WIDTH,SCREEN_HEIGHT),Color.White);
                            icirrusBtn.DrawButton(spriteBatch, Color.Black);
                            break;
                        case CHUG_JUG_LEVEL_DATA_IDX:
                            spriteBatch.Draw(backgrounds[CHUG_JUG_LEVEL_DATA_IDX],new Rectangle(0,0,SCREEN_WIDTH,SCREEN_HEIGHT),Color.White);
                            chugBtn.DrawButton(spriteBatch, Color.Black);
                            break;
                    }
                    
                    sonicBtn.DrawButton(spriteBatch, Color.Purple);
                    icirrusBtn.DrawButton(spriteBatch, Color.Purple);
                    chugBtn.DrawButton(spriteBatch, Color.Purple);
                    break;

                case GAME_PLAY:
                    spriteBatch.Draw(gridBg, gridBgRec, bgColor);

                    spriteBatch.DrawString(labelFont,"Score: "+score,new Vector2(550,0),Color.Black);
                    spriteBatch.DrawString(labelFont, "Streak:" + streak, new Vector2(950,0), Color.Black);

                    foreach (Arrow arrow in levelData[currLevel])
                    {
                        arrow.Draw(spriteBatch);
                    }
                    amyAnim.Draw(spriteBatch, Color.White, SpriteEffects.None);
                    amyAnim.Draw(spriteBatch, bgColor * 0.65f, SpriteEffects.None);
                    
                    for (int i = 0; i < NUM_COLL; i++)
                    {
                        spriteBatch.Draw(collImg, collRects[i], Color.White);
                        if (i < NUM_COLL - 1) spriteBatch.Draw(collImg, pathRects[i], Color.Gray * 0.25f);
                    }
                    
                    
                    spriteBatch.Draw(collImg,new Rectangle(collisionRec.X, collisionRec.Y + collisionRec.Height, collisionRec.Width + 1, SCREEN_HEIGHT - collisionRec.Y + collisionRec.Height), Color.Black);
                    spriteBatch.Draw(arrowImg[3], new Rectangle(collRects[0].X, collisionRec.Y + collisionRec.Height, arrowImg[0].Width,arrowImg[0].Height - 30), Color.White);
                    spriteBatch.Draw(arrowImg[1], new Rectangle(collRects[1].X, collisionRec.Y + collisionRec.Height, arrowImg[0].Width,arrowImg[0].Height - 30), Color.White);
                    spriteBatch.Draw(arrowImg[0], new Rectangle(collRects[2].X, collisionRec.Y + collisionRec.Height, arrowImg[0].Width,arrowImg[0].Height - 30), Color.White);
                    spriteBatch.Draw(arrowImg[2], new Rectangle(collRects[3].X, collisionRec.Y + collisionRec.Height, arrowImg[0].Width,arrowImg[0].Height - 30), Color.White);
                    if(kb.IsKeyDown(S_KEY))
                        spriteBatch.Draw(arrowImg[3], new Rectangle(collRects[0].X, collisionRec.Y + collisionRec.Height, arrowImg[0].Width,arrowImg[0].Height - 30), Color.Lime);

                    if(kb.IsKeyDown(A_KEY))
                        spriteBatch.Draw(arrowImg[1], new Rectangle(collRects[1].X, collisionRec.Y + collisionRec.Height, arrowImg[0].Width,arrowImg[0].Height - 30), Color.Lime);

                    if(kb.IsKeyDown(D_KEY))
                        spriteBatch.Draw(arrowImg[0], new Rectangle(collRects[2].X, collisionRec.Y + collisionRec.Height, arrowImg[0].Width,arrowImg[0].Height - 30), Color.Lime);
                    if(kb.IsKeyDown(W_KEY))
                        spriteBatch.Draw(arrowImg[2], new Rectangle(collRects[3].X, collisionRec.Y + collisionRec.Height, arrowImg[0].Width,arrowImg[0].Height - 30), Color.Lime);

                    
                    spriteBatch.Draw(collImg,collisionRec,Color.DarkGray * 0.5f);
                    break;

                case END:
                    backToStartBtn.DrawButton(spriteBatch,Color.Purple);
                    break;
                
                case DIALOG:
                    spriteBatch.Draw(bg, bgRec, Color.Black);
                    spriteBatch.Draw(dialogImg,dialogRect,Color.White);
                    spriteBatch.DrawString(labelFont,dialogStr,dialogPos,Color.White);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        
    }
}
