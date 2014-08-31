/*
 * Public static class ShipManager
 * @author Freddy Garcia, Theodore Greene
 * 
 * Version: $1.0.0$
 *           
 * 
 * Revisions: 1.0.1 - (Theodore Greene)
 *                  -Created Method Stubs
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


    public class ShipManager : Game
    {

        public ShipManager()
        {

        }

        /// <summary>
        /// Creates a ship based of parameters
        /// </summary>
        /// <param name="objPos">Object Position</param>
        /// <param name="objTexture">Object Texture</param>
        /// <param name="objSpeed">Speed</param>
        /// <param name="objHealth">Health</param>
        /// <param name="objArmour">Armour</param>
        /// <param name="projectileTexture">Bullet Texture</param>
        /// <param name="shipType">Ship Type</param>
        /// <param name="st">Shot Timer</param>
        /// <returns>Returns a ship</returns>
        public static Enemy CreateShip(Rectangle objPos, Texture2D objTexture, double objSpeed, int objHealth, double objArmour, double damage, Texture2D projectileTexture, ShipType shipType, int st, PlayerShip p)
        {
            switch (shipType)
            {
                // case ShipType.Player:
                //   Player player
                case ShipType.enemy_fighter:
                    EnemyFighter enemyFighter = new EnemyFighter(objPos, projectileTexture, objSpeed, objHealth, projectileTexture, damage, st);
                    return enemyFighter;

                case ShipType.kamikaze:
                    Kamikaze kamikazeFighter = new Kamikaze(objPos, objTexture, objSpeed, objHealth, projectileTexture, damage, p);
                    return kamikazeFighter;

                case ShipType.bomber:
                    Bomber bomber = new Bomber(objPos, objTexture, objSpeed, objHealth, projectileTexture, damage, p);
                        
                    return bomber;
                default:
                    return null;
            }
        }
    }
}
