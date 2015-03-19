using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HEdit.Forms
{
    public partial class Canvas : Form
    {
        private Rectangle _canvasRect;
        private Rectangle _safeZone;
        
        private FormationPallete _fRef;

        private List<Ship> _shipList;
        public List<Ship> ShipList { get { return _shipList; } }
        
        public Canvas(FormationPallete fp)
        {
            InitializeComponent();
            _fRef = fp;
            _canvasRect = new Rectangle(0, 0, 1024,576);//16:9
            _safeZone = new Rectangle(_canvasRect.Left + _fRef.ShipRepresentation.Width / 2, _canvasRect.Top + _fRef.ShipRepresentation.Height / 2, _canvasRect.Width - _fRef.ShipRepresentation.Width, _canvasRect.Height - _fRef.ShipRepresentation.Height);
            
            PictureBox hero = new PictureBox();
            hero.Image = Properties.Resources.phoenix;
            hero.Size = Properties.Resources.phoenix.Size;
            hero.BorderStyle = BorderStyle.FixedSingle;
            hero.Location = new Point(_canvasRect.Width / 2 + _canvasRect.Left - hero.Image.Width / 2, _canvasRect.Height - hero.Image.Height - 5);
            hero.Show();
            Controls.Add(hero);

            _shipList = new List<Ship>();
        }

        public void AddShip(int x, int y)
        {
            Point p = new Point(x, y);
            if (this._safeZone.Contains(p))
            {
                /* Get the current clicked button from the FP
                 * Create ship at that x and y with that type
                 * Add the ship to list
                 * Add the ship's representation to the list of controls
                 * */
                string currentType = _fRef.ShipType;
                PictureBox picture = new PictureBox();

                picture.Image = _fRef.ShipRepresentation.Image;
                picture.Size = _fRef.ShipRepresentation.Image.Size;
                picture.Location = new Point(x - picture.Width / 2, y - (picture.Height / 2));
                picture.BorderStyle = BorderStyle.FixedSingle;

                //Add the control
                Controls.Add(picture);
                Ship tmp = new Ship(x, y, currentType,picture);
                _shipList.Add(tmp);
            }
            else
            {
                return;
            }
        }

        private void forceCanvasPaint_Paint(object sender, PaintEventArgs e)
        {
            //Create a graphics object
            System.Drawing.Graphics graphics;
            graphics = CreateGraphics();

            //Draw the filled in rectangle
            graphics.FillRectangle(new System.Drawing.SolidBrush(Color.White), _canvasRect);


            //Draw the grid columns
            int numSubDivsC = 12;
            int numSubDivsR = 7;
            Rectangle gridSubDivSize = new Rectangle(_canvasRect.Location, new Size(_canvasRect.Width / numSubDivsC, _canvasRect.Height / numSubDivsR));
            int y = gridSubDivSize.Location.Y;
            for (int i = 0; i < numSubDivsR; i++)
            {
                int x = _canvasRect.Left + 2;
                gridSubDivSize.Location = new Point(x, y);
                for (int j = 0; j < numSubDivsC; j++)
                {
                    //Draw the current rectangle
                    graphics.DrawRectangle(new Pen(Color.DarkGray, 2), gridSubDivSize);

                    //Shift the rectangle over by the width of the rectangle
                    x += gridSubDivSize.Width;
                    //Set the new location
                    gridSubDivSize.Location = new Point(x, y);
                }
                y += gridSubDivSize.Height;
            }
            //Draw the boarder
            //Create graphics for this control
            graphics.DrawRectangle(new Pen(Color.Red, 2), _safeZone);
            graphics.DrawRectangle(new Pen(Color.Black, 2), _canvasRect);
            graphics.Dispose();
        }
        
        
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            _fRef.ChangeCoords(e.X, e.Y);
        }

        private void Canvas_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse_e = (MouseEventArgs)e;
            AddShip(mouse_e.X, mouse_e.Y);
        }

    }
}
