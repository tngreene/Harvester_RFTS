/*
 * public class Button : GameObject
 * @author Derrick Hunt
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
    /// "Buttons" 
    /// </summary>
    public class Button : GameObject
    {
        /// <summary>
        /// Creates a new button with a position and size as well
        /// as the texture of the button based on the parameters.
        /// </summary>
        /// <param name="rect">Position and size of the button</param>
        /// <param name="texture">Texture of the button</param>
        public Button(Rectangle rect, Texture2D texture)
            : base(rect, texture)
        {
        }
        public override void Update(GameTime gameTime, Rectangle clientRectangle) { throw new NotImplementedException(); }
    }
}
