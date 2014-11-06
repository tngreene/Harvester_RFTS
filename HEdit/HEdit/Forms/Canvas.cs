using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GSDIIITool.Forms
{
    public partial class Canvas : Form
    {
        private Rectangle _canvasRect;
        private Rectangle _safeZone;
        private PictureBox _heroReference;
        private FormationPallete _fRef;
        private List<Ship> _shipList;

        public Canvas(FormationPallete fp)
        {
            InitializeComponent();
            _fRef = fp;
            _canvasRect = new Rectangle(0, 0, 1024,576);//16:9
            _safeZone = new Rectangle(_canvasRect.Left + _fRef.ShipRepresentation.Width / 2, _canvasRect.Top + _fRef.ShipRepresentation.Height / 2, _canvasRect.Width - _fRef.ShipRepresentation.Width, _canvasRect.Height - _fRef.ShipRepresentation.Height);
            _heroReference = new PictureBox();
            _heroReference.Image = Properties.Resources.phoenix;
            _heroReference.Size = Properties.Resources.phoenix.Size;
            _heroReference.BorderStyle = BorderStyle.FixedSingle;
            _heroReference.Location = new Point(_canvasRect.Width / 2 + _canvasRect.Left - _heroReference.Image.Width / 2, _canvasRect.Height - _heroReference.Image.Height - 5);
            _heroReference.Show();
            Controls.Add(_heroReference);
            _shipList = new List<Ship>();
        }

        public void AddShip(int x, int y)
        {
            Point p = new Point(x, y);
            if (_fRef.ShipType == "PhxMkII")
            {

            }
            else if (this._safeZone.Contains(p))
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

        /*/// <summary>
        /// Create an enemy
        /// </summary>
        /// <param name="shipType">the ship name to be passed in</param>
        /// <param name="buttonID"> the buttonID that we are using</param>
        public void AddEnemy(Button buttonID, string shipType)
        {
            Point pLocal = this.PointToClient(MousePosition);
            //Create a ship at the location where the mouse is and with the name passed in, -300 in the x and -10 in the y because the canvas is off set by that much
            _newShip = new Ship(pLocal.X, pLocal.Y, shipType);

            //Set up picturebox properties
            #region
            PictureBox enemy = new PictureBox();
            enemy.Size = _button.Size;
            enemy.BorderStyle = BorderStyle.FixedSingle;

            Controls.Add(enemy);

            //Picture box settings
            enemy.Image = buttonID.Image;

            //Set the location
            //Subtract half the picturebox's width and subtract 27 from the Y to account for the top bar and also half the height to place it in the center
            //Exactly where your center mouse is
            enemy.Location = new Point((_newShip.X - enemy.Width / 2), (_newShip.Y - 27 - (enemy.Height / 2)));

            //Add the enemy to the list of picture boxes
            _pictureBoxList.Add(enemy);
            #endregion


            _shipList.Add(_newShip);

            //Output for testing purposes you can use this to double check what is in the text boxes on the panel
            Console.WriteLine(_newShip.X.ToString() + " " + _newShip.Y.ToString());

        }*/
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
