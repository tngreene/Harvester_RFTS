/*
 * public abstract class HealthBar : MovableGameObject
 * @author Derrick Hunt
 * 
 * Version:
 *      $1.6.0$
 * 
 * Revisions:
 *      1.3.4: (Freddy Garcia)
 *          - Changed health into a double (cause doubles are cooler)
 *          - Renamed texture parameter in Draw to spritebatch
 *          - Changed health into currentHealth. Made a maxHealth variable in order
 *            to differentiate between the current health of the player and the maximum health that the player can have
 *          - Draw method now checks for percentage of health remaining rather than specific values.
 *          - Created two new methods: AddHealth(double num) and RemoveHealth(double num)
 *              These methods will handle changing the value of the health bar according to the health of the player
 *          - Moved UpdateHUD method from Level class to HealthBar class in order to manage the change in health bar in this class
 *     1.5.0 (Derrick)
 *          - fixed to make the health bar actually reflect player health, move (go up and down), and change colors correctly.
 *     1.6.0 (Derrick)
 *          - made some slight code changes to fit the health bar into the new and more cohesive HUD class.
 *                                             
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
    /// The health bar that will be displayed on the top-left corner of the screen.
    /// </summary>
    public class HealthBar : GameObject
    {
        #region Attributes
        private Color color; //the color of the health bar
        private PlayerShip playerShip; //the player ship
        private Texture2D border; //the border around the health bar
        private Rectangle borderPos; //the position for the health bar border
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a health bar with a specified position / size, 
        /// texture, and the amount of health that the player has.
        /// </summary>
        public HealthBar(Rectangle rect, Texture2D texture, PlayerShip p, Texture2D border) 
            : base(rect, texture)
        {
            playerShip = p;
            this.border = border;
            borderPos = rect;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This is called when the health bar should draw itself
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.ObjectTexture, this.ObjectPosition, color);
            spriteBatch.Draw(border, borderPos, Color.Black);
        }

        /// <summary>
        /// Updates the health bar of the player based on the current health percentage of the player
        /// </summary>
        public override void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            // updates the width of the health bar based on the percentage of health remaining (rather than static numbers)
            this.ObjectWidth = (int) playerShip.CurrentHealth / (playerShip.MaxHealth/100);

            //changes the color of health bar
            // Green bar when health >= 75%
            if (playerShip.CurrentHealth / playerShip.MaxHealth >= .75)
            {
                color = Color.Green;
            }
            // Yellow bar when health >= 50% && < 75%
            else if (playerShip.CurrentHealth / playerShip.MaxHealth >= .50 && playerShip.CurrentHealth / playerShip.MaxHealth < .75)
            {
                color = Color.Yellow;
            }
            // Orange bar when health >= 25% && < 50%
            else if (playerShip.CurrentHealth / playerShip.MaxHealth >= .25 && playerShip.CurrentHealth / playerShip.MaxHealth < .50)
            {
                color = Color.Orange;
            }
            // Red bar when health >= 0% && < 25%
            else
            {
                color = Color.Red;
            }
        }
        #endregion
    }
}
