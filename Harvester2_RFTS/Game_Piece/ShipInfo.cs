/*
 * public abstract class Ship : MovableGameObject
 * @author Freddy Garcia / Derrick
 * 
 * Version:
 *      $1.5.0
 * 
 * Revisions:
 *      1.1.0: (Derrick Hunt)
 *          - Added an Explode method which will display an animation when a ship dies
 *      1.5.0 (Derrick Hunt) Cleaned up some seriously messed up code reduncies, put stuff in the
 *              child classes that they actually should be in
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
    public enum ShipType //for use with loading with the tool
    {
        player,
        enemy_fighter,
        bomber,
        kamikaze,
        imperial_boss
    }

    /// <summary>
    /// ABSTRACT ship class. Will contain methods and attributes which both
    /// the player and the enemy ships will have in common.
    /// </summary>
    public abstract class ShipInfo : MovableGameObject
    {
        #region Attributes
        protected ShipType shipType;
        protected double currentHealth; // The amount of health that the ship currently has
        protected int maxHealth; // The maximum amount of health that the ship can have 
        protected double damage; //the damage of the ships bullets
        protected List<Bullet> bullets; //the list of the ships bullets
        protected Texture2D bulletTexture; //the bullet texture
        protected bool isActive; // if the ship is alive
        protected bool hasPowerUp; //if the ship is carrying a powerup
        protected bool hasExploded; // if the ship has exploded or not yet
        protected int colorCounter = 0; //used to animate ships getting hit
        #endregion

        #region Properties
        public ShipType ShipType { get { return shipType; } }
        public Boolean IsActive { get { return isActive; } set { isActive = value; } }
        public Texture2D BulletTexture { get { return bulletTexture; } }
        public List<Bullet> Bullets { get { return bullets; } set { bullets = value; } }
        public double CurrentHealth { get { return currentHealth; }
            set
            {
                currentHealth = value;
                //health can't go above max
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
                //health can't go below zero
                if (currentHealth < 0)
                    currentHealth = 0;
            }
        }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public double Damage { get { return damage; } set { damage = value; } }
        public bool HasExploded { get { return hasExploded;} set { hasExploded = value; } }
        public bool HasPowerUp { get { return hasPowerUp; } set { hasPowerUp = value; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new Ship with position/size and texture, as well
        /// speed, health, armor, and bullet texture based on the parameters
        /// </summary>
        public ShipInfo(Rectangle objPos, Texture2D objTexture, double objSpeed, double objHealth, Texture2D bulletTexture, double damage)
         : base(objPos, objTexture, objSpeed)
        {
            //ship health is set
            maxHealth = (int) objHealth; //whatever health we are instantiated with is the max
            currentHealth = maxHealth;

            //create bullet list
            this.Bullets = new List<Bullet>();
            this.damage = damage; //save bullet damage
            this.bulletTexture = bulletTexture; //save bullet texture

            hasExploded = false;//not dead yet
            this.isActive = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// The ship takes damage equal to the damage in the parameter
        /// </summary>
        /// <param name="damage">the amt of damage taken</param>
        public virtual void TakeHit(int damage)
        {
            currentHealth -= damage;

            //set color to flash red when hit
            this.TintColor = Color.Red;

            // The ship dies when health is less than or equal to 0
            if (currentHealth <= 0)
            {
                this.isActive = false;
            }
        }

        /// <summary>
        /// Fires the main weapon of the ship, should be diff for all ships
        /// </summary>
        abstract public void Fire();

        /// <summary>
        /// Updates the ship position, bullets, etc
        /// returns the ship color to normal after 3 frames
        /// </summary>
        public override void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            //reset back to white if not white and we dont have a powerup
            if (this.TintColor != Color.White && hasPowerUp == false)
            {
                colorCounter++;
                if (colorCounter == 3)
                {
                    this.TintColor = Color.White;
                    colorCounter = 0;
                }
            }

            //reset back to pink if not pink and we have a powerup
            if (this.TintColor != Color.HotPink && hasPowerUp)
            {
                colorCounter++;
                if (colorCounter == 3)
                {
                    this.TintColor = Color.HotPink;
                    colorCounter = 0;
                }
            }
        }
        #endregion
    }
}
