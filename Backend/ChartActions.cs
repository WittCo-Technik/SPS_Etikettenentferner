using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;




//---------------------------Chart-AKTIONEN---------------------------------------//
namespace SPSCtrl
{
    [Serializable]
    public partial class MainGUI : Form
    {
        bool[] pointsPassed = new bool[32];
        private int last_hersteller, last_schild_L, last_schild_R, last_pallette, state;
        private bool lastUN;
        private XmlDocument xmlschilder;

        /// <summary>
        /// Zeichnet den Graph mit den aktuell aktivierten Koordinaten neu und ruft anschliessend RedrawSchild() auf.
        /// </summary>
        private void RedrawChart() //Unsauber, sollte überarbeitet werden
        {
            int lineWidth = 2; 

            this.chart1.ChartAreas[0].Position.X = 0; //Linken Rand entfernen
            this.chart1.ChartAreas[0].Position.Width = 100;
            this.chart1.ChartAreas[0].Position.Height = 100;
            this.chart1.ChartAreas[0].Position.Y = 0;

            //Ist-Koordinaten darstellen
            chart1.Series.Clear();
            Series coor = new Series("Ist-Koordianten");

            coor.Points.DataBindXY(SPSController.X.Take(SPSController.CountXY).ToArray(), SPSController.Y.Take(SPSController.CountXY).ToArray());
            coor.ChartType = SeriesChartType.Line;
            chart1.Series.Add(coor);
            chart1.Series[chart1.Series.Count - 1].MarkerStyle = MarkerStyle.Circle;
            chart1.Series[chart1.Series.Count - 1].Color = Color.Blue;
            chart1.Series[chart1.Series.Count - 1].BorderWidth = 3;

            if (lastMarkedList >= 0 && lastMarkedList < SPSController.CountXY)
            {
                chart1.Series[chart1.Series.Count - 1].Points[lastMarkedList].MarkerSize = 15; //Hebt den ausgewählten Punkt der Liste im Graphen hervor
            }

            //Soll-Koordinaten darstellen
            int cnt = 0;
            ListViewItem[] itemX = new ListViewItem[xylist_size], itemY = new ListViewItem[xylist_size];
            xtar_list.Items.CopyTo(itemX, 0);
            ytar_list.Items.CopyTo(itemY, 0);
            short[] w_X = new short[xylist_size], w_Y = new short[xylist_size];


            for (int i = 0; i < xylist_size; i++)
            {
                w_X[i] = (short)int.Parse(itemX[i].Text);
                w_Y[i] = (short)int.Parse(itemY[i].Text);

                if (w_X[i] > 0 || w_Y[i] > 0)
                    cnt++;
            }


            Series w_coor = new Series("Soll-Koordinaten");
            w_coor.Points.DataBindXY(w_X.Take(cnt).ToArray(), w_Y.Take(cnt).ToArray());
            w_coor.ChartType = SeriesChartType.Line;
            chart1.Series.Add(w_coor);
            chart1.Series[chart1.Series.Count - 1].MarkerStyle = MarkerStyle.Circle;
            chart1.Series[chart1.Series.Count - 1].Color = Color.Red;
            chart1.Series[chart1.Series.Count - 1].BorderWidth = lineWidth;

            AddMouseOnChart();

            RedrawSchild(); //Schild muss derzeit dringend nach den Koordinaten gezeichnet werden.
        }
        /// <summary>
        /// Zeichnet wenn möglich ein Schild mit festgelegtem Rand um die aktuellen Koordinaten.
        /// </summary>
        private void RedrawSchild()
        {
            try
            {
                /*
                double margin = 25;
                double max_x, min_x, max_y, min_y;
                max_x = SPSController.X.Max() + margin;
                min_x = SPSController.X.Where(num => num > 0).Min() - margin; //Nullen aussortieren
                max_y = SPSController.Y.Max() + margin;
                min_y = SPSController.Y.Where(num => num > 0).Min() - margin; //Nullen aussortieren
                */

                LoadSchild(productlist.SelectedIndex, out int[] x, out int[] y);

                Series schild = new Series("Schild");
                //schild.Points.DataBindXY(new double[] { min_x, max_x, max_x, min_x, min_x }, new double[] { min_y, min_y, max_y, max_y, min_y });
                schild.Points.DataBindXY(x, y);
                schild.ChartType = SeriesChartType.Line;
                chart1.Series.Add(schild);
                chart1.Series[chart1.Series.Count - 1].Color = Color.Black;


                //Passe die Skalierung der Chart an die Schilddimensionen an.
                foreach(Series ser in chart1.Series)
                {
                    ser["PixelWidth"] = "20";
                }
                chart1.ChartAreas[0].AxisX.Minimum = (int)chart1.Series[chart1.Series.Count - 1].Points[0].XValue -20;
                chart1.ChartAreas[0].AxisX.Maximum = (int)chart1.Series[chart1.Series.Count - 1].Points[1].XValue + 20;
                chart1.ChartAreas[0].AxisY.Minimum = (int)chart1.Series[chart1.Series.Count - 1].Points[0].YValues[0] - 20;
                chart1.ChartAreas[0].AxisY.Maximum = (int)chart1.Series[chart1.Series.Count - 1].Points[2].YValues[0] + 100;

            }
            catch (InvalidOperationException)
            {
                SendToConsole("Leere Liste ausgewählt\n");
            }
        }

