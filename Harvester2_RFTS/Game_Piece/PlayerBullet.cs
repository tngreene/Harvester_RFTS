/*
 * public abstract class Bullet : MovableGameObject
 * @author Derrick H.
 * 
 * Version:
 *      $1.0.1$
 * 
 * Revisions:
 *      1.0.1: (Freddy Garcia)
 *          - Changed the parameter for the bullet speed to double
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
    public class PlayerBullet : Bullet
    {
        #region Constructor
        /// <summary>
        /// Creates a new bullet for the player
        /// </summary>
        /// <param name="rect">The position and size of the bullet</param>
        /// <param name="pBulletTexture">The texture of the bullet</param>
        /// <param name="speed">The speed of the bullet</param>
        /// <param name="dmg">The damage of the bullet</param>
        public PlayerBullet(Rectangle rect, Texture2D pBulletTexture, double speed)
            : base(rect, pBulletTexture, speed)
        {
        }
        #endregion

        public override void Update(GameTime gameTime, Rectangle clientRectangle) { throw new NotImplementedException(); }
    }
}
