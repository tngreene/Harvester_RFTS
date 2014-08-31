using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Harvester
{
    class ImperialBullet : Bullet
    {
        #region Constructor
        //class for making the bosses lasers
        public ImperialBullet(Rectangle rect, Texture2D text, double objSpeed)
            : base(rect, text, objSpeed)
        {
            //make active
            this.IsActive = true;
        }
        #endregion
        public override void Update(GameTime gameTime, Rectangle clientRectangle) { throw new NotImplementedException(); }
    }
}