        /// <summary>
        /// Wird vom Timer aufgerufen, liest und zeichnet live die aktuellen Koordinaten der Düsen und Metadaten (Hersteller etc.)
        /// </summary>
        private void RedrawLiveNozzle()
        {
            //Daten holen
            SPSController.ReadLiveData();

            //Daten in Graph zeichnen
            Series livedot = new Series("Düse");
            short xlive, ylive;


            if (!back)
            {
                xlive = SPSController.X_Live_Links;
                ylive = SPSController.Y_Live_Links;

                livedot.Points.AddXY(xlive, ylive);
                livedot.ChartType = SeriesChartType.Line;

                chart1.Series.Remove(livedot);
                chart1.Series.Add(livedot);

                //Düse Links
                chart1.Series[chart1.Series.Count - 1].MarkerStyle = MarkerStyle.Cross;
                chart1.Series[chart1.Series.Count - 1].MarkerSize = 15;
                chart1.Series[chart1.Series.Count - 1].Color = Color.Black;

            }
            else
            {
                xlive = SPSController.X_Live_Rechts;
                ylive = SPSController.Y_Live_Rechts;

                livedot.Points.AddXY(xlive, ylive);
                livedot.ChartType = SeriesChartType.Line;

                chart1.Series.Remove(livedot);
                chart1.Series.Add(livedot);

                //Düse Rechts
                chart1.Series[chart1.Series.Count - 1].MarkerStyle = MarkerStyle.Cross;
                chart1.Series[chart1.Series.Count - 1].MarkerSize = 15;
                chart1.Series[chart1.Series.Count - 1].Color = Color.DarkViolet;
            }

            //Markiere bereits abgearbeitete Punkte in der Liste
            int i = 0;
            int margin = 10;
            foreach (DataPoint xy in chart1.Series[0].Points)
            {
                if (Math.Abs(xlive - xy.XValue) < margin && Math.Abs(ylive - xy.YValues[0]) < margin)
                {
                    pointsPassed[i] = true;
                    //DrawListMarks();
                }
                i++;
            }
        }

