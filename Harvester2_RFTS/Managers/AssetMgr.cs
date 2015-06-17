/*
 * public class AssetMgr
 * @author Theodore Greene
 * 
 * Notes: This class is supposed to gather and hold all the asssets (Textures, Sounds, Fonts, Levels, etc) for this game.
 * It can be accessed by using AssetMgr.Inst().ThingEtc...
 * The class is set up as a singleton
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;

namespace Harvester
{
    /// <summary>
    /// A struct that represents a the x,y,and name (type) of a ship
    /// </summary>
    public struct ShipData
    {
        public int x;
        public int y;
        public string shipName;

        public ShipData(int pX, int pY, string pName)
        {
            x = pX;
            y = pY;
            shipName = pName;
        }
    }

    /// <summary>
    /// A formation is a list of shipData
    /// </summary>
    public struct Formation
    {
        public List<ShipData> ships;
    }
    public enum AssetType
    {
        Sound,
        SpriteFont,
        Texture2D
    }

    public enum LoadType
    {
        EnemySetting,
        Formation,
        GameSettings,
        PlayerSetting,
        Highscore,
        //Level Difficulties
        Easy,
        Medium,
        Hard,
        Boss
    }
    
    public class AssetMgr
    {
        //Attributes
        #region Arrays

        //Array of strings to be used through out the various content loading
        private String[] _loadArray;
        public String[] LoadArray { get { return _loadArray; } }

        //A level array is a list of formations, each formation is a list of ships
        private List<Formation> _easyLevelArray;
        public List<Formation> EasyLevelArray { get { return _easyLevelArray; } }
        private List<Formation> _mediumLevelArray;
        public List<Formation> MediumevelArray { get { return _mediumLevelArray; } }
        private List<Formation> _hardLevelArray;
        public List<Formation> HardLevelArray { get { return _hardLevelArray; } }
        
        //Texture Dictionary
        private Dictionary<String, Texture2D> _textureDic;
        public Dictionary<String, Texture2D> TextureDic { get { return _textureDic; } }
        
        //Spritefont Dictionary
        private Dictionary<String, SpriteFont> _spriteFontDic;
        public Dictionary<String, SpriteFont> SpriteFontDic { get { return _spriteFontDic; } }

        //Audio Dictionary
        private Dictionary<String, SoundEffect> _soundDic;
        public Dictionary<String, SoundEffect> SoundDic { get { return _soundDic; } }

        //Variable for the enum
        private static LoadType _theLoadType;

        /// <summary>
        /// Property for the load type
        /// </summary>
        public static LoadType TheLoadType { get { return _theLoadType; } set { _theLoadType = value; } }

        #endregion

        #region Set up the Singleton
       
        //Instance of the assetManager
        private static AssetMgr _instance;

        //AssetManager's Accessor
        public static AssetMgr Inst()
        {
            //if it hasn't been initilized
            if (_instance == null)
            {
                //Make a new one
                _instance = new AssetMgr();
            }

            //Otherwise return the inStance of
            return _instance;
        }

        #endregion
        /// <summary>
        /// Private contructor ensures nothing can make a new Asset Manager
        /// </summary>
        private AssetMgr()
        {
            _loadArray = new String[64];
           
            _textureDic = new Dictionary<string, Texture2D>();
            _soundDic = new Dictionary<string, SoundEffect>();
            _spriteFontDic = new Dictionary<string, SpriteFont>();

            _easyLevelArray = new List<Formation>();
            _mediumLevelArray = new List<Formation>();
            _hardLevelArray = new List<Formation>();

            //Get the current directory and store it as a string
            String currentDirectory = Directory.GetCurrentDirectory();

            //Add on the Content Directory
            currentDirectory += "\\Content";

            try
            {
                //Try to Get all the files in the current directory of the certain file type and put them in a string 
                _loadArray = Directory.GetFiles(currentDirectory, "*.xnb", SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Fills up the various content dictionaries
        /// </summary>
        public void FillDictionaries(string key, object value)
        {
            //Check the object's type and depending on that certain type
            //Add it to that type's dictionary
            if (value is SoundEffect)
            {
                _soundDic.Add(key, (SoundEffect)value);
                
            }
            else if (value is SpriteFont)
            {
                _spriteFontDic.Add(key, (SpriteFont)value);
            }
            else if (value is Texture2D)
            {
                _textureDic.Add(key, (Texture2D)value);
            }
            else
            {
                throw new Exception("Type" + value + " not allowed!");
            }
        }

        /// <summary>
        /// Fills up the Easy, Medium, and Hard Level Arrays
        /// </summary>
        public void FillLevelArrays()
        {
            /* 1.) Get all of the .frm files in the folder
             * 2.) For all the files in the loadArray
             *      a.) Attempt to read that ship data
             *      b.) Add it to a formation's list of ships
             *      c.) Based on the first letter of the file name add it to a certain difficulty level array
             * 3.) Close out the FileIO stream readers and such
             */
            string currentDirectory = Directory.GetCurrentDirectory();
            currentDirectory += "\\Content\\Mods\\Formations";

            //Array of strings of levels to load
            String[] _loadLevelArray = Directory.GetFiles(currentDirectory, "*.frm", SearchOption.AllDirectories);

            //Create a general stream
            Stream inStream = null;
            BinaryReader input = null;

            // Try to make and read data
            try
            {
                for (int i = 0; i < _loadLevelArray.Length; i++)
                {
                    inStream = File.OpenRead(_loadLevelArray[i]);
                    input = new BinaryReader(inStream);

                    //Temporary ship data
                    ShipData tempData = new ShipData();

                    //Temporary formation that will later be added to a list somewhere
                    Formation tempForm;
                    tempForm.ships = new List<ShipData>();
                    
                    char difficulty = input.ReadChar();
                    //Read in the number of ships to be used as the for loop condition
                    int listCount = input.ReadInt32();

                    for (int j = 0; j < listCount; j++)
                    {
                        // Read some data
                        //HARDCODED
                        tempData.x = (int)(input.ReadDouble() * Game.ScreenWidth);
                        tempData.y = (int)(input.ReadDouble() * Game.ScreenHeight);
                        tempData.shipName = input.ReadString();

                        //Add this ship to the current formation
                        tempForm.ships.Add(tempData);
                    }

                    //If the first letter is e
                    if (difficulty == 'e')
                    {
                        _easyLevelArray.Add(tempForm);
                    }
                    //If the first letter is m
                    else if (difficulty == 'm')
                    {
                        _mediumLevelArray.Add(tempForm);
                    }
                    else if (difficulty == 'h')
                    {
                        _hardLevelArray.Add(tempForm);
                    }
                    else
                    {
                        continue;
                    }

                    input.Close();
                    inStream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading" + inStream + ": " + e.Message);
            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                }
                if (inStream != null)
                {
                    inStream.Close();
                }
            }
        }
    }
}