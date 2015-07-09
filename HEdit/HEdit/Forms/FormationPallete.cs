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

        public Size ButtonSize { get { return _button.Size; } }
        
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
       
        /// <summary>
        /// Add's a ship to the program
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="name">The ship type (used for opening a file</param>
        /// <returns>If it succeded or not</returns>
        public bool AddShip(int x, int y, string name="")
        {
            if (_canvas.SafeZone.Contains(x, y))
            {
                int enemyCenter = (_button.Height / 2);

                //Set up the picture box
                PictureBox enemy_rep = new PictureBox();

                if (name == "")
                {
                    enemy_rep.Image = _button.Image;
                    name = _button.Name;
                }
                else
                {
                    switch (name)
                    {
                        case "enemy_fighter":
                            enemy_rep.Image = HEdit.Properties.Resources.enemy_fighter;
                            break;
                        case "bomber":
                            enemy_rep.Image = HEdit.Properties.Resources.bomber;
                            break;
                        case "kamikaze":
                            enemy_rep.Image = HEdit.Properties.Resources.kamikaze;
                            break;
                        default:
                            MessageBox.Show("File is corrupted, found shiptype " + name);
                            return false;
                    }
                }
                enemy_rep.Size = _button.Size;
                enemy_rep.Location = new Point((int)x - enemyCenter, (int)y - enemyCenter);
                enemy_rep.BorderStyle = BorderStyle.FixedSingle;
               
                //Using that info add a new ship to the list
                _shipList.Add(new Ship(x, y, name, enemy_rep));

                //Add the control to the canvas
                _canvas.Controls.Add(enemy_rep);
            }
            else
            {
                return false;
            }
            return true;
        }

        public void PrintShipList()
        {
            for (int i = 0; i < ShipList.Count; i++)
            {
                string p = ShipList[i].ToString();
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
            _canvas.Top = this.Top;
            _canvas.Left = this.Left + this.Width;

            _shipList = new List<Ship>();
        }
        
        #region Tool Strip Buttons
        //When you click on save button
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            DialogResult diagResult = saveFileDialog1.ShowDialog();
            if (diagResult != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            try
            {
                //make a new File Stream, using means dispose of it after this is done
                using (FileStream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    //Make a new binaryWriter, using means dispose of it after this is done
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        //Write a difficulty
                        char difficulty = '\0';
                        switch (this.difficultySelectList.SelectedIndex)
                        {
                            case 0:
                                difficulty = 'e';
                                break;
                            case 1:
                                difficulty = 'm';
                                break;
                            case 2:
                                difficulty = 'h';
                                break;
                        }
                        writer.Write(difficulty);

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
            DialogResult diagResult = openFileDialog1.ShowDialog();
            if(diagResult != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            
            //Reset the whole canvas
            //Clear the lists
            this.Reset();

            Stream inStream = null;
            BinaryReader reader = null;

            string realFileName = openFileDialog1.FileName;

            if (realFileName.EndsWith(".frm") == false)
            {
                realFileName += ".frm";
            }

            try
            {
                //make a new File Stream, using means dispose of it after this is done
                inStream = File.OpenRead(realFileName);

                //Make a new binaryReader, using means dispose of it after this is done
                reader = new BinaryReader(inStream);

                char difficulty = reader.ReadChar();

                switch (difficulty)
                {
                    case 'e':
                        this.difficultySelectList.SelectedIndex = 0;
                        break;
                    case 'm':
                        this.difficultySelectList.SelectedIndex = 1;
                        break;
                    case 'h':
                    default:
                        this.difficultySelectList.SelectedIndex = 2;
                        break;
                }
                //read in the number of ships in the ship list. It isn't nesasary to the rest of the process
                //but if you don't everything will get out of place
                int ship_count = reader.ReadInt32();

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    //Read x
                    int x = reader.ReadInt32();
                    //Read y
                    int y = reader.ReadInt32();
                    //Read the name
                    string name = reader.ReadString();

                    bool result = AddShip(x, y, name);
                    if (result == false)
                    {
                        throw new Exception("File contained invalid ship type, could not continue loading. Check for data corruption");
                    }
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

        private void focusAlways(object sender, EventArgs e)
        {
            this.Select();
        }
    }
}