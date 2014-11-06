using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GSDIIITool
{
    /// <summary>
    /// A Ship class to help with writing
    /// </summary>
    public class Ship
    {
        //Attributes

        //ints for x and y
        private int _x;
        private int _y;

        //string name
        private String _name;
        private PictureBox _representation;

        //Properties for position
        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }

        /// <summary>
        /// Property for the name
        /// </summary>
        public String Name { get { return _name; } set { _name = value; } }
        
        public PictureBox Represenation { get { return _representation; } set { _representation = value; } }
       
        /// <summary>
        /// Constructor for ship
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="name">name of ship</param>
        public Ship(int x, int y, String name, PictureBox representation)
        {
            _x = x;
            _y = y;
            _name = name;
            _representation = representation;
        }
    }
}
