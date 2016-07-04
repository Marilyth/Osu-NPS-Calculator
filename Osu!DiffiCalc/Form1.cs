using System;
using System.Web.UI.DataVisualization.Charting;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Osu_DiffiCalc
{
    public partial class Form1 : Form
    {
        private Calculation.Basic diffiCalc;
        public int mode = -1;

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
    }
}
