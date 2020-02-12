
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;



//---------------------------Button-AKTIONEN---------------------------------------//
namespace SPSCtrl
{
    public partial class MainGUI : Form
    {

        /// <summary>
        /// Aktiviert Buttons zum Speichern/Abbrechen
        /// </summary>
        private void EnableSaveDiscardButtons(object sender, EventArgs e)
        {
            this.savebutton.Enabled = true;
            this.discardbutton.Enabled = true;
        }

        /// <summary>
        /// Deaktiviert Buttons zum Speichern/Abbrechen
        /// </summary>
        private void DisableSaveDiscardButtons(object sender, EventArgs e)
        {
            this.savebutton.Enabled = false;
            this.discardbutton.Enabled = false;
        }

        /// <summary>
        /// Sendet eine Verbindungsanfrage and ConnectToSPS();
        /// </summary>

        private void Connectbutton_Click(object sender, EventArgs e)
        {
            ConnectToSPS(ip_text.Text);
        }

        /// <summary>
        /// Aktiviert die Funktion zum Zurückschreiben auf die SPS und deaktiviert die Buttons zum Speichern und Zurücksetzen.
        /// </summary>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            Write_XYList(xtar_list, ytar_list);
            //DisableSaveDiscardButtons(sender, e);
        }

        private void Autochoose_CheckedChanged(object sender, EventArgs e)
        {
            AutoChooseProduct(sender, e);
            this.autochoose.BackColor = this.autochoose.Checked ? Color.LightGreen : Color.LightGray;
            ClearListMarks();
        }

        /// <summary>
        /// Stellt den Zustand der Soll Listen, des aktuell gewählten Produkts wieder her.
        /// </summary>
        private void Discardbutton_Click(object sender, EventArgs e)
        {
            Init_XYLists();
        }

        /// <summary>
        /// Deaktiviert Steuerelemente und erstellt ein Backup der aktuellen Liste aus der SPS.
        /// </summary>
        private void BackupButton_Click(object sender, EventArgs e)
        {
            BackupLists();
        }

        /// <summary>
        /// Ruft ein neues DialogFenster zur Auswahl eines Backups auf
        /// </summary>
        private void RestoreButton_Click(object sender, EventArgs e)
        {
            //this.Hide();
            using (BackupRestoreGUI restoreDial = new BackupRestoreGUI(this))
            {
                restoreDial.ShowDialog();
                this.Show();
            }
        }

        private void dump_Click(object sender, EventArgs e)
        {
            DumpSchildSizeXML();
        }

        private void Impressum_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format("Autor:\tJonathan Siems\nKontakt: \tjonathan.siems@gmail.com\n\t015733123395\n\nZuletzt kompiliert: {0}", buildDate.ToString()), "Impressum", MessageBoxButtons.OK);
        }
    }
}




