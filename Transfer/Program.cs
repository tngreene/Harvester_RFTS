using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Transfer
{
    /* .frm - Formation File Spec
     * 1 char, with potential values of e,m,h
     * 1 int, for the number of ships in the formation
     * An array of a "Ship" struct of 2 doubles (x,y) and a string, with potential values of "enemy_fighter", "bomber", "kamikaze"
     * which is as long as the number of ships in the formation
     * 
     * The x and y position represents a relative position for a 16:9 scale
     * char (difficulty:e,m,h)
     * int (shipCount: n >= 0)
     * array[shipCount] (list of ships: ship<double, double,string> (relative_x, relative_y, shiptype:"enemy_fighter" , "bomber" , "kamikaze"))
     * So, ex
     */
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fStream = null;
            BinaryReader reader = null;
            BinaryWriter writer = null;

            try
            {
                string[] allFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\ToConvert", "*.frm");

                int ship_count = 0;
                int x = 0;
                int y = 0;
                string name = "";
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Converted");
                foreach (string s in allFiles)
                {
                    fStream = new FileStream(s, FileMode.Open);
                    reader = new BinaryReader(fStream);
                    string newFile = s;
                    newFile = newFile.Insert(s.Length - 1 - 3, "_hd");
                    newFile = newFile.Replace("\\ToConvert\\", "\\Converted\\");
                    writer = new BinaryWriter(new FileStream(newFile, FileMode.Create));
                    char difficulty = Path.GetFileName(s)[0];
                    //Write the first letter as the difficulty
                    writer.Write((char)difficulty);

                    ship_count = reader.ReadInt32();
                    writer.Write((Int32)ship_count);

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        //Read x
                        x = reader.ReadInt32();
                        //Read y
                        y = reader.ReadInt32();
                        //Read the name
                        name = reader.ReadString();

                        double hd_x = x;
                        double hd_y = y;
                        hd_x /= 1920.00;
                        hd_y /= 1080;
                        writer.Write((double)hd_x);
                        writer.Write((double)hd_y);
                        writer.Write((string)name);
                    }
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
            finally
            {
                if (fStream != null)
                {
                    fStream.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
