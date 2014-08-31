using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Harvester
{
    public class CooldownBar : GameObject
    {
        #region Attributes
        private Color color; //the color of the cooldown bar
        private PlayerShip playerShip; //the player ship
        private Texture2D border; //the border around the cooldown bar
        private Rectangle borderPos; //the position for the cooldown bar border
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a health bar with a specified position / size, 
        /// texture, and the amount of health that the player has.
        /// </summary>
        public CooldownBar(Rectangle rect, Texture2D texture, PlayerShip p, Texture2D border) 
            : base(rect, texture)
        {
            playerShip = p;
            color = Color.White;
            this.border = border;
            borderPos = rect;
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            //increment the currentTime if it less than maxTime
            if (playerShip.Harvester.CurrentTime < playerShip.Harvester.MaxTime)
            {
                playerShip.Harvester.CurrentTime += 1;
                color = Color.Red;
            }
            else if (playerShip.Harvester.CurrentTime == playerShip.Harvester.MaxTime)
            {
                color = Color.White;
            }

            //make the bar grow
            this.ObjectWidth = playerShip.Harvester.CurrentTime / 3;
        }

        /// <summary>
        /// This is called when the cooldown bar should draw itself
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.ObjectTexture, this.ObjectPosition, color);
            spriteBatch.Draw(border, borderPos, Color.Black);
        }
        #endregion
        
    }
}
