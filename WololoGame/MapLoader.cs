using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WololoGame
{
    class MapLoader
    {
        public void LoadMap(Game1 game, string mapfile)
        {
            using (StreamReader levelFileStream = new StreamReader(TitleContainer.OpenStream("Content/maps/begining.txt")))
            {
                string line;
                while ((line = levelFileStream.ReadLine())!= null)
                {
                    string[] fields = Regex.Split(line, "\\s+");

                    if (fields.Length > 1)
                    {
                        if (fields[0].ToLower() == "player")
                        {

                           
                        }
                        else if (fields[0].ToLower() == "player")
                        {

                        }
                    }
                }
            }
        }
    }
}
