/*
 * public class Background : GameObject
 * @author Derrick Huntah!
 * 
 * Version:
 *      $1.1.0$
 * 
 * Revisions:
 *      1.1.0: (Derrick Hunt)
 *          - Removed Draw method and added it to abstract game object class
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
    /// In-game background display
    /// </summary>
    public class Background : GameObject
    {
        /// <summary>
        /// Creates a background object with a position / size, 
        /// and texture based on the parameters
        /// </summary>
        /// <param name="rect">The position and size of the background </param>
        /// <param name="tex">The texture of the background</param>
        public Background(Rectangle rect, Texture2D tex) 
            : base(rect, tex)
        {
        }
    }
}
