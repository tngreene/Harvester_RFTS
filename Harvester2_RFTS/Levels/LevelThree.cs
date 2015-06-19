/*
 * public class LevelThree : Microsoft.Xna.Framework.DrawableGameComponent
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
    /// Level 3
    /// </summary>
    public class LevelThree : Level
    {
        //The Boss
        ImperialBoss boss;
        Texture2D bossTexture;
        public bool bossSpawned = false;
        public bool BossSpawned
        {
            get { return bossSpawned; }
            set { bossSpawned = value; }
        }

        #region Constructor
        /// <summary>
        /// Creates a new level in the game
        /// </summary>
        /// <param name="theGame">teh game engine</param>
        public LevelThree(Engine theGame)
            : base(theGame)
        {
            //load spawning variables
            maxEnemies = 50;
            difficulty = SpawnType.HardWaves;
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
            backgrounds[0] = new Background(new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), AssetManager.Instance.FindTexture("space1"));
            backgrounds[1] = new Background(new Rectangle(0, -GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), AssetManager.Instance.FindTexture("space2"));
            backgrounds[2] = new Background(new Rectangle(0, -GraphicsDevice.Viewport.Height * 2, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), AssetManager.Instance.FindTexture("space3"));
            backgrounds[3] = new Background(new Rectangle(0, -GraphicsDevice.Viewport.Height * 3, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), AssetManager.Instance.FindTexture("space4"));
            
            bossTexture = AssetManager.Instance.FindTexture("imperial_boss");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.LevelScore < 5000)
            {
                bossSpawned = false;
                boss = null;
            }

            if (boss != null)
            Console.WriteLine(boss.CurrentHealth.ToString());

            //SPAWN BOSS
            if (this.LevelScore >= 5000 && bossSpawned == false)
            {
                boss = new ImperialBoss(new Rectangle(240, -500, 400, 240), bossTexture, 2, 7500, AssetManager.Instance.FindTexture("imperial_laser"), 1, this);
                this.enemies.Add(boss);
                bossSpawned = true;
            }

            base.Update(gameTime);
        }
        #endregion
    }
}