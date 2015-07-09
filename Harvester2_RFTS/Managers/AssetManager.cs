/*
 * public class MediaManager
 * @author Theodore Greene
 * 
 * Notes: This is what is known as a Singleton. A Singleton is a little weird, it is a static class with public methods and can only be made once.
 *        This helps reduce the number of global variables while providing functionality.
 *        See also:http://msdn.microsoft.com/en-us/library/ff650316.aspx and http://www.dotnetperls.com/singleton-static
 *        To call any method or get/ set any property say AssetManager.Instance.*Something*
 *        This isn't a replacement for static classes, this is only for when you one want ONE instance.
 * 
 * Version: $1.0.0$
 * 
 * Revisions: 1.0.1(Theodore Greene)
 *          -Created the class and its basic contents
 *            1.2(Theodore Greene)
 *            -Finished working methods FindFiles,FindTextures,FindSpriteFonts
 *            1.2.3(Theodore Greene)
 *            -Added LoadFromSource to read in tool files, its not quite done because it can't read anything but formation but that's all thats done for nowww
 * 
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

    public class AssetManager
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
        private static AssetManager _instance = new AssetManager();

        //AssetManager's Accessor
        public static AssetManager Instance
        {
            get
            {
                //Otherwise return the inStance of
                return _instance;
            }
        }

        #endregion
        /// <summary>
        /// Private contructor ensures nothing can make a new Asset Manager
        /// </summary>
        private AssetManager()
        {
            _loadArray = new String[64];

            _textureDic = new Dictionary<string, Texture2D>();
            _soundDic = new Dictionary<string, SoundEffect>();
            _spriteFontDic = new Dictionary<string, SpriteFont>();

            _easyLevelArray = new List<Formation>();
            _mediumLevelArray = new List<Formation>();
            _hardLevelArray = new List<Formation>();
        }

        public void FillManager(ContentManager contentManager)
        {
            //Get the current directory and store it as a string
            String currentDirectory = Directory.GetCurrentDirectory();

            //Add on the Content Directory
            currentDirectory += "\\Content";
            
            try
            {
                //Try to Get all the files in the current directory of the certain file type and put them in a string 
                _loadArray = Directory.GetFiles(currentDirectory, "*.xnb", SearchOption.AllDirectories);
            
                //Loop through the whole array fixing the paths and loading content
                for (int i = 0; i < AssetManager.Instance.LoadArray.Length; i++)
                {
                    //Remove the large unimportant path infront
                    AssetManager.Instance.LoadArray[i] = AssetManager.Instance.LoadArray[i].Remove(0, currentDirectory.Count() + 1);

                    //Find out where the \ is, used for substring
                    int slashPlace = 0;
                    for (slashPlace = 0; slashPlace < AssetManager.Instance.LoadArray[i].Length; slashPlace++)
                    {
                        if (AssetManager.Instance.LoadArray[i].ToCharArray()[slashPlace] == '\\')
                        {
                            break;
                        }
                    }

                    //Turns something like Folder\\asset.xnb into asset, later on when one would like to set a game piece's graphic they'll use this
                    string key = AssetManager.Instance.LoadArray[i].Substring(slashPlace + 1);
                    key = key.Substring(0, key.Length - 4);

                    //Turns something like Folder\\asset.xnb into Folder\\asset
                    string location = AssetManager.Instance.LoadArray[i].Remove(AssetManager.Instance.LoadArray[i].Length - 4);

                    //Based on the folder name decide which dictionary it should populate
                    switch (AssetManager.Instance.LoadArray[i].Substring(0, slashPlace))
                    {
                        case "Sound":
                            //Pass in the key (file name - .xnb) and the loaded texture
                            AssetManager.Instance.FillDictionaries(key, contentManager.Load<SoundEffect>(location));
                            break;
                        case "Fonts":
                            AssetManager.Instance.FillDictionaries(key, contentManager.Load<SpriteFont>(location));
                            break;
                        case "Movies":
                            //contentManager.Load<Video>(location);
                            break;
                        default:
                            FillDictionaries(key, contentManager.Load<Texture2D>(location));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            FillLevelArrays();
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
        /// Attempts to retrieve a texture from the texture pool
        /// </summary>
        /// <param name="assetName">The file name of the asset you want to attempt to retrieve (without extension)</param>
        /// <returns>The Texture2D if found, null if not found</returns>
        public Texture2D FindTexture(string assetName)
        {
            if (_textureDic.ContainsKey(assetName) == true)
            {
                return this._textureDic[assetName];
            }
            else
            {
                throw new Exception(assetName + "has not been found!");
            }
        }

        /// <summary>
        /// Attempts to retrieve a SpriteFont from the SpriteFont Pool
        /// </summary>
        /// <param name="assetName">The file name of the asset you want to attempt to retrieve (without extension)</param>
        /// <returns>The Texture2D if found, null if not found</returns>
        public SpriteFont FindSpriteFont(string assetName)
        {
            if (_spriteFontDic.ContainsKey(assetName) == true)
            {
                return this._spriteFontDic[assetName];
            }
            else
            {
                throw new Exception(assetName + "has not been found!");
            }
        }
        /// <summary>
        /// Attempts to retrieve a SoundEffect from the SoundEffect Pool
        /// </summary>
        /// <param name="assetName">The file name of the asset you want to attempt to retrieve (without extension)</param>
        /// <returns>The SoundEffect if found, null if not found</returns>
        public SoundEffect FindSound(string assetName)
        {
            if (_soundDic.ContainsKey(assetName) == true)
            {
                return this._soundDic[assetName];
            }
            else
            {
                throw new Exception(assetName + "has not been found!");
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
            currentDirectory += "\\Content\\Formations";

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
                        tempData.x = input.ReadInt32();
                        tempData.y = input.ReadInt32();
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

            if (_easyLevelArray.Count == 0 || _mediumLevelArray.Count == 0 || _hardLevelArray.Count == 0)
            {
                throw new Exception("Not enough levels to run game, ensure your formations folder is in the right place or make more!");
            }
        }
    }
}
       /* //Attributes

        //Arrays
        #region

        //Array of strings to be used through out the various content loading
        private String[] _loadArray;

        //Array of strings to be used to retrive texture values from the dictionary
        private String[] _textureArray;

        //Array of strigns to be used to retrive spritefont values from the dictionary
        private String[] _spritefontArray;

        //Array of strings to be used to retrive spritefont values from the dictionary
        private String[] _audioArray;

        //Variable for the enum
        private static LoadType _theLoadType;

        /// <summary>
        /// Property for the load type
        /// </summary>
        public static LoadType TheLoadType { get { return _theLoadType; } set { _theLoadType = value; } }

        #endregion

        ///<summary>
        ///Set up the Singleton
        ///</summary>
        #region
        //Here is how this works

        //First make a static variable of type Asset Manager
        private static AssetManager _instance;

        //Then make a property so you can access it
        public static AssetManager Instance
        {
            get
            {
                //if it hasn't been initilized
                if (_instance == null)
                {
                    //Make a new one
                    _instance = new AssetManager();
                }

                //Otherwise return the inStance of
                return _instance;
            }
        }

        #endregion
        /// <summary>
        /// Private contructor ensures nothing can make a new Asset Manager
        /// </summary>
        private AssetManager()
        {
            //Initialize the arrays
            //64 is the highest that I think we will ever have of any one element(64 images, 64 sounds, 64 etc) but it can be changed
            _loadArray = new String[64];
            _textureArray = new String[64];
            //I think we probably will never have no more than 8 sprite fonts
            _spritefontArray = new String[8];
            
            _audioArray = new String[64];
        }

        /// <summary>
        /// Goes to the content directory and gets all the files of a certain type
        /// </summary>
        /// <param name="fileExtension">file extension you want to find include the .</param>
        /// <returns>array of strings for use as keys</returns>
        public string[] FindFiles(string fileExtension)
        {
            //Get the current directory and store it as a string
            String currentDirectory = Directory.GetCurrentDirectory();

            //Create a directory info for use
            DirectoryInfo dInfo = new DirectoryInfo(currentDirectory);

            try
            {
                //While there is some directory info
                while (dInfo != null)
                {

                    //Move up
                    dInfo = dInfo.Parent.Parent;

                    //If the current file path is Harvester (The first one
                    if (dInfo.Name == "Harvester")
                    {

                        //It should be at ...\GSD III Project\Harvester
                        //Move into art
                        currentDirectory = dInfo.FullName + "\\HarvesterContent";
                        //Break out of the loop
                        break;
                    }

                }

                //Get all the files in the current directory of the certain file type and put them in a string 
                _loadArray = Directory.GetFiles(currentDirectory, "*" + fileExtension, SearchOption.AllDirectories);

                //Loop through to save only the closet path and no file extension
                for (int i = 0; i < _loadArray.Length; i++)
                {
                    //Remove the large unimportant path infront
                    _loadArray[i] = _loadArray[i].Remove(0, currentDirectory.Count());
                    //Remove the file extenstion

                    //If the extension is png
                    if (fileExtension == ".png")
                    {
                        //take off the last 4 charecters(. + png)
                        _loadArray[i] = _loadArray[i].Remove(_loadArray[i].Count() - 4);
                    }

                    //If it is spritefont
                    if (fileExtension == ".spritefont")
                    {
                        //take off the last 11 charecters . + spritefont
                        _loadArray[i] = _loadArray[i].Remove(_loadArray[i].Count() - 11);
                    }
                    if (fileExtension == ".wav")
                    {
                        //take off the last 4 charecters(. + wav)
                        _loadArray[i] = _loadArray[i].Remove(_loadArray[i].Count() - 4);
                    }
                }

                //If the program is reading in textures
                if (fileExtension == ".png")
                {
                    //Copy it to the array of strings representing textures so that way it wont change when we load something else
                    _loadArray.CopyTo(_textureArray, 0);
                }
                //If the program is reading in spritefonts
                if (fileExtension == ".spritefont")
                {
                    //Copy it to the array of strings representing spritefonts so that way it wont change when we load something else
                    _loadArray.CopyTo(_spritefontArray, 0);
                }
                //If the program is reading in audio wavs
                if (fileExtension == ".wav")
                {
                    _loadArray.CopyTo(_audioArray, 0);
                }
                return _loadArray;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        
        /// <summary>
        /// Return the current directory 
        /// </summary>
        /// <returns>arrayList of items to be used</returns>
        /// <param name="fileName">fileName to read</param>
        /// <param name="loadType">a load type to switch around, possible values are EnemySettings,Formation,GameSettings, and PlayerSetting</param>
        public ArrayList LoadFromSource(string fileName, LoadType loadType)
        {
            ArrayList arrayList = new ArrayList();

            //Move through the directories
            #region
            //Get where you currently are, the bottem
            string currentDirectory = Directory.GetCurrentDirectory();

            //Create directory info for use
            DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory);

            //Move up three directories
            for (int i = 0; i < 3; i++)
            {
                //get the parent of the current directory
                directoryInfo = Directory.GetParent(currentDirectory);

                //set the current directory to the parent (inside of the full path so you can keep moving up
                currentDirectory = directoryInfo.FullName;
            }

            //Go down one level into Mods
            currentDirectory = currentDirectory + "\\Mods";
            #endregion

            #region
            //Load it 

            //Create a general stream
            Stream inStream = null;
            BinaryReader input = null;

            //Extension to be added in
            string fileExtension = " ";

            //Switch on the directories name
            switch (loadType)
            {
                case LoadType.EnemySetting:
                    currentDirectory = currentDirectory + "\\EnemySettings";
                    fileExtension = ".est";
                    break;

                case LoadType.Formation:
                    currentDirectory = currentDirectory + "\\Formations";
                    fileExtension = ".frm";
                    break;

                case LoadType.GameSettings:
                    currentDirectory = currentDirectory + "\\Game Settings";
                    fileExtension = ".gst";
                    break;

                case LoadType.PlayerSetting:
                    currentDirectory = currentDirectory + "\\Player Settings";
                    fileExtension = ".pst";
                    break;
            }

            // Try to make and read data
            try
            {
                inStream = File.OpenRead(currentDirectory + "\\" + fileName + fileExtension);
                input = new BinaryReader(inStream);

                //Read in the number of ships to be used as the for loop condition
                int listCount= input.ReadInt32();


                for (int i = 0; i < listCount; i++)
                {
                    // Read some data
                    int x = input.ReadInt32();
                    int y = input.ReadInt32();
                    String shipType = input.ReadString();

                    //Add the items to the arrayList
                    arrayList.Add(x);
                    arrayList.Add(y);
                    arrayList.Add(shipType);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading" + fileName + ": " + e.Message);
            }
            finally
            {
                if (input != null)
                    input.Close();
                else if (inStream != null)
                    inStream.Close();
            }

            return arrayList;

            #endregion
        }
    }*/
//}
