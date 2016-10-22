using System;
using System.IO;
using Osu_DiffiCalc;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu_DiffiCalc.Calculation
{
    class Basic
    {
        internal List<OsuMaps.Map> localMaps = new List<OsuMaps.Map>();

        public void singleAdd()
        {
            OpenFileDialog choose = new OpenFileDialog();
            choose.Filter = "Osu! Files (.osu)|*.osu";
            DialogResult result = choose.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = choose.FileName;
                addMap(file);
            }
        }

        public void folderAdd()
        {
            FolderBrowserDialog choose = new FolderBrowserDialog();

            DialogResult result = choose.ShowDialog();
            if(result == DialogResult.OK)
            {
                string folder = choose.SelectedPath;

                autoCalc(new DirectoryInfo(folder));
            }
        }

        private void autoCalc(DirectoryInfo d)
        {
            FileInfo[] fis = d.GetFiles("*.osu", SearchOption.AllDirectories);
            foreach (FileInfo fi in fis)
            {
                    addMap(fi.FullName);
            }
        }

        public void addMap(string file)
        {
            localMaps.Add(new OsuMaps.Map(file));

            OsuMaps.Map thisMap = localMaps[localMaps.Count - 1];

            if(thisMap.dump)
            {
                localMaps.Remove(thisMap);
                return;
            }

            thisMap.level = Advanced.AllMode.setGetNPS(thisMap.hitObjects);
            if(thisMap.hitObjects.Count > 0) thisMap.peak = thisMap.hitObjects.Max(x => x.NPS);
        }
    }
}
