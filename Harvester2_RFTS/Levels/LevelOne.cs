/*
 * public class LevelOne : Microsoft.Xna.Framework.DrawableGameComponent
 * @author Derrick Hunt
 * 
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
    /// <summary>
    /// LevelOne
    /// </summary>
    public class LevelOne : Level
    {
        #region Constructor
        /// <summary>
        /// Creates a new level in the game
        /// </summary>
        /// <param name="theGame">teh game engine</param>
        public LevelOne(Engine theGame)
            : base(theGame)
        {
            //load spawning variables
            maxEnemies = 30;
            difficulty = SpawnType.EasyWaves;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes all necessary values to their defaults before the level begins
        /// to run.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads all the content for this level
        /// </summary>
        protected override void LoadContent()
        {
            //backgrounds
            backgrounds[0] = new Background(new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), AssetManager.Instance.FindTexture("water_world1"));
            backgrounds[1] = new Background(new Rectangle(0, -GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), AssetManager.Instance.FindTexture("water_world2"));
            backgrounds[2] = new Background(new Rectangle(0, -GraphicsDevice.Viewport.Height * 2, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), AssetManager.Instance.FindTexture("water_world3"));
            backgrounds[3] = new Background(new Rectangle(0, -GraphicsDevice.Viewport.Height * 3, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), AssetManager.Instance.FindTexture("water_world4"));
            base.LoadContent();
        }
        #endregion
    }
}