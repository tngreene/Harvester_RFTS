/*
 * public class GamePause
 * @author Zach Whitman/Derrick Hunt
 * 
 * Version:
 *      $1.5.0$
 *      
 * 
 * Revisions:
 *          1.5 - added proper textures and made it work 100% - Derrick Hunt
 * 
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
    /// <summary>
    /// Menu that will be displayed when the player pauses the game
    /// </summary>
    public class GamePause : Menu
    {
        #region Attributes
        //textures
        private Texture2D background;
        private Texture2D title;
        private Texture2D pauseButtonTexture;
        private Texture2D quitButtonTexture;
        // Buttons for the pause menu
        private Button resumeButton;
        private Button quitButton;
        #endregion

        #region Properties
        /// <summary>
        /// The texture for the background of the pause menu
        /// </summary>
        public Texture2D Background
        {
            get { return background; }
            set { background = value; }
        }

        /// <summary>
        /// The texture for the title displayed during the pause menu
        /// </summary>
        public Texture2D Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// The texture for the pause button
        /// </summary>
        public Texture2D PauseButtonTexture
        {
            get { return pauseButtonTexture; }
            set { pauseButtonTexture = value; }
        }

        /// <summary>
        /// The texture for the quit button in the pause menu
        /// </summary>
        public Texture2D QuitButtonTexture
        {
            get { return pauseButtonTexture; }
            set { pauseButtonTexture = value; }
        }

        /// <summary>
        /// The button to resume the game
        /// </summary>
        public Button ResumeButton
        {
            get { return resumeButton; }
            set { resumeButton = value; }
        }

        /// <summary>
        /// The button to quit the game
        /// </summary>
        public Button QuitButton
        {
            get { return quitButton; }
            set { quitButton = value; }
        }
        #endregion

        #region Constructor
        public GamePause(Engine theGame)
            : base(theGame)
        {
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //load textures
            background = AssetManager.Instance.FindTexture("pause_menu_background");
            title = AssetManager.Instance.FindTexture("pause_menu_title");
            pauseButtonTexture = AssetManager.Instance.FindTexture("pause_menu_resume_button_texture");
            quitButtonTexture = AssetManager.Instance.FindTexture("pause_menu_quit_button_texture");
            //create buttons
            resumeButton = new Button(new Rectangle(220, 300, 400, 400), pauseButtonTexture);
            quitButton = new Button(new Rectangle(660, 300, 400, 400), quitButtonTexture);
        }

        public override void Update(GameTime gameTime)
        {
            //check which buttons are clicked and update game FSM
            if (gameEngine.CurrentMouseState.LeftButton == ButtonState.Pressed && gameEngine.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                //resume button
                if (resumeButton.ObjectPosition.Contains(new Point(gameEngine.CurrentMouseState.X, gameEngine.CurrentMouseState.Y)))
                {
                    gameEngine.CurrentState = Gamestate.Gameplay;
                    gameEngine.PrevState = Gamestate.GamePause;
                }
                //quit button
                if (quitButton.ObjectPosition.Contains(new Point(gameEngine.CurrentMouseState.X, gameEngine.CurrentMouseState.Y)))
                {
                    gameEngine.GameplayState = GameplayState.LevelOne;

                    gameEngine.CurrentState = Gamestate.MainMenu;
                    gameEngine.PrevState = Gamestate.GameOver;//set to gameover because we skipped it

                    gameEngine.ResetMusic = true;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw background
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            //draw title
            spriteBatch.Draw(title, new Vector2(0, 50), Color.White);
            //draw buttons
            resumeButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }
        #endregion
    }
}
