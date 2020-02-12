using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;


//---------------------------Produktliste---------------------------------------//
namespace SPSCtrl
{
    public partial class MainGUI : Form
    {
        List<string> products = new List<string>();

        /// <summary>
        /// Liest aus .csv Datei (Einträge \n-separiert) die Bezeichner der Produkte ein. 
        /// <para>Es ist dringlichst darauf zu achten, dass die Liste mit der in der SPS programmierten Reihenfolge überein stimmt. </para>
        /// </summary>
        private void ReadInProducts()
        {
            if (File.Exists(Environment.CurrentDirectory + @"\Produkte.csv")) //Navigate from bin to project folder
            {
                SendToConsole("CSV Datei existiert.\n");
                //var reader = new StreamReader(Environment.CurrentDirectory + @"..\..\..\Produkte.csv");
                using (StreamReader reader = new StreamReader(Environment.CurrentDirectory + @"\Produkte.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        products.Add(reader.ReadLine());
                    }
                }
                if (products.Count > 0)
                {
                    SendToConsole("Produktliste wurde eingelesen.\n");
                }
            }
            else
            {
                SendToConsole("CSV Datei wurde nicht gefunden!\n");
                connectbutton.Enabled = false;
                this.consoleOutput.BackColor = Color.Red;
            }

            this.productlist.Items.AddRange(products.ToArray());
            this.entrys_label.Text = "Einträge: " + products.Count.ToString();
        }


        /// <summary>
        /// Liest beim Aufruf eine neue Liste Koordinaten aus der SPS, entsprechend des in der Produktliste gewählten Eintrags aus.
        /// </summary>
        private void ProductListChanged()
        {
            string hersteller;
            string schild;

            this.SPSController.ReadDBEntry(productlist.SelectedIndex);

            this.pallette_manual_label.Text = "Pallette: ";
            //Checkt die Zahl am Ende eines Produktes zur Pallettenzuordnung
            if (!int.TryParse("" + this.productlist.SelectedItem.ToString()[this.productlist.SelectedItem.ToString().Length - 1], out int pal))
            {
                int.TryParse("" + this.productlist.SelectedItem.ToString()[this.productlist.SelectedItem.ToString().Length - 3], out pal);
            }

            if (this.productlist.SelectedItem.ToString().ToLower().Contains("null")) //Wenn Produkt Null, dann Pallette nicht definiert.
            {
                pal = 0;
            }

            switch (pal)
            {
                case 1:
                    this.pallette_manual_label.Text += "Holz";
                    break;
                case 2:
                    this.pallette_manual_label.Text += "Stahlrahmen";
                    break;
                case 3:
                    this.pallette_manual_label.Text += "PE-Kufen";
                    break;
                case 4:
                    this.pallette_manual_label.Text += "Stahlkufen";
                    break;
                case 5:
                    this.pallette_manual_label.Text += "Holzkomposit";
                    break;
                default:
                    this.pallette_manual_label.Text += "Nicht definiert";
                    break;
            }

            //Label Hersteller manuelle Auswahl
            if (this.productlist.SelectedItem.ToString().ToLower().Contains("schutz"))
                hersteller = "Schütz";
            else if (this.productlist.SelectedItem.ToString().ToLower().Contains("mauser"))
                hersteller = "Mauser";
            else if (this.productlist.SelectedItem.ToString().ToLower().Contains("werit"))
                hersteller =  "Werit";
            else if (this.productlist.SelectedItem.ToString().ToLower().Contains("fusti"))
                hersteller = "Fusti";
            else if (this.productlist.SelectedItem.ToString().ToLower().Contains("maschio"))
                hersteller = "Maschio";
            else if (this.productlist.SelectedItem.ToString().ToLower().Contains("soltralenz"))
                hersteller = "Sotralentz";
            else
                hersteller = "Fehler";

            this.manu_manual_label.Text = "Hersteller: " + hersteller;

            if (this.productlist.SelectedItem.ToString().ToLower().Contains("_xs"))
                schild = "XS";
            else if (this.productlist.SelectedItem.ToString().ToLower().Contains("_s"))
                schild = "S";
            else if (this.productlist.SelectedItem.ToString().ToLower().Contains("_l"))
                schild = "L";
            else if (this.productlist.SelectedItem.ToString().ToLower().Contains("_xl"))
                schild = "XL";
            else
                schild = "Fehler";

			if (this.productlist.SelectedItem.ToString().ToLower().Contains("_r_"))
				schild = "S (Hinten)";

			this.schild_manual_label.Text = "Schildgröße: " + schild;

			//Wenn "_UN" im Eintrag, setze UN Flag.
            this.un_manual_label.Text = (this.productlist.SelectedItem.ToString().ToLower().Contains("_un"))? "UN: JA" : "UN: NEIN";

			//Wenn "_R_" im Eintrag setze Spezialfall (Schütz,S,Hinten), wenn "_R" im Eintrag, setze Rastbar Flag.

			this.rastbar_manual_label.Text = (this.productlist.SelectedItem.ToString().ToLower().EndsWith("_r")) ? "Rastbar: JA" : "Rastbar: NEIN";




			//Zeige gewählten Eintrag unter der Liste (zur besseren Sichtbarkeit).
			this.product_label.Text = productlist.SelectedItem.ToString();

            Init_XYLists();
            comment.Text = LoadComment(productlist.SelectedIndex);
            this.drawtimer.Enabled = true;
        }


