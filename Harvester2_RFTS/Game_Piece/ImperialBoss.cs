//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Harvester
{
    public class ImperialBoss : Enemy
    {
        //timers for triggering the bosses weapons
        private double lazerTimer = 0;
        private double spawnTimer = 0;
        private bool hasHitSide;
        private bool hasHitUp;
        private SoundEffect fireSound;
        private Level level;
        #region properties
        public bool HasHitSide { get { return hasHitSide; } set { hasHitSide = value; } }
        #endregion

        public ImperialBoss(Rectangle objPos, Texture2D objTexture, double objSpeed, int objHealth, Texture2D bulletTexture, double damage, Level l)
            : base(objPos, objTexture, objSpeed, objHealth, bulletTexture, damage)
        {
            hasHitSide = false;
            hasHitUp = false;

            fireSound = AssetMgr.Inst().SoundDic["imperial_laser_fire"];

            this.level = level;
        }

        #region methods
        /// <summary>
        /// Fires the laser beam
        /// </summary>
        public override void Fire()
        {
            //make two laser beams
            ImperialBullet b1 = new ImperialBullet(new Rectangle(this.ObjectXPos + 120, this.ObjectYPos + 20, 30, 30), this.BulletTexture, 20);
            ImperialBullet b2 = new ImperialBullet(new Rectangle(this.ObjectXPos + 270, this.ObjectYPos + 220, 30, 30), this.BulletTexture, 20);

            this.Bullets.Add(b1);
            this.Bullets.Add(b2);

            fireSound.Play(0.7f, 0, 0);
        }
        /// <summary>
        /// Spawns random enemies near the boss ship
        /// </summary>
        public void SpawnEnemyFighter()
        {
            //spawn fighters
            EnemyFighter e1 = (EnemyFighter)SpawnManager.Instance.SpawnSingle(100, -1, -1);
            EnemyFighter e2 = (EnemyFighter)SpawnManager.Instance.SpawnSingle(100, -1, -1);

            e1.ObjectXPos = this.ObjectXPos;
            e2.ObjectXPos = this.ObjectXPos + 400;

            e1.ObjectYPos = this.ObjectYPos + 200;
            e2.ObjectYPos = this.ObjectYPos + 200;

            if (e1.ObjectXPos < 0)
            {
                e1.ObjectXPos = 0;
            }
            if (e2.ObjectXPos > 1280)
            {
                e2.ObjectXPos = 1280-50;
            }

           // l3.Enemies.Add(e1);
            //l3.Enemies.Add(e2);
        }

        public override void Update(GameTime gameTime, Rectangle clientRectangle)
        {
            base.Update(gameTime, clientRectangle);

            //update our bullets
            for (int i = 0; i < this.Bullets.Count; i++)
            {
                if (i == 0)
                {
                    this.Bullets[i].ObjectXPos = this.ObjectXPos + 110;
                }
                else if (i == 1)
                {
                    this.Bullets[i].ObjectXPos = this.ObjectXPos + 260;
                }

                this.Bullets[i].ObjectYPos = this.ObjectYPos + 220;
                this.Bullets[i].ObjectHeight += (int)this.Bullets[i].Speed;

                if (this.Bullets[i].ObjectHeight > 2880*3)
                    this.Bullets.Remove(bullets[i]);
            }

            //code for spawning enemy fighters/ firing lazers
            spawnTimer++;

            if (this.Bullets.Count == 0)
                lazerTimer++;
            if (lazerTimer >= 300)
            {
                Fire();
                lazerTimer = 0;
            }

            if (spawnTimer >= 300)
            {
                SpawnEnemyFighter();
                spawnTimer = 0;
            }

            //Movement for boss
            if (this.hasHitUp == false)
            {
                this.ObjectYPos += (int)this.Speed -1;
                if (this.ObjectYPos >= 185)
                {
                    hasHitUp = true;
                }
            }
            if (this.hasHitUp == true)
            {
                this.ObjectYPos -= (int)this.Speed - 1;

                if (this.ObjectYPos <= 0)
                {
                    hasHitUp = false;
                }
            }
            if (this.HasHitSide == false)
            {
                this.ObjectXPos += (int)this.Speed - 1;

                if (this.ObjectXPos >= 1280 - 400)
                {
                    this.HasHitSide = true;
                }
            }
            if (this.HasHitSide == true)
            {
                this.ObjectXPos -= (int)this.Speed - 1;

                if (this.ObjectXPos <= 0)
                {
                    this.HasHitSide = false;
                }
            }
        }
        /// <summary>
        /// Draws boss
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw bullets
            for (int i = 0; i < this.Bullets.Count; i++)
            {
                this.Bullets[i].Draw(spriteBatch);
            }

            //draw ship
            base.Draw(spriteBatch);
        }
        #endregion

        
    }
}
