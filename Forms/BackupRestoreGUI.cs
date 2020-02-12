using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SPSCtrl
{
    public partial class BackupRestoreGUI : Form
    {
        private MainGUI main;
        public BackupRestoreGUI(MainGUI main)
        {
            this.main = main;
            InitializeComponent();
            this.FillBackupList();
        }

        /// <summary>
        /// Befüllt die Liste mit allen zum Wiederherstellen verfügbaren Backups
        /// </summary>
        private void FillBackupList()
        {
            foreach (string filename in Directory.GetFiles(main.GetBackupDirectory(), "*.xml")) //Erstellt für jede Datei im Backupordner einen Listeneintrag.
            {
                ListViewItem file = new ListViewItem(Path.GetFileName(filename));
                file.SubItems.Add(File.GetCreationTime(filename).ToString());
                backuplist.Items.Add(file);
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView listview = new ListView();
            listview = (ListView)sender;

            try
            {
                if (listview.SelectedItems[0] != null)
                {
                    accept.Enabled = true;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                accept.Enabled = false;
            }
        }

        private void Accept_Click(object sender, EventArgs e)
        {
            main.RestoreLists(backuplist.SelectedItems[0].Text, this);
        }

        private void abort_Click(object sender, EventArgs e)
        {
            main.SendToConsole("Wiederherstellung abgebrochen.\n");
        }
    }
}
