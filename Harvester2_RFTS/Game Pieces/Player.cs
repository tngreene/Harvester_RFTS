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
    public class Player
    {
        #region Attributes
        private int score;
        private String name;
        private PlayerShip playerShip;
        private Engine gameEngine;
        #endregion

        #region Properties
        public int Score { get { return score; } set { score = value; } }
        public String Name { get { return name; } }
        public PlayerShip PlayerShip { get { return playerShip; } set { playerShip = value; } }
        #endregion

        #region Constructor
        public Player(string name, Engine g)
        {
            gameEngine = g;

            //set score and name
            score = 0;
            this.name = name;

            //create our ship
            //create the player's ship for the level
            playerShip = new PlayerShip(new Rectangle((1280 - 25) / 2, 695 - 90, 50, 50), AssetManager.Instance.FindTexture("phoenix"), 5, 300, 
                AssetManager.Instance.FindTexture("phoenix_bullet"), 20, new Harvester(new Rectangle(1280 / 2, 720 - 90, 50, 50),
                    AssetManager.Instance.FindTexture("harvester_spritesheet"), 100), gameEngine);
        }
        #endregion

        #region Methods
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
