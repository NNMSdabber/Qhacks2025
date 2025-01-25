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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public class Game1 : Game
    {
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;

        public const Keys W_KEY = Keys.W;
        public const Keys A_KEY = Keys.A;
        public const Keys S_KEY = Keys.S;
        public const Keys D_KEY = Keys.D;
        public const Keys SELECT_KEY = Keys.Enter;

        public const int MAX_ARROWS = 4;
        public const int RIGHT = 0;
        public const int LEFT = 1;
        public const int UP = 2;
        public const int DOWN = 3;

        public const int MENU = 0;
        public const int GAME_PLAY = 1;
        public const int END = 2;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public int gameplayState = GAME_PLAY;

        public static Texture2D[] arrowImg = new Texture2D[MAX_ARROWS];
        private LinkedList<Arrow> arrows = new LinkedList<Arrow>();
        private Arrow[] levelSonic = new Arrow[36];
        private Arrow[] levelPokemon = new Arrow[15];

        
        public static Random rng = new Random();

        private Rectangle collisionRec = new Rectangle(0,SCREEN_HEIGHT-100,SCREEN_WIDTH,30);

        public KeyboardState kb = new KeyboardState();
        public KeyboardState prevkb = new KeyboardState();

        private int score = 0;

        private Song sonic;
        private Song pokemon;


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

            arrowImg[RIGHT] = Content.Load<Texture2D>("Images/Arrows/RightArrow");
            arrowImg[LEFT] = Content.Load<Texture2D>("Images/Arrows/LeftArrow");
            arrowImg[UP] = Content.Load<Texture2D>("Images/Arrows/ArrowUp");
            arrowImg[DOWN] = Content.Load<Texture2D>("Images/Arrows/ArrowDown");

            sonic = Content.Load<Song>("Audio/Music/Sonic");
            pokemon = Content.Load<Song>("Audio/Music/icirrus");
            // TODO: use this.Content to load your game content here

            for (int i = 0; i < 30; i++)
            {
                arrows.AddLast(new Arrow(new Vector2(100,0-200*i),8,rng.Next(0,4)));
            }

            SetUpSonic();
            SetUpPokemon();

            MediaPlayer.Play(pokemon);

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

            

            switch (gameplayState) 
            {
                case MENU:

                break;

                case GAME_PLAY:

                    foreach (Arrow arrow in levelPokemon)
                    {
                        arrow.Update(gameTime);

                       if(kb.IsKeyDown(W_KEY) && !prevkb.IsKeyDown(W_KEY) && arrow.GetDirection() == UP)
                       {
                            if(arrow.GetArrowRect().Intersects(collisionRec) && arrow.GetIsAvailable() == true)
                            {
                                score += 50;
                                arrow.SetIsAvailable(false);
                            }
                       }
                       else if (kb.IsKeyDown(A_KEY) && !prevkb.IsKeyDown(A_KEY) && arrow.GetDirection() == LEFT)
                       {
                           if (arrow.GetArrowRect().Intersects(collisionRec) && arrow.GetIsAvailable() == true)
                           {
                               score += 50;
                               arrow.SetIsAvailable(false);
                            }
                        }
                       else if (kb.IsKeyDown(S_KEY) && !prevkb.IsKeyDown(S_KEY) && arrow.GetDirection() == DOWN)
                       {
                           if (arrow.GetArrowRect().Intersects(collisionRec) && arrow.GetIsAvailable() == true)
                           {
                               score += 50;
                                arrow.SetIsAvailable(false);
                            }

                        }
                       else if (kb.IsKeyDown(D_KEY) && !prevkb.IsKeyDown(D_KEY) && arrow.GetDirection() == RIGHT)
                       {
                           if (arrow.GetArrowRect().Intersects(collisionRec) && arrow.GetIsAvailable() == true)
                           {
                                arrow.SetIsAvailable(false);
                                score += 50;
                           }

                       }
                    }

                    if (levelSonic[34].GetArrowRect().Y > SCREEN_HEIGHT)
                    {
                        SetUpSonic();
                    }
                    if (levelPokemon[14].GetArrowRect().Y > SCREEN_HEIGHT)
                    {
                        SetUpPokemon(); 
                    }
                    break;

                case END:

                break;

            }



            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void SetUpPokemon()
        {
            levelPokemon[0] = new Arrow(new Vector2(150, 0), 5, LEFT);
            levelPokemon[1] = new Arrow(new Vector2(50, -150), 5, DOWN);

            levelPokemon[2] = new Arrow(new Vector2(250, -250), 5, RIGHT);
            levelPokemon[3] = new Arrow(new Vector2(350, -350), 5, UP);

            levelPokemon[4] = new Arrow(new Vector2(150, -500), 5, LEFT);
            levelPokemon[5] = new Arrow(new Vector2(250, -575), 5, RIGHT);

            levelPokemon[6] = new Arrow(new Vector2(350, -700), 5, UP);
            levelPokemon[7] = new Arrow(new Vector2(50, -800), 5, DOWN);

            levelPokemon[8] = new Arrow(new Vector2(150, -900), 5, LEFT);
            levelPokemon[9] = new Arrow(new Vector2(350, -1000), 5, UP);

            levelPokemon[10] = new Arrow(new Vector2(50, -1200), 5, DOWN);
            levelPokemon[11] = new Arrow(new Vector2(250, -1270), 5, RIGHT);

            levelPokemon[12] = new Arrow(new Vector2(50, -1370), 5, DOWN);
            levelPokemon[13] = new Arrow(new Vector2(250, -1390), 5, RIGHT);
            levelPokemon[14] = new Arrow(new Vector2(250, -1480), 5, RIGHT);
        }

        private void SetUpSonic()
        {
            levelSonic[0] = new Arrow(new Vector2(50,0),7,DOWN);
            levelSonic[1] = new Arrow(new Vector2(150,-100), 7, LEFT);
            levelSonic[2] = new Arrow(new Vector2(50, -150), 7, DOWN);
            levelSonic[3] = new Arrow(new Vector2(50, -250), 7, DOWN);

            levelSonic[4] = new Arrow(new Vector2(50, -400), 7, DOWN);
            levelSonic[5] = new Arrow(new Vector2(150, -450), 7, LEFT);
            levelSonic[6] = new Arrow(new Vector2(250, -470), 7, RIGHT);
            levelSonic[7] = new Arrow(new Vector2(350, -500), 7, UP);

            levelSonic[8] = new Arrow(new Vector2(50, -650), 7, DOWN);
            levelSonic[9] = new Arrow(new Vector2(150, -700), 7, LEFT);
            levelSonic[10] = new Arrow(new Vector2(250, -750), 7, RIGHT);
            levelSonic[11] = new Arrow(new Vector2(150, -800), 7, LEFT);

            levelSonic[12] = new Arrow(new Vector2(50, -950), 7, DOWN);
            levelSonic[13] = new Arrow(new Vector2(150, -950), 7, LEFT);
            levelSonic[14] = new Arrow(new Vector2(250, -1000), 7, RIGHT);
            levelSonic[15] = new Arrow(new Vector2(150, -1075), 7, LEFT);

            levelSonic[16] = new Arrow(new Vector2(150, -1250), 7, LEFT);
            levelSonic[17] = new Arrow(new Vector2(150, -1250), 7, RIGHT);
            levelSonic[18] = new Arrow(new Vector2(50, -1300), 7, DOWN);
            levelSonic[19] = new Arrow(new Vector2(350, -1300), 7, UP);

            levelSonic[20] = new Arrow(new Vector2(50, -1400), 7, DOWN);
            levelSonic[21] = new Arrow(new Vector2(150, -1500), 7, LEFT);
            levelSonic[22] = new Arrow(new Vector2(50, -1550), 7, DOWN);
            levelSonic[23] = new Arrow(new Vector2(50, -1650), 7, DOWN);

            levelSonic[24] = new Arrow(new Vector2(50, -1800), 7, DOWN);
            levelSonic[25] = new Arrow(new Vector2(150, -1850), 7, LEFT);
            levelSonic[26] = new Arrow(new Vector2(250, -1850), 7, RIGHT);
            levelSonic[27] = new Arrow(new Vector2(350, -1900), 7, UP);

            levelSonic[28] = new Arrow(new Vector2(50, -2050), 7, DOWN);
            levelSonic[29] = new Arrow(new Vector2(150, -2050), 7, LEFT);
            levelSonic[30] = new Arrow(new Vector2(250, -2100), 7, RIGHT);
            levelSonic[31] = new Arrow(new Vector2(150, -2150), 7, LEFT);

            levelSonic[32] = new Arrow(new Vector2(150, -2300), 7, LEFT);
            levelSonic[33] = new Arrow(new Vector2(250, -2300), 7, RIGHT);
            levelSonic[34] = new Arrow(new Vector2(50, -2450), 7, DOWN);
            levelSonic[35] = new Arrow(new Vector2(350, -2450), 7, UP);
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

            switch (gameplayState)
            {
                case MENU:
                    
                    break;

                case GAME_PLAY:
      
                    foreach (Arrow arrow in levelPokemon)
                    {
                        arrow.Draw(spriteBatch);
                    }

                    spriteBatch.Draw(arrowImg[RIGHT],collisionRec,Color.Black);

                    break;

                case END:

                    break;

            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
