//Class author: Peter O'Neal
//Used for the bomber beetle bullets
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Harvester
{
    public class Bomb : Bullet
    {
        #region Attribs
        private int frame;
        private bool lockOn;
        #endregion

        #region Props
        public int Frame { get { return frame; } set { frame = value; } }
        public bool LockOn { get { return lockOn; } set { lockOn = value; } }
        #endregion

        #region Constructor
        public Bomb(Rectangle rect, Texture2D text, double objSpeed)
            : base(rect, text, objSpeed)
        {
            //set frame
            frame = 0;

            //set lock on
            lockOn = false;

            //make bomb inactive
            this.IsActive = false;
        }
        #endregion
        public override void Update(GameTime gameTime, Rectangle clientRectangle) { throw new NotImplementedException(); }
    }
}