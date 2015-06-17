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
    /// <summary>
    /// Represents the player in the game
    /// </summary>
    public class PlayerMgr
    {
        #region Attributes
        private int score;
        private PlayerShip playerShip;
        #endregion

        #region Properties
        public int Score { get { return score; } set { score = value; } }
        public PlayerShip PlayerShip { get { return playerShip; } set { playerShip = value; } }
        #endregion

        private static PlayerMgr _instance;
        public static PlayerMgr Inst()
        {
            if (_instance == null)
            {
                _instance = new PlayerMgr();
            }

            //Otherwise return the inStance of
            return _instance;
        }

        #region Constructor
        private PlayerMgr()
        {
            //set score and name
            score = 0;

            //create our ship
            //create the player's ship for the level
            playerShip = new PlayerShip(new Rectangle((1280 - 25) / 2, 695 - 90, 50, 50), AssetMgr.Inst().TextureDic["phoenix"], 5, 300, 
                AssetMgr.Inst().TextureDic["phoenix_bullet"], 20, new Harvester(new Rectangle(1280 / 2, 720 - 90, 50, 50),
                    AssetMgr.Inst().TextureDic["harvester_spritesheet"], 100));
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            playerShip.Update(gameTime, clientRectangle);
        }

        /// <summary>
        /// checks to see if player is dead
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            if (playerShip != null && playerShip.CurrentHealth <= 0)
                return true;
            else
                return false;
        }
        #endregion
    }
}
