/*
 * public abstract class Harvester
 * @author Derrick Huntah!
 * 
 * Version:
 *      $1.0.0$
 * 
 * Revisions:
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Harvester
{
    /// <summary>
    /// Represents the player's Harvester claw used to steal energy from enemies
    /// </summary>
    public class Harvester : GameObject
    {
        #region Attributes
        private Rectangle hitbox; //contains the 'claw' part of the harvester
        private int damage; //the damage the harvester does to enemies
        private double conversion; //the percentage of damage converted from absorbing
        private Texture2D harvester; //the claw texture
        private Random gen; //used to change the harvester color
        private int frame; //the current frame of animation
        private Boolean isActive; //whether the harvester is fired or not
        private const int MAX_TIME = 300; //the total time/size of the bar of the cooldown
        private int currentTime; //the current time of the cooldown
        #endregion

        #region Properties
        public Boolean IsActive { get { return isActive; } set { isActive = value; } }
        public int Frame { get { return frame; } set { frame = value; } }
        public int MaxTime { get { return MAX_TIME; } }
        public int CurrentTime { get { return currentTime; } set { currentTime = value; } }
        #endregion

        #region Constructor
        //Sets up basic attributes for the harvester claw
        public Harvester(Rectangle rect, Texture2D harvester, int damage)
            : base (rect, harvester)
        {
            this.TintColor = Color.White;
            this.damage = damage;
            isActive = false;
            conversion = 0.25;
            gen = new Random();

            currentTime = MAX_TIME;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Shoots the claw
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="p"></param>
        public void DrawClaw(SpriteBatch spriteBatch, PlayerShip p)
        {
            if (isActive)
            {
                //make a random color
                int r = gen.Next(256);
                int g = gen.Next(256);
                int b = gen.Next(256);
                int a = gen.Next(256);
                this.TintColor = new Color(r, g, b, a);
                //draw the current frame of animation
                spriteBatch.Draw(this.ObjectTexture, new Vector2(p.ObjectXPos, p.ObjectYPos - 180), new Rectangle(50 * frame, 0, 50, 180), this.TintColor);
            }
        }

        /// <summary>
        /// Dynamically changes the hitbox of the harvester claw
        /// and checks whether or not it intersects 
        /// </summary>
        public Boolean Absorb(Rectangle rect, PlayerShip p)
        {
            //only can run if it is active
            if (this.IsActive)
            {
                //make hitbox based on frame
                hitbox = new Rectangle(p.ObjectXPos, p.ObjectYPos - (frame * 30), 50, 50);
                //see if it hits something
                if (hitbox.Intersects(rect))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
