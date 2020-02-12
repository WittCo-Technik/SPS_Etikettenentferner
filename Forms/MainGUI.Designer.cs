using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sharp7;

namespace SPSCtrl
{
    partial class MainGUI
    {
        /// Erforderliche Designervariable.
        private System.ComponentModel.IContainer components = null;

        /// Verwendete Ressourcen bereinigen.
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox conSettingsBox;
        private Label ip_label;
        private TextBox ip_text;
        private Button connectbutton;
        private TextBox consoleOutput;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private GroupBox groupBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private ListView xis_list;
        private ColumnHeader xis;
        private ListView yis_list;
        private ColumnHeader yis;
        private ListView xtar_list;
        private ColumnHeader xtar;
        private ListView ytar_list;
        private ColumnHeader ytar;
        private GroupBox selectionBox;
        private Label pallette_manual_label;
        private Button savebutton;
        private ListBox productlist;
        private Label cnt_label;
        private Timer drawtimer;
        private GroupBox coorBox;
        private Button discardbutton;
        private ToolTip toolTip1;
        private Label lastcleanR_label;
        private Label lastcleanL_label;
        private Label lastcycle_label;
        private Label yliveleft_label;
        private Label xliveleft_label;
        private Button backupButton;
        private Label schildsizeFront_label;
        private Label un_label;
        private Label pallet_label;
        private Label manu_label;
        private Label entrys_label;
        private Button restoreButton;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private ToolTip backupTooltip;
        private ProgressBar backupprogress;
        private Label mouse_y;
        private Label mouse_x;
        private CheckBox autochoose;
        private Label schildsizeBack_label;
        private GroupBox groupBox2;
        private Label yliveright_label;
        private Label xliveright_label;
        private Label status;
        private CheckBox excel_checkbox;
        private GroupBox groupBox3;
        private Label lastbackup;
        private GroupBox data_auto_box;
        private Button dump;
        private GroupBox data_manual_box;
        private Label manu_manual_label;
        private Label un_manual_label;
        private Label schild_manual_label;
        private Button Impressum;
        private Label comment_label;
        private TextBox comment;
        private Label product_label;
        private Label auswahl_label;
		private Label rastbar_label;
		private Label rastbar_manual_label;
	}

    

}

