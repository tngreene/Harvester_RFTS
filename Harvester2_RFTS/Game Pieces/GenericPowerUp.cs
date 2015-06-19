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
    /// this represents the mystery powerup the enemies drop when they die
    /// </summary>
    public class GenericPowerUp : PowerUp
    {
        #region Attributes
        private Random gen;
        #endregion

        #region Const
        public GenericPowerUp(Rectangle rect, Texture2D texture, int speed)
            : base(rect, texture, speed)
        {
            gen = new Random();
            Fire();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a random powerup for an enemy
        /// </summary>
        public override void Fire()
        {
            this.IsActive = true;
        }

        /// <summary>
        /// change the color of the power up icon
        /// </summary>
        public override void Update()
        {
            //make a random color
            int r = gen.Next(256);
            int g = gen.Next(256);
            int b = gen.Next(256);
            int a = gen.Next(256);
            this.TintColor = new Color(r, g, b, a);

            //move down the screen
            this.ObjectYPos += (int)this.Speed;
        }
        #endregion
    }
}
