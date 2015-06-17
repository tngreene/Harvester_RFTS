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
    class InvincibilityPowerUp : PowerUp
    {
        #region Attributes
        private PlayerShip p;
        private int counter;
        private Random gen;
        private SoundEffect activateShield;
        #endregion

        #region Constructor
        //Class for the invincible powerup
        public InvincibilityPowerUp(Rectangle objPos, Texture2D objTexture, int objSpeed, PlayerShip p)
            : base(objPos,objTexture,objSpeed)
        {
            this.p = p;
            gen = new Random();
            activateShield = AssetMgr.Inst().SoundDic["invincibility_shield"];
        }
        #endregion

        #region Methods
        /// <summary>
        /// activates shield
        /// </summary>
        public override void Fire()
        {
            this.IsActive = true;
            this.HasFired = true;
            //play sound
            activateShield.Play();
        }

        public override void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            if (IsActive)
            {
                counter++;

                //change color to make it look cool
                int r = gen.Next(256);
                int g = gen.Next(256);
                int b = gen.Next(256);
                int a = gen.Next(256);
                this.TintColor = new Color(r, g, b, a);

                //update it's position to stay on top of player
                this.ObjectPosition = new Rectangle(p.ObjectXPos - 10, p.ObjectYPos - 10, p.ObjectWidth + 20, p.ObjectHeight + 20);

                //invincibility lasts for 10 seconds
                if (counter >= 600)
                {
                    this.IsActive = false;
                }
            }
        }
        #endregion
        
    }
}
