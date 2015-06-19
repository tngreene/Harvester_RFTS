/*
 * public interface Menu
 * @author Zach Whitman
 * 
 * Version:
 *      $2.1.3.Bada$$
 * 
 * Revisions:
 *      2.0.0: (Freddy Garcia)
 *          - Turned the Menu class into a DrawableGameComponent
 *      2.1.3: (Idunno)
 *          - Added DrawableGameComponent methods that will be usable by child methods
 *          - Added a rectangle to replace vectors in order for buttons to be clickable
 *          - Previous and current mouse states will make sure multiple clicks are handled properly
 * Did NOT get to GameOver or GamePause yet, I will finish MainMenu first before hand
 *      2.2 (Derrick Hunt)
 *          Changed some attributes, made more efficient with mousevariables and shtuff.
 */
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
    public abstract class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Attributes
        //the game engine
        protected Engine gameEngine;
        #endregion

        #region Properties
        public Engine GameEngine { get { return gameEngine; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Menu's constuctor, brings in a Game object that will handle all of the Game's information
        /// </summary>
        /// <param name="theGame"></param>
        public Menu(Engine theGame)
            : base(theGame)
        {
            gameEngine = theGame;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the Menu
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Abstract method will have the Menu's load their content images for buttons and background images
        /// </summary>
        protected abstract override void LoadContent();

        /// <summary>
        /// Abstract Update method will have the Menu's be able to update their stuff every frame per second thing
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract override void Update(GameTime gameTime);

        /// <summary>
        /// Abstract Draw method that will be used by the Menu classes to draw their menus buttons and backgrounds 
        /// </summary>
        /// <param name="gameTime">The gameTime of the game</param>
        /// <param name="gameTime">The spritebatch from the engine and its graphics device</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        #endregion
    }
}
