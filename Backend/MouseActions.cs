using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;


//---------------------------MAUS-AKTIONEN---------------------------------------//
namespace SPSCtrl
{
    public partial class MainGUI
    {
        private int lastMarkedList = -1; //Zuletzt markiertes Koordinatenpaar in einer Liste
        private int lastMarkedChart = -1; //Zuletzt markiertes Koordinatenpaar im Chart
        private bool mouseAdded = false;
        private double mouseChartX = 0, mouseChartY = 0;

        private void AddMouseOnChart()
        {
            //Fügt einen Mouse Event Listener hinzu, der die aktuelle Position des Mauszeigers über dem Graphen anzeigt
            if (!this.mouseAdded)
            {
                this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ChartMousePosition);
                this.mouseAdded = true;
            }
        }

        private void ListMousePosition(object sender, MouseEventArgs e)
        {
            int xitem = xis_list.Items.IndexOf(xis_list.GetItemAt(e.X, e.Y));
            int yitem = yis_list.Items.IndexOf(yis_list.GetItemAt(e.X, e.Y));
            int xtaritem = xtar_list.Items.IndexOf(xtar_list.GetItemAt(e.X, e.Y));
            int ytaritem = ytar_list.Items.IndexOf(ytar_list.GetItemAt(e.X, e.Y));


            if (xitem >= 0)
                lastMarkedList = xitem;
            else if (yitem >= 0)
                lastMarkedList = yitem;
            else if (ytaritem >= 0)
                lastMarkedList = yitem;
            else if (xtaritem >= 0)
                lastMarkedList = yitem;
            else
                lastMarkedList = -1;

        }

        /// <summary>
        /// Schreibt die aktuelle Mausposition im Graph global in mouseX, mouseY
        /// </summary>
        private void ChartMousePosition(object sender, MouseEventArgs e)
        {
            mouseChartX = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            mouseChartY = chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);

            this.mouse_x.Text = String.Format("X: {0}", mouseChartX.ToString("####.##"));
            this.mouse_y.Text = String.Format("Y: {0}", mouseChartY.ToString("####.##"));

            int margin = 15;
            
            int i = 0;
            foreach (DataPoint xy in chart1.Series[0].Points)
            {
                if ((Math.Abs(mouseChartX - xy.XValue) < margin && Math.Abs(mouseChartY - xy.YValues[0]) < margin))
                {
                    lastMarkedChart = i;
                    DrawListMarks();
                }
                i++;
            }
        }
    }
}