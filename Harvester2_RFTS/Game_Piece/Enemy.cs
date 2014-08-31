/*
 * public abstract class Enemy : Ship
 * @author Derrick Hunt / Peter O'Neal
 * 
 * Version:
 *      $1.1.1$
 * 
 * Revisions:
 *      1.1.0: (Derrick Hunt)
 *          - Added a TakeHit method for the ship to be able to take damage
 *      1.1.1: (Freddy Garcia)
 *          - Removed the bool isActive parameter from the constructor
 *      1.1.2: (Peter O'Neal)
 *          - Added properties and attributes for when enemies hit screen edges
 *      1.1.3 (Peter O'Neal)    
 *          - Did all the enemy bullet stuff
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Harvester
{
    /// <summary>
    /// Enemies that will spawn on the ship which can shoot down the ship
    /// </summary>
    public abstract class Enemy : ShipInfo
    {
        #region Attributes
        protected Random rand; 
        protected int score; //the score the player gets for killing this enemy
        private static int powerUpCounter; //used for generating powerup chance
        #endregion

        #region Props
        public int Score { get { return score; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new enemy which will have position/size,
        /// texture, speed, health determined by parameters.
        /// </summary>
        public Enemy(Rectangle objPos, Texture2D objTexture, double objSpeed, int objHealth, Texture2D bulletTexture, double damage)
            : base(objPos, objTexture, objSpeed, objHealth, bulletTexture, damage)
        {
            hasPowerUp = false;

            rand = new Random();
            powerUpCounter++;
            //used to spawn enemy powerups randomly
            if (powerUpCounter > 8)
            {
                powerUpCounter = 0;
                hasPowerUp = true;
            }
            if (hasPowerUp)
                this.TintColor = Color.HotPink;
        }
        #endregion

        //since this class is also abstract
        //it does not have to implement
        //the abstract methods of ship
        public override void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            base.Update(gameTime, clientRectangle);            
        }
    }
}
