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
    class DamagePowerUp : PowerUp
    {
        private PlayerShip p;
        int counter;

        #region Constructor
        public DamagePowerUp(Rectangle objPos, Texture2D objTexture, int objSpeed, PlayerShip p)
            : base(objPos,objTexture,objSpeed)
        {
            this.p = p;
        }
        #endregion

        #region methods
        //Activated damage powerup
        public override void Fire()
        {
            this.IsActive = true;
            this.HasFired = true;
        }

        public override void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            if (IsActive)
            {
                counter++;
                p.Damage = 40;
                //color is changed in playership class

                if (counter >= 600)
                {
                    this.IsActive = false;
                    p.Damage = 20;
                }
            }
        }
        #endregion

        
    }
}
