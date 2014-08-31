/*
 * public class BackgroundBox : MovableGameObject
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
    /// Background for the menus
    /// </summary>
    public class BackgroundBox : MovableGameObject
    {
        // The back color for the background
        Color color;

        /// <summary>
        /// Creates a new background box for the menu with a specified 
        /// position, texture, speed of movement, and back color based on 
        /// the parameters
        /// </summary>
        /// <param name="rectangle">The position and size of the background</param>
        /// <param name="texture">The image of the background</param>
        /// <param name="speed">The speed at which the background can move</param>
        /// <param name="color">The back color of the backvround</param>
        public BackgroundBox(Rectangle rectangle, Texture2D texture, double speed, Color color)
            : base(rectangle, texture, speed)
        {
            this.color = color;
        }

        //Console.WriteLine();
        /// <summary>
         //This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.ObjectTexture, this.ObjectPosition, color);
        }

        public override void Update(GameTime gameTime, Rectangle clientRectangle) { throw new NotImplementedException(); }
    }
}
