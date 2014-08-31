/*
 * public abstract class GameObject
 * @author Derrick Hunt / Peter O'Neal
 * 
 * Version:
 *      $1.1.4$
 * 
 * Revisions:
 *      1.0.2: (Derrick Hunt)
 *          - Added properties to get and set x / y positions of object
 *      1.1.2: (Derrick Hunt)
 *          - Added a basic draw method for all game objects
 *      1.1.4: (Derrick Hunt)
 *          - Added properties to get width and height of the rectangle
 *      1.1.5 (Derrick)
 *          added regions, cleaned up the code
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
    /// These objects do not move!
    /// </summary>
    public abstract class GameObject
    {
        #region Attr
        protected Rectangle objectPosition; // position of the object
        protected Texture2D objectTexture; // texture of the object
        protected Color tintColor; // Color tint of the object
        #endregion

        #region Properties
        public Rectangle ObjectPosition { get { return objectPosition; } set { objectPosition = value; } }
        /// <summary>
        /// gets and sets the x position of the object's rectangle
        /// </summary>
        public int ObjectXPos { get { return objectPosition.X; } set { objectPosition.X = value; } }
        /// <summary>
        /// gets and sets the y position of the object's rectangle
        /// </summary>
        public int ObjectYPos { get { return objectPosition.Y; } set { objectPosition.Y = value; } }
        /// <summary>
        /// gets and sets the width of the object's rectangle
        /// </summary>
        public int ObjectWidth { get { return objectPosition.Width; } set { objectPosition.Width = value; } }
        /// <summary>
        /// gets and sets the height of the object's rectangle
        /// </summary>
        public int ObjectHeight { get { return objectPosition.Height; } set { objectPosition.Height = value; } }
        public Texture2D ObjectTexture { get { return objectTexture; } set { objectTexture = value; } }
        public Color TintColor { get { return tintColor; } set { tintColor = value; } }
        #endregion

        #region Constr
        /// <summary>
        /// Creates a new game object, and sets the values of the position
        /// and texture of the object equal to the parameters
        /// </summary>
        /// <param name="objPos">Position and size of the object</param>
        /// <param name="objTexture">Texture of the object</param>
        public GameObject(Rectangle objPos, Texture2D objTexture)
        {
            objectPosition = objPos;
            objectTexture = objTexture;
            //default color to white
            tintColor = Color.White;
        }
        #endregion

        public abstract void Update(GameTime gameTime, Rectangle clientRectangle);
		
        #region Methods
        /// <summary>
        /// Draws the game object
        /// </summary>
        /// <param name="texture">spritebatch object that draws the texture</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.ObjectTexture, this.ObjectPosition, tintColor);
        }
        #endregion
    }
}