        /// <summary>
        /// Wählt anhand der erhaltenen Daten automatisch das passende Produkt aus der Liste.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoChooseProduct(object sender, EventArgs e)
        {
            if (this.autochoose.Checked)
            {
                productlist.Enabled = false;
                data_auto_box.Visible = true;
                data_manual_box.Visible = false;
                int index = 0;

                bool inList = false;
   

                string hersteller;
                string pallette = SPSController.pallette.ToString();
                string schild;
                string un = "";
                string schuetz_R;
				string rastbar = "";
                short schildGroesse;

                switch (SPSController.hersteller)
                {
                    case 1: hersteller = "maschio"; break;
                    case 2: hersteller = "fusti"; break;
                    case 3: hersteller = "mauser"; break;
                    case 4: hersteller = "schutz"; break;
                    case 5: hersteller = "soltralenz"; break;
                    case 6: hersteller = "werit"; break;
                    default: hersteller = "Daten fehlerhaft"; break;
                }

                //Auswahl Vorder/Hinter-Schild
                schildGroesse = CheckBack()? SPSController.schildGroesse_Rechts : SPSController.schildGroesse_Links;
                //Spezialfall Schutz Hinten, Schild S
                schuetz_R = (hersteller == "schutz" && back && schildGroesse == 2) ? "_r_" : "";

                switch (schildGroesse)
                {
                    case 1: schild = "_xs"; break;
                    case 2: schild = "_s"; break;
                    case 3: schild = "_l"; break;
                    case 4: schild = "_xl"; break;
                    case 5: schild = "Kein Schild"; break;
                    default: schild = ""; break;
                }

                if (SPSController.unNummer && !back)
                {
                    un = "_un";
                }

				if (SPSController.rastbar)
				{
					rastbar = "_r";
				}

                for (int i = 0; i < products.Count; i++)
                {
                    string listEntry = productlist.Items[i].ToString().ToLower();
                    if (listEntry.Contains(hersteller) && listEntry.Contains(pallette) && listEntry.Contains(schild) && listEntry.Contains(schuetz_R) && listEntry.Contains(un) && listEntry.EndsWith(rastbar))
                    {
                        inList = true;
                        index = i;
                        break; //Bricht Schleife beim ersten Fund ab, damit Einträge mit UN nicht ausgewählt werden wenn keine UN Nummer
                    }
                }

                if (inList)
                {
                    productlist.SetSelected(index, true);
                }
            }
            else
            {
                productlist.Enabled = true;
                data_auto_box.Visible = false;
                data_manual_box.Visible = true;
            }
        }
    }
}