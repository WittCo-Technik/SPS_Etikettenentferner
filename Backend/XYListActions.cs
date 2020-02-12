
using System;
using System.Drawing;
using System.Windows.Forms;


//---------------------------XY-List-AKTIONEN---------------------------------------//
namespace SPSCtrl
{
    public partial class MainGUI : Form
    {
        int lastCoor = 0;
        /// <summary>
        /// Ermöglicht das Bearbeiten eines Punktes durch  SingleClick auf das Label (Workaround).
        /// </summary>
        private void ClickEditLabel(object sender, EventArgs e)
        {
            ListView list = sender as ListView;
            lastCoor = int.Parse(list.SelectedItems[0].Text);
            list.SelectedItems[0].BeginEdit();
        }
        /// <summary>
        /// Überprüft bei Eingabe von Zeichen in eine der Listen ob der Wert numerisch ist und setzt ihn zurück, wenn die Überprüfung fehlschlägt.
        /// </summary>
        private void CheckNumericValue(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
                return;
            else
            {
                if (!int.TryParse(e.Label, out _)) //Wenn nicht geparsed werden kann -> zurück auf den gespeicherten Wert
                {
                    e.CancelEdit = true;
                    SendToConsole(String.Format("Falsches Zeichenformat in {0} Zeile {1} eingegeben: {2}\n", productlist.SelectedItem.ToString(), e.Item, e.Label));
                    consoleOutput.BackColor = Color.Orange;
                }
            }
        }

        /// <summary>
        /// Erstellt neue XY-Listen aus den XY-Koordinaten die momentan im Backend gespeichert wurden.  
        /// </summary>
        private void Init_XYLists()
        {
            Clear_XYLists(); // Listen leeren um sie neu zu befüllen.

            for (int i = 0; i < xylist_size; i++) //Liest Koordianten aus SPSController in Tabellen ein
            {
                //Ist
                xis_list.Items.Add(new ListViewItem(String.Format("{0}", SPSController.GetXVal(i)), 0));
                yis_list.Items.Add(new ListViewItem(String.Format("{0}", SPSController.GetYVal(i)), 0));

                //Soll
                xtar_list.Items.Add(new ListViewItem(String.Format("{0}", SPSController.GetXVal(i)), 0));
                ytar_list.Items.Add(new ListViewItem(String.Format("{0}", SPSController.GetYVal(i)), 0));
            }
            cnt_label.Text = "Punkte: " + SPSController.CountXY.ToString();
        }

        /// <summary>
        /// Diese Funktion übergibt Tabellenwerte zurück an das SPSController-Objekt und triggert die Funktion zum Schreiben des Buffers zurück auf die SPS.
        /// </summary>
        /// <param name="listx"></param>
        /// <param name="listy"></param>
        private void Write_XYList(ListView listx, ListView listy)
        {
            ListViewItem[] itemBufferX = new ListViewItem[xylist_size]; //Arrays zum Speichern der Listenelemente
            ListViewItem[] itemBufferY = new ListViewItem[xylist_size];

            listx.Items.CopyTo(itemBufferX, 0); //Kopiert Inhalt der Liste in Array
            listy.Items.CopyTo(itemBufferY, 0);

            for (int i = 0; i < xylist_size; i++) //Schreibt die Tabellenwerte in den Short-Buffer(w_XKoor, w_YKoor) des SPSController Objekts
            {
                SPSController.WriteX(short.Parse(itemBufferX[i].Text), i);
                SPSController.WriteY(short.Parse(itemBufferY[i].Text), i);
            }

            if (SPSController.WriteDBentry(productlist.SelectedIndex) == 0)
            {
                SendToConsole("Schreiben erfolgreich.\n");
                SPSController.ReadDBEntry(productlist.SelectedIndex);
            }
            else
            {
                SendToConsole("Fehler beim Schreiben!\n");
                this.consoleOutput.BackColor = Color.Red;
            }

            Init_XYLists();
        }

        /// <summary>
        /// Entfernt alle Einträge aus den XY-ListView-Listen
        /// </summary>
        private void Clear_XYLists() //Entfernt alle Einträge der ListView Listen
        {
            xis_list.Items.Clear();
            yis_list.Items.Clear();
            xtar_list.Items.Clear();
            ytar_list.Items.Clear();
        }

        private void DrawListMarks()
        {

            for(int i = 0; i < xis_list.Items.Count; i++)
            {
                xis_list.Items[i].BackColor = Color.FromName("ControlLight");
                yis_list.Items[i].BackColor = Color.White;
                xis_list.Items[i].ForeColor = Color.Black;
                yis_list.Items[i].ForeColor = Color.Black;
                xtar_list.Items[i].BackColor = Color.FromName("ControlLight");
                ytar_list.Items[i].BackColor = Color.White;
                xtar_list.Items[i].ForeColor = Color.Black;
                ytar_list.Items[i].ForeColor = Color.Black;
            }


            //for (int j = 0; j < pointsPassed.Length; j++)
            //{
            //    if (pointsPassed[j] == true)
            //    {
            //        xis_list.Items[j].BackColor = Color.DarkGray;
            //        yis_list.Items[j].BackColor = Color.DarkGray;
            //    }
            //}


            if (lastMarkedChart != -1)
            {
                xis_list.Items[lastMarkedChart].BackColor = Color.Black;
                yis_list.Items[lastMarkedChart].BackColor = Color.Black;
                xis_list.Items[lastMarkedChart].ForeColor = Color.White;
                yis_list.Items[lastMarkedChart].ForeColor = Color.White;
                xtar_list.Items[lastMarkedChart].BackColor = Color.Black;
                ytar_list.Items[lastMarkedChart].BackColor = Color.Black;
                xtar_list.Items[lastMarkedChart].ForeColor = Color.White;
                ytar_list.Items[lastMarkedChart].ForeColor = Color.White;
            }
        }

        private void ClearListMarks()
        {
            for (int i = 0; i < xis_list.Items.Count; i++)
            {
                xis_list.Items[i].BackColor = Color.FromName("ControlLight");
                yis_list.Items[i].BackColor = Color.White;
                xis_list.Items[i].ForeColor = Color.Black;
                yis_list.Items[i].ForeColor = Color.Black;
            }
            pointsPassed = new bool[31];
            lastMarkedChart = -1;
        }
    }
}
