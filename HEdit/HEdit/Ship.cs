using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace HEdit
{
    /// <summary>
    /// A Ship class to help with writing
    /// </summary>
    public class Ship
    {
        //Attributes

        //ints for x and y
        private double _x;
        private double _y;

        //string name
        private String _name;
        private PictureBox _representation;

        //Properties for position
        public double X { get { return _x; } set { _x = value; } }
        public double Y { get { return _y; } set { _y = value; } }

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
        public Ship(double x, double y, String name, PictureBox representation)
        {
            _x = x;
            _y = y;
            _name = name;
            _representation = representation;
        }
    }
}
