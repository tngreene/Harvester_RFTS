/*
 * Public Class Engine
 * @author: Peter O'Neal/Derrick Hunt
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
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
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
        LevelComplete,
        ExitProgram,
        FatalError
    }

    //all possible gameplay states
    public enum GameplayState
    {
        LevelOne,
        LevelTwo,
        LevelThree
    }
    
    public enum InputEntity
    {
        Nothing,
        ClickedPlay
    }

    /// <summary>
    /// The main game engine; starting point for the game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        //Temporary measures just to get this thing running
        public const int WIDTH = 1920;
        public const int HEIGHT = 1080;

        #region Attributes
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //game states
        private InputEntity inputEntity;// previous game state
        private Gamestate currentState;//game will start in main menu state
        //private int currentLevel;
        private GameplayState gameplayState; //the current gameplay state
        
        //levels
        private Level theLevel;// point to the current level
        
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

        private bool resetMusic;
        
        #endregion

        #region Properties
        public bool ResetMusic { get { return resetMusic; } set { resetMusic = value; } }
        
        //game states
        public InputEntity LastInput{get { return inputEntity; }set { inputEntity = value; }}
        public Gamestate CurrentState{get { return currentState; }set { currentState = value; }}
        
        //mouse states
        public MouseState CurrentMouseState { get { return currentMouseState; } set { currentMouseState = value; } }
        public MouseState PreviousMouseState { get { return previousMouseState; } set { previousMouseState = value; } }
        //kbstate
        public KeyboardState KbState { get { return kbState; } set { kbState = value; } }
        
        //Level properties so they can be switched within them
        public Level TheLevel { get { return theLevel; } set { theLevel = value; } }
        public GameplayState GameplayState { get { return gameplayState; } set { gameplayState = value; } }
        #endregion

        #region Constructor
        public Game()
        {
            //create graphics
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //set current state to the main menu
            currentState = Gamestate.MainMenu;
            inputEntity = InputEntity.Nothing;

            //set the current gameplayState
            gameplayState = GameplayState.LevelOne;
            //gameplayState = GameplayState.LevelTwo;
            //gameplayState = GameplayState.LevelThree;

            //default the game to 720p
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            //graphics.ToggleFullScreen();
            //Mute all
            SoundEffect.MasterVolume = 0;
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
            #region Manager Loading Stuff

            //Get the current directory and store it as a string
            String currentDirectory = Directory.GetCurrentDirectory();

            //Add on the Content Directory
            currentDirectory += "\\Content";

            //Loop through the whole array fixing the paths and loading content
            for (int i = 0; i < AssetMgr.Inst().LoadArray.Length; i++)
            {
                //Remove the large unimportant path infront
                AssetMgr.Inst().LoadArray[i] = AssetMgr.Inst().LoadArray[i].Remove(0, currentDirectory.Count() + 1);

                //Find out where the \ is, used for substring
                int slashPlace = 0;
                for (slashPlace = 0; slashPlace < AssetMgr.Inst().LoadArray[i].Length; slashPlace++)
                {
                    if (AssetMgr.Inst().LoadArray[i].ToCharArray()[slashPlace] == '\\')
                    {
                        break;
                    }
                }
                
                //Turns something like Folder\\asset.xnb into asset, later on when one would like to set a game piece's graphic they'll use this
                string key = AssetMgr.Inst().LoadArray[i].Substring(slashPlace + 1);
                key = key.Substring(0, key.Length - 4);

                //Turns something like Folder\\asset.xnb into Folder\\asset
                string location = AssetMgr.Inst().LoadArray[i].Remove(AssetMgr.Inst().LoadArray[i].Length - 4);

                //Based on the folder name decide which dictionary it should populate
                switch (AssetMgr.Inst().LoadArray[i].Substring(0, slashPlace))
                {
                    case "Sound":
                        //Pass in the key (file name - .xnb) and the loaded texture
                        AssetMgr.Inst().FillDictionaries(key, Content.Load<SoundEffect>(location));
                        break;
                    case "Fonts":
                        AssetMgr.Inst().FillDictionaries(key, Content.Load<SpriteFont>(location));
                        break;
                    default:
                        AssetMgr.Inst().FillDictionaries(key, Content.Load<Texture2D>(location));
                        break;
                }
            }
            #endregion
            
            //Fill up all the level arrays
            AssetMgr.Inst().FillLevelArrays();
            //create all game components
            theLevel = new Level(this.Window.ClientBounds);
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
           // l1Music = AssetMgr.Inst().SoundDic["
            //l2Music = AssetMgr.Inst().SoundDic["level_two_music"];
            //l3Music = AssetMgr.Inst().SoundDic["level_three_music"];
            menuMusic = AssetMgr.Inst().SoundDic["no_more_lies"];
            currentMusic = menuMusic.CreateInstance();
            currentMusic.Play();

            //make the mouse line up with cursor
            Mouse.WindowHandle = this.Window.Handle;

            //load the crosshair image
            crosshairTexture = AssetMgr.Inst().TextureDic["cross_hair"];
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
        /// The lookup table for the finite statemachine
        /// </summary>
        /// <param name="curState">Current State of the program</param>
        /// <param name="inputEntity">What just happened</param>
        /// <returns></returns>
        private Gamestate LookUpTable(Gamestate curState, InputEntity inputEntity)
        {
            switch (curState)
            {
                case Gamestate.MainMenu:
                    pauseMenu.Enabled = false; //disable pause menu
                    pauseMenu.Visible = false;
                    gameOverMenu.Enabled = false; //disable the game over menu
                    gameOverMenu.Visible = false;
                    //currentLevel.Enabled = false; //disable main menu
                    //currentLevel.Visible = false;
                    levelMenu.Enabled = false; //disable levelmenu
                    levelMenu.Visible = false;
                    
                    mainMenu.Enabled = true; //enable and draw
                    mainMenu.Visible = true;
                    switch (inputEntity)
                    {
                        case InputEntity.Nothing:
                            return Gamestate.MainMenu;

                        case InputEntity.ClickedPlay:
                            theLevel.ChangeLevel(1);
                            return Gamestate.Gameplay;

                        default:
                            return Gamestate.FatalError;
                    }
                case Gamestate.Gameplay:
                    mainMenu.Enabled = false;
                    mainMenu.Visible = false;
                    switch (inputEntity)
                    {
                        case InputEntity.Nothing:
                            return Gamestate.Gameplay;
                    }

                    return Gamestate.FatalError;
                case Gamestate.GamePause:
                    return Gamestate.FatalError;
                case Gamestate.GameOver:
                    return Gamestate.FatalError;
                case Gamestate.LevelComplete:
                    //If you are not at the final level
                        //Display level complete splash
                        //if the user clicks the next button
                        //Transition to the next level
                        //if the user clicks the back button
                        //Go to the beginning
                    //If you are on the final level
                        //Display the final level complete splash
                    return Gamestate.FatalError;
                case Gamestate.ExitProgram:
                    return Gamestate.FatalError;
                case Gamestate.FatalError:
                default:
                    break;
            }
            return Gamestate.FatalError;
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

            if (currentState == Gamestate.ExitProgram)
            {
                this.Exit();
            }

            Gamestate transition = LookUpTable(currentState, inputEntity);
            if (transition != Gamestate.FatalError)
            {
                currentState = transition;
                inputEntity = InputEntity.Nothing;

                //Make decision on how to set the previous state based on what happenes
                //
                //update mouse states
                previousMouseState = currentMouseState;
                currentMouseState = Mouse.GetState();

                if (currentState == Gamestate.MainMenu)
                {

                }
                if (currentState == Gamestate.Gameplay)
                {
                    PlayerMgr.Inst().Update(gameTime, this.Window.ClientBounds);                    
                    theLevel.Update(gameTime, this.Window.ClientBounds);
                }
                if (currentState == Gamestate.LevelComplete)
                {

                }

                //update kbState
                kbState = Keyboard.GetState();

                //get the current cursor pos (for crosshair)
                cursorPos = new Vector2(currentMouseState.X - 25, currentMouseState.Y - 25);

            }
            else
            {
                throw new Exception("Entered in a non existant game state!" + currentState);
            }
            //Look up the transition
        
            //pick what music to play
            if (resetMusic == true)
            {
                currentMusic.Dispose();

                

                resetMusic = false;
                
            }

            
            /*
            
            
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

                //if (inputEntity == Gamestate.GameOver)
                {
                    Reset();
                  //  inputEntity = Gamestate.MainMenu;
                }


                //currentLevel.Enabled = true;
               // currentLevel.Visible = true;
                //gameOverMenu.Accuracy = currentLevel.Accuracy;
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
               // currentLevel.Enabled = false; //disable level
                levelMenu.Visible = true;

                levelMenu.Enabled = true;
                levelMenu.Visible = true;
                break;
            case Gamestate.MainMenu:
                //reset the game if we lost
               // if (inputEntity == Gamestate.GameOver)
                {
                    Reset();
                //    inputEntity = Gamestate.MainMenu;
                }
                
                break;
            case Gamestate.GamePause:
                //currentLevel.Enabled = false; //disable pause menu
                //currentLevel.Visible = true;
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
                //currentLevel.Enabled = false; //disable the game over menu
                //currentLevel.Visible = true;
                mainMenu.Enabled = false; //disable main menu
                mainMenu.Visible = false;
                levelMenu.Enabled = false; //disable levelmenu
                levelMenu.Visible = false;

                gameOverMenu.Enabled = true; //enable and draw game over menu
                gameOverMenu.Visible = true;
                break;
            }
            */
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

            //draw based on the FSM
            switch (currentState)
            {
                case Gamestate.MainMenu:
                    mainMenu.Draw(gameTime, this.spriteBatch);
                    break;
                case Gamestate.Gameplay:
                    theLevel.Draw(gameTime, this.spriteBatch);
                    break;
                case Gamestate.GamePause:
                    theLevel.Draw(gameTime, this.spriteBatch);
                    pauseMenu.Draw(gameTime, this.spriteBatch); //draw on top of gameplay
                    break;
                case Gamestate.GameOver:
                    theLevel.Draw(gameTime, this.spriteBatch);
                    gameOverMenu.Draw(gameTime, this.spriteBatch); //draw on top of gameplay
                    break;
                case Gamestate.LevelComplete:
                    theLevel.Draw(gameTime, this.spriteBatch);
                    levelMenu.Draw(gameTime, this.spriteBatch); //draw on top of gameplay
                    break;
            }

            //draw the crosshair freely in menus
            if (currentState != Gamestate.Gameplay)
            {
                spriteBatch.Draw(crosshairTexture, cursorPos, Color.White);
            }
            //this.IsMouseVisible = true;//use this to debug the crosshair position
            
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
            //player.Score = 0;

            //reset levels
            //l1.Reset();
           // l2.Reset();
            //l3.Reset();
        }
        #endregion
    }
}