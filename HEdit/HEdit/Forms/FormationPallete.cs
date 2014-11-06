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
using GSDIIITool.Forms;

namespace GSDIIITool
{
    public partial class FormationPallete : Form
    {
        //Attributes
        #region
        private Point canvasCoords;
        private TextBox _fileName;
        private TextBox _filePath;

        //button you clicked
        private Button _button;
        public String ShipType { get { return _button.Name; } }
        public PictureBox ShipRepresentation {
            get {
                PictureBox tmp = new PictureBox();
                tmp.Image = _button.Image;
                return tmp;
            }
        }
        //A list of ships to keep track of, this will be used later in saving
        private List<Ship> _shipList;

        //A list of picture boxes to keep track of, this will be used in removing them
        private List<PictureBox> _pictureBoxList;

        private Canvas c;
        //public TextBox FileName { get { return _fileName; } }
        //public TextBox FilePath { get { return _filePath; } }

        #endregion
        public FormationPallete()
        {          
            _fileName = this.FileName;
            _filePath = new TextBox();
            _filePath.Text = Directory.GetCurrentDirectory().ToString();

            //Thanks to Eli Gazit at this post http://social.msdn.microsoft.com/forums/en-US/winforms/thread/2995a8cf-62af-446e-87ab-75045d670942
            ToolStripMenuItem button = new ToolStripMenuItem();

            button.Size = new Size(0, 0);

            InitializeComponent();
            this.difficultySelectList.SelectedIndex = 0;
                        
            _button = enemy_fighter;

            c = new Canvas(this);
            c.Show();
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
            //Set the name
            //_shipType = enemy_fighter.Name;

            //Activate the mouse
            
        }

        private void BomberBeetle_Click(object sender, EventArgs e)
        {
            //Set the button
            _button = bomber;

            //Set the name
            //_shipType = bomber.Name;
            //Activate the mouse
            
        }

        private void Kamikaze_Click(object sender, EventArgs e)
        {
            //Set the button
            _button = kamikaze;

            //Set the name
            //_shipType = kamikaze.Name;

            //Activate the mouse
            
        }
        
        ///<summary>
        ///Clicking the Hero's button
        ///</summary>
        //Toggle between showing the refrence hero image
        private void PhxMkII_Click(object sender, EventArgs e)
        {
            //HeroShipPicture.Visible = !HeroShipPicture.Visible;
        }
        #endregion
        
                
        private void FormationPallete_Load(object sender, EventArgs e)
        {

        }

        private void FormationPallete_MouseMove(object sender, MouseEventArgs e)
        {
            mouse_coords.Text = this.PointToScreen(e.Location).X + "," + this.PointToScreen(e.Location).Y;
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
                using (FileStream stream = new FileStream(this._filePath.Text + "\\" + difficultyText + this.FileName.Text + ".frm", FileMode.Create))
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
                MessageBox.Show("The message can't be opened " + fE.Message);
            }

            catch (Exception ex)
            {
                MessageBox.Show("The message can't be opened " + ex.Message);
            }
        }

        //When you click on open
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            //Make each box invisible
            foreach (PictureBox p in _pictureBoxList)
            {
                p.Visible = false;
            }
            //Reset the whole canvas
            //Clear the lists
            _shipList.Clear();
            _pictureBoxList.Clear();

            Stream inStream = null;
            BinaryReader reader = null;
            try
            {
                //make a new File Stream, using means dispose of it after this is done

                inStream = File.OpenRead(this._filePath.Text + "\\" + this.FileName.Text + ".frm");

                //Make a new binaryReader, using means dispose of it after this is done
                reader = new BinaryReader(inStream);

                //read in the number of ships in the ship list. It isn't nesasary to the rest of the process
                //but if you don't everything will get out of place
                int temp = reader.ReadInt32();

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    //Read x
                    int x = reader.ReadInt32();
                    //Read y
                    int y = reader.ReadInt32();
                    //Read the name
                    string name = reader.ReadString();

                    //Using that info add a new ship to the list
                   // _shipList.Add(new Ship(x, y, name));
                }

                //For every ship in the list
                for (int i = 0; i < _shipList.Count; i++)
                {
                    var list = GSDIIITool.Properties.Resources.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo("en-us"),true,true);

