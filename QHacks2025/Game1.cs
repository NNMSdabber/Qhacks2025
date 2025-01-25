using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

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

        private const int SONIC_LEVEL_DATA_IDX = 0;
        private const int ICIRRUS_LEVEL_DATA_IDX = 1;
        private const int CHUG_JUG_LEVEL_DATA_IDX = 2;
        
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

        private Button startBtn;

        private int curMenuNum = SONIC_LEVEL_DATA_IDX;

        private Button sonicBtn;
        private Button icirrusBtn;
        private Button chugBtn;
        private Button backToStartBtn;

        public static Texture2D bg;
        private Rectangle bgRec = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
        private Color bgColor = new Color(255, 255, 255, 255);

        public Texture2D gridBg;
        private Rectangle gridBgRec = new Rectangle(((SCREEN_WIDTH - 500) / 2 - 250) + 500, SCREEN_HEIGHT /2 - 250, 500, 500);

        public static Texture2D[] arrowImg = new Texture2D[MAX_ARROWS];
        private LinkedList<Arrow> arrows = new LinkedList<Arrow>();

        public static Texture2D barImg;
        
        private Arrow[][] levelData = new Arrow[][] { new Arrow[36], new Arrow[15], new Arrow[5]};

        public static SpriteFont labelFont;
        public static SpriteFont titleFont;

        private Rectangle collisionRec = new Rectangle(0,SCREEN_HEIGHT-100,SCREEN_WIDTH,15);

        public KeyboardState kb = new KeyboardState();
        public KeyboardState prevkb = new KeyboardState();

        private int score = 0;
        private int streak = 0;

        public MouseState mouse;
        public MouseState mousePrev;

        public static Texture2D amySpriteSheet;
        private Animation amyAnim;
        
        public static Texture2D buttonImg; 

        private static Song menuMusic;
        private static Song sonic;
        private static Song pokemon;
        private static Song chugJugWithYou;

        private Texture2D[] backgrounds = new Texture2D[3];


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
            // TODO: Add your initialization logic here
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

            startBtn = new Button(buttonImg, 400, 400, "Start");
            
            sonicBtn = new Button(buttonImg, 100, 400, "Windy Hill");
            icirrusBtn = new Button(buttonImg, 500, 400, "Icirrus City");
            chugBtn = new Button(buttonImg, 900, 400, "Chug Jug With You");
            backToStartBtn = new Button(buttonImg,500,400,"Press K to return to menu");
            
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
            // TODO: Unload any non ContentManager content here
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

            switch (gameplayState) 
            {
                case MENU:
                    if (startBtn.rec.Contains(mouse.Position.ToVector2()))
                    {
                        if(startBtn.CheckIfClicked(mouse, mousePrev))
                        {
                            gameplayState = SELECT;
                            score = 0;
                        }
                    }

                    if (kb.IsKeyDown(SELECT_KEY) && !prevkb.IsKeyDown(SELECT_KEY))
                    {
                        gameplayState = SELECT;
                    }
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
                                break;
                            case ICIRRUS_LEVEL_DATA_IDX:
                                MediaPlayer.Play(pokemon);
                                break;
                            case CHUG_JUG_LEVEL_DATA_IDX:
                                MediaPlayer.Play(chugJugWithYou);
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
                    
                    foreach (Arrow arrow in levelData[(int)currLevel])
                    {
                        arrow.Update(gameTime);

                        if (arrow.GetArrowRect().Y > SCREEN_HEIGHT && arrow.GetIsAvailable() == true)
                        {
                            streak = 0;
                        }
                        
                       if(kb.IsKeyDown(W_KEY) && !prevkb.IsKeyDown(W_KEY) && arrow.GetDirection() == UP)
                       {
                           amyAnim.TranslateTo((int)amyPosList[1].X, (int)amyPosList[1].Y);
                            if (arrow.GetArrowRect().Intersects(collisionRec) && arrow.GetIsAvailable() == true)
                            {
                                score += 50;
                                streak++;
                                arrow.SetIsAvailable(false);
                            }
                            
                       }
                       else if (kb.IsKeyDown(A_KEY) && !prevkb.IsKeyDown(A_KEY) && arrow.GetDirection() == LEFT)
                       {
                           amyAnim.TranslateTo((int)amyPosList[2].X, (int)amyPosList[2].Y);
                           if (arrow.GetArrowRect().Intersects(collisionRec) && arrow.GetIsAvailable() == true)
                           {
                               streak++;
                               score += 50;
                               arrow.SetIsAvailable(false);
                           }

                        }
                       else if (kb.IsKeyDown(S_KEY) && !prevkb.IsKeyDown(S_KEY) && arrow.GetDirection() == DOWN)
                       {
                           amyAnim.TranslateTo((int)amyPosList[3].X, (int)amyPosList[3].Y);
                           if (arrow.GetArrowRect().Intersects(collisionRec) && arrow.GetIsAvailable() == true)
                           {
                                streak++;
                                score += 50;
                                arrow.SetIsAvailable(false);
                           }

                        }
                       else if (kb.IsKeyDown(D_KEY) && !prevkb.IsKeyDown(D_KEY) && arrow.GetDirection() == RIGHT)
                       {

                           amyAnim.TranslateTo((int)amyPosList[4].X, (int)amyPosList[4].Y);
                           if (arrow.GetArrowRect().Intersects(collisionRec) && arrow.GetIsAvailable() == true)
                           {
                               arrow.SetIsAvailable(false);
                               score += 50;
                               streak++;
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

                    if (levelData[SONIC_LEVEL_DATA_IDX][34].GetArrowRect().Y > SCREEN_HEIGHT)
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
                        gameplayState = END;
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
                        MediaPlayer.Play(menuMusic);
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
            levelData[SONIC_LEVEL_DATA_IDX][2] = new Arrow(new Vector2(50, -150), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][3] = new Arrow(new Vector2(50, -250), 7, DOWN);

            levelData[SONIC_LEVEL_DATA_IDX][4] = new Arrow(new Vector2(50, -400), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][5] = new Arrow(new Vector2(150, -450), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][6] = new Arrow(new Vector2(250, -470), 7, RIGHT);
            levelData[SONIC_LEVEL_DATA_IDX][7] = new Arrow(new Vector2(350, -500), 7, UP);

            levelData[SONIC_LEVEL_DATA_IDX][8] = new Arrow(new Vector2(50, -650), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][9] = new Arrow(new Vector2(150, -700), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][10] = new Arrow(new Vector2(250, -750), 7, RIGHT);
            levelData[SONIC_LEVEL_DATA_IDX][11] = new Arrow(new Vector2(150, -800), 7, LEFT);

            levelData[SONIC_LEVEL_DATA_IDX][12] = new Arrow(new Vector2(50, -950), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][13] = new Arrow(new Vector2(150, -950), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][14] = new Arrow(new Vector2(250, -1000), 7, RIGHT);
            levelData[SONIC_LEVEL_DATA_IDX][15] = new Arrow(new Vector2(150, -1075), 7, LEFT);

            levelData[SONIC_LEVEL_DATA_IDX][16] = new Arrow(new Vector2(150, -1250), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][17] = new Arrow(new Vector2(150, -1250), 7, RIGHT);
            levelData[SONIC_LEVEL_DATA_IDX][18] = new Arrow(new Vector2(50, -1300), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][19] = new Arrow(new Vector2(350, -1300), 7, UP);

            levelData[SONIC_LEVEL_DATA_IDX][20] = new Arrow(new Vector2(50, -1400), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][21] = new Arrow(new Vector2(150, -1500), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][22] = new Arrow(new Vector2(50, -1550), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][23] = new Arrow(new Vector2(50, -1650), 7, DOWN);

            levelData[SONIC_LEVEL_DATA_IDX][24] = new Arrow(new Vector2(50, -1800), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][25] = new Arrow(new Vector2(150, -1850), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][26] = new Arrow(new Vector2(250, -1850), 7, RIGHT);
            levelData[SONIC_LEVEL_DATA_IDX][27] = new Arrow(new Vector2(350, -1900), 7, UP);

            levelData[SONIC_LEVEL_DATA_IDX][28] = new Arrow(new Vector2(50, -2050), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][29] = new Arrow(new Vector2(150, -2050), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][30] = new Arrow(new Vector2(250, -2100), 7, RIGHT);
            levelData[SONIC_LEVEL_DATA_IDX][31] = new Arrow(new Vector2(150, -2150), 7, LEFT);

            levelData[SONIC_LEVEL_DATA_IDX][32] = new Arrow(new Vector2(150, -2300), 7, LEFT);
            levelData[SONIC_LEVEL_DATA_IDX][33] = new Arrow(new Vector2(250, -2300), 7, RIGHT);
            levelData[SONIC_LEVEL_DATA_IDX][34] = new Arrow(new Vector2(50, -2450), 7, DOWN);
            levelData[SONIC_LEVEL_DATA_IDX][35] = new Arrow(new Vector2(350, -2450), 7, UP);
        }

        public void SetUpChugJug()
        {
            for (int i = 0; i < 5; i++)
            {
                int num = rng.Next(0, 4);
                levelData[CHUG_JUG_LEVEL_DATA_IDX][i] = new Arrow(new Vector2((int)rng.Next(0, 360), -40*4*i), 6, num);
            }
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
                    spriteBatch.DrawString(titleFont, "Robt QHacks 2025", Vector2.Zero, Color.Red);
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

                    foreach (Arrow arrow in levelData[(int)currLevel])
                    {
                        arrow.Draw(spriteBatch);
                    }
                    amyAnim.Draw(spriteBatch, Color.White, SpriteEffects.None);
                    amyAnim.Draw(spriteBatch, bgColor * 0.65f, SpriteEffects.None);
                    
                    spriteBatch.Draw(barImg,collisionRec,Color.White * 0.5f);
                    break;

                case END:
                    backToStartBtn.DrawButton(spriteBatch,Color.Purple);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        //Pre: The mouse button number and the state of the mouse
        //Post: If the mouse button is pressed
        //Desc: Checks if a sepcific mouse button is pressed
        public static bool IsMouseButtonPressed(byte index, MouseState mouse)
        {
            //Chooses which mouse button to check based on the index
            switch (index)
            {
                case 0:
                    //Returns if the left mouse button is pressed
                    return mouse.LeftButton == ButtonState.Pressed;

                case 1:
                    //Returns if the right mouse button is pressed
                    return mouse.RightButton == ButtonState.Pressed;

                default:
                    //Throws an exception if no valid index was given
                    throw new EntryPointNotFoundException("Invalid index passed.");
            }
        }
    }
}
