/*
 * Represents the Bomber enemies in the game
 * @author Derrick Hunt
 * 
 * @version 1.0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Harvester
{
    public class Bomber : Enemy
    {
        #region Attributes
        private Bomb bomb; //the bomb of the bomber
        private int direction; //direction ship is moving, 0 for right 1 for left
        private PlayerShip playerShip; //ref to playership, needed to lock on
        private bool soundPlayed; //if the detonation sound has played
        private bool hasHitUp = false;
        private bool hasHitSide = false;
        private int timer; //used to shoot the bomb less freq
        private int counter;
        public static int classCounter;
        #endregion

        #region Properties
        public Bomb Bomb { get { return bomb; } }
        public bool SoundPlayed { get { return soundPlayed; } set { soundPlayed = value; } }
        public int Timer { get { return timer; } set { timer = value; } }
        public bool HasHitUp { get { return hasHitUp; } set { hasHitUp = value; } }
        public bool HasHitSide { get { return hasHitSide; } set { hasHitSide = value; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new bomber which will have position/size,
        /// texture, speed, health, and armor determined by parameters.
        /// </summary>
        public Bomber(Rectangle objPos, Texture2D objTexture, double objSpeed, int objHealth, Texture2D bulletTexture, double damage, PlayerShip p)
            : base(objPos, objTexture, objSpeed, objHealth, bulletTexture, damage)
        {
            //set bomb
            bomb = new Bomb(new Rectangle(this.ObjectXPos, this.ObjectYPos, 35, 35), bulletTexture, 5);

            //set direction
            direction = rand.Next(2);

            //set player ref
            playerShip = p;

            //set type
            shipType = ShipType.bomber;

            //set timer
            classCounter++;

            if (classCounter > 2)
                classCounter = 0;

            if (classCounter == 0)
                timer = 120;
            else if (classCounter == 1)
                timer = 289;
            else if (classCounter == 2)
                timer = 416;

            counter = 0;

            //set damage to 30 regardless
            this.Damage = 30;

            //how much it's worth when killed
            score = 200;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Take a hit
        /// </summary>
        /// <param name="dmg">damage to take</param>
        public override void TakeHit(int dmg)
        {
            base.TakeHit(dmg);
        }

        /// <summary>
        /// fires the bomb
        /// </summary>
        public override void Fire()
        {
            //set bomb on fighter
            bomb = new Bomb(new Rectangle(this.ObjectXPos, this.ObjectYPos, 35, 35), bulletTexture, 5);
            bomb.IsActive = true;
            //add the bomb to the bullet list
            this.Bullets.Add(bomb);
            //set again so the bomb will play noise
            soundPlayed = false;
        }

        /// <summary>
        /// updates the bomber's position and the bomb if fired
        /// </summary>
        public override void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            //used to animate taking damage
            base.Update(gameTime,clientRectangle);

            //make them fire
            if (bomb.IsActive == false)
            {
                counter++;
                if (counter == timer)
                {
                    this.Fire();
                    counter = 0;
                }
            }

            //if we cross player make it lock (and subsequently blow up)
            if (this.bomb.ObjectYPos > playerShip.ObjectYPos && this.bomb.ObjectYPos < playerShip.ObjectYPos + 50)
            {
                bomb.LockOn = true;
            }

            //update bomb pos when not locked
            if (bomb.LockOn == false && bomb.IsActive)
            {
                bomb.ObjectYPos += (int)bomb.Speed;
            }

            //remove bomb if off screen
            if (bomb.ObjectYPos > 800)
            {
                bomb.IsActive = false;
            }

            //remove bomb if not active
            if (bomb.IsActive == false)
            {
                this.Bullets.Remove(bomb);
            }

            //move bomber in W formation
            if (this.hasHitSide == false)
            {
                this.ObjectXPos += (int)this.Speed;
                if (this.ObjectXPos >= 1230)
                {
                    this.hasHitSide = true;
                }
            }
            if (this.hasHitSide == true)
            {
                this.ObjectXPos -= (int)this.Speed;
                if (this.ObjectXPos <= 0)
                {
                    this.hasHitSide = false;
                }

            }
            if (this.hasHitUp == false)
            {
                this.ObjectYPos += (int)this.Speed;
                if (this.ObjectYPos >= 190)
                {
                    this.hasHitUp = true;
                }
            }
            if (this.hasHitUp == true)
            {
                this.ObjectYPos -= (int)this.Speed;
                if (this.ObjectYPos <= 0)
                {
                    this.hasHitUp = false;
                }
            }
        }

        /// <summary>
        /// draws the ship and bullets
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw the bomb if we have one
            for (int i = 0; i < this.Bullets.Count; i++)
            {
                this.Bullets[i].Draw(spriteBatch);
            }

            if (isActive)
            {
                //draw the ship
                base.Draw(spriteBatch);
            }
        }
        #endregion
        
    }
}
