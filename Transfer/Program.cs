using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Transfer
{
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
                    ship_count = reader.ReadInt32();
                    writer.Write(ship_count);

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
                        writer.Write(hd_x);
                        writer.Write(hd_y);
                        writer.Write(name);
                    }
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
