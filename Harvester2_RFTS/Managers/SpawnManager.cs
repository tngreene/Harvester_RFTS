/*
 * Public class SpawnWaves
 * @author Theodore Greene
 * 
 * Version: $1.0.0$
 * 
 * Revisions: 
 *          1.0.1 (Theodore Greene)
 *              -Added attributes and the basics of file reading
 *          1.0.2 (Theodore Greene)
 *              -Added Methods, major overhalls into ModLoader
 *          1.7 (Theodore Greene)
 *              -SpawnSingle, made a singleton, other major overhauls
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Harvester
{
    public enum SpawnType
    {
        SpawnSingle,
        SpawnASetWave,
        SpawnRandomWave,
        EasyWaves,
        MediumWaves,
        HardWaves
    }
    /// <summary>
    /// This class handles the way waves will spawn in the levels (tentative)
    /// </summary>
    public class SpawnManager
    {
        //Timers for spawning
        //General
        private double fighterTime = 3;
        
        //Kamikaze
        private double kamikazeTime = 6;
        
        private Random rand = new Random();
        //Bomber
        private double bomberTimer;
        
        //Random Object
        
        
        //Enemy
        private Enemy enemy;

        //Number to keep track of the total enemies
        private int enemiesTotal;

        //The player ship
        private SpawnType spawnType;

        private PlayerShip playerShip;

        //Properties
        public SpawnType SpawnType
        {
            get { return spawnType; }
            set { spawnType = value; }
        }
        #region Set up the Singleton
        //Here is how this works

        //First make a static variable of type SpawnManager
        private static SpawnManager _instance;

        //Then make a property so you can access it
        public static SpawnManager Instance
        {
            get
            {
                //if it hasn't been initilized
                if (_instance == null)
                {
                    //Make a new one
                    _instance = new SpawnManager();
                }

                //Otherwise return the instance of
                return _instance;
            }
        }
        #endregion
        /// <summary>
        /// Private contructor ensures nothing can make a new Asset Manager
        /// </summary>
        private SpawnManager()
        {
            /*The time delay is lower because the system also relys on random values, to make everything match up is harder
             * Because these values get changed later, these could be set higher to allow a buffer for when the player 
             * starts so that they don't start facing enemies right away. That might be too easy though
            */
            fighterTime = 1;
            kamikazeTime = 2;
            bomberTimer = rand.Next(1,10);
            playerShip = PlayerMgr.Inst().PlayerShip;

            //Set the enemy total to 0
            enemiesTotal = 0;
        }

        #region SpawnSingle
        /// <summary>
        /// SpawnSingle Spawns single units in random places
        /// </summary>
        /// <param name="frequency">The number of times to call this, keep this low</param>
        /// <param name="amount">The highest number of units allowed, also keep this low</param>
        /// <param name="shipType">The type of ship to spawn, we use this instead of timers because other wise the bomber that takes the longest won't spawn, the short times will spawn first</param>
        /// <returns>Returns a list of Enemies</returns>
        public Enemy SpawnSingle(int fighterFrequency, int kamikazeFrequency, int bomberFrequency)
        {
            //Temp Ship type
            ShipType tempShipType = ShipType.enemy_fighter;
            //This while loop is made so you can exit the ifs and continue on
            while (true)
            {
                //Set the ship type based on the frequency, place the least probably at the top and go down the list        
                if (rand.Next(0, 100) <= bomberFrequency)
                {
                    //set the ship type
                    tempShipType = ShipType.bomber;
                    //Exit the loop
                    break;
                }
                if (rand.Next(0, 100) <= kamikazeFrequency)
                {
                    //set the ship type
                    tempShipType = ShipType.kamikaze;
                    //Exit the loop
                    break;
                }
                if (rand.Next(0, 100) <= fighterFrequency)
                {
                    //set the ship type
                    tempShipType = ShipType.enemy_fighter;
                    //Exit the loop
                    break;
                }

                //If all else fails this makes sure you don't get stuck in an infinte loop
                break;
            }
                
                //switch around the ship type
                switch (tempShipType)
                {
                    //spawns the fighter enemies
                    case ShipType.enemy_fighter:

                        //create a new enemy fighter
                        enemy = new EnemyFighter(new Rectangle((rand.Next(50, Game.WIDTH) - 50), rand.Next(50, Game.HEIGHT / 2), 50, 50), AssetMgr.Inst().TextureDic["enemy_fighter"], 3, 100, AssetMgr.Inst().TextureDic["enemy_bullet"], 25, 3);

                        //if there is an enemy
                        if (enemy != null)
                        {
                            //Increase the number of enemies
                            enemiesTotal++;
                            //Return the enemy
                            return enemy;
                        }
                        break;

                    //spawns the kamikaze fighters, read above
                    case ShipType.kamikaze:

                        enemy = new Kamikaze(new Rectangle(rand.Next(50, Game.WIDTH), -55, 50, 50), AssetMgr.Inst().TextureDic["kamikaze"], 4, 50, AssetMgr.Inst().TextureDic["enemy_bullet"], 0, playerShip);
                        if (enemy != null)
                        {
                            enemiesTotal++;
                            return enemy;
                        }
                        break;

                    case ShipType.bomber:
                        //Read above
                        //spawns bomber
                        enemy = new Bomber(new Rectangle(-10, rand.Next(50, Game.HEIGHT / 2 - 100), 50, 50), AssetMgr.Inst().TextureDic["bomber"], 2, 100, AssetMgr.Inst().TextureDic["bomb"], 100, playerShip);

                        if (enemy != null)
                        {
                            
                            enemy = new Bomber(new Rectangle(-10, rand.Next(50, Game.HEIGHT / 2 - 100) , 50, 50), AssetMgr.Inst().TextureDic["bomber"],2, 100, AssetMgr.Inst().TextureDic["bomb"],100, playerShip);
                            
                            if (enemy != null)
                            {
                                bomberTimer = rand.Next(1,50);
                                enemiesTotal++;
                                return enemy;
                            }
                        }

                        break;
                }
            
            return null;
        }
        #endregion

        #region SpawnASetWave
        /// <summary>
        /// Pass the file name to the AssetManager to get back data
        /// Use the data to create formation as a list of enemies
        /// Use the list of enemy ships to spawn them
        /// </summary>
        /// <param name="frequency">A number to decide the frequency(time) of the waves</param>
        /// <param name="fileName">The file name to be used</param>
        /*public List<Enemy> SpawnASetWave(int frequency, string fileName)
        {
            ArrayList enemyLoadList = new ArrayList();
            //A list of enemies to be filled
            if (Engine.currentLevel == Engine.l1)
            {
                enemyLoadList = AssetManager.Instance.LoadFromSource(fileName, LoadType.Easy);
            }
            if (Engine.currentLevel == Engine.l2)
            {
                enemyLoadList = AssetManager.Instance.LoadFromSource(fileName, LoadType.Medium);
            }
            if (Engine.currentLevel == Engine.l3)
            {
                enemyLoadList = AssetManager.Instance.LoadFromSource(fileName, LoadType.Hard);
            }
            //The final list of completed enemies to be spawned
            List<Enemy> finalEnemyList = new List<Enemy>();

            //For the length of the array list
            for (int i = 0; i < enemyLoadList.Count; i += 3)
            {
                //Parse the string to find what enum it is, no try catch so be careful. There shouldbe no way you can mess it up but file io is strange
                ShipType loadedShipType = (ShipType)Enum.Parse(typeof(ShipType), (string)enemyLoadList[i + 2]);

                //Switch off the load type
                switch (loadedShipType)
                {
                    //add a ship to the list since the pattern in enemyLoadList is int,int,string you do [i],[i+1],[i+2],[i+3] then increment by three to get the next quad
                    case ShipType.enemy_fighter:
                        Enemy tempEnemyFighter = new EnemyFighter(new Rectangle((int)enemyLoadList[i], (int)enemyLoadList[i + 1], 50, 50), AssetManager.Instance.FindTexture((string)enemyLoadList[i + 2]), 3.0, 100, AssetManager.Instance.TextureDic["enemy_bullet"],5, 3);
                        finalEnemyList.Add(tempEnemyFighter);
                        break;
                    case ShipType.kamikaze:

                        Enemy tempKamikaze = new Kamikaze(new Rectangle((int)enemyLoadList[i], (int)enemyLoadList[i + 1], 50, 50), AssetManager.Instance.FindTexture((string)enemyLoadList[i + 2]), 4, 100, AssetManager.Instance.TextureDic["kamikaze"],50, playerShip);
                        finalEnemyList.Add(tempKamikaze);
                        break;

                    case ShipType.bomber:
                        Enemy tempBomber = new Bomber(new Rectangle((int)enemyLoadList[i], (int)enemyLoadList[i + 1], 50, 50), AssetManager.Instance.FindTexture((string)enemyLoadList[i + 2]), 2, 100, AssetManager.Instance.TextureDic["bomb"], 100, playerShip);
                        finalEnemyList.Add(tempBomber);
                        break;

                    case ShipType.imperial_boss:
                        break;
                }
            }
            return finalEnemyList;
        }*/
        #endregion

        #region SpawnARandomWave
        /// <summary>
        /// SpawnsARandomWave
        /// Use the data to create formation as a list of enemies
        /// Use the list of enemy ships to spawn them
        /// </summary>
        /// <param name="frequency">A number to decide the frequency(time) of the waves</param>
        /// <param name="amount">A number to decide the number waves at a time</param>
        /// <param name="fileName">The file name to be used</param>
        public List<Enemy> SpawnARandomWave(double frequency, SpawnType difficulty)
        {
            List<Formation> formationList = new List<Formation>();
            switch (difficulty)
            {
                case SpawnType.EasyWaves:
                    formationList = AssetMgr.Inst().EasyLevelArray;
                    break;
                case SpawnType.MediumWaves:
                    formationList = AssetMgr.Inst().MediumevelArray;
                    break;
                case SpawnType.HardWaves:
                    formationList = AssetMgr.Inst().HardLevelArray;
                    break;
            }

            //If a random number is less than or equal to the frequency do the rest
            if (rand.NextDouble() <= frequency)
            {
                Formation randForm = formationList[rand.Next(0, formationList.Count)];
                //A list of enemies to be filled
                //List<Formation> enemyLoadList = AssetMgr.Inst().LoadFromSource(fileName, LoadType.Formation);

                //The final list of completed enemies to be spawned
                List<Enemy> finalEnemyList = new List<Enemy>();

                //For the length of the array list
                for (int i = 0; i < randForm.ships.Count; i += 3)
                {
                    //Parse the string to find what enum it is, no try catch so be careful. There shouldbe no way you can mess it up but file io is strange
                    ShipType loadedShipType = (ShipType)Enum.Parse(typeof(ShipType), randForm.ships[i].shipName);

                    //Switch off the load type
                    switch (loadedShipType)
                    {
                        //add a ship to the list since the pattern in enemyLoadList is int,int,string you do [i],[i+1],[i+2],[i+3] then increment by three to get the next quad
                        case ShipType.enemy_fighter:
                            Enemy tempEnemyFighter = new EnemyFighter(new Rectangle(randForm.ships[i].x, randForm.ships[i].y, 50, 50), AssetMgr.Inst().TextureDic[randForm.ships[i].shipName], 3.0, 100, AssetMgr.Inst().TextureDic["enemy_bullet"], 5, 3);
                            finalEnemyList.Add(tempEnemyFighter);
                            break;
                        case ShipType.kamikaze:

                            Enemy tempKamikaze = new Kamikaze(new Rectangle(randForm.ships[i].x, randForm.ships[i].y, 50, 50), AssetMgr.Inst().TextureDic[randForm.ships[i].shipName], 4, 100, AssetMgr.Inst().TextureDic["kamikaze"], 50, playerShip);
                            finalEnemyList.Add(tempKamikaze);
                            break;

                        case ShipType.bomber:
                            Enemy tempBomber = new Bomber(new Rectangle(randForm.ships[i].x, randForm.ships[i].y, 50, 50), AssetMgr.Inst().TextureDic[randForm.ships[i].shipName], 2, 100, AssetMgr.Inst().TextureDic["bomb"], 100, playerShip);
                            finalEnemyList.Add(tempBomber);
                            break;

                        case ShipType.imperial_boss:
                            break;
                    }
                }
                return finalEnemyList;
            }
            List<Enemy> nothingList = new List<Enemy>();
            //Otherwise return nothing
            return nothingList;
        }
        #endregion
    }
}