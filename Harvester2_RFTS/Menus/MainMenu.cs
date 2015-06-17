/*
 * public class MainMenu
 * @author Zachary Whitman/Derrick Hunt
 * 
 * Version:
 *      $1.5.0MOARBada$$
 * 
 * Revisions:
 *       1.2.0: (Dunno)
 *          - Made a rectangle variable instead of a vector for menu buttons
 *          - Added abstract methods from the menu class. If start is clicked, stuff happens!
 *       1.3.0: (Derrick Hunt)
 *          - Added main buttons and background and whatnot
 *       1.4.0: (Derrick Hunt)
 *          - Cleaned up code with abstraction
 *       1.5.0: (Derrick Hunt)
 *          - Animated the menu, custom new graphics. A+++ material here
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Harvester
{
    //main menu state
    public enum MainMenuState
    {
        MainMenu,
        CreditsMenu,
        ManualMenu
    }

    /// <summary>
    /// The main menu that will be displayed when the player has loaded the game
    /// </summary>
    public class MainMenu : Menu
    {
        #region Attributes
        //texture graphics for the main menu
        private Texture2D logoTexture;
        private Texture2D playButtonTexture;
        private Texture2D creditsButtonTexture;
        private Texture2D manualButtonTexture;
        private Texture2D backgroundTexture;
        //texture graphics for the credits menu
        private Vector2 creditsBackgroundPos;
        private Texture2D creditsBackgroundTexture;
        private Texture2D creditsBackButtonTexture;
        //texture graphics for the manual menu
        private Vector2 manualBackgroundPos;
        private Texture2D manualBackgroundTexture;
        private Texture2D manualBackButtonTexture;
        //random moving boxes for overall background
        private Texture2D backgroundBoxTexture;
        private List<BackgroundBox> backgroundBoxes;

        //buttons for the main menu
        private Button playButton;
        private Button creditsButton;
        private Button manualButton;
        //buttons for the options menu
        private Button creditsBackButton;
        //buttons for the manual menu
        private Button manualBackButton;
        //Buttons for scrolling text
        private Button upButton;
        private Button downButton;
        //Text list for the manual
        private List<String> textLineList;

        private MainMenuState currentState;
        private MainMenuState prevState;
        private int menuCounter; //used to animate the menu
        private Texture2D downButtonTexture;
        private Texture2D upButtonTexture;
        #endregion

        #region Properties
        /// <summary>
        /// The texture for the main logo of the game (Harvester)
        /// </summary>
        public Texture2D LogoTexture
        {
            get { return logoTexture; }
            set { logoTexture = value; }
        }

        /// <summary>
        /// The texture for the play button
        /// </summary>
        public Texture2D PlayButtonTexture
        {
            get { return playButtonTexture; }
            set { playButtonTexture = value; }
        }

        /// <summary>
        /// The texture for the credits button
        /// </summary>
        public Texture2D CreditsButtonTexture
        {
            get { return creditsButtonTexture; }
            set { creditsButtonTexture = value; }
        }

        /// <summary>
        /// The texture for the manual button
        /// </summary>
        public Texture2D ManualButtonTexture
        {
            get { return manualButtonTexture; }
            set { manualButtonTexture = value; }
        }

        /// <summary>
        /// The texture for the main background 
        /// </summary>
        public Texture2D BackgroundTexture
        {
            get { return backgroundTexture; }
            set { backgroundTexture = value; }
        }

        /// <summary>
        /// The position of the background for the credits section
        /// of the menu
        /// </summary>
        public Vector2 CreditsBackgroundPos
        {
            get { return creditsBackgroundPos; }
            set { creditsBackgroundPos = value; }
        }

        /// <summary>
        /// The texture for the background of the options section
        /// </summary>
        public Texture2D CreditsBackgroundTexture
        {
            get { return creditsBackgroundTexture; }
            set { creditsBackgroundTexture = value; }
        }

        /// <summary>
        /// The texture for the back button of the credits section
        /// </summary>
        public Texture2D CreditsBackButtonTexture
        {
            get { return creditsBackButtonTexture; }
            set { creditsBackButtonTexture = value; }
        }

        /// <summary>
        /// The position of the background for the manual section
        /// </summary>
        public Vector2 ManualBackgroundPos
        {
            get { return manualBackgroundPos; }
            set { manualBackgroundPos = value; }
        }

        /// <summary>
        /// The texture for the background of the manual section
        /// </summary>
        public Texture2D ManualBackgroundTexture
        {
            get { return manualBackgroundTexture; }
            set { manualBackgroundTexture = value; }
        }

        /// <summary>
        /// The texture for the back button of the manual section
        /// </summary>
        public Texture2D ManualBackButtonTexture
        {
            get { return manualBackButtonTexture; }
            set { manualBackButtonTexture = value; }
        }

        /// <summary>
        /// The texture for the randomly moving background boxes
        /// </summary>
        public Texture2D BackgroundBoxTexture
        {
            get { return backgroundBoxTexture; }
            set { backgroundBoxTexture = value; }
        }

        /// <summary>
        /// The list of all of the randomly moving background boxes
        /// </summary>
        public List<BackgroundBox> BackgroundBoxes
        {
            get { return backgroundBoxes; }
            set { backgroundBoxes = value; }
        }

        /// <summary>
        /// The button to play the game
        /// </summary>
        public Button PlayButton
        {
            get { return playButton; }
            set { playButton = value; }
        }

        /// <summary>
        /// The button to go to the credits section
        /// </summary>
        public Button CreditsButton
        {
            get { return creditsButton; }
            set { creditsButton = value; }
        }

        /// <summary>
        /// The button to go to the manual section
        /// </summary>
        public Button ManualButton
        {
            get { return manualButton; }
            set { manualButton = value; }
        }

        /// <summary>
        /// The button which returns to the main section
        /// from the credits section
        /// </summary>
        public Button CreditsBackButton
        {
            get { return creditsBackButton; }
            set { creditsBackButton = value; }
        }

        /// <summary>
        /// The button which returns to the main section
        /// from the manual section
        /// </summary>
        public Button ManualBackButton
        {
            get { return manualBackButton; }
            set { manualBackButton = value; }
        }

        /// <summary>
        /// The button which scrolls text upward
        /// </summary>
        public Button UpButton
        {
            get { return upButton; }
            set { upButton = value; }
        }

        /// <summary>
        /// The button which scrolls text downward
        /// </summary>
        public Button DownButton
        {
            get { return downButton; }
            set { downButton = value; }
        }

        /// <summary>
        /// The list of strings that will appear in the manual
        /// </summary>
        public List<String> TextLineList
        {
            get { return textLineList; }
            set { textLineList = value; }
        }

        /// <summary>
        /// The current state that the main menu is currently in
        /// </summary>
        public MainMenuState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        /// <summary>
        /// The previous state that the main menu is currently in
        /// </summary>
        public MainMenuState PrevState
        {
            get { return prevState; }
            set { prevState = value; }
        }

        /// <summary>
        /// Used to calculate the speed at which the background animates between
        /// the different sections
        /// </summary>
        public int MenuCounter
        {
            get { return menuCounter; }
            set { menuCounter = value; }
        }

        /// <summary>
        /// The texture for the up text-scrolling button
        /// </summary>
        public Texture2D UpButtonTexture
        {
            get { return upButtonTexture; }
            set { upButtonTexture = value; }
        }

        /// <summary>
        /// The texture for the down text-scrolling button
        /// </summary>
        public Texture2D DownButtonTexture
        {
            get { return downButtonTexture; }
            set { downButtonTexture = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates the main menu
        /// </summary>
        public MainMenu(Game theGame)
            : base(theGame)
        {
            //create list of background boxes
            backgroundBoxes = new List<BackgroundBox>();

            //set initial state to main menu
            currentState = MainMenuState.MainMenu;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the Main Menu
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //set value of the menuCounter
            menuCounter = GraphicsDevice.Viewport.Width;
        }

        /// <summary>
        /// Loads the content of the main menu
        /// </summary>
        protected override void LoadContent()
        {
            //load in all textures
            //main menu textures
            logoTexture = AssetMgr.Inst().TextureDic["main_menu_logo"];
            playButtonTexture = AssetMgr.Inst().TextureDic["main_menu_play"];
            creditsButtonTexture = AssetMgr.Inst().TextureDic["main_menu_credits"];
            manualButtonTexture = AssetMgr.Inst().TextureDic["main_menu_manual"];
            backgroundBoxTexture = AssetMgr.Inst().TextureDic["main_menu_box"];
            backgroundTexture = AssetMgr.Inst().TextureDic["main_menu_background"];
            //credits menu textures
            creditsBackgroundTexture = AssetMgr.Inst().TextureDic["credits_menu_background"];
            creditsBackButtonTexture = AssetMgr.Inst().TextureDic["main_menu_back_left"];
            //manual menu textures
            manualBackgroundTexture = AssetMgr.Inst().TextureDic["manual_menu_background"];
            manualBackButtonTexture = AssetMgr.Inst().TextureDic["main_menu_back_right"];
            //up and down button textures
            upButtonTexture = AssetMgr.Inst().TextureDic["manual_up_button_temp"];
            downButtonTexture = AssetMgr.Inst().TextureDic["manual_down_button_temp"];

            //create all buttons
            //main menu buttons
            playButton = new Button(new Rectangle( 440, 272, 400, 400), playButtonTexture);
            creditsButton = new Button(new Rectangle(850, 372, 200, 200), creditsButtonTexture);
            manualButton = new Button(new Rectangle(230, 372, 200, 200), manualButtonTexture);
            //credits menu buttons
            creditsBackgroundPos = new Vector2(1295, 225);
            creditsBackButton = new Button(new Rectangle(1295, 612, 200, 100), creditsBackButtonTexture);
            //manual menu buttons
            manualBackgroundPos = new Vector2(-1265, 225);
            manualBackButton = new Button(new Rectangle(-215, 612, 200, 100), manualBackButtonTexture);
            //Scroll buttons
            upButton = new Button(new Rectangle(-1200, 255, 25, 25), upButtonTexture);
            downButton = new Button(new Rectangle(-1200, 655, 25, 25), downButtonTexture);
            
            #region The Instructions
            //Text list
            textLineList = new List<string>();
            //You'll need to add in the info line by line
            textLineList.Add("                                            Welcome to Harvester!\n");
            textLineList.Add("\n");
            textLineList.Add("A = Move Left\n");
            textLineList.Add("W = Move Forward\n");
            textLineList.Add("D = Move Right\n");
            textLineList.Add("S = Move Backward\n");
            textLineList.Add("Q/Middle Click = Fire Powerup\n");
            textLineList.Add("Click = Shoot\n");
            textLineList.Add("Right Click = Fire the Harvester\n");
            textLineList.Add("The Harvester kills the enemy when they touch and regenerates a small portion of the player's health");
            textLineList.Add("A powerup is a special bonus attack that has its own unique effect");
            textLineList.Add("\n");
            #endregion

            //randomly generate background boxes with random color and size
            Random gen = new Random();
            for (int i = 0; i < 600; i++)
            {
                int r = gen.Next(256);
                int g = gen.Next(256);
                int b = gen.Next(256);
                int a = gen.Next(256);
                Color color = new Color(r, g, b, a);
                int size = gen.Next(5) + 5;
                BackgroundBox temp = (new BackgroundBox(new Rectangle(gen.Next(1280*3)-1270, gen.Next(720*3) - 710,
                    size, size), backgroundBoxTexture, size*2, color));
                backgroundBoxes.Add(temp);
            }
        }

        /// <summary>
        /// Updates the game every frame per second thing
        /// Checks to see if the mouse was clicked
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //check which buttons are clicked
            if ((gameRef.CurrentMouseState.LeftButton == ButtonState.Pressed && gameRef.PreviousMouseState.LeftButton == ButtonState.Released) &&
                menuCounter == GraphicsDevice.Viewport.Width)//can only check for click if menu isn't animating
            {
                //play button (change gamestate to the game)
                if (playButton.ObjectPosition.Contains(new Point(gameRef.CurrentMouseState.X, gameRef.CurrentMouseState.Y)))
                {
                    gameRef.LastInput = InputEntity.ClickedPlay;
                    //reset main menu states
                    currentState = MainMenuState.MainMenu;
                    prevState = MainMenuState.MainMenu;

                    gameRef.ResetMusic = true;
                }

                //credits button (shift menu to the right)
                if (creditsButton.ObjectPosition.Contains(new Point(gameRef.CurrentMouseState.X, gameRef.CurrentMouseState.Y)))
                {
                    currentState = MainMenuState.CreditsMenu;
                    prevState = MainMenuState.MainMenu;
                    menuCounter = 0;
                    
                }
                //credits back button (shift menu back to main)
                if (creditsBackButton.ObjectPosition.Contains(new Point(gameRef.CurrentMouseState.X, gameRef.CurrentMouseState.Y)))
                {
                    currentState = MainMenuState.MainMenu;
                    prevState = MainMenuState.CreditsMenu;
                    menuCounter = 0;
                }

                //manual button (shift menu to the left)
                if (manualButton.ObjectPosition.Contains(new Point(gameRef.CurrentMouseState.X, gameRef.CurrentMouseState.Y)))
                {
                        currentState = MainMenuState.ManualMenu;
                        prevState = MainMenuState.MainMenu;
                        menuCounter = 0;
                    
                }
                //manual back button (shift menu back to main)
                if (manualBackButton.ObjectPosition.Contains(new Point(gameRef.CurrentMouseState.X, gameRef.CurrentMouseState.Y)))
                {
                        currentState = MainMenuState.MainMenu;
                        prevState = MainMenuState.ManualMenu;
                        menuCounter = 0;
                    
                }
            }

            //animate the menu if the menuCounter has been reset
            if (menuCounter < GraphicsDevice.Viewport.Width)
            {
                switch (currentState)
                {
                    case MainMenuState.MainMenu:
                        if (prevState == MainMenuState.CreditsMenu)
                        {
                            ShiftMenu(8);
                        }
                        else if (prevState == MainMenuState.ManualMenu)
                        {
                            ShiftMenu(-8);
                        }
                        menuCounter += 8;
                        break;
                    case MainMenuState.CreditsMenu:
                        ShiftMenu(-8);
                        menuCounter += 8;
                        break;
                    case MainMenuState.ManualMenu:
                        ShiftMenu(8);
                        menuCounter += 8;
                        break;
                }
            }

            //update background rectangles
            Random gen = new Random();
            for (int i = 0; i < backgroundBoxes.Count; i++)
            {
                int direction = gen.Next(16);
                if (direction == 0)
                    backgroundBoxes[i].ObjectXPos += (int)backgroundBoxes[i].Speed;
                if (direction == 1)
                    backgroundBoxes[i].ObjectXPos -= (int)backgroundBoxes[i].Speed;
                if (direction == 2)
                    backgroundBoxes[i].ObjectYPos += (int)backgroundBoxes[i].Speed;
                if (direction == 3)
                    backgroundBoxes[i].ObjectYPos -= (int)backgroundBoxes[i].Speed;
            }
        }


        /// <summary>
        /// Draws the menu's images, the start button and later on the background image
        /// </summary>
        /// <param name="gameTime">The gameTime of the game</param>
        /// <param name="spriteBatch">The spritebatch from the engine and its graphics device</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw the background texture and background boxes
            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
            for (int i = 0; i < backgroundBoxes.Count; i++)
            {
                backgroundBoxes[i].Draw(spriteBatch);
            }
            //draw the logo
            spriteBatch.Draw(logoTexture, new Vector2(0, 50), Color.White);

            //draw buttons
            playButton.Draw(spriteBatch);
            manualButton.Draw(spriteBatch);
            creditsButton.Draw(spriteBatch);
            manualBackButton.Draw(spriteBatch);
            creditsBackButton.Draw(spriteBatch);
            upButton.Draw(spriteBatch);
            downButton.Draw(spriteBatch);

            //draw menus
            spriteBatch.Draw(creditsBackgroundTexture, creditsBackgroundPos, Color.White);
            spriteBatch.Draw(manualBackgroundTexture, manualBackgroundPos, Color.White);

            //Draw the game instructions
            for (int i = 0; i < textLineList.Count; i++)
            {
                                                                    //Formerly MainMenuFont
                spriteBatch.DrawString(AssetMgr.Inst().SpriteFontDic["ScoreFont"],textLineList[i], new Vector2(manualBackgroundPos.X + 30, 300f+(i*25f)), Color.White);
            }
        }
        #endregion

        #region Private/Helper Methods
        /// <summary>
        /// Shifts the menu screen towards a direction
        /// specified by the parameter.
        /// </summary>
        /// <param name="direction">The direction towards where the screen shifts</param>
        private void ShiftMenu(int direction)
        {
            //shift all menu elements a certain direction
            //buttons, backgrounds, and boxes
            //boxes
            for (int i = 0; i < backgroundBoxes.Count; i++)
            {
                for (int j = 0; j < Math.Abs(direction); j++)
                {
                    if (direction > 0)
                    {
                        backgroundBoxes[i].ObjectXPos++;
                    }
                    else if (direction <= 0)
                    {
                        backgroundBoxes[i].ObjectXPos--;
                    }
                }
            }
            //buttons
            playButton.ObjectXPos += direction;
            manualButton.ObjectXPos += direction;
            creditsButton.ObjectXPos += direction;
            creditsBackButton.ObjectXPos += direction;
            manualBackButton.ObjectXPos += direction;

            //background textures
            creditsBackgroundPos.X += direction;
            manualBackgroundPos.X += direction;

        }

        /// <summary>
        /// This scrolls the menu, basicly decides what items of a list to draw
        /// </summary>
        /// <param name="upOrDown">-1 to move up, 1 to move down, 0 to stay still</param>
        private void ScrollManual(int upOrDown)
        {
            List<String> activeList = textLineList;
        }
        #endregion
    }
}
