using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HEdit.Forms;

namespace HEdit
{
    public partial class FormationPallete : Form
    {
        //Attributes
        #region

        /// <summary>
        /// Although not ideal the button contains the following information vital to EveryThing.
        /// Data for the game, the ship's name: _button.Name
        /// Data for placing: _button.Size (50,50)
        /// </summary>
        private Button _button;
        public String ShipType { get { return _button.Name; } }
        
        public PictureBox ShipRepresentation 
        {
            get {
                PictureBox tmp = new PictureBox();
                tmp.Image = _button.Image;
                return tmp;
            }
        }
        
        //A list of ships to keep track of, this will be used later in saving
        private List<Ship> _shipList;
        public List<Ship> ShipList { get { return _shipList; } }
        
        private Canvas _canvas;
        
        #endregion
        public FormationPallete()
        {           
            InitializeComponent();
            this.difficultySelectList.SelectedIndex = 0;
            _button = enemy_fighter;

            this.Reset();
        }
        public void ChangeCoords(int x, int y)
        {
            this.mouse_coords.Text = x + "," + y;
        }

        #region Button Clicks
        private void regularEnemy_Click(object sender, EventArgs e)
        {
            //Set the button
            _button = enemy_fighter;
        }

        private void BomberBeetle_Click(object sender, EventArgs e)
        {
            //Set the button
            _button = bomber;            
        }

        private void Kamikaze_Click(object sender, EventArgs e)
        {
            //Set the button
            _button = kamikaze;            
        }
        #endregion
       
        public void AddShip(double x, double y)
        {
            if (_canvas.SafeZone.Contains((int)x, (int)y))
            {
                int enemyCenter = (_button.Height / 2);

                //Set up the picture box
                PictureBox enemy_rep = new PictureBox();
                enemy_rep.Image = _button.Image;
                enemy_rep.Size = _button.Size;
                enemy_rep.Location = new Point((int)x - enemyCenter, (int)y - enemyCenter);
                enemy_rep.BorderStyle = BorderStyle.FixedSingle;
               
                //Using that info add a new ship to the list
                _shipList.Add(new Ship(x, y, this.ShipType, enemy_rep));

                //Add the control to the canvas
                _canvas.Controls.Add(enemy_rep);
            }
            else
            {
                return;
            }
        }
        
        public void Reset()
        {
            if (_canvas != null)
            {
                _canvas.Close();
            }
            _canvas = new Canvas(this);
            _canvas.Show();
            _shipList = new List<Ship>();
        }
        
        #region Tool Strip Buttons
        //When you click on save button
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                string difficultyText = difficultySelectList.SelectedItem.ToString();
                switch (difficultyText)
                {
                    case "Easy":
                        difficultyText = "e_";
                        break;
                    case "Medium":
                        difficultyText = "m_";
                        break;
                    case "Hard":
                        difficultyText = "h_";
                        break;
                }
                //make a new File Stream, using means dispose of it after this is done
                using (FileStream stream = new FileStream(Directory.GetCurrentDirectory().ToString() + "\\" + difficultyText + this.FileName.Text + ".frm", FileMode.Create))
                {
                    //Make a new binaryWriter, using means dispose of it after this is done
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        //Write the total ship count. This is used to read in the correct number of lines later when the file is read in
                        writer.Write(_shipList.Count);

                        //For the length of the list
                        for (int i = 0; i < _shipList.Count; i++)
                        {
                            //Write x position
                            writer.Write(_shipList[i].X);
                            //Write y position
                            writer.Write(_shipList[i].Y);
                            //Write name
                            writer.Write(_shipList[i].Name);
                        }
                    }
                }
                //Show that it was a success
                MessageBox.Show("Success!");
            }
            catch (FileNotFoundException fE)
            {
                MessageBox.Show(fE.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        //When you click on open
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            //Reset the whole canvas
            //Clear the lists
            this.Reset();

            Stream inStream = null;
            BinaryReader reader = null;
            try
            {
                //make a new File Stream, using means dispose of it after this is done
                inStream = File.OpenRead(Directory.GetCurrentDirectory().ToString() + "\\" + this.FileName.Text + ".frm");

                //Make a new binaryReader, using means dispose of it after this is done
                reader = new BinaryReader(inStream);

                //read in the number of ships in the ship list. It isn't nesasary to the rest of the process
                //but if you don't everything will get out of place
                int ship_count = reader.ReadInt32();

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    //Read x
                    double x = reader.ReadDouble();
                    //Read y
                    double y = reader.ReadDouble();
                    //Read the name
                    string name = reader.ReadString();

                    AddShip(x * _canvas.Width, y * _canvas.Height);
                }
                //Show that it was a success
                MessageBox.Show("Success!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                else if (inStream != null)
                    inStream.Close();
            }
        }

        //When you click on start
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            //Clear the lists
            this.Reset();
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            Help helpForm = new Help();
            helpForm.Show();
        }

        private void FormationPallete_LocationChanged(object sender, EventArgs e)
        {
            _canvas.Top = this.Top;
            _canvas.Left = this.Left + this.Width;
        }
    }
}