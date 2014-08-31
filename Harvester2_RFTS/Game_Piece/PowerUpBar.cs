using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Harvester
{
    public class PowerUpBar : GameObject
    {
        #region Attributes
        private PlayerShip playerShip; //ref to playership for powerup
        private Texture2D blackholePowerUpIcon;
        private Texture2D invincibilityPowerUpIcon;
        private Texture2D doubleDamagePowerUpIcon;
        #endregion

        #region Constructor
        public PowerUpBar(Rectangle rect, Texture2D texture, PlayerShip playerShip)
            : base(rect, texture)
        {
            this.playerShip = playerShip;

            //set power up icon textures
            blackholePowerUpIcon = AssetMgr.Inst().TextureDic["blackhole_icon"];
            invincibilityPowerUpIcon = AssetMgr.Inst().TextureDic["invincibility_icon"];
            doubleDamagePowerUpIcon = AssetMgr.Inst().TextureDic["double_damage_icon"];
        }
        #endregion

        #region Methods
        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw the powerup's icon if there is one
            if (playerShip.HasPowerUp)
            {
                if (playerShip.PowerUp is BlackholePowerUp)
                {
                    spriteBatch.Draw(blackholePowerUpIcon, this.ObjectPosition, Color.White);
                }
                else if (playerShip.PowerUp is DamagePowerUp)
                {
                    spriteBatch.Draw(doubleDamagePowerUpIcon, this.ObjectPosition, Color.White);
                }
                else if (playerShip.PowerUp is InvincibilityPowerUp)
                {
                    spriteBatch.Draw(invincibilityPowerUpIcon, this.ObjectPosition, Color.White);
                }
            }

            //draw the border
            base.Draw(spriteBatch);
        }
        #endregion
        public override void Update(GameTime gameTime, Rectangle clientRectangle) { throw new NotImplementedException(); }
    }
}
