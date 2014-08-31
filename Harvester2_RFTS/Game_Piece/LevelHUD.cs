using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Harvester
{
    //contains the methods and objects necessary for drawing
    //and update the HUD in the levels
    public class LevelHUD
    {
        #region Attributes
        private HealthBar healthBar; //the health bar of the player
        private CooldownBar cooldownBar; //the cool down bar of the player
        private PowerUpBar powerUpBar; //the player's power up bar
        private SpriteFont HUDFont; //the hud font
        private Level level; //ref to player object (so we can print score)
        #endregion

        #region Constructor
        public LevelHUD(PlayerShip p, Texture2D bar, Texture2D barOutline, Texture2D powerUpBarOutline, SpriteFont hudFont, Level level)
        {
            //create all hud assets

            //health bar
            healthBar = new HealthBar(new Rectangle(10, 10, 100, 20), bar, p, barOutline);
            //cooldown bar
            cooldownBar = new CooldownBar(new Rectangle(10, 40, 100, 20), bar, p, barOutline);
            //power up bar
            powerUpBar = new PowerUpBar(new Rectangle(1220, 10, 50, 50), powerUpBarOutline, p);
            //hud font (for writing score)
            HUDFont = hudFont;
            this.level = level;
        }
        #endregion

        #region Methods
        public void Draw(SpriteBatch spritebatch)
        {
            healthBar.Draw(spritebatch);
            cooldownBar.Draw(spritebatch);
            powerUpBar.Draw(spritebatch);
            spritebatch.DrawString(HUDFont, level.LevelScore.ToString(), new Vector2(1120, 10), Color.White);
        }

        public void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            healthBar.Update(gameTime, clientRectangle);
            cooldownBar.Update(gameTime, clientRectangle);
        }
        #endregion
    }
}
