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
            using (StreamReader levelFileStream = new StreamReader(TitleContainer.OpenStream(mapfile)))
            {
                string line;
                while ((line = levelFileStream.ReadLine())!= null)
                {
                    string[] fields = Regex.Split(line, "\\s+");

                    if (fields.Length > 1)
                    {
                        if (fields[0].ToLower() == "player")
                        {
                            game.CreatePlayer(new Vector4(int.Parse(fields[1]) / 24.0f, int.Parse(fields[2]) / 24.0f
                                , int.Parse(fields[3]) / 24.0f, int.Parse(fields[4]) / 24.0f));

                        }
                        else if (fields[0].ToLower() == "terrain")
                        {
                            int vis = fields.Length >= 6 ? int.Parse(fields[5]) : 0;
                            Visibility v = Visibility.Both;
                            if (vis == 1)
                                v = Visibility.SunnyModeOnly;
                            else if (vis == 2)
                                v = Visibility.NightModeOnly;

                            game.CreateGrassyTerrain(new Vector4(int.Parse(fields[1]) / 24.0f, int.Parse(fields[2]) / 24.0f
                                , int.Parse(fields[3]) / 24.0f, int.Parse(fields[4]) / 24.0f),v);
                        }
                        else if (fields[0].ToLower() == "moving_terrain")
                        {
                            int vis = fields.Length >= 6 ? int.Parse(fields[5]) : 0;
                            Visibility v = Visibility.Both;
                            if (vis == 1)
                                v = Visibility.SunnyModeOnly;
                            else if (vis == 2)
                                v = Visibility.NightModeOnly;

                            game.CreateMovingGrassyTerrain(new Vector4(int.Parse(fields[1]) / 24.0f, int.Parse(fields[2]) / 24.0f
                                , int.Parse(fields[3]) / 24.0f, int.Parse(fields[4]) / 24.0f), v, int.Parse(fields[6]) / 24.0f, int.Parse(fields[7]) / 24.0f, float.Parse(fields[8]) / 24.0f);
                        }
                        else if (fields[0].ToLower() == "butterfly")
                        {
                            game.CreateButterfly(new Vector4(int.Parse(fields[1]) / 24.0f, int.Parse(fields[2]) / 24.0f
                                , float.Parse(fields[3]) / 24.0f, float.Parse(fields[4]) / 24.0f));

                        }

                    }
                }
            }
        }
    }
}
