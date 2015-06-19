/*
 * by Derrick
 * 
 * version 1.0.0
 * 
 * revisions:
 * 
 * 
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Harvester
{
    public class Explosion : GameObject
    {
        #region Attr
        int frame; //the current frame of the explosion animation
        #endregion

        #region Props
        public int Frame { get { return frame; } set { frame = value; } }
        #endregion

        #region Constr
        public Explosion(Rectangle pos, Texture2D texture)
            : base(pos, texture)
        {
            frame = 0; //initialize to first frame
        }
        #endregion

        #region Methods
        //overload draw for spritesheet
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.objectTexture, new Vector2(this.ObjectXPos, this.ObjectYPos), new Rectangle(90 * frame, 0, 90, 90), Color.White);
        }
        #endregion
    }
}
