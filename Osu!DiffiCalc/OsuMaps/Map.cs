using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu_DiffiCalc.OsuMaps
{
    class Map
    {
        public string song, artist, mapper, difficulty, filePath;
        public int modeRes = -1, mode, autoID;
        public double level { get; set; }
        public double peak { get; set; }
        public bool dump = false;

        public List<Object.HitObject> hitObjects;

        public Map(string path)
        {
            autoID = Program.autoID;
            Program.autoID++;
            hitObjects = new List<Object.HitObject>();
            readFile(path);
        }

        private void readFile(string path)
        {
            filePath = path;

            StreamReader sr = new StreamReader(path);
            string line;

            while((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("Mode:"))
                {
                    mode = int.Parse(line.Split(':')[1]);
                    if (mode != 4 && mode != Program.mode) { dump = true; return; }
                }
                else if (line.StartsWith("Title:")) song = line.Split(':')[1];
                else if (line.StartsWith("Artist:")) artist = line.Split(':')[1];
                else if (line.StartsWith("Creator:")) mapper = line.Split(':')[1];
                else if (line.StartsWith("Version:")) difficulty = line.Split(':')[1];
                else if (Program.mode == 3 && line.StartsWith("CircleSize:"))
                {
                    int columns = int.Parse(line.Split(':')[1]);
                    if (Program.is4k && columns != 4) { dump = true; return; }
                    else if (!Program.is4k && columns != 7) { dump = true; return; }
                }
                else if (line.StartsWith("[HitObjects]"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineInformation = line.Split(',');
                        try
                        {
                            hitObjects.Add(new Object.HitObject(int.Parse(lineInformation[0]), int.Parse(lineInformation[1]), int.Parse(lineInformation[2])));
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        public static string gameMode(int gMode)
        {
            switch (gMode)
            {
                case 0:
                    return "Standard";
                case 1:
                    return "Taiko";
                case 2:
                    return "Catch the Beat";
                case 3:
                    return "Mania";
                default:
                    return "Not a Map";
            }
        }
    }
}
