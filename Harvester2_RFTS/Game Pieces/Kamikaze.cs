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
    public class Kamikaze : Enemy
    {
        #region Attr
        private bool lockOn;//if the kamikaze has locked on to the player
        private PlayerShip playerShip;
        private int direction; //direction ship is moving, 0 for right 1 for left
        private SoundEffect lockedOnSound;
        #endregion

        #region Properties
        public bool LockOn { get { return lockOn; } set { lockOn = value; } }
        #endregion

        #region Constructor
        public Kamikaze(Rectangle objPos, Texture2D objTexture, double objSpeed, int objHealth, Texture2D bulletTexture, double damage, PlayerShip p)
            : base(objPos, objTexture, objSpeed, objHealth, bulletTexture, damage)
        {
            //set lock
            lockOn = false;

            lockedOnSound = AssetManager.Instance.FindSound("kamikaze_lock");

            //set direction
            direction = rand.Next(2);

            //link ref to player
            playerShip = p;

            score = 150;
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
        /// fire the kamikaze once locked
        /// </summary>
        public override void Fire()
        {
            if (this.ObjectYPos > 0)
            {
                lockOn = true;
                lockedOnSound.Play(0.5f, 0, 0);
            }
        }
  
        //update the kamikaze pos and lock variable
        public override void Update()
        {
            //used to animate taking damage
            base.Update();

            //determine if we should fire
            if (lockOn == false && this.ObjectXPos >= playerShip.ObjectXPos && this.ObjectXPos <= playerShip.ObjectXPos + 20)
            {
                this.Fire();
            }

            //move ship down if locked on
            if (lockOn == true)
            {
                this.TintColor = Color.Yellow;
                this.ObjectYPos += 16;
            }

            //move up the kamikaze on teh screen
            if (this.ObjectYPos <= 150)
            {
                this.ObjectYPos += (int)this.Speed;
            }

            //move ship side to side
            if (lockOn == false)
            {
                if (direction == 0)
                {
                    //move right
                    this.ObjectXPos += (int)this.Speed;
                    //switch direction if we hit the side
                    if (this.ObjectXPos + 50 >= 1280)
                    {
                        direction = 1;
                    }
                }
                 if (direction == 1)
                {
                    //move left
                    this.ObjectXPos -= (int)this.Speed;
                    //switch direction if we hit the side
                    if (this.ObjectXPos <= 0)
                    {
                        direction = 0;
                    }
                }
            }

            //delete ship if offscreen
            if (this.ObjectYPos > 770)
            {
                this.IsActive = false;
            }
        }

        /// <summary>
        /// draws the ship
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                //draw the ship
                base.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
