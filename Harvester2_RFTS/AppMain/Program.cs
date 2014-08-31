using System;

namespace Harvester
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            AssetMgr.Inst();
            using (var game = new Game())
                game.Run();
        }
        
    }
#endif
}

