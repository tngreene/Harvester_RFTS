
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
    public abstract class PowerUp : MovableGameObject
    {
        #region Attr
        private bool isActive;
        private bool hasFired; //if it has been fired or not
        #endregion

        #region Props
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        public Boolean HasFired { get { return hasFired; } set { hasFired = value; } }
        #endregion

        #region Constr
        public PowerUp(Rectangle objPos, Texture2D objTexture, int objSpeed)
            : base(objPos, objTexture, objSpeed)
        {
            isActive = false;
            hasFired = false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Activates the powerup
        /// </summary>
        public abstract void Fire();

        /// <summary>
        /// implemented in children, updates the powerup
        /// </summary>
        public override void Update(GameTime gameTime, Rectangle clientRectangle) { throw new NotImplementedException(); }
        #endregion
    }
}
