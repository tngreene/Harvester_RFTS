/*
 * Public class Options
 * author: Peter O'Neal
 * 
 * Version 1.0.1
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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Harvester.Menus
{
    public class Options : Menu
    {
        /// <summary>
        /// Options's constuctor, brings in a Game object that will handle all of the Game's information
        /// </summary>
        /// <param name="theGame"></param>
        public Options(Engine theGame)
            : base(theGame)
        {
            gameEngine = theGame;
            Game.IsMouseVisible = true;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

    }
}
