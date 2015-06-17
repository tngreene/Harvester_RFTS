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
        #region Attributes
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
        private GameplayState state;
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
        public LevelCompleteMenu(Game theGame)
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
            continueButtonTexture = AssetMgr.Inst().TextureDic["continue_button_texture"];
            quitButtonTexture = AssetMgr.Inst().TextureDic["pause_menu_quit_button_texture"];
            quitButton = new Button(new Rectangle(660, 300, 400, 400), quitButtonTexture);
            continueButton = new Button(new Rectangle(220, 300, 400, 400), continueButtonTexture);

            //load textures
            gameCompleteTitle = AssetMgr.Inst().TextureDic["game_complete_title"];
            title = AssetMgr.Inst().TextureDic["level_complete_title"];
            background = AssetMgr.Inst().TextureDic["pause_menu_background"];
            accuracyFont = AssetMgr.Inst().SpriteFontDic["AccuracyFont"];
            scoreFont = AssetMgr.Inst().SpriteFontDic["ScoreFont"];
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            //check which buttons are clicked and update game FSM
            if (mouseState.LeftButton == ButtonState.Pressed && gameRef.PreviousMouseState.LeftButton == ButtonState.Released)
            {
                //continue button
                if (continueButton.ObjectPosition.Contains(new Point(mouseState.X, mouseState.Y)))
                {
                    //move to next level
                    if (gameRef.GameplayState == GameplayState.LevelOne)
                    {
                        gameRef.GameplayState = GameplayState.LevelTwo;

                        gameRef.CurrentState = Gamestate.Gameplay;
                        //gameRef.PrevState = Gamestate.LevelComplete;
                    }
                    //move to next level
                    else if (gameRef.GameplayState == GameplayState.LevelTwo)
                    {
                        gameRef.GameplayState = GameplayState.LevelThree;

                        //gameRef.PrevState = Gamestate.LevelComplete;
                        gameRef.CurrentState = Gamestate.Gameplay;
                    }
                    //end game (beaten)
                    else if (gameRef.GameplayState == GameplayState.LevelThree)
                    {
                        gameRef.GameplayState = GameplayState.LevelOne;

                        gameRef.CurrentState = Gamestate.MainMenu;
                        //gameRef.PrevState = Gamestate.GameOver;
                    }

                    gameRef.ResetMusic = true;
                }
                //quit button
                if (quitButton.ObjectPosition.Contains(new Point(mouseState.X, mouseState.Y)))
                {
                    gameRef.CurrentState = Gamestate.MainMenu;
                    //gameRef.PrevState = Gamestate.GameOver;

                    gameRef.ResetMusic = true;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw background
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            //draw title
            if (state == GameplayState.LevelThree)
                spriteBatch.Draw(gameCompleteTitle, new Vector2(0, 50), Color.White);
            else
                spriteBatch.Draw(title, new Vector2(0, 50), Color.White);

            //draw buttons
            if (state == GameplayState.LevelThree)
            {
                continueButton.Draw(spriteBatch);
            }
            else
            {
                continueButton.Draw(spriteBatch);
                quitButton.Draw(spriteBatch);
            }

            // Draw Accuracy and Score
            string accuracyString = "Accuracy = " + accuracy + "%";
            if (accuracy < 10)
            {
                accuracyString = accuracyString.Substring(0, 4); //Only keeps X.00
            }
            else if (accuracy >= 10)
            {
                accuracyString = accuracyString.Substring(0, 5); //Only keeps XX.00
            }

            string scoreString = "Level Score = " + gameRef.TheLevel.LevelScore;
            string totalScoreString = "Total Score = " + PlayerMgr.Inst().Score;
            spriteBatch.DrawString(scoreFont, scoreString, new Vector2(220, 250), Color.White);
            spriteBatch.DrawString(accuracyFont, accuracyString, new Vector2(660, 250), Color.White);
            spriteBatch.DrawString(scoreFont, totalScoreString, new Vector2(220, 270), Color.White);


        }
        #endregion 

    }
}
