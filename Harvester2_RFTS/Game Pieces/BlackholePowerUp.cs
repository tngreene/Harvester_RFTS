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
    /// Fires a black hole that sucks up enemies and blows them up
    /// </summary>
    class BlackholePowerUp : PowerUp
    {
        #region Attributes
        private int counter; //tells when to update the frame
        private int counterFull; //counter for how long the hole stays fully open
        private int frame; //the current frame (for animating)
        private Rectangle center; //the center of the hole, where people will die
        private SoundEffect sound; //the sound of the hole
        #endregion

        #region Properties
        public Rectangle Center { get { return center; } }
        #endregion

        #region Constr
        /// <summary>
        /// Creates a black hole powerup object
        /// </summary>
        /// <param name="rect">the powerups rectangle</param>
        /// <param name="texture">should be the blackholespritesheet</param>
        /// <param name="speed">set to 5</param>
        public BlackholePowerUp(Rectangle rect, Texture2D texture, int speed)
            : base(rect, texture, speed)
        {
            //set variables
            frame = 0;
            counterFull = 0;
            counter = 0;

            sound = AssetManager.Instance.FindSound("blackhole_fire");
        }
        #endregion

        #region Methods
        /// <summary>
        /// make it active when it is fired
        /// </summary>
        public override void Fire()
        {
            this.IsActive = true;
            this.HasFired = true;
            sound.Play(1f, 0, 0);
            sound.Play(1f, .5f, 0);
            sound.Play(1f, .1f, 0);

        }

        /// <summary>
        /// update the hole's position and animation
        /// </summary>
        public override void Update()
        {
            //only update when it is active
            if (this.IsActive)
            {
                //update the center
                center = new Rectangle(this.ObjectXPos + 90, this.ObjectYPos + 90, 20, 20);

                //move up the screen when fired
                if (this.ObjectYPos > 75)
                {
                    this.ObjectYPos -= (int)this.Speed;
                }

                //start animating
                if (this.ObjectYPos <= 75)
                {
                    counter++;
                    counterFull++;
                    if (counter == 3)
                    {
                        counter = 0;
                        //make frame go up while we are less than 5 secs
                        if (counterFull < 125)
                        {
                            frame++;
                            //reset frame to keep spinning
                            if (frame >= 10)
                            {
                                frame = 8;
                            }
                        }
                        //time for hole to collapse, set to inactive once done animating
                        else if (counterFull >= 125)
                        {
                            frame--;
                            if (frame < 0)
                            {
                                this.IsActive = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// draw the current frame of animation
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.ObjectTexture, new Vector2(this.ObjectXPos, this.ObjectYPos), new Rectangle(200 * frame, 0, 200, 200), this.TintColor);
        }
        #endregion

    }
}
