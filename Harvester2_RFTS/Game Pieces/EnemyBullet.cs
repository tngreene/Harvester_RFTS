//Extends Bullet
//@author: Peter O'Neal
// Version:
//       $1.0.1$
//       used for generating enemy bullets
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Harvester
{
    public class EnemyBullet : Bullet
    {
        #region Constructor
        //Enemy Bullet class for making enemy fighters shoot
        public EnemyBullet(Rectangle rect, Texture2D eBulletTexture, double speed)
            :base(rect,eBulletTexture,speed)
        {
        }
        #endregion
    }
}
