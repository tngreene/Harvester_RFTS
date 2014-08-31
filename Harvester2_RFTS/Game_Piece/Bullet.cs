/*
 * public abstract class Bullet : MovableGameObject
 * @author Derrick Hunt / Peter O'Neal
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
    /// Represents a bullet on the screen
    /// </summary>
    public abstract class Bullet : MovableGameObject
    {
        #region Attributes
        protected bool isActive;
        #endregion

        #region Props
        public Boolean IsActive { get { return isActive; } set { isActive = value; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new bullet with a position/size,
        /// texture, and speed based on parameters. Also
        /// sets the damage of the bullet
        /// </summary>
        public Bullet(Rectangle objPos, Texture2D objTexture, double objSpeed)
            : base(objPos, objTexture, objSpeed)
        {
            isActive = true;
        }
        #endregion
    }
}
