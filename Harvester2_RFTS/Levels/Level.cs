/*
 * public abstract class Level : Microsoft.Xna.Framework.DrawableGameComponent
 * @author Peter O'Neal/Derrick Hunt
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
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Harvester
{
    /// <summary>
    /// Represents the game board. Methods and the game change it's properties
    /// </summary>
    public class Level
    {
        #region Attributes
        //the score on the level
        private int levelScore;

        // Array of the background images to scroll
        private Background[] backgrounds;//the current backgrounds
        private Rectangle backgroundRect;//Rectangle that will draw the background, set by client bounds
        
        //player ship stufff
        private int h_framesElapsed = 0;//used for harvester animation
        private Texture2D crosshairTexture;
        private Vector2 cursorPos;
        //list of enemies in the level (should be loaded with tool)
        private List<Enemy> enemies = new List<Enemy>();

        //enemy fighter textures
        private Texture2D enemyShip;
        private Texture2D enemyBullet;
        //enemy bomber texture and sound
        private Texture2D bombline;
        private Texture2D bomber;
        private Texture2D bomb;
        private SoundEffect bombExplosionSound;

        //enemy kamikaze texture
        private Texture2D kamikaze;
        //random generator
        Random rand = new Random();
        //sprite fonts for HUD
        private SpriteFont font1;
        //HUD textures and objects
        private Texture2D barTexture;
        private Texture2D barOutlineTexture;
        private Texture2D powerUpOutline;
        private List<Texture2D> powerUps;
        private LevelHUD levelHUD;

        //Put power up icons here
        private List<PowerUp> powerList = new List<PowerUp>();
        private Texture2D powerUpIcon;

        //Explosion list and texture
        private List<Explosion> explosions;
        private Texture2D explosion;
        private int e_framesElapsed = 0; //used to animate explosions
        private SoundEffect explosionSound;

        private SoundEffectInstance backgroundMusic;

        // Number of Enemies per Level
        private int maxEnemies;
        //Total killed
        private int totalKilled;

        //number of enemies killed
        private int enemiesLeft;
        //Boss mode activated
        private bool bossMode;
        //Difficulty setting
        private SpawnType difficulty;

        // Accuracy
        private int bulletsFired;
        private int bulletsHit;
        private double accuracy;
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
        /// <param name="clientRectangle"> the bounds of the screen used for drawing the background</param>
        public Level(Rectangle clientRectangle)
        {
            enemiesLeft = enemies.Count;
            totalKilled = 0;
            this.backgroundRect = new Rectangle(0, -720, 1280, 720);
            
            //instantiate array of backgrounds
            //To fix the problem I change the array back to 4 and commented out the other parts. When switching from levels fill the array diffrently, this
            //can be done with a simple if
            backgrounds = new Background[4];

            // Set the accuracy counters
            bulletsFired = 0;
            bulletsHit = 0;

            //load ship images
            enemyShip = AssetMgr.Inst().TextureDic["enemy_fighter"];
            kamikaze = AssetMgr.Inst().TextureDic["kamikaze"];
            bomber = AssetMgr.Inst().TextureDic["bomber"];

            // Load the generic power up icon
            powerUpIcon = AssetMgr.Inst().TextureDic["powerup_icon"];

            enemyBullet = AssetMgr.Inst().TextureDic["enemy_bullet"];
            bomb = AssetMgr.Inst().TextureDic["bomb"];

            //bomb animation texture and sound
            bombline = AssetMgr.Inst().TextureDic["bombline"]; //for animating explosions
            bombExplosionSound = AssetMgr.Inst().SoundDic["bomb_explosion"];

            //crosshair image
            crosshairTexture = AssetMgr.Inst().TextureDic["cross_hair"];

            //load all background images in children

            //load the HUD for the level
            barTexture = AssetMgr.Inst().TextureDic["bar"];
            barOutlineTexture = AssetMgr.Inst().TextureDic["bar_border"];
            powerUpOutline = AssetMgr.Inst().TextureDic["power_up_border"];
            font1 = AssetMgr.Inst().SpriteFontDic["InGameFont"];
            levelHUD = new LevelHUD(PlayerMgr.Inst().PlayerShip, barTexture, barOutlineTexture, powerUpOutline, font1, this);

            //load explosion texture and list
            explosions = new List<Explosion>();
            explosion = AssetMgr.Inst().TextureDic["explosion_1"];
            explosionSound = AssetMgr.Inst().SoundDic["ship_explosion"];

            
            //bossTexture = AssetMgr.Inst().TextureDic["imperial_boss"];
        }
        #endregion

        #region Methods
        
        /// <summary>
        /// Updates the game, making sure the background remains scrolling and the player and enemies are 
        /// responding / acting correctly.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="clientRectangle">Provides the bounds of the screen</param>
        public void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            Console.WriteLine(totalKilled.ToString());
            
            //update backgrounds
            UpdateBackground(backgrounds);

            //move crosshair with player
            #region Crosshair movement code
            cursorPos = new Vector2((int)PlayerMgr.Inst().PlayerShip.ObjectXPos, (int)PlayerMgr.Inst().PlayerShip.ObjectYPos - 420);
            /*while (cursorPos.Y <= 0)
            {
                cursorPos.Y = 10;
            }*/
            #endregion

            #region Spawns enemies
            if (enemiesLeft == 0)
            {
                if (totalKilled < maxEnemies)
                {
                    //Console.WriteLine(enemies.Count.ToString());
                    enemies.AddRange(SpawnManager.Instance.SpawnARandomWave(1, difficulty));
                    //Console.WriteLine(enemies.Count.ToString());

                    enemies.Add(SpawnManager.Instance.SpawnSingle(90, 80, 15));
                    enemies.Add(SpawnManager.Instance.SpawnSingle(90, 80, 15));
                    enemies.Add(SpawnManager.Instance.SpawnSingle(90, 80, 15));

                    enemiesLeft = enemies.Count;
                }
            }
            #endregion
            
            //update enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gameTime,clientRectangle);
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
                        if (temp.Bomb.ObjectXPos < 0 && temp.Bomb.ObjectXPos + temp.Bomb.ObjectWidth > clientRectangle.Width + 50)
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
            if (PlayerMgr.Inst().PlayerShip.PowerUp is BlackholePowerUp && PlayerMgr.Inst().PlayerShip.PowerUp.IsActive)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    BlackholePowerUp temp = (BlackholePowerUp)PlayerMgr.Inst().PlayerShip.PowerUp;
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
                    if (PlayerMgr.Inst().PlayerShip.PowerUp.ObjectYPos <= 75)
                    {
                        int suction = 20;
                        if (enemies[i].ObjectXPos + 25 != PlayerMgr.Inst().PlayerShip.PowerUp.ObjectXPos + 100 && enemies[i].ObjectYPos + 25 != PlayerMgr.Inst().PlayerShip.PowerUp.ObjectYPos + 100)
                        {
                            if (enemies[i].ObjectXPos + 25 > PlayerMgr.Inst().PlayerShip.PowerUp.ObjectXPos + 100)
                            {
                                enemies[i].ObjectXPos -= suction;
                            }
                            if (enemies[i].ObjectXPos + 25 < PlayerMgr.Inst().PlayerShip.PowerUp.ObjectXPos + 100)
                            {
                                enemies[i].ObjectXPos += suction;
                            }
                            if (enemies[i].ObjectYPos + 25 > PlayerMgr.Inst().PlayerShip.PowerUp.ObjectYPos + 100)
                            {
                                enemies[i].ObjectYPos -= suction;
                            }
                            if (enemies[i].ObjectYPos + 25 < PlayerMgr.Inst().PlayerShip.PowerUp.ObjectYPos + 100)
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
                powerList[i].Update(gameTime, clientRectangle);
            }

            //animate the harvester
            #region Harvester Animating Code5
            if (PlayerMgr.Inst().PlayerShip.Harvester.IsActive)
            {
                h_framesElapsed += 1;
                if (h_framesElapsed == 3)
                {
                    h_framesElapsed = 0;
                    PlayerMgr.Inst().PlayerShip.Harvester.Frame++;
                    
                    //reset the harvester
                    if (PlayerMgr.Inst().PlayerShip.Harvester.Frame > 6)
                    {
                        PlayerMgr.Inst().PlayerShip.Harvester.Frame = 0;
                        PlayerMgr.Inst().PlayerShip.Harvester.IsActive = false;
                        PlayerMgr.Inst().PlayerShip.Harvester.CurrentTime = 0;
                    }
                }
            }
            #endregion

            //checks all the potential collisions
            CheckCollisions();

            //update HUD
            levelHUD.Update(gameTime,clientRectangle);
            KeyboardState kbState = Keyboard.GetState();
            //check for pause
            if (kbState.IsKeyDown(Keys.Escape))
            {
                //CurrentState = Gamestate.GamePause;
                //PrevState = Gamestate.Gameplay;
            }
 
            //end game if player dies
            if (PlayerMgr.Inst().IsDead())
            {
                //CurrentState = Gamestate.GameOver;
                //PrevState = Gamestate.Gameplay;

                //add to player's score
                PlayerMgr.Inst().Score += levelScore;
            }

            //write the number of enemies in the console
            Console.WriteLine(enemies.Count.ToString());


            //accuracy counting
            if (bulletsFired != 0 && bulletsHit != 0)
            {
                accuracy = (double)bulletsHit / (double)bulletsFired * 100;
            }

            //update the game state machine if we beat the level
            if (totalKilled >= maxEnemies && enemies.Count == 0)
            {
                //CurrentState = Gamestate.LevelComplete;
                //PrevState = Gamestate.Gameplay;

                //reset playership each level
                //PlayerMgr.Inst().PlayerShip.Reset();

                //add to player's score
                PlayerMgr.Inst().Score += levelScore;
            }

            //BOSS Code
            /*if (this.LevelScore < 5000)
            {
                bossSpawned = false;
                boss = null;
            }

            if (boss != null)
            Console.WriteLine(boss.CurrentHealth.ToString());

            //SPAWN BOSS
            if (this.LevelScore >= 5000 && bossSpawned == false)
            {
                boss = new ImperialBoss(new Rectangle(240, -500, 400, 240), bossTexture, 2, 7500, AssetMgr.Inst().TextureDic["imperial_laser"), 1, this);
                this.enemies.Add(boss);
                bossSpawned = true;
            }

            base.Update(gameTime);*/

        }

        /// <summary>
        /// Draws the background and enemies to the screen, 
        /// in addition to any bullets or abilities that are currently being used
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw the backgrounds
            DrawBackground(backgrounds, gameTime, spriteBatch);

            //draw the powerups that are dropped
            for (int i = 0; i < powerList.Count; i++)
            {
                powerList[i].Draw(spriteBatch);
            }

            //draw the player and bullets and powerups
            PlayerMgr.Inst().PlayerShip.Draw(spriteBatch);

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

        /// <summary>
        /// Resets the level when the game is restarted
        /// </summary>
        public void Reset()
        {

            //reset the ship
            PlayerMgr.Inst().PlayerShip.Reset();

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

        //Changes between levels, pass in the level you will be changing to
        public void ChangeLevel(int lvlNumber)
        {
            switch (lvlNumber)
            {
                case 1:
                    backgroundMusic = AssetMgr.Inst().SoundDic["level_one_music"].CreateInstance();
                    backgroundMusic.Play();
                    backgrounds[0] = new Background(new Rectangle(0, 0, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["water_world1"]);
                    backgrounds[1] = new Background(new Rectangle(0, -backgroundRect.Height, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["water_world2"]);
                    backgrounds[2] = new Background(new Rectangle(0, -backgroundRect.Height * 2, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["water_world3"]);
                    backgrounds[3] = new Background(new Rectangle(0, -backgroundRect.Height * 3, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["water_world4"]);
                    break;
                case 2:
                    backgroundMusic = AssetMgr.Inst().SoundDic["level_two_music"].CreateInstance();
                    backgroundMusic.Play();
                    backgrounds[0] = new Background(new Rectangle(0, 0, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["lava_world1"]);
                    backgrounds[1] = new Background(new Rectangle(0, -backgroundRect.Height, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["lava_world2"]);
                    backgrounds[2] = new Background(new Rectangle(0, -backgroundRect.Height * 2, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["lava_world3"]);
                    backgrounds[3] = new Background(new Rectangle(0, -backgroundRect.Height * 3, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["lava_world4"]);
                    break;
                case 3:
                    backgroundMusic = AssetMgr.Inst().SoundDic["level_three_music"].CreateInstance();
                    backgroundMusic.Play();

                    backgrounds[0] = new Background(new Rectangle(0, 0, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["space1"]);
                    backgrounds[1] = new Background(new Rectangle(0, -backgroundRect.Height, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["space2"]);
                    backgrounds[2] = new Background(new Rectangle(0, -backgroundRect.Height * 2, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["space3"]);
                    backgrounds[3] = new Background(new Rectangle(0, -backgroundRect.Height * 3, backgroundRect.Width, backgroundRect.Height), AssetMgr.Inst().TextureDic["space4"]);
                    
                    break;
                default:
                    throw new Exception("Level not found!");
            }
        }
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
                backgrounds[i].ObjectYPos += 2;//2 is the scroll speed
                //reset images for continuous scrolling
                if (backgrounds[i].ObjectYPos >= backgroundRect.Height * (backgrounds.Length - 1))
                {
                    backgrounds[i].ObjectYPos = -backgroundRect.Height;
                }
            }
        }

        /// <summary>
        /// Draws the scrolling background in the level
        /// </summary>
        private void DrawBackground(Background[] backgrounds, GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 4; i++)
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
                for (int j = 0; j < PlayerMgr.Inst().PlayerShip.Bullets.Count; j++)
                {
                    if (PlayerMgr.Inst().PlayerShip.Bullets[j].ObjectPosition.Intersects(enemies[i].ObjectPosition) && enemies[i].HasExploded == false)
                    {
                        enemies[i].TakeHit((int)PlayerMgr.Inst().PlayerShip.Damage);
                        PlayerMgr.Inst().PlayerShip.Bullets.Remove(PlayerMgr.Inst().PlayerShip.Bullets[j]);
                        bulletsHit += 1;

                    }
                }

                //check if we collide with someone
                if (PlayerMgr.Inst().PlayerShip.ObjectPosition.Intersects(enemies[i].ObjectPosition) && enemies[i].HasExploded == false)
                {
                    if (enemies[i] is ImperialBoss)
                    {
                        enemies[i].TakeHit(1);
                        PlayerMgr.Inst().PlayerShip.TakeHit(1);
                    }
                    else
                    {
                        enemies[i].TakeHit(100);
                        PlayerMgr.Inst().PlayerShip.TakeHit(50);
                    }
                }

                //check if we are shot
                for (int j = 0; j < enemies[i].Bullets.Count; j++)
                {
                    if (enemies[i].Bullets[j].ObjectPosition.Intersects(PlayerMgr.Inst().PlayerShip.ObjectPosition))
                    {
                        PlayerMgr.Inst().PlayerShip.TakeHit((int)enemies[i].Damage);

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
                if (PlayerMgr.Inst().PlayerShip.Harvester.Absorb(enemies[i].ObjectPosition, PlayerMgr.Inst().PlayerShip) && enemies[i].HasExploded == false)
                {
                    if (enemies[i] is ImperialBoss)
                    {
                        //cant gain health
                    }
                    else
                    {
                        PlayerMgr.Inst().PlayerShip.CurrentHealth += 100;
                        enemies[i].IsActive = false;
                    }
                }
            }

            //check if we grab a powerup
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].ObjectPosition.Intersects(PlayerMgr.Inst().PlayerShip.ObjectPosition))
                {
                    //bonus points for picking up extra powerups
                    if (PlayerMgr.Inst().PlayerShip.PowerUp != null)
                    {
                        levelScore += 100;
                    }
                    else
                    {
                        //make the player have a power up
                        PlayerMgr.Inst().PlayerShip.HasPowerUp = true;
                    }

                    //remove the power up from the level
                    powerList.Remove(powerList[i]);
                }
            }
        }
        #endregion
    }      
}