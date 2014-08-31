/*
 * Public static class LevelManager
 * @author Derrick Hunt / Peter O'Neal
 * 
 * Version: $1.0.0$
 * 
 * Revisions:
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harvester
{
    public class LevelMgr
    {
        #region Set up the Singleton

        //Instance of the assetManager
        private static LevelMgr _instance;

        //AssetManager's Accessor
        public static LevelMgr Inst()
        {
            //if it hasn't been initilized
            if (_instance == null)
            {
                //Make a new one
                _instance = new LevelMgr();
            }

            //Otherwise return the inStance of
            return _instance;
        }

        private LevelMgr()
        {

        }
        #endregion
    }
}
