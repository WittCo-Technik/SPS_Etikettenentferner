using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;



//---------------------------Backup/Restore-Funktionen--------------------------------------//
namespace SPSCtrl
{
    public partial class MainGUI : Form
    {
        private string lastExcelBackupPath;

        /// <summary>
        /// Checkt ob ein Backup-Ordner vorhanden ist und gibt dessen Pfad zurück.
        /// <para> Ist kein Ordner vorhanden wird ein neuer erstellt.</para>
        /// </summary>
        public string GetBackupDirectory()
        {
            string backupFolderPath = Environment.CurrentDirectory + String.Format(@"\Backup");

            if (!Directory.Exists(backupFolderPath))
            {
                Directory.CreateDirectory(backupFolderPath);
            }
            return backupFolderPath + @"\";
        }

        private string GetFilepathWithDate(bool auto = false)
        {
            //Akt. Datum
            string date = String.Format("{0}-{1}-{2}_{3}{4}{5}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second); //Akt. Datum

            //Dateipfad: Backup Ordner (wird erstellt wenn nicht vorhanden) + Dateiname, kodiert mit aktuellem Datum
            return  auto? GetBackupDirectory() + String.Format(@"AutoBackup {0}", date): GetBackupDirectory() + String.Format(@"Backup {0}", date);
        }

        /// <summary>
        /// Schreibt ein aktuelles Backup in einem seperaten Ordner auf Höhe der .exe-Datei
        /// </summary>
        private void BackupLists(bool auto = false)
        {
            string filepath = "";

            if (products.Count > 0) //Backup wird nur ausgeführt wenn Produktliste gefüllt
            {
                //------------------Excel-Backup------------------//
                if (this.excel_checkbox.Checked)
                {
                    this.backupprogress.Visible = true; //Zeige Ladebalken
                    this.selectionBox.Enabled = false; // Deaktiviere Bedienelemente
                    this.coorBox.Enabled = false;

                    //Neue Excel Mappe anlegen
                    Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
                    excelapp.ScreenUpdating = false; //Neuzeichnen deaktivieren
                    Microsoft.Office.Interop.Excel.Workbook map = excelapp.Workbooks.Add();
                    Microsoft.Office.Interop.Excel.Worksheet page = map.Worksheets[1];

                    page.Name = "Backupdaten";

                    int index = 0;

                    for (int i = 0; i < products.Count; i++)
                    {
                        index = i * 33;

                        page.Cells[(index) + 1, 1].Value = productlist.Items[i].ToString(); // Produktbezeichner

                        SPSController.ReadDBEntry(i);
                        for (int j = 0; j < xylist_size; j++) // Koordianten
                        {
                            page.Cells[index + j + 2, 1].Value = SPSController.GetXVal(j);
                            page.Cells[index + j + 2, 2].Value = SPSController.GetYVal(j);

                        }
                        this.backupprogress.PerformStep(); //Step Progressbar
                    }
                    SPSController.ReadDBEntry(productlist.SelectedIndex); //Zurücksetzen auf ausgewählten Eintrag 

                    filepath = GetFilepathWithDate();
                    map.SaveAs(filepath + ".xlsx");
                    map.Close();
                    excelapp.ScreenUpdating = true; //Neuzeichnen aktivieren
                    excelapp.Quit();

                    FileInfo exfi = new FileInfo(filepath + ".xlsx");
                    exfi.IsReadOnly = true; //Setze Schreibschutz für Datei

                    lastExcelBackupPath = filepath;
                    this.backupprogress.Value = 0;

                    SendToConsole("Excel-Backupdatei erstellt");
                }

                //------------------XML-Backup------------------//

                this.backupprogress.Visible = true; //Zeige Ladebalken
                this.selectionBox.Enabled = false; // Deaktiviere Bedienelemente
                this.coorBox.Enabled = false;

                XmlDocument xmlDoc = new XmlDocument();
                XmlNode produktliste = xmlDoc.CreateElement("Produkte"); // Root-Node
                xmlDoc.AppendChild(produktliste);

                int k = 0;
                foreach (string entry in products)
                {
                    SPSController.ReadDBEntry(k);

                    XmlNode produkt = xmlDoc.CreateElement(entry);
                    produktliste.AppendChild(produkt);

                    for (int l = 0; l < xylist_size; l++) //X-Koordianten schreiben
                    {
                        XmlNode coor = xmlDoc.CreateElement("X" + l.ToString());
                        coor.InnerText = SPSController.GetXVal(l).ToString();
                        produkt.AppendChild(coor);
                    }

                    for (int l = 0; l < xylist_size; l++) //Y-Koordianten schreiben
                    {
                        XmlNode coor = xmlDoc.CreateElement("Y" + l.ToString());
                        coor.InnerText = SPSController.GetYVal(l).ToString();
                        produkt.AppendChild(coor);
                    }

                    k++;
                    this.backupprogress.PerformStep(); //Step Progressbar
                }
                filepath = GetFilepathWithDate(auto);
                xmlDoc.Save(filepath + ".xml");

                FileInfo xmlfi = new FileInfo(filepath + ".xml");
                xmlfi.IsReadOnly = true; //Setze Schreibschutz für Datei

                this.backupprogress.Visible = false; //Verstecke Ladebalken
                this.backupprogress.Value = 0;
                this.selectionBox.Enabled = true; // Aktiviere Bedienelemente
                this.coorBox.Enabled = true;
                SPSController.ReadDBEntry(productlist.SelectedIndex); //Zurücksetzen auf ausgewählten Eintrag

                SendToConsole(auto ? "Automatisches XML-Backup erstellt\n" : "XML-Backup erstellt\n");

                LastBackupDate(); //Aktuelisiert die Anzeige des letzten Backup Datums
            }
            else
            {
                SendToConsole("Backup kann nicht erstellt werden:\n Keine Einträge in Produktliste!\n");
            }
        }

        /// <summary>
        /// Spielt ein gespeichertes Backup nach Auswahl wieder ein.
        /// </summary>
        public void RestoreLists(string restorefile, BackupRestoreGUI restoreWindow)
        {
            restoreWindow.Close();

            try
            {
                this.backupprogress.Visible = true;
                this.selectionBox.Enabled = false; // Deaktiviere Bedienelemente
                this.coorBox.Enabled = false;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(GetBackupDirectory() + restorefile);

                XmlNode root = xmlDoc.SelectSingleNode("/Produkte");

                XmlNodeList produkte = root.ChildNodes;

                for (int i = 0; i < produkte.Count; i++)
                {
                    XmlNode produkt = produkte[i];

                    for (int j = 0; j < xylist_size; j++)
                    {
                        short xval = Int16.Parse(produkt.SelectSingleNode("X" + j).InnerText);
                        short yval = Int16.Parse(produkt.SelectSingleNode("Y" + j).InnerText);

                        SPSController.WriteX(xval, j);
                        SPSController.WriteY(yval, j);

                        //Console.WriteLine(xval + " " + yval + "\n");
                    }
                    SPSController.WriteDBentry(i);
                    this.backupprogress.PerformStep();
                }
                SendToConsole(restorefile + " wiederhergestellt!");
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show(e.Message.ToString() + "Die gelesene Anzahl Einträge stimmt nicht mit der erwarteten Menge überein!");
                SendToConsole("Fehler beim Einspielen des Backups!\n Bitte manuell auslesen ");
            }
            SPSController.ReadDBEntry(productlist.SelectedIndex);
            Init_XYLists();
            this.backupprogress.Visible = false;
            this.backupprogress.Value = 0;
            this.selectionBox.Enabled = true; // Deaktiviere Bedienelemente
            this.coorBox.Enabled = true;
        }

        public void LastBackupDate()
        {
            try
            {
                var dir = new DirectoryInfo(GetBackupDirectory());
                this.lastbackup.Text = "Letztes Backup erstellt:\n" + dir.GetFiles().OrderByDescending(f => f.LastWriteTime).First().CreationTime.ToString();
            }
            catch(InvalidOperationException)
            {
                this.lastbackup.Text = "Letztes Backup erstellt:\nKeine Backups vorhanden.";
            }
        }
    }
}