        private void RedrawLiveLabels()
        {
            //Labels beschriften
            this.xliveleft_label.Text = "X: " + SPSController.X_Live_Links.ToString();
            this.yliveleft_label.Text = "Y: " + SPSController.Y_Live_Links.ToString();
            this.xliveright_label.Text = "X: " + SPSController.X_Live_Rechts.ToString();
            this.yliveright_label.Text = "Y: " + SPSController.Y_Live_Rechts.ToString();

            this.lastcycle_label.Text = "Letzter Zyklus: " + SPSController.lastCycleTime.ToString() + " Sek.";
            this.lastcleanL_label.Text = "Letzte Reinigungszeit (Vorne): " + SPSController.lastCycleTime.ToString() + " Sek.";
            this.lastcleanR_label.Text = "Letzte Reinigungszeit (Hinten): " + SPSController.lastCleanTimeR.ToString() + " Sek.";

            //Folgende Metadaten-Labels werden nur aktualisiert wenn es eine Änderung zum vorherigen Wert gab.
            if (!(last_hersteller == SPSController.hersteller) ||
                !(last_pallette == SPSController.pallette) ||
                !(last_schild_L == SPSController.schildGroesse_Links) ||
                !(last_schild_R == SPSController.schildGroesse_Rechts) ||
                !(state == SPSController.state) ||
                !(lastUN == SPSController.unNummer))
            {
                this.manu_label.Text = "Hersteller: ";

                switch (SPSController.hersteller)
                {
                    case 1: this.manu_label.Text += "Maschio"; break;
                    case 2: this.manu_label.Text += "Fusti"; break;
                    case 3: this.manu_label.Text += "Mauser"; break;
                    case 4: this.manu_label.Text += "Schütz"; break;
                    case 5: this.manu_label.Text += "Sotralentz"; break;
                    case 6: this.manu_label.Text += "Werit"; break;
                    default: this.manu_label.Text += "Daten fehlerhaft"; break;
                }

                this.pallet_label.Text = "Pallette: ";
                switch (SPSController.pallette)
                {
                    case 1: this.pallet_label.Text += "Holz"; break;
                    case 2: this.pallet_label.Text += "Stahlrahmen"; break;
                    case 3: this.pallet_label.Text += "PE-Kufen"; break;
                    case 4: this.pallet_label.Text += "Stahlkufen"; break;
                    case 5: this.pallet_label.Text += "Holz-Komposit"; break;
                    default: this.pallet_label.Text += "Daten fehlerhaft"; break;
                }

                this.un_label.Text = "UN: ";
                if (SPSController.unNummer)
                {
                    this.un_label.Text += "JA";
                }
                else
                {
                    this.un_label.Text += "NEIN";
                }

				this.rastbar_label.Text = "Rastbar:";
				if (SPSController.rastbar)
				{
					this.rastbar_label.Text += "JA";
				}
				else
				{
					this.rastbar_label.Text += "NEIN";
				}

				this.schildsizeFront_label.Text = "Schildgröße Vorne: ";
                switch (SPSController.schildGroesse_Links)
                {
                    case 1: this.schildsizeFront_label.Text += "XS"; break;
                    case 2: this.schildsizeFront_label.Text += "S"; break;
                    case 3: this.schildsizeFront_label.Text += "L"; break;
                    case 4: this.schildsizeFront_label.Text += "XL"; break;
                    case 5: this.schildsizeFront_label.Text += "Kein Schild"; break;
                    default: this.schildsizeFront_label.Text += "Daten fehlerhaft"; break;
                }

                this.schildsizeBack_label.Text = "Schildgröße Hinten: ";
                switch (SPSController.schildGroesse_Rechts)
                {
                    case 1: this.schildsizeBack_label.Text += "XS"; break;
                    case 2: this.schildsizeBack_label.Text += "S"; break;
                    case 3: this.schildsizeBack_label.Text += "L"; break;
                    case 4: this.schildsizeBack_label.Text += "XL"; break;
                    case 5: this.schildsizeBack_label.Text += "Kein Schild"; break;
                    default: this.schildsizeBack_label.Text += "Daten fehlerhaft"; break;
                }

                switch (SPSController.state)
                {
                    case 0: this.status.Text = "Status:\nBEREIT"; break;
                    case 1: this.status.Text = "Status:\nFahrt in Nullposition Hinten"; break;
                    case 2: this.status.Text = "Status:\nReinigung Hinten"; break;
                    case 3: this.status.Text = "Status:\nFahrt in Nullposition Vorne"; break;
                    case 4: this.status.Text = "Status:\nReinigung Vorne"; break;
                    case 5: this.status.Text = "Status:\nFERTIG"; break;
                    default: this.status.Text = "Status:\nUnbekannt"; break;
                }

                last_hersteller = SPSController.hersteller;
                last_pallette = SPSController.pallette;
                last_schild_L = SPSController.schildGroesse_Links;
                last_schild_R = SPSController.schildGroesse_Rechts;
                state = SPSController.state;
                lastUN = SPSController.unNummer;
            }
        }

        /// <summary>
        /// Methode zum Speichern der Schildgrößen als XML File. (Wenn die Maße der Schilder neu gespeichert werden sollen,
        /// muss die Methode RedrawSchild() auf die dynamische Anpassung anhand der abgefahrenen Koordianten eingestellt werden.
        /// </summary>
        private void DumpSchildSizeXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode root = xmlDoc.CreateElement("Schildgroessen"); // Root-Node
            xmlDoc.AppendChild(root);

