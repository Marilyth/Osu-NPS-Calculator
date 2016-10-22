using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Osu_DiffiCalc
{
    public partial class Form1 : Form
    {
        private Calculation.Basic diffiCalc;
        System.Timers.Timer activeCapture;
        private string pastTitle, tempPath;

        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowsAdded += DataGridView1_RowsAdded;
            diffiCalc = new Calculation.Basic();
            ResetChart();
            ResetDatagridview();
        }

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            label1.Text = $"Mapcount: {dataGridView1.RowCount -1}";
            this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkGray;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            diffiCalc.singleAdd();
            addRowRecent(diffiCalc.localMaps.Count -1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int preCount = diffiCalc.localMaps.Count;
            diffiCalc.folderAdd();
            for (int i = preCount; i < diffiCalc.localMaps.Count; i++)
            {
                addRowRecent(i);
            }
        }

        private void addRowRecent(int index)
        {
            try
            {
                OsuMaps.Map curMap = diffiCalc.localMaps[index];
                this.dataGridView1.Rows.Add(curMap.peak, double.Parse(String.Format("{0:0.00}", curMap.level)), curMap.artist, curMap.song, curMap.difficulty, curMap.mapper, curMap.autoID, curMap.hitObjects.Count);
            }
            catch (Exception)
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetChart();
            try
            {
                int path = (int)dataGridView1.Rows[e.RowIndex].Cells[6].Value;
                OsuMaps.Map selectedMap = diffiCalc.localMaps.Find(x => x.autoID == path);
                foreach (OsuMaps.Object.HitObject obj in selectedMap.hitObjects)
                {
                    chart1.Series["Series1"].Points.AddXY(obj.time / 1000 + "s", obj.NPS);
                }
            }
            catch (Exception) { }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonClick(3, true);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonClick(0, false);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonClick(1, false);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonClick(2, false);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonClick(3, false);
        }

        private void radioButtonClick(int mode, bool is4k)
        {
            Program.mode = mode;
            Program.is4k = is4k;
            ResetDatagridview();
            diffiCalc = new Calculation.Basic();
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void ResetDatagridview()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            dataGridView1.GridColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Gray;
        }

        private void ResetChart()
        {
            chart1.Series["Series1"].Points.Clear();
            chart1.Series["Series1"].IsVisibleInLegend = false;
            chart1.Series["Series1"].Color = Color.Black;
            chart1.BackColor = Color.DimGray;
            chart1.ChartAreas[0].BackColor = Color.DarkGray;
            chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Program.objectSkip = (int)numericUpDown1.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if(tempPath == null)
                {
                    //Choose Songs Folder
                    FolderBrowserDialog choose = new FolderBrowserDialog();

                    DialogResult result = choose.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        tempPath = choose.SelectedPath;
                    }
                }
                activeCapture = new System.Timers.Timer(5000);
                activeCapture.Elapsed += ActiveCapture_Elapsed1;
                activeCapture.Start();

                Program.mode = 4;
            }
            else
            {
                activeCapture.Stop();
            }
        }

        public void UpdateUI(string matchingPaths)
        {
            diffiCalc.addMap(matchingPaths);

            addRowRecent(diffiCalc.localMaps.Count - 1);

            ResetChart();
            try
            {
                OsuMaps.Map selectedMap = diffiCalc.localMaps.Find(x => x.filePath == matchingPaths);
                foreach (OsuMaps.Object.HitObject obj in selectedMap.hitObjects)
                {
                    chart1.Series["Series1"].Points.AddXY(obj.time / 1000 + "s", obj.NPS);
                }
            }
            catch (Exception) { }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private void ActiveCapture_Elapsed1(object sender, System.Timers.ElapsedEventArgs e)
        {
            string title = GetActiveWindowTitle();

            if (title != null && title.Length > 7 && title.StartsWith("osu!  -") && title != pastTitle)
            {
                pastTitle = title;
                string[] map = title.Remove(0, 8).Split('[');
                string[] matchingPaths = Directory.GetFiles(tempPath, $"*{map[0].Split(System.IO.Path.GetInvalidFileNameChars())[0]}*[{map[1].Split(System.IO.Path.GetInvalidFileNameChars())[0]}*", SearchOption.AllDirectories);


                this.Invoke(new Action<string>(UpdateUI), matchingPaths[0]);
            }
        }

        private string GetActiveWindowTitle()
        {
            StringBuilder Buff = new StringBuilder(256);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, 256) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
    }
}
