/*
 * public abstract class MovableGameObject : GameObject
 * @author Derrick Hunt / Peter O'Neal
 * 
 * Version:
 *      $1.0.0$
 * 
 * Revisions:
 *      1.0.1
 *          Derrick - added regions, cleaned up code
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
    /// Game objects that will be displayed on the screen.
    /// These objects are movable!
    /// </summary>
    public abstract class MovableGameObject : GameObject
    {
        #region Attr
        protected double speed; // the speed of the object
        #endregion

        #region Properties
        public double Speed { get { return speed; } set { speed = value; } }
        #endregion

        #region Constr
        /// <summary>
        /// Creates a new movable game object, and sets the position / size,
        /// speed, and the texture of the object based on the parameters
        /// </summary>
        /// <param name="objPos">Position and size of the object</param>
        /// <param name="objTexture">Texture of the object</param>
        /// <param name="objSpeed">Speed of the obj</param>
        public MovableGameObject(Rectangle objPos, Texture2D objTexture, double objSpeed)
            : base(objPos, objTexture)
        {
            speed = objSpeed;
        }
        #endregion
    }
}
