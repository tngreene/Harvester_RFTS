/*
 * public abstract class Enemy : Ship
 * @author Derrick Hunt / Peter O'Neal
 * 
 * Version:
 *      $1.5.0$
 * 
 * Revisions:
 *      1.1.0: (Derrick Hunt)
 *          - Added a TakeHit method for the ship to be able to take damage
 *      1.1.1: (Freddy Garcia)
 *          - Removed the bool isActive parameter from the constructor
 *      1.1.2: (Peter O'Neal)
 *          - Added properties and attributes for when enemies hit screen edges
 *      1.1.3 (Peter O'Neal)    
 *          - Did all the enemy bullet stuff
 *      1.5.0 (Derrick)
 *             Merged a lot between the enemy and enemy fighter class cause theyre not the same thing
 *             not sure why we had both doing the same stuff
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
    /// Green enemy ships, basic fighters which can shoot down the ship
    /// </summary>
    public class EnemyFighter : Enemy
    {
        #region Attributes
        private double shotTimer; //time between shots
        private int direction; //direction ship is moving, 0 for right 1 for left
        private int originX; // starting x pos
        private SoundEffect fireBulletSound;
        #endregion

        #region Properties
        //timer for when enemies shoot
        public double ShotTimer { get { return shotTimer; } set { shotTimer = value; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new enemy which will have position/size,
        /// texture, speed, health, and armor determined by parameters.
        /// </summary>
        public EnemyFighter(Rectangle objPos, Texture2D objTexture, double objSpeed, int objHealth, Texture2D bulletTexture, double damage, int st)
            : base(objPos, objTexture, objSpeed, objHealth, bulletTexture, damage)
        {
            //set shot timing
            shotTimer = st;

            //set direction randomly
            direction = rand.Next(2);

            //set origin
            originX = objPos.X;
            originX = objPos.Y;

            this.Speed = 2;

            fireBulletSound = AssetMgr.Inst().SoundDic["fire_bullet_enemy"];

            score = 100;
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
        /// fires the bullets
        /// </summary>
        public override void Fire()
        {
            //only fire if we're alive
            if (this.IsActive == true)
            {
                EnemyBullet ebullet1 = new EnemyBullet(new Rectangle(this.ObjectXPos + 5, this.ObjectYPos, 10, 20), this.BulletTexture, 5);
                EnemyBullet ebullet2 = new EnemyBullet(new Rectangle(this.ObjectXPos + 35, this.ObjectYPos, 10, 20), this.BulletTexture, 5);
                this.Bullets.Add(ebullet1);
                this.Bullets.Add(ebullet2);

                fireBulletSound.Play(0.1f, 0, 0);
            }
        }

        /// <summary>
        /// updates the enemy's position and bullets
        /// </summary>
        public override void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            //used to animate taking damage
            base.Update(gameTime, clientRectangle);

            //make them fire
            shotTimer++;
            if (shotTimer == 30)
            {
                this.Fire();
                shotTimer = 0;
            }

            //update bullet pos
            for (int i = 0; i < this.Bullets.Count; i++)
            {
                Bullets[i].ObjectYPos += (int)Bullets[i].Speed;
                //remove if off screen
                if (Bullets[i].ObjectYPos > 750)
                {
                    Bullets.Remove(Bullets[i]);
                }
            }

            //move ship side to side
            if (direction == 0)
            {
                //move right
                this.ObjectXPos += (int)this.Speed;
                //switch direction if we hit the side or move too far away from origin
                if (this.ObjectXPos >= 1280 || this.ObjectXPos >= 1230)
                {
                    direction = 1;
                }
            }
            else if (direction == 1)
            {
                //move left
                this.ObjectXPos -= (int)this.Speed;
                //switch direction if we hit the side or move too far away from origin
                if (this.ObjectXPos <= 0 )
                {
                    direction = 0;
                }
            }
        }

        /// <summary>
        /// draws the ship and bullets
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw all bullets (make sure to draw all bullets before removing from screen)
            for (int i = 0; i < this.Bullets.Count; i++)
            {
                Bullets[i].Draw(spriteBatch);
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
