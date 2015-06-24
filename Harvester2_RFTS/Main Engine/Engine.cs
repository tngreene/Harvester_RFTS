/*
 * Public Class Engine
 * @author: Peter O'Neal/Freddy Garcia/Derrick Hunt
 * 
 * Version: $1.2.2$
 * 
 * Revisions:
 *      1.0.1: (Somebody >.>)
 *          - Added enumerated type for gamestates
 *      1.0.2: (Somebody <.<)
 *          - All classes now share the same spritebatch!
 *      1.2.1: (Sigh..)
 *          - Added Move and CheckBoundaries methods
 *      1.2.2 (Derrick H.)
 *          - Added crosshair image for the mouse
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Harvester
{

    //All possible gamestates
    public enum Gamestate
    {
        MainMenu,
        Gameplay,
        GamePause,
        GameOver,
        LevelComplete
    }

    //all possible gameplay states
    public enum GameplayState
    {
        LevelOne,
        LevelTwo,
        LevelThree
    }

    /// <summary>
    /// The main game engine; starting point for the game
    /// </summary>
    public class Engine : Microsoft.Xna.Framework.Game
    {
        #region Attributes
        private static GraphicsDeviceManager graphics;
        public static int ScreenWidth { get { return graphics.GraphicsDevice.DisplayMode.Width; } }
        public static int ScreenHeight { get { return graphics.GraphicsDevice.DisplayMode.Height; } }

        private SpriteBatch spriteBatch;
        //game states
        private Gamestate prevState;// previous game state
        private Gamestate currentState;//game will start in main menu state
        private GameplayState gameplayState; //the current gameplay state
        //player
        private Player player;
        //levels
        private Level currentLevel;// point to the current level
        private LevelOne l1;
        private LevelTwo l2;
        private LevelThree l3;
        //menus
        private MainMenu mainMenu;//the main menu
        private GameOver gameOverMenu;// the game over menu
        private GamePause pauseMenu;// the pause menu
        private LevelCompleteMenu levelMenu;// the level complete menu
        //private LevelCompleteMenu levelCompleteMenu; //menu for after completing a level
        //cursor for menus
        private Texture2D crosshairTexture; //the texture for the crosshair/cursor
        private Vector2 cursorPos;
        //mouse states
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        //keyboard state
        private KeyboardState kbState;
        //music
        private SoundEffectInstance currentMusic;//the current music
        private SoundEffect menuMusic;
        private SoundEffect l1Music;
        private SoundEffect l2Music;
        private SoundEffect l3Music;
        private bool resetMusic;

        //for playing the startup movie
        /*TODO: For some reason it is impossible to get this to work
         * private Video video;
        private VideoPlayer videoPlayer;
        private Texture2D videoTexture;*/
        private bool playOnce = true;
        #endregion

        #region Properties
        public bool ResetMusic { get { return resetMusic; } set { resetMusic = value; } }
        
        //Return this engine's graphics device
        public GraphicsDevice EngineGraphicsDevice { get { return this.GraphicsDevice; } }
        //game states
        public Gamestate PrevState{get { return prevState; }set { prevState = value; }}
        public Gamestate CurrentState{get { return currentState; }set { currentState = value; }}
        
        //mouse states
        public MouseState CurrentMouseState { get { return currentMouseState; } set { currentMouseState = value; } }
        public MouseState PreviousMouseState { get { return previousMouseState; } set { previousMouseState = value; } }
        //kbstate
        public KeyboardState KbState { get { return kbState; } set { kbState = value; } }
        //the player variable
        public Player Player { get { return player; } set { player = value; } }
        //Level properties so they can be switched within them
        public Level CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
        public GameplayState GameplayState { get { return gameplayState; } set { gameplayState = value; } }
        #endregion

        #region Constructor
        public Engine()
        {
            //create graphics
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //set current state to the main menu
            currentState = Gamestate.MainMenu;

            //set the current gameplayState
            gameplayState = GameplayState.LevelOne;

            //default the game to 720p
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            //graphics.ToggleFullScreen();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            AssetManager.Instance.FillManager(this.Content);

            //make the player
            player = new Player("P1", this);

            //create all game components
            //LEVEL ONE
            l1 = new LevelOne(this);
            l1.Visible = false;         // Disables the level from being visible
            l1.Enabled = false;         // Disables the level from calling its update method
            Components.Add(l1);         // Adds the level to allow it to do stuff

            //LEVEL TWO
            l2 = new LevelTwo(this);
            l2.Visible = false;         // Disables the level from being visible
            l2.Enabled = false;         // Disables the level from calling its update method
            Components.Add(l2);         // Adds the level to allow it to do stuff

            //LEVEL THREE
            l3 = new LevelThree(this);
            l3.Visible = false;         // Disables the level from being visible
            l3.Enabled = false;         // Disables the level from calling its update method
            Components.Add(l3);         // Adds the level to allow it to do stuff

            //MAIN MENU
            mainMenu = new MainMenu(this);
            mainMenu.Visible = false;
            mainMenu.Enabled = false;
            Components.Add(mainMenu);

            //PAUSE MENU
            pauseMenu = new GamePause(this);
            pauseMenu.Visible = false;
            pauseMenu.Enabled = false;
            Components.Add(pauseMenu);

            //GAME OVER MENU
            gameOverMenu = new GameOver(this);
            gameOverMenu.Visible = false;
            gameOverMenu.Enabled = false;
            Components.Add(gameOverMenu);

            //LEVEL COMPLETE MENU
            levelMenu = new LevelCompleteMenu(this);
            levelMenu.Visible = false;
            levelMenu.Enabled = false;
            Components.Add(levelMenu);
            
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

            //make the background music
            l1Music = AssetManager.Instance.FindSound("level_one_music");
            l2Music = AssetManager.Instance.FindSound("level_two_music");
            l3Music = AssetManager.Instance.FindSound("level_three_music");
            menuMusic = AssetManager.Instance.FindSound("no_more_lies");
            currentMusic = menuMusic.CreateInstance();
            currentMusic.Play();
            
            //load video and video player
            //video = Content.Load<Video>("Movies\\start_up_movie");
            //videoPlayer = new VideoPlayer();
            
            //make the mouse line up with cursor
            Mouse.WindowHandle = this.Window.Handle;

            //load the crosshair image
            crosshairTexture = AssetManager.Instance.FindTexture("cross_hair");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //play the movie
            /*if (videoPlayer.State == MediaState.Stopped && playOnce)
            {
                videoPlayer.Play(video);
                playOnce = false;
            }*/

            //pick what music to play
            if (resetMusic == true)
            {
                currentMusic.Dispose();

                if (gameplayState == GameplayState.LevelOne && currentState != Gamestate.MainMenu)
                {
                    currentMusic = l1Music.CreateInstance();
                }
                else if (gameplayState == GameplayState.LevelTwo && currentState != Gamestate.MainMenu)
                {
                    currentMusic = l2Music.CreateInstance();
                }
                else if (gameplayState == GameplayState.LevelThree && currentState != Gamestate.MainMenu)
                {
                    currentMusic = l3Music.CreateInstance();
                }
                else
                {
                    currentMusic = menuMusic.CreateInstance();
                }

                resetMusic = false;
                if (currentMusic != null)
                {
                    currentMusic.Play();
                }
            }

            //update mouse states
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            //update kbState
            kbState = Keyboard.GetState();

            //get the current cursor pos (for crosshair)
            cursorPos = new Vector2(currentMouseState.X - 25, currentMouseState.Y - 25);

            //update the current level
            if (gameplayState == GameplayState.LevelOne)
            {
                currentLevel = l1;
            }
            else if (gameplayState == GameplayState.LevelTwo)
            {
                currentLevel = l2;
            }
            else if (gameplayState == GameplayState.LevelThree)
            {
                currentLevel = l3;
            }

            //UPDATE GAME ONCE VIDEO IS DONE
            //if (videoPlayer.State == MediaState.Stopped)
            {
                //switch the gamestate (FSM)
                switch (currentState)
                {
                    case Gamestate.Gameplay:
                        pauseMenu.Enabled = false; //disable pause menu
                        pauseMenu.Visible = false;
                        gameOverMenu.Enabled = false; //disable the game over menu
                        gameOverMenu.Visible = false;
                        mainMenu.Enabled = false; //disable main menu
                        mainMenu.Visible = false;
                        levelMenu.Enabled = false; //disable levelmenu
                        levelMenu.Visible = false;

                        if (prevState == Gamestate.GameOver)
                        {
                            Reset();
                            prevState = Gamestate.MainMenu;
                        }


                        currentLevel.Enabled = true;
                        currentLevel.Visible = true;
                        gameOverMenu.Accuracy = currentLevel.Accuracy;
                        //gameOverMenu.Score = player.Score;
                        //levelMenu.TotalScore = player.Score;
                        break;
                    case Gamestate.LevelComplete:
                        levelMenu.Accuracy = currentLevel.Accuracy;
                        //levelMenu.Score = currentLevel.LevelScore;

                        pauseMenu.Enabled = false; //disable pause menu
                        pauseMenu.Visible = false;
                        gameOverMenu.Enabled = false; //disable the game over menu
                        gameOverMenu.Visible = false;
                        mainMenu.Enabled = false; //disable main menu
                        mainMenu.Visible = false;
                        currentLevel.Enabled = false; //disable level
                        levelMenu.Visible = true;

                        levelMenu.Enabled = true;
                        levelMenu.Visible = true;
                        break;
                    case Gamestate.MainMenu:
                        //reset the game if we lost
                        if (prevState == Gamestate.GameOver)
                        {
                            Reset();
                            prevState = Gamestate.MainMenu;
                        }
                        pauseMenu.Enabled = false; //disable pause menu
                        pauseMenu.Visible = false;
                        gameOverMenu.Enabled = false; //disable the game over menu
                        gameOverMenu.Visible = false;
                        currentLevel.Enabled = false; //disable main menu
                        currentLevel.Visible = false;
                        levelMenu.Enabled = false; //disable levelmenu
                        levelMenu.Visible = false;

                        mainMenu.Enabled = true; //enable and draw
                        mainMenu.Visible = true;
                        break;
                    case Gamestate.GamePause:
                        currentLevel.Enabled = false; //disable pause menu
                        currentLevel.Visible = true;
                        gameOverMenu.Enabled = false; //disable the game over menu
                        gameOverMenu.Visible = false;
                        mainMenu.Enabled = false; //disable main menu
                        mainMenu.Visible = false;
                        levelMenu.Enabled = false; //disable levelmenu
                        levelMenu.Visible = false;

                        pauseMenu.Enabled = true;//make the pause menu run
                        pauseMenu.Visible = true;
                        break;
                    case Gamestate.GameOver:
                        pauseMenu.Enabled = false; //disable pause menu
                        pauseMenu.Visible = false;
                        currentLevel.Enabled = false; //disable the game over menu
                        currentLevel.Visible = true;
                        mainMenu.Enabled = false; //disable main menu
                        mainMenu.Visible = false;
                        levelMenu.Enabled = false; //disable levelmenu
                        levelMenu.Visible = false;

                        gameOverMenu.Enabled = true; //enable and draw game over menu
                        gameOverMenu.Visible = true;
                        break;
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
            GraphicsDevice.Clear(Color.HotPink);

            spriteBatch.Begin();

            /*#region Play Intro Video
            //PLAY INTRO BEFORE ANYTHING ELSE
            // Only call GetTexture if a video is playing or paused
            if (videoPlayer.State != MediaState.Stopped)
                videoTexture = videoPlayer.GetTexture();

            // Drawing to the rectangle will stretch the video to fill the screen
            Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);

            // Draw the video, if we have a texture to draw.
            if (videoTexture != null)
            {
                spriteBatch.Draw(videoTexture, screen, Color.White);
            }
            #endregion

            //DRAW GAME ONCE VIDEO IS DONE
            if (videoPlayer.State == MediaState.Stopped)*/
            {
                //draw based on the FSM
                switch (currentState)
                {
                    case Gamestate.MainMenu:
                        mainMenu.Draw(gameTime, this.spriteBatch);
                        break;
                    case Gamestate.Gameplay:
                        currentLevel.Draw(gameTime, this.spriteBatch);
                        break;
                    case Gamestate.GamePause:
                        currentLevel.Draw(gameTime, this.spriteBatch);
                        pauseMenu.Draw(gameTime, this.spriteBatch); //draw on top of gameplay
                        break;
                    case Gamestate.GameOver:
                        currentLevel.Draw(gameTime, this.spriteBatch);
                        gameOverMenu.Draw(gameTime, this.spriteBatch); //draw on top of gameplay
                        break;
                    case Gamestate.LevelComplete:
                        currentLevel.Draw(gameTime, this.spriteBatch);
                        levelMenu.Draw(gameTime, this.spriteBatch); //draw on top of gameplay
                        break;
                }

                //draw the crosshair freely in menus
                if (currentState != Gamestate.Gameplay)
                {
                    spriteBatch.Draw(crosshairTexture, cursorPos, Color.White);
                }
                //this.IsMouseVisible = true;//use this to debug the crosshair position
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

        #region Private/Helper Methods
        /// <summary>
        /// Call this when the player dies to reset the game
        /// </summary>
        private void Reset()
        {
            //reset player variables
            player.Score = 0;

            //reset levels
            l1.Reset();
            l2.Reset();
            l3.Reset();
        }
        #endregion
    }
}