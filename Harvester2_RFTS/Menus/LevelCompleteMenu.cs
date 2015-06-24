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
    class LevelCompleteMenu : Menu
    {
        #region Attr
        //textures
        private Texture2D title;
        private Texture2D gameCompleteTitle;
        private Texture2D continueButtonTexture;
        private Texture2D quitButtonTexture;
        private Texture2D background;
        private SpriteFont accuracyFont;
        private SpriteFont scoreFont;

        //buttons
        private Button continueButton;
        private Button quitButton;

        //variables
        private PlayerShip p;
        private Level l;
        private double accuracy;
        private int totalScore;
        private int score;
        #endregion

        #region Properties
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }
        public double Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }

        public Button ContinueButton
        {
            get { return continueButton; }
            set { continueButton = value; }
        }

        public Button QuitButton
        {
            get { return quitButton; }
            set { quitButton = value; }
        }
        #endregion

        #region Constr
        public LevelCompleteMenu(Engine theGame)
            : base(theGame)
        {
            score = 0;
            accuracy = 0;
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //create buttons
            continueButtonTexture = AssetManager.Instance.FindTexture("continue_button_texture");
            quitButtonTexture = AssetManager.Instance.FindTexture("pause_menu_quit_button_texture");
            quitButton = new Button(new Rectangle(660, 300, 400, 400), quitButtonTexture);
            continueButton = new Button(new Rectangle(220, 300, 400, 400), continueButtonTexture);

            //load textures
            gameCompleteTitle = AssetManager.Instance.FindTexture("game_complete_title");
            title = AssetManager.Instance.FindTexture("level_complete_title");
            background = AssetManager.Instance.FindTexture("pause_menu_background");
            accuracyFont = AssetManager.Instance.FindSpriteFont("AccuracyFont");
            scoreFont = AssetManager.Instance.FindSpriteFont("ScoreFont");
        }

        public override void Update(GameTime gameTime)
        {


            //check which buttons are clicked and update game FSM
            if (gameEngine.CurrentMouseState.LeftButton == ButtonState.Pressed && gameEngine.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                //continue button
                if (continueButton.ObjectPosition.Contains(new Point(gameEngine.CurrentMouseState.X, gameEngine.CurrentMouseState.Y)))
                {
                    //move to next level
                    if (gameEngine.GameplayState == GameplayState.LevelOne)
                    {
                        gameEngine.GameplayState = GameplayState.LevelTwo;

                        gameEngine.CurrentState = Gamestate.Gameplay;
                        gameEngine.PrevState = Gamestate.LevelComplete;
                    }
                    //move to next level
                    else if (gameEngine.GameplayState == GameplayState.LevelTwo)
                    {
                        gameEngine.GameplayState = GameplayState.LevelThree;

                        gameEngine.PrevState = Gamestate.LevelComplete;
                        gameEngine.CurrentState = Gamestate.Gameplay;
                    }
                    //end game (beaten)
                    else if (gameEngine.GameplayState == GameplayState.LevelThree)
                    {
                        gameEngine.GameplayState = GameplayState.LevelOne;

                        gameEngine.CurrentState = Gamestate.MainMenu;
                        gameEngine.PrevState = Gamestate.GameOver;
                    }

                    gameEngine.ResetMusic = true;
                }
                //quit button
                if (quitButton.ObjectPosition.Contains(new Point(gameEngine.CurrentMouseState.X, gameEngine.CurrentMouseState.Y)))
                {
                    gameEngine.CurrentState = Gamestate.MainMenu;
                    gameEngine.PrevState = Gamestate.GameOver;

                    gameEngine.ResetMusic = true;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw background
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            //draw title
            if (gameEngine.GameplayState == GameplayState.LevelThree)
                spriteBatch.Draw(gameCompleteTitle, new Vector2(0, 50), Color.White);
            else
                spriteBatch.Draw(title, new Vector2(0, 50), Color.White);

            //draw buttons
            if (gameEngine.GameplayState == GameplayState.LevelThree)
            {
                continueButton.Draw(spriteBatch);
            }
            else
            {
                continueButton.Draw(spriteBatch);
                quitButton.Draw(spriteBatch);
            }

            // Draw Accuracy and Score
            string accuracyString = String.Format("Accuracy = {0:0.##%}", accuracy);
         

            string scoreString = "Level Score = " + gameEngine.CurrentLevel.LevelScore;
            string totalScoreString = "Total Score = " + gameEngine.Player.Score;
            spriteBatch.DrawString(scoreFont, scoreString, new Vector2(220, 250), Color.White);
            spriteBatch.DrawString(accuracyFont, accuracyString, new Vector2(660, 250), Color.White);
            spriteBatch.DrawString(scoreFont, totalScoreString, new Vector2(220, 270), Color.White);


        }
        #endregion 

    }
}