            int i = 0;
            foreach (string entry in products)
            {
                productlist.SetSelected(i, true);
                Init_XYLists();
                RedrawChart();

                XmlNode produkt = xmlDoc.CreateElement(entry);
                root.AppendChild(produkt);

                XmlNode X = xmlDoc.CreateElement("X");
                produkt.AppendChild(X);
                XmlNode Y = xmlDoc.CreateElement("Y");
                produkt.AppendChild(Y);
                XmlNode comm = xmlschilder.CreateElement("Kommentar");
                produkt.AppendChild(comm);


                for (int j = 0; j < 5; j++)
                {
                    try
                    {
                        X.InnerText += chart1.Series[chart1.Series.Count - 1].Points[j].XValue + ",";
                        Y.InnerText += chart1.Series[chart1.Series.Count - 1].Points[j].YValues[0] + ",";
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        if (products[i].ToLower().Contains("null"))
                        {
                            X.InnerText += "0,";
                            Y.InnerText += "0,";
                        }
                    }
                }
                X.InnerText = X.InnerText.Substring(0, X.InnerText.Length - 1);
                Y.InnerText = Y.InnerText.Substring(0, Y.InnerText.Length - 1);
                i++;
            }
            productlist.SetSelected(0, true);
            xmlDoc.Save(Environment.CurrentDirectory + @"\Schildgroessen.xml");
        }

        /// <summary>
        /// Methode zum Laden eines Kommentars aus XML Datei, passend zur aktuellen Schildkonfiguration.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string LoadComment(int index)
        {
            this.comment.BackColor = Color.FromName("Window");
            this.comment.ForeColor = Color.Black;
            return xmlschilder.SelectSingleNode("//" + products[index] + "/Kommentar").InnerText;
        }
        /// <summary>
        /// Methode zum Hinzufügen eines Kommentars zur aktuellen Schildkonfiguration.
        /// </summary>
        private void SetComment(int index, string comment)
        {
            xmlschilder.SelectSingleNode("//" + products[index] + "/Kommentar").InnerText = comment;
            xmlschilder.Save(Environment.CurrentDirectory + @"\Schildgroessen.xml");
            this.comment.BackColor = Color.LightGreen;
        }
        /// <summary>
        /// Lädt Schildkoordinaten aus XML Datei
        /// </summary>
        /// 
        private void LoadSchild(int index, out int[] xvalues, out int[] yvalues)
        {
            XmlNode X = xmlschilder.SelectSingleNode("//" + products[index] + "/X");
            XmlNode Y = xmlschilder.SelectSingleNode("//" + products[index] + "/Y");
            

            string[] xarray = X.InnerText.Split(',');
            string[] yarray = Y.InnerText.Split(',');

            xvalues = new int[5];
            yvalues = new int[5];

            for (int i = 0; i < 5; i++)
            {
                xvalues[i] = Int16.Parse(xarray[i]);
                yvalues[i] = Int16.Parse(yarray[i]);
            }
        }

        /* Veraltete Hilfsfunktion zum Hinzufügen eines Kommentar-Knotens in der XML Datei
        private void AddKommentar()
        {
            xmlschilder = new XmlDocument(); // Lädt Schilder
            xmlschilder.Load(Environment.CurrentDirectory + @"\Schildgroessen.xml");

            for (int i= 0; i < 208; i++)
            {
                XmlNode prod = xmlschilder.SelectSingleNode("//" + products[i]);
                XmlNode comm = xmlschilder.CreateElement("Kommentar");
                prod.AppendChild(comm);
                //Console.WriteLine(prod.InnerText);
            }
            xmlschilder.Save(Environment.CurrentDirectory + @"\Schildgroessen.xml");
        }
        */

        /// <summary>
        /// Soll zu Programmstart aufgerufen werden und den Inhalt (Schilder und Kommentare) aus einer XML-Datei bereitstellen.
        /// </summary>
        private void LoadSchildXML()
        {
            try
            {
                xmlschilder = new XmlDocument(); // Lädt Schilder
                xmlschilder.Load(Environment.CurrentDirectory + @"\Schildgroessen.xml");
            }
            catch(System.IO.FileNotFoundException)
            {
                SendToConsole("Schildkonfigurationen konnten nicht geladen werden!");
                connectbutton.Enabled = false;
                this.consoleOutput.BackColor = Color.Red;
            }
        }
    }
}
