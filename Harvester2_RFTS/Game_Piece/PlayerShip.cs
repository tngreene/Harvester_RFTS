/*
 * public abstract class Player : Ship
 * @author Derrick Hunt / Peter O'Neal
 * 
 * Version:
 *      $1.1.3$
 * 
 * Revisions:
 *      1.1.0: (Not sure?)
 *          - Added a ProcessInput method
 *      1.1.3: (Freddy Garcia)
 *          - Changed the cooldown timer to a double
 *          - Changed the UpdatePlayerBullets to check for bullets going out through the left and right sides of the screen
 *      1.2.0 Derrick - added stuff for powerup generating
 *      
 *      1.5.0 Derrick - added code for firing off the black hole power up
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
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Harvester
{
    /// <summary>
    /// The ship that will be controllable by the player in the game
    /// </summary>
    public class PlayerShip :  ShipInfo
    {
        #region Attributes
        private int fireRate; //the speed that the main gun can be fired (shots/60frames)
        private int fireCounter; //used to determine when the ship can fire
        private SoundEffect fireBullet;
        private Harvester harvester; //the ship's Harvester weapon
        private SoundEffect fireHarvester;
        private Random rand = new Random();

        //stuff for powerups
        private PowerUp powerUp; //the ship's power up
        private bool generated; //if we have generated a powerup (reset to false when powerup is fired)
        private SoundEffect doubleDamageBulletSound;
        #endregion

        #region Properties
        public int FireRate { get { return fireRate; } set { fireRate = value; } }
        public Harvester Harvester { get { return harvester; } set { harvester = value; } }
        public PowerUp PowerUp { get { return powerUp; } set { powerUp = value; } }
        #endregion

        #region Constr
        /// <summary>
        /// Creates a new player which will have position/size,
        /// texture, speed, health, and armor determined by parameters.
        /// </summary>
        /// <param name="objPos">The position/size of the player</param>
        /// <param name="objTexture">The texture of the player</param>
        /// <param name="objSpeed">The speed of the player</param>
        /// <param name="objHealth">The health of the player</param>
        /// <param name="bulletTexture">the texture of the player bullet</param>
        /// <param name="damage">damage of bullets</param>
        /// <param name="h">the harvester object</param>
        /// <param name="gameEngine">reference to the engine for input</param>
        public PlayerShip(Rectangle objPos, Texture2D objTexture, double objSpeed, double objHealth, Texture2D bulletTexture, double damage,
            Harvester h)
            : base(objPos, objTexture, objSpeed, objHealth, bulletTexture, damage)
        {
            //set defaults for variables
            fireRate = 10;
            fireCounter = 0;

            //link up to harvester obj
            harvester = h;
            
            //set sounds
            fireBullet = AssetMgr.Inst().SoundDic["fire_bullet_normal"];
            fireHarvester = AssetMgr.Inst().SoundDic["science_fiction_phaser_gun"];
            doubleDamageBulletSound = AssetMgr.Inst().SoundDic["fire_bullet_double_damage"];
        }
        #endregion

        #region Methods
        /// <summary>
        /// Take a hit (do nothing if we are invincible)
        /// </summary>
        /// <param name="dmg">damage to take</param>
        public override void TakeHit(int dmg)
        {
            if (this.PowerUp != null && this.PowerUp is InvincibilityPowerUp && this.PowerUp.IsActive)
            {
                //do nothing
            }
            else
            {
                base.TakeHit(dmg);
            }
        }


        /// <summary>
        /// Fires the main weapon of the player
        /// </summary>
        public override void Fire()
        {
            //increment fireCounter
            fireCounter++;

            //check how many frames has passed and compare to fire rate
            if (fireCounter == fireRate)
            {
                //create two bullets and add to list
                PlayerBullet pBullet = new PlayerBullet(new Rectangle((int) this.ObjectXPos+5, (int) this.ObjectYPos, 10, 20), this.BulletTexture, 20);
                PlayerBullet pBullet2 = new PlayerBullet(new Rectangle((int)this.ObjectXPos + 35, (int)this.ObjectYPos, 10, 20), this.BulletTexture, 20);
                this.Bullets.Add(pBullet);
                //gameEngine.CurrentLevel.BulletsFired += 1;
                this.Bullets.Add(pBullet2);
                //gameEngine.CurrentLevel.BulletsFired += 1;

                //change sound if powerup activated
                if (powerUp != null && powerUp.IsActive && powerUp.HasFired && powerUp is DamagePowerUp)
                {
                    doubleDamageBulletSound.Play(0.6f, 0, 0);
                }
                else
                {
                    //play normal sound
                    fireBullet.Play(0.4f, 0.0f, 0.0f);
                }
                //reset fireCounter
                fireCounter = 0;
            }
        }

        /// <summary>
        /// Fires the harvester
        /// </summary>
        public void Harvest()
        {
            //can only fire if cooled down
            if (harvester.CurrentTime == harvester.MaxTime)
            {
                harvester.IsActive = true;

                //play sound
                fireHarvester.Play(0.5f, 0.0f, 0.0f);
            }
        }
        /// <summary>
        /// Fires powerup
        /// </summary>
        public void FirePowerUp()
        {
            //fire the powerup if not already active, reset variables
            if (powerUp != null && powerUp.IsActive == false && powerUp.HasFired == false)
            {
                powerUp.Fire();
            }
        }

        /// <summary>
        /// Updates ship position and bullets
        /// </summary>
        public override void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            //used to animate taking damage
            base.Update(gameTime,clientRectangle);

            #region Ship power up code
            //generate a power up if we pick one up
            if (this.HasPowerUp == true && generated == false)
                GeneratePowerUp();

            //reset bools if we have no powerup
            if (powerUp == null)
            {
                this.HasPowerUp = false;
                generated = false;
            }

            //delete our powerup if it is no longer active and has been fired
            if (powerUp != null && (powerUp.IsActive == false && powerUp.HasFired == true))
            {
                powerUp = null;
            }

            //update our powerup if it is active
            if (powerUp!= null && powerUp.IsActive)
            {
                powerUp.Update(gameTime, clientRectangle);
            }

            //make the black hole move with ship if not fired
            if (powerUp is BlackholePowerUp && powerUp.HasFired == false)
            {
                powerUp.ObjectXPos = this.ObjectXPos - 75;
                powerUp.ObjectYPos = this.ObjectYPos - 75;
            }
            #endregion

            //sort through list and move bullets
            for (int i = 0; i < this.Bullets.Count; i++)
            {
                Bullets[i].ObjectYPos -= (int)Bullets[i].Speed;
                //remove bullets if they go off the screen
                if (Bullets[i].ObjectYPos < -10)
                    Bullets.Remove(Bullets[i]);
            }

            //move ship from kbstate
            #region Keyboard movement code
            //For the current keyboard state
            KeyboardState kbState = Keyboard.GetState();

            if (kbState.GetPressedKeys().Length != 0)
            {
                int stophere = 0;
            }

            //move up if just W pressed
            if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D))
            {
                this.ObjectYPos -= (int)this.Speed;
            }
            //move left if just A pressed
            if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.W) && kbState.IsKeyUp(Keys.S))
            {
                this.ObjectXPos -= (int)this.Speed;
            }
            //move right if just D pressed
            if (kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.W) && kbState.IsKeyUp(Keys.S))
            {
                this.ObjectXPos += (int)this.Speed;
            }
            //move down if just S pressed
            if (kbState.IsKeyDown(Keys.S) && kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D))
            {
                this.ObjectYPos += (int)this.Speed;
            }
            //move up and left is W and A pressed
            if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D))
            {
                this.ObjectYPos -= (int)this.Speed - 1;
                this.ObjectXPos -= (int)this.Speed - 1;
            }
            //move up and left is W and D pressed
            if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.A))
            {
                this.ObjectYPos -= (int)this.Speed - 1;
                this.ObjectXPos += (int)this.Speed - 1;
            }
            //move down and left is S and A pressed
            if (kbState.IsKeyDown(Keys.S) && kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D))
            {
                this.ObjectYPos += (int)this.Speed - 1;
                this.ObjectXPos -= (int)this.Speed - 1;
            }
            //move up and left is S and D pressed
            if (kbState.IsKeyDown(Keys.S) && kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.A))
            {
                this.ObjectYPos += (int)this.Speed - 1;
                this.ObjectXPos += (int)this.Speed - 1;
            }

            //make sure we stay on screen
            if (this.ObjectXPos < 0)
            {
                this.ObjectXPos += (int)this.Speed;
            }
            if (this.ObjectYPos < clientRectangle.Height - 450)
            {
                this.ObjectYPos += (int)this.Speed;
            }

            if (this.ObjectXPos > clientRectangle.Width - 50)
            {
                this.ObjectXPos -= (int)this.Speed;
            }
            if (this.ObjectYPos > clientRectangle.Height - 50)
            {
                this.ObjectYPos -= (int)this.Speed;
            }

            //dont let the player move past the upper third of screen
            //eventually put this logic in the movement code to get rid of bouncing
            if (this.ObjectYPos < (clientRectangle.Height / 3))
                this.ObjectYPos = clientRectangle.Height / 3;
            #endregion

            //update ship firing
            #region Firing code
            MouseState mouseState = Mouse.GetState();
            
            //fire bullets if harvester not active and LM clicked
            if ((kbState.IsKeyDown(Keys.Enter) || mouseState.LeftButton == ButtonState.Pressed) && this.Harvester.IsActive == false)
            {
                this.Fire();
            }
            //fires the harvester if RM clicked
            else if ((kbState.IsKeyDown(Keys.RightShift) || mouseState.RightButton == ButtonState.Pressed))
            {
                this.Harvest();
            }
            //fire the powerup if we have one
            else if(mouseState.MiddleButton == ButtonState.Pressed && powerUp != null || kbState.IsKeyDown(Keys.Q))
            {
                this.FirePowerUp(); 
                //hasStartedTimer = true;
            }
            #endregion
        }

        /// <summary>
        /// randomly gives the player one of the powerups
        /// </summary>
        private void GeneratePowerUp()
        {
            if (generated == false)
            {
                int temp = rand.Next(3);
                
                switch (temp)
                {
                    case 0:
                        this.powerUp = new DamagePowerUp(this.ObjectPosition, this.ObjectTexture, 0, this);
                        break;
                    case 1:
                        this.powerUp = new InvincibilityPowerUp(new Rectangle(this.ObjectXPos - 10, this.ObjectYPos - 10, this.ObjectWidth + 20, this.ObjectHeight+ 20), AssetMgr.Inst().TextureDic["invincibility_shield"], 0,this);
                        
                        break;
                    case 2:
                        this.powerUp = new BlackholePowerUp(new Rectangle(this.ObjectXPos-100, this.ObjectYPos-100, 200, 200), AssetMgr.Inst().TextureDic["blackhole_spritesheet"], 5);
                        break;
                }

            }
            generated = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw the black hole power up if its activated (beneath ship)
            if (powerUp != null && powerUp.IsActive && powerUp is BlackholePowerUp)
            {
                powerUp.Draw(spriteBatch);
            }

            //draw the harvester
            harvester.DrawClaw(spriteBatch, this);

            //draw bullets
            for (int i = 0; i < this.Bullets.Count; i++)
            {
                if (this.PowerUp != null && (this.PowerUp is DamagePowerUp && this.PowerUp.IsActive))
                    this.Bullets[i].TintColor = Color.Black;
                else
                    this.Bullets[i].TintColor = Color.White;

                this.Bullets[i].Draw(spriteBatch);
            }

            //draw ship
            base.Draw(spriteBatch);

            //draw the shield power up if its activated (on top of ship)
            if (powerUp != null && powerUp.IsActive && powerUp is InvincibilityPowerUp)
            {
                powerUp.Draw(spriteBatch);
            }
        }

        public void Reset()
        {
            currentHealth = maxHealth;
            this.ObjectPosition = new Rectangle((1280-25) / 2, 695 - 90, 50, 50);

            hasPowerUp = false;
            powerUp = null;
            generated = false;

            this.Harvester.CurrentTime = this.Harvester.MaxTime;
            this.Bullets.Clear();
        }

        #endregion
    }
}