                    //Make a new enemy PictureBox
                    PictureBox enemy = new PictureBox();

                    //Set the size
                    enemy.Size = new Size(100, 100);

                    //Set the border
                    enemy.BorderStyle = BorderStyle.FixedSingle;

                    //Add the control
                    Controls.Add(enemy);

                    //Big Switch around names
                    #region
                    switch (_shipList[i].Name)
                    {
                        case ("enemy_fighter"):
                            enemy.Image = GSDIIITool.Properties.Resources.enemy_fighter;
                            //Set the location
                            //Subtract half the picturebox's width and subtract 27 from the Y to account for the top bar and also half the height to place it in the center
                            //Exactly where the imported ship is
                            enemy.Location = new Point(_shipList[i].X - enemy.Width / 2, _shipList[i].Y - 27 - (enemy.Height / 2));
                            //Show the box
                            enemy.Show();
                            break;
                        case ("bomber"):
                            enemy.Image = GSDIIITool.Properties.Resources.bomber;
                            //Set the location
                            //Subtract half the picturebox's width and subtract 27 from the Y to account for the top bar and also half the height to place it in the center
                            //Exactly where the imported ship is
                            enemy.Location = new Point(_shipList[i].X - enemy.Width / 2, _shipList[i].Y - 27 - (enemy.Height / 2));
                            //Show the box
                            enemy.Show();
                            break;
                        /*case ("imperial_boss"):
                            enemy.Image = GSDIIITool.Properties.Resources.Ship___Imperial_v2__Boss_;
                            //Set the location
                            //Subtract half the picturebox's width and subtract 27 from the Y to account for the top bar and also half the height to place it in the center
                            //Exactly where the imported ship is
                            enemy.Location = new Point(_shipList[i].X - enemy.Width / 2, _shipList[i].Y - 27 - (enemy.Height / 2));
                            //Show the box
                            enemy.Show();
                            break;*/
                        case ("kamikaze"):
                            enemy.Image = GSDIIITool.Properties.Resources.kamikaze;
                            //Set the location
                            //Subtract half the picturebox's width and subtract 27 from the Y to account for the top bar and also half the height to place it in the center
                            //Exactly where the imported ship is
                            enemy.Location = new Point(_shipList[i].X - enemy.Width / 2, _shipList[i].Y - 27 - (enemy.Height / 2));
                            //Show the box
                            enemy.Show();
                            break;
                    }
                    #endregion

                    //For testing purposes you can use output to double check what is in the text boxes on the panel
                    //Console.WriteLine(_newShip.X.ToString() + " " + _newShip.Y.ToString());
                    //Add the enemy to the list of picture boxes
                    _pictureBoxList.Add(enemy);
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
            //Make each box invisible
            foreach (PictureBox p in _pictureBoxList)
            {
                p.Visible = false;
            }
            //Clear the lists
            _shipList.Clear();
            _pictureBoxList.Clear();
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            Help helpForm = new Help();
            helpForm.Show();
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //make a new stream writer
            StreamWriter output = null;

            try
            {
                //Create the stream writer
                output = new StreamWriter(this._filePath.Text + "\\" + this.FileName.Text + ".txt");

                //Write some info
                output.WriteLine("Start");

                //Write the ship list number
                output.WriteLine("Number of ships: " + _shipList.Count.ToString());
                output.WriteLine("Difficulty: " + difficultySelectList.SelectedItem.ToString());

                for (int i = 0; i < _shipList.Count; i++)
                {
                    //Write spacing
                    output.WriteLine();
                    //Write name, I know it is first when all the other times it was last but this is for readability
                    output.WriteLine("Name: " + _shipList[i].Name);
                    //Write X
                    output.WriteLine("X Position: " + _shipList[i].X);
                    //Write Y
                    output.WriteLine("Y Position: " + _shipList[i].Y);
                }
                //Write spacing
                output.WriteLine();
                //Write some other info
                output.WriteLine("End");

                //Show that it was a success
                MessageBox.Show("Success!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error writing file: " + ex.Message);
            }
            finally
            {
                if (output != null)
                {
                    output.Close();
                }
            }
        }
        #endregion

        private void FormationPallete_LocationChanged(object sender, EventArgs e)
        {
            c.Top = this.Top;
            c.Left = this.Left + this.Width;
        }
    }
}
