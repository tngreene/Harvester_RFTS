/*
 * public abstract class Level : Microsoft.Xna.Framework.DrawableGameComponent
 * @author Freddy Garcia/Peter O'Neal/Derrick Hunt
 * 
 * Version:
 *      $1.5.3$
 * 
 * Revisions:
 *      1.0.1: (Derrick Hunt)
 *          - Made the class much more abstract to avoid doing more work in child classes
 *      1.0.2: (Peter O'Neal / Derrick Hunt)
 *          - Added ship and movement
 *      1.3.3: (Derrick Hunt)
 *          - More abstract! Also added UpdateBackground, DrawBackground, UpdateHUD and DrawHUD methods in order to handle the background movement
 *      1.4.3: (Peter O'Neal)
 *          - Added collision detection method for the enemies
 *      1.5.3: (Freddy Garcia)
 *          - Removed UpdateHUD from level class and moved it into the HealthBar class
 *      1.6.3: (Peter O'Neal)
 *          - Added score and made enemies spawn after 5 seconds
 *      1.6.4: (Peter O'Neal)
 *          - Added enemy AI
 *      1.6.5: (Peter O'Neal)    
 *          - Added Kamizake ships and collision detection
 *      1.6.6:(Peter O'Neal)
 *          - Fixed how the kamikazes spawn and move
 *      1.6.7:(Peter O'Neal)
 *          - Made enemies shoot and also tweaked the Kamikaze movement
 *      1.6.8:(Peter O'Neal)
 *          - Made cursor a little different and made restrictions on player movement
 *      1.6.9:(Freddy Garcia)
 *          - Claw is no longer exploitable
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
    /// Class for generating all the levels. Basic level structure is created here!
    /// </summary>
    public abstract class Level : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Attributes
        //the game engine
        protected Engine gameEngine;
        //the score on the level
        protected int levelScore;
        // Array of the background images to scroll
        protected Background[] backgrounds;//the current backgrounds
        //player ship stufff
        protected int h_framesElapsed = 0;//used for harvester animation
        protected Texture2D crosshairTexture;
        protected Vector2 cursorPos;
        //list of enemies in the level (should be loaded with tool)
        protected List<Enemy> enemies = new List<Enemy>();
        //enemy fighter textures
        protected Texture2D enemyShip;
        protected Texture2D enemyBullet;
        //enemy bomber texture and sound
        protected Texture2D bombline;
        protected Texture2D bomber;
        protected Texture2D bomb;
        private SoundEffect bombExplosionSound;
        //enemy kamikaze texture
        protected Texture2D kamikaze;
        //random generator
        Random rand = new Random();
        //sprite fonts for HUD
        protected SpriteFont font1;
        //HUD textures and objects
        protected Texture2D barTexture;
        protected Texture2D barOutlineTexture;
        protected Texture2D powerUpOutline;
        protected List<Texture2D> powerUps;
        protected LevelHUD levelHUD;
        //Put power up icons here
        protected List<PowerUp> powerList = new List<PowerUp>();
        protected Texture2D powerUpIcon;
        //Explosion list and texture
        private List<Explosion> explosions;
        private Texture2D explosion;
        protected int e_framesElapsed = 0; //used to animate explosions
        protected SoundEffect explosionSound;

        // Number of Enemies per Level
        protected int maxEnemies;
        //Total killed
        protected int totalKilled;

        //number of enemies killed
        protected int enemiesLeft;
        //Boss mode activated
        protected bool bossMode;
        //Difficulty setting
        protected SpawnType difficulty;

        // Accuracy
        protected int bulletsFired;
        protected int bulletsHit;
        protected double accuracy;
        #endregion

        #region Properties
        public double Accuracy { get { return accuracy; } set { accuracy = value; } }
        public int BulletsFired { get { return bulletsFired; } set { bulletsFired = value; } }
        public int MaxEnemies { get { return maxEnemies; } set { maxEnemies = value; } }
        public List<Enemy> Enemies { get { return enemies; } set { enemies = value; } }
        public int LevelScore { get { return levelScore; } set { levelScore = value; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new level in the game
        /// </summary>
        /// <param name="theGame">teh game engine</param>
        public Level(Engine theGame)
            :base(theGame)
        {
            enemiesLeft = enemies.Count;
            totalKilled = 0;
            gameEngine = theGame;
            
            //instantiate array of backgrounds
            //To fix the problem I change the array back to 4 and commented out the other parts. When switching from levels fill the array diffrently, this
            //can be done with a simple if
            backgrounds = new Background[4];
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes all necessary values to their defaults before the level begins
        /// to run.
        /// </summary>
        public override void Initialize()
        {  
            base.Initialize();
        }

        /// <summary>
        /// Loads all the content for this level
        /// </summary>
        protected override void LoadContent()
        {
            // Set the accuracy counters
            bulletsFired = 0;
            bulletsHit = 0;

            //load ship images
            enemyShip = AssetManager.Instance.FindTexture("enemy_fighter");
            kamikaze = AssetManager.Instance.FindTexture("kamikaze");
            bomber = AssetManager.Instance.FindTexture("bomber");

            // Load the generic power up icon
            powerUpIcon = AssetManager.Instance.FindTexture("powerup_icon");

            enemyBullet = AssetManager.Instance.FindTexture("enemy_bullet");
            bomb = AssetManager.Instance.FindTexture("bomb");

            //bomb animation texture and sound
            bombline = AssetManager.Instance.FindTexture("bombline"); //for animating explosions
            bombExplosionSound = AssetManager.Instance.FindSound("bomb_explosion");

            //crosshair image
            crosshairTexture = AssetManager.Instance.FindTexture("cross_hair");

            //load all background images in children

            //load the HUD for the level
            barTexture = AssetManager.Instance.FindTexture("bar");
            barOutlineTexture = AssetManager.Instance.FindTexture("bar_border");
            powerUpOutline = AssetManager.Instance.FindTexture("power_up_border");
            font1 = AssetManager.Instance.FindSpriteFont("InGameFont");
            levelHUD = new LevelHUD(gameEngine.Player.PlayerShip, barTexture, barOutlineTexture, powerUpOutline, font1, this);

            //load explosion texture and list
            explosions = new List<Explosion>();
            explosion = AssetManager.Instance.FindTexture("explosion_1");
            explosionSound = AssetManager.Instance.FindSound("ship_explosion");
        }

        /// <summary>
        /// Updates the game, making sure the background remains scrolling and the player and enemies are 
        /// responding / acting correctly.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(totalKilled.ToString());
            
            //update backgrounds
            UpdateBackground(backgrounds);

            //update the player and player bullets, moving and firing
            gameEngine.Player.PlayerShip.Update();

            //move crosshair with player
            #region Crosshair movement code
            cursorPos = new Vector2((int)gameEngine.Player.PlayerShip.ObjectXPos, (int)gameEngine.Player.PlayerShip.ObjectYPos - 420);
            while (cursorPos.Y <= 0)
            {
                cursorPos.Y = 10;
            }
            #endregion

            #region Spawns enemies
            if (enemiesLeft == 0)
            {
                if (totalKilled < maxEnemies)
                {
                    //Console.WriteLine(enemies.Count.ToString());
                    enemies.AddRange(SpawnManager.Instance.SpawnARandomWave(1, difficulty, gameEngine.Player.PlayerShip));
                    //Console.WriteLine(enemies.Count.ToString());

                    enemies.Add(SpawnManager.Instance.SpawnSingle(90, 80, 15, gameEngine.Player.PlayerShip));
                    enemies.Add(SpawnManager.Instance.SpawnSingle(90, 80, 15, gameEngine.Player.PlayerShip));
                    enemies.Add(SpawnManager.Instance.SpawnSingle(90, 80, 15, gameEngine.Player.PlayerShip));

                    enemiesLeft = enemies.Count;
                }
            }
            #endregion
            
            //update enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update();
            }

            //make the bombs animate
            #region Bomb Animating Code
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].ShipType == ShipType.bomber)
                {
                    //cast to bomber
                    Bomber temp = (Bomber)enemies[i];

                    if (temp.Bomb.LockOn == true)
                    {
                        //make the bomb stretch across screen
                        temp.Bomb.ObjectTexture = bombline;
                        temp.Bomb.ObjectWidth += 40;
                        temp.Bomb.ObjectXPos -= 20;

                        //play sound once on detonation
                        if (temp.SoundPlayed == false)
                        {
                            bombExplosionSound.Play(0.5f, 0f, 0f);
                            temp.SoundPlayed = true;
                        }

                        //delete it once it hits the sides (this only happens if the player dodges)
                        if (temp.Bomb.ObjectXPos < 0 && temp.Bomb.ObjectXPos + temp.Bomb.ObjectWidth > GraphicsDevice.Viewport.Width + 50)
                        {
                            temp.Bomb.IsActive = false;
                        }
                    }
                }
            }
            #endregion

            //add explosions to screen and delete enemies from screen
            #region Add Explosions/Delete Enemies Code
            for (int i = 0; i < enemies.Count; i++)
            {
                // Ship explodes when it runs out of health
                if (enemies[i].IsActive == false && enemies[i].HasExploded == false)
                {
                    //drop a powerup before we remove the enemy
                    if (enemies[i].HasPowerUp == true)
                    {
                        //create a new generic powerup
                        GenericPowerUp p = new GenericPowerUp(new Rectangle(enemies[i].ObjectXPos+8, enemies[i].ObjectYPos+8, 35, 35), powerUpIcon, 5);
                        //add to powerup list
                        powerList.Add(p);
                    }

                    //add a new explosion at enemy pos
                    explosions.Add(new Explosion(enemies[i].ObjectPosition, explosion));
                    explosionSound.Play(0.5f, 0f, 0f);
                    //once we have finished exploding
                    enemies[i].HasExploded = true;
                    //add score
                    levelScore += enemies[i].Score;
                }

                // Remove enemy from screen once its bullets are gone
                if (enemies[i].IsActive == false && enemies[i].Bullets.Count == 0)
                {
                    //remove enemy
                    enemies.Remove(enemies[i]);
                    totalKilled++;
                    enemiesLeft--;
                }
            }
            #endregion

            //make the black hole suck up people!
            #region Blackhole PowerUp Code
            if (gameEngine.Player.PlayerShip.PowerUp is BlackholePowerUp && gameEngine.Player.PlayerShip.PowerUp.IsActive)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    BlackholePowerUp temp = (BlackholePowerUp)gameEngine.Player.PlayerShip.PowerUp;
                    if (enemies[i].ObjectPosition.Intersects(temp.Center))
                    {
                        if (enemies[i] is ImperialBoss)
                        {
                            enemies[i].TakeHit(1);
                        }
                        else
                        {
                            enemies[i].TakeHit(100);
                        }
                    }

                    //move them closer to the hole once in position
                    if (gameEngine.Player.PlayerShip.PowerUp.ObjectYPos <= 75)
                    {
                        int suction = 20;
                        if (enemies[i].ObjectXPos + 25 != gameEngine.Player.PlayerShip.PowerUp.ObjectXPos + 100 && enemies[i].ObjectYPos + 25 != gameEngine.Player.PlayerShip.PowerUp.ObjectYPos + 100)
                        {
                            if (enemies[i].ObjectXPos + 25 > gameEngine.Player.PlayerShip.PowerUp.ObjectXPos + 100)
                            {
                                enemies[i].ObjectXPos -= suction;
                            }
                            if (enemies[i].ObjectXPos + 25 < gameEngine.Player.PlayerShip.PowerUp.ObjectXPos + 100)
                            {
                                enemies[i].ObjectXPos += suction;
                            }
                            if (enemies[i].ObjectYPos + 25 > gameEngine.Player.PlayerShip.PowerUp.ObjectYPos + 100)
                            {
                                enemies[i].ObjectYPos -= suction;
                            }
                            if (enemies[i].ObjectYPos + 25 < gameEngine.Player.PlayerShip.PowerUp.ObjectYPos + 100)
                            {
                                enemies[i].ObjectYPos += suction;
                            }
                        }
                    }
                }
            }
            #endregion

            //animate explosions
            #region Explosion Animating Code
            e_framesElapsed++;
            if (e_framesElapsed == 5)
            {
                e_framesElapsed = 0;//reset the counter

                for (int i = 0; i < explosions.Count; i++)
                {
                    explosions[i].Frame++;//update frame

                    //remove if we go past last frame
                    if (explosions[i].Frame > 5)
                    {
                        explosions.Remove(explosions[i]);
                    }
                }

            }
            #endregion

            //update the dropped powerups
            for (int i = 0; i < powerList.Count; i++)
            {
                powerList[i].Update();
            }

            //animate the harvester
            #region Harvester Animating Code5
            if (gameEngine.Player.PlayerShip.Harvester.IsActive)
            {
                h_framesElapsed += 1;
                if (h_framesElapsed == 3)
                {
                    h_framesElapsed = 0;
                    gameEngine.Player.PlayerShip.Harvester.Frame++;
                    
                    //reset the harvester
                    if (gameEngine.Player.PlayerShip.Harvester.Frame > 6)
                    {
                        gameEngine.Player.PlayerShip.Harvester.Frame = 0;
                        gameEngine.Player.PlayerShip.Harvester.IsActive = false;
                        gameEngine.Player.PlayerShip.Harvester.CurrentTime = 0;
                    }
                }
            }
            #endregion

            //checks all the potential collisions
            CheckCollisions();

            //update HUD
            levelHUD.Update();

            //check for pause
            if (gameEngine.KbState.IsKeyDown(Keys.Escape))
            {
                gameEngine.CurrentState = Gamestate.GamePause;
                gameEngine.PrevState = Gamestate.Gameplay;
            }
 
            //end game if player dies
            if (gameEngine.Player.IsDead())
            {
                gameEngine.CurrentState = Gamestate.GameOver;
                gameEngine.PrevState = Gamestate.Gameplay;

                //add to player's score
                gameEngine.Player.Score += levelScore;
            }

            //write the number of enemies in the console
            Console.WriteLine(enemies.Count.ToString());


            //accuracy counting
            if (bulletsFired != 0 && bulletsHit != 0)
            {
                accuracy = (double)bulletsHit / (double)bulletsFired;// *100;
            }

            //update the game state machine if we beat the level
            if (totalKilled >= maxEnemies && enemies.Count == 0)
            {
                gameEngine.CurrentState = Gamestate.LevelComplete;
                gameEngine.PrevState = Gamestate.Gameplay;

                //reset playership each level
                gameEngine.Player.PlayerShip.Reset();

                //add to player's score
                gameEngine.Player.Score += levelScore;
            }

        }

        /// <summary>
        /// Draws the background and enemies to the screen, 
        /// in addition to any bullets or abilities that are currently being used
        /// </summary>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw the backgrounds
            DrawBackground(backgrounds, gameTime, spriteBatch);

            //draw the powerups that are dropped
            for (int i = 0; i < powerList.Count; i++)
            {
                powerList[i].Draw(spriteBatch);
            }

            //draw the player and bullets and powerups
            gameEngine.Player.PlayerShip.Draw(spriteBatch);

            //draw all the enemies and their bullets
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }
            
            //draw explosions
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(spriteBatch);
            }

            //draw the crosshair
            spriteBatch.Draw(crosshairTexture, cursorPos, Color.White);

            //draw the HUD
            levelHUD.Draw(spriteBatch);
        }
        #endregion

        #region Private/Helper Methods
        /// <summary>
        /// Updates the positions of the background textures.
        /// Called in update
        /// </summary>
        /// <param name="backgrounds">the list of backgrounds to move</param>
        private void UpdateBackground(Background[] backgrounds)
        {
            //scroll backgrounds down vertically
            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgrounds[i].ObjectYPos += 2;
                //reset images for continuous scrolling
                if (backgrounds[i].ObjectYPos >= GraphicsDevice.Viewport.Height * (backgrounds.Length - 1))
                {
                    backgrounds[i].ObjectYPos = -GraphicsDevice.Viewport.Height;
                }
            }
        }

        /// <summary>
        /// Draws the scrolling background in the level
        /// </summary>
        private void DrawBackground(Background[] backgrounds, GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < backgrounds.Length; i++)
                backgrounds[i].Draw(spriteBatch);
        }

        /// <summary>
        /// Checks for collisions of all of the objects in the level
        /// </summary>
        private void CheckCollisions()
        {
            //check if we get shot or move into another ship
            for (int i = 0; i < enemies.Count; i++)
            {
                //if we shot someone
                for (int j = 0; j < gameEngine.Player.PlayerShip.Bullets.Count; j++)
                {
                    if (gameEngine.Player.PlayerShip.Bullets[j].ObjectPosition.Intersects(enemies[i].ObjectPosition) && enemies[i].HasExploded == false)
                    {
                        enemies[i].TakeHit((int)gameEngine.Player.PlayerShip.Damage);
                        gameEngine.Player.PlayerShip.Bullets.Remove(gameEngine.Player.PlayerShip.Bullets[j]);
                        bulletsHit += 1;

                    }
                }

                //check if we collide with someone
                if (gameEngine.Player.PlayerShip.ObjectPosition.Intersects(enemies[i].ObjectPosition) && enemies[i].HasExploded == false)
                {
                    if (enemies[i] is ImperialBoss)
                    {
                        enemies[i].TakeHit(1);
                        gameEngine.Player.PlayerShip.TakeHit(1);
                    }
                    else
                    {
                        enemies[i].TakeHit(100);
                        gameEngine.Player.PlayerShip.TakeHit(50);
                    }
                }

                //check if we are shot
                for (int j = 0; j < enemies[i].Bullets.Count; j++)
                {
                    if (enemies[i].Bullets[j].ObjectPosition.Intersects(gameEngine.Player.PlayerShip.ObjectPosition))
                    {
                        gameEngine.Player.PlayerShip.TakeHit((int)enemies[i].Damage);

                        if (enemies[i] is ImperialBoss)
                        {
                            //dont remove bullets
                        }
                        else
                        {
                            enemies[i].Bullets.Remove(enemies[i].Bullets[j]);
                        }
                    }
                }

                //if we claw someone
                if (gameEngine.Player.PlayerShip.Harvester.Absorb(enemies[i].ObjectPosition, gameEngine.Player.PlayerShip) && enemies[i].HasExploded == false)
                {
                    if (enemies[i] is ImperialBoss)
                    {
                        //cant gain health
                    }
                    else
                    {
                        gameEngine.Player.PlayerShip.CurrentHealth += 100;
                        enemies[i].IsActive = false;
                    }
                }
            }

            //check if we grab a powerup
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].ObjectPosition.Intersects(gameEngine.Player.PlayerShip.ObjectPosition))
                {
                    //bonus points for picking up extra powerups
                    if (gameEngine.Player.PlayerShip.PowerUp != null)
                    {
                        levelScore += 100;
                    }
                    else
                    {
                        //make the player have a power up
                        gameEngine.Player.PlayerShip.HasPowerUp = true;
                    }

                    //remove the power up from the level
                    powerList.Remove(powerList[i]);
                }
            }
        }

        /// <summary>
        /// Resets the level when the game is restarted
        /// </summary>
        public void Reset()
        {

            //reset the ship
            gameEngine.Player.PlayerShip.Reset();

            //reset the enemies
            enemies.Clear();
            totalKilled = 0;
            enemiesLeft = 0;
            //reset level variables
            powerList.Clear();
            levelScore = 0;
            bulletsHit = 0;
            bulletsFired = 0;

        }

        #endregion
    }      
}