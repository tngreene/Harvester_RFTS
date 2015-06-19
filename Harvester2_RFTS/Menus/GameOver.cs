/*
 * public class GameOver
 * @author Zach Whitman/Derrick Hunt
 * 
 * Version:
 *      $1.5.0$
 * 
 * Revisions:
 *      1.5 (Derrick) - added graphics, made the menu work 100% properly
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
    /// Menu that will be displayed when the player has lost the game
    /// </summary>
    class GameOver : Menu
    {
        #region Attributes
        //textures
        private Texture2D background;
        private Texture2D title;
        private Texture2D restartButtonTexture;
        private Texture2D quitButtonTexture;
        private SpriteFont accuracyFont;
        private SpriteFont scoreFont;
        private double accuracy;
        private int score;

        // Buttons for the pause menu
        private Button restartButton;
        private Button quitButton;
        #endregion

        #region Properties
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public double Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }
        /// <summary>
        ///  The texture for the background of the game over screen
        /// </summary>
        public Texture2D Background
        {
            get { return background; }
            set { background = value; }
        }

        /// <summary>
        /// The texture for the title (Game Over)
        /// </summary>
        public Texture2D Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// The texture for the restart button
        /// </summary>
        public Texture2D RestartButtonTexture
        {
            get { return restartButtonTexture; }
            set { restartButtonTexture = value; }
        }

        /// <summary>
        /// The texture for the quit button
        /// </summary>
        public Texture2D QuitButtonTexture
        {
            get { return quitButtonTexture; }
            set { quitButtonTexture = value; }
        }

        /// <summary>
        /// The button to restart the game
        /// </summary>
        public Button RestartButton
        {
            get { return restartButton; }
            set { restartButton = value; }
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
        /// <summary>
        /// The game over menu is created and the screen is displayed.
        /// </summary>
        /// <param name="currentPlayer"></param>
        public GameOver(Engine theGame)
            : base(theGame)
        {
            accuracy = 0;
            score = 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //load textures
            background = AssetManager.Instance.FindTexture("game_over_menu_background");
            title = AssetManager.Instance.FindTexture("game_over_menu_title");
            restartButtonTexture = AssetManager.Instance.FindTexture("game_over_menu_restart_button_texture");
            quitButtonTexture = AssetManager.Instance.FindTexture("game_over_menu_quit_button_texture");
            accuracyFont = AssetManager.Instance.FindSpriteFont("AccuracyFont");
            scoreFont = AssetManager.Instance.FindSpriteFont("ScoreFont");

            //create buttons
            restartButton = new Button(new Rectangle(220, 300, 400, 400), restartButtonTexture);
            quitButton = new Button(new Rectangle(660, 300, 400, 400), quitButtonTexture);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //check for clicks and change game state accordingly
            if (gameEngine.CurrentMouseState.LeftButton == ButtonState.Pressed && gameEngine.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                //restart button
                if (restartButton.ObjectPosition.Contains(new Point(gameEngine.CurrentMouseState.X, gameEngine.CurrentMouseState.Y)))
                {
                    gameEngine.GameplayState = GameplayState.LevelOne;

                    gameEngine.CurrentState = Gamestate.Gameplay;
                    gameEngine.PrevState = Gamestate.GameOver;

                    gameEngine.ResetMusic = true;
                }
                //quit button
                if (quitButton.ObjectPosition.Contains(new Point(gameEngine.CurrentMouseState.X, gameEngine.CurrentMouseState.Y)))
                {
                    gameEngine.GameplayState = GameplayState.LevelOne;

                    gameEngine.CurrentState = Gamestate.MainMenu;
                    gameEngine.PrevState = Gamestate.GameOver;

                    gameEngine.ResetMusic = true;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw background
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            //draw title
            spriteBatch.Draw(title, new Vector2(0, 50), Color.White);
            //draw buttons
            restartButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
            // Draw Accuracy and Score
            string accuracyString = "Accuracy = " + accuracy + "%";
            string scoreString = "Total Score = " + gameEngine.Player.Score;
            spriteBatch.DrawString(scoreFont, scoreString, new Vector2(225, 250), Color.White);
            spriteBatch.DrawString(accuracyFont, accuracyString, new Vector2(660, 250), Color.White);
        }
        #endregion
    }
}
