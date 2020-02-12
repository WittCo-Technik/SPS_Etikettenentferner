using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace SPSCtrl
{
    public partial class MainGUI : Form
    {
        private DateTime buildDate = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime;

        private const string startIP = "192.168.0.189";
        private SPSBackend SPSController; //Instanz des Backends

        private static int xylist_size; //
        private bool back; //Flag, ob hinteres Schild aktiv


        public MainGUI()
        {
            InitializeComponent(); // Initialisiert Oberfläche

            xylist_size = SPSBackend.xylist_size;

            this.ip_text.Text = startIP; //Schreibt voreingestellte IP in Textfeld

            SendToConsole("Releasemodus\n");
            
            ReadInProducts(); //Liest Produkte aus csv Datei
            LastBackupDate();
            LoadSchildXML();

            this.Text = String.Format("SPS Control (Ver. {0}-{1}-{2})", buildDate.Day, buildDate.Month, buildDate.Year);
        }

        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainGUI));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.conSettingsBox = new System.Windows.Forms.GroupBox();
			this.connectbutton = new System.Windows.Forms.Button();
			this.ip_text = new System.Windows.Forms.TextBox();
			this.consoleOutput = new System.Windows.Forms.TextBox();
			this.ip_label = new System.Windows.Forms.Label();
			this.selectionBox = new System.Windows.Forms.GroupBox();
			this.auswahl_label = new System.Windows.Forms.Label();
			this.product_label = new System.Windows.Forms.Label();
			this.lastbackup = new System.Windows.Forms.Label();
			this.autochoose = new System.Windows.Forms.CheckBox();
			this.entrys_label = new System.Windows.Forms.Label();
			this.productlist = new System.Windows.Forms.ListBox();
			this.backupprogress = new System.Windows.Forms.ProgressBar();
			this.cnt_label = new System.Windows.Forms.Label();
			this.restoreButton = new System.Windows.Forms.Button();
			this.backupButton = new System.Windows.Forms.Button();
			this.excel_checkbox = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comment_label = new System.Windows.Forms.Label();
			this.comment = new System.Windows.Forms.TextBox();
			this.data_manual_box = new System.Windows.Forms.GroupBox();
			this.manu_manual_label = new System.Windows.Forms.Label();
			this.pallette_manual_label = new System.Windows.Forms.Label();
			this.un_manual_label = new System.Windows.Forms.Label();
			this.schild_manual_label = new System.Windows.Forms.Label();
			this.lastcycle_label = new System.Windows.Forms.Label();
			this.Impressum = new System.Windows.Forms.Button();
			this.lastcleanL_label = new System.Windows.Forms.Label();
			this.lastcleanR_label = new System.Windows.Forms.Label();
			this.dump = new System.Windows.Forms.Button();
			this.data_auto_box = new System.Windows.Forms.GroupBox();
			this.schildsizeBack_label = new System.Windows.Forms.Label();
			this.manu_label = new System.Windows.Forms.Label();
			this.pallet_label = new System.Windows.Forms.Label();
			this.un_label = new System.Windows.Forms.Label();
			this.schildsizeFront_label = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.yliveright_label = new System.Windows.Forms.Label();
			this.xliveright_label = new System.Windows.Forms.Label();
			this.status = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.yliveleft_label = new System.Windows.Forms.Label();
			this.xliveleft_label = new System.Windows.Forms.Label();
			this.mouse_y = new System.Windows.Forms.Label();
			this.mouse_x = new System.Windows.Forms.Label();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.coorBox = new System.Windows.Forms.GroupBox();
			this.discardbutton = new System.Windows.Forms.Button();
			this.xis_list = new System.Windows.Forms.ListView();
			this.xis = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.savebutton = new System.Windows.Forms.Button();
			this.yis_list = new System.Windows.Forms.ListView();
			this.yis = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ytar_list = new System.Windows.Forms.ListView();
			this.ytar = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.xtar_list = new System.Windows.Forms.ListView();
			this.xtar = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.drawtimer = new System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
			this.backupTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.rastbar_label = new System.Windows.Forms.Label();
			this.rastbar_manual_label = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.conSettingsBox.SuspendLayout();
			this.selectionBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.data_manual_box.SuspendLayout();
			this.data_auto_box.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.coorBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.80252F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.68277F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.56723F));
			this.tableLayoutPanel1.Controls.Add(this.conSettingsBox, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.groupBox1, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.coorBox, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.620209F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.37979F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1904, 1045);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// conSettingsBox
			// 
			this.conSettingsBox.Controls.Add(this.connectbutton);
			this.conSettingsBox.Controls.Add(this.ip_text);
			this.conSettingsBox.Controls.Add(this.consoleOutput);
			this.conSettingsBox.Controls.Add(this.ip_label);
			this.conSettingsBox.Controls.Add(this.selectionBox);
			this.conSettingsBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.conSettingsBox.Location = new System.Drawing.Point(3, 3);
			this.conSettingsBox.Name = "conSettingsBox";
			this.tableLayoutPanel1.SetRowSpan(this.conSettingsBox, 2);
			this.conSettingsBox.Size = new System.Drawing.Size(351, 1018);
			this.conSettingsBox.TabIndex = 7;
			this.conSettingsBox.TabStop = false;
			this.conSettingsBox.Text = "Verbindungseinstellungen";
			// 
			// connectbutton
			// 
			this.connectbutton.Location = new System.Drawing.Point(185, 14);
			this.connectbutton.Name = "connectbutton";
			this.connectbutton.Size = new System.Drawing.Size(75, 24);
			this.connectbutton.TabIndex = 6;
			this.connectbutton.Text = "Verbinden";
			this.connectbutton.UseVisualStyleBackColor = true;
			this.connectbutton.Click += new System.EventHandler(this.Connectbutton_Click);
			// 
			// ip_text
			// 
			this.ip_text.Location = new System.Drawing.Point(65, 17);
			this.ip_text.Name = "ip_text";
			this.ip_text.Size = new System.Drawing.Size(112, 20);
			this.ip_text.TabIndex = 3;
			this.ip_text.TextChanged += new System.EventHandler(this.Ip_TextChanged);
			this.ip_text.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EnterIP);
			// 
			// consoleOutput
			// 
			this.consoleOutput.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.consoleOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.consoleOutput.ForeColor = System.Drawing.SystemColors.Window;
			this.consoleOutput.Location = new System.Drawing.Point(6, 42);
			this.consoleOutput.Multiline = true;
			this.consoleOutput.Name = "consoleOutput";
			this.consoleOutput.ReadOnly = true;
			this.consoleOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.consoleOutput.Size = new System.Drawing.Size(254, 57);
			this.consoleOutput.TabIndex = 12;
			this.consoleOutput.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ResetConsole);
			// 
			// ip_label
			// 
			this.ip_label.AutoSize = true;
			this.ip_label.Location = new System.Drawing.Point(5, 20);
			this.ip_label.Name = "ip_label";
			this.ip_label.Size = new System.Drawing.Size(61, 13);
			this.ip_label.TabIndex = 0;
			this.ip_label.Text = "IP-Adresse:";
			// 
			// selectionBox
			// 
			this.selectionBox.Controls.Add(this.auswahl_label);
			this.selectionBox.Controls.Add(this.product_label);
			this.selectionBox.Controls.Add(this.lastbackup);
			this.selectionBox.Controls.Add(this.autochoose);
			this.selectionBox.Controls.Add(this.entrys_label);
			this.selectionBox.Controls.Add(this.productlist);
			this.selectionBox.Controls.Add(this.backupprogress);
			this.selectionBox.Controls.Add(this.cnt_label);
			this.selectionBox.Controls.Add(this.restoreButton);
			this.selectionBox.Controls.Add(this.backupButton);
			this.selectionBox.Controls.Add(this.excel_checkbox);
			this.selectionBox.Location = new System.Drawing.Point(0, 105);
			this.selectionBox.Name = "selectionBox";
			this.selectionBox.Size = new System.Drawing.Size(345, 796);
			this.selectionBox.TabIndex = 6;
			this.selectionBox.TabStop = false;
			this.selectionBox.Text = "Auswahl";
			this.selectionBox.Visible = false;
			// 
			// auswahl_label
			// 
			this.auswahl_label.AutoSize = true;
			this.auswahl_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.auswahl_label.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.auswahl_label.Location = new System.Drawing.Point(11, 653);
			this.auswahl_label.Name = "auswahl_label";
			this.auswahl_label.Size = new System.Drawing.Size(83, 22);
			this.auswahl_label.TabIndex = 15;
			this.auswahl_label.Text = "Auswahl:";
			// 
			// product_label
			// 
			this.product_label.AutoSize = true;
			this.product_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.product_label.Location = new System.Drawing.Point(92, 653);
			this.product_label.Name = "product_label";
			this.product_label.Size = new System.Drawing.Size(59, 22);
			this.product_label.TabIndex = 14;
			this.product_label.Text = "Schild";
			// 
			// lastbackup
			// 
			this.lastbackup.AutoSize = true;
			this.lastbackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.lastbackup.Location = new System.Drawing.Point(12, 697);
			this.lastbackup.Name = "lastbackup";
			this.lastbackup.Size = new System.Drawing.Size(186, 18);
			this.lastbackup.TabIndex = 11;
			this.lastbackup.Text = "Letztes Backup erstellt am:";
			// 
			// autochoose
			// 
			this.autochoose.AutoSize = true;
			this.autochoose.BackColor = System.Drawing.Color.LightGray;
			this.autochoose.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.autochoose.Enabled = false;
			this.autochoose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
			this.autochoose.Location = new System.Drawing.Point(24, 16);
			this.autochoose.Name = "autochoose";
			this.autochoose.Size = new System.Drawing.Size(198, 24);
			this.autochoose.TabIndex = 13;
			this.autochoose.Text = "Automatische Auswahl";
			this.autochoose.UseVisualStyleBackColor = false;
			this.autochoose.CheckedChanged += new System.EventHandler(this.Autochoose_CheckedChanged);
			// 
			// entrys_label
			// 
			this.entrys_label.AutoSize = true;
			this.entrys_label.Location = new System.Drawing.Point(111, 838);
			this.entrys_label.Name = "entrys_label";
			this.entrys_label.Size = new System.Drawing.Size(49, 13);
			this.entrys_label.TabIndex = 12;
			this.entrys_label.Text = "Einträge:";
			// 
			// productlist
			// 
			this.productlist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.productlist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold);
			this.productlist.FormattingEnabled = true;
			this.productlist.ItemHeight = 20;
			this.productlist.Location = new System.Drawing.Point(15, 68);
			this.productlist.Name = "productlist";
			this.productlist.Size = new System.Drawing.Size(207, 582);
			this.productlist.TabIndex = 10;
			this.productlist.SelectedIndexChanged += new System.EventHandler(this.Productlist_SelectedIndexChanged);
			// 
			// backupprogress
			// 
			this.backupprogress.Location = new System.Drawing.Point(12, 772);
			this.backupprogress.Maximum = 208;
			this.backupprogress.Name = "backupprogress";
			this.backupprogress.Size = new System.Drawing.Size(272, 18);
			this.backupprogress.Step = 1;
			this.backupprogress.TabIndex = 9;
			this.backupprogress.Visible = false;
			// 
			// cnt_label
			// 
			this.cnt_label.AutoSize = true;
			this.cnt_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.cnt_label.Location = new System.Drawing.Point(21, 47);
			this.cnt_label.Name = "cnt_label";
			this.cnt_label.Size = new System.Drawing.Size(62, 18);
			this.cnt_label.TabIndex = 4;
			this.cnt_label.Text = "Punkte: ";
			// 
			// restoreButton
			// 
			this.restoreButton.Enabled = false;
			this.restoreButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.restoreButton.Location = new System.Drawing.Point(148, 739);
			this.restoreButton.Name = "restoreButton";
			this.restoreButton.Size = new System.Drawing.Size(126, 28);
			this.restoreButton.TabIndex = 8;
			this.restoreButton.Text = "Wiederherstellen";
			this.restoreButton.UseVisualStyleBackColor = true;
			this.restoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
			// 
			// backupButton
			// 
			this.backupButton.Enabled = false;
			this.backupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.backupButton.Location = new System.Drawing.Point(12, 739);
			this.backupButton.Name = "backupButton";
			this.backupButton.Size = new System.Drawing.Size(75, 27);
			this.backupButton.TabIndex = 7;
			this.backupButton.Text = "Backup";
			this.backupTooltip.SetToolTip(this.backupButton, "Erstellt eine Sicherungskopie aller Einträge");
			this.backupButton.UseVisualStyleBackColor = true;
			this.backupButton.Click += new System.EventHandler(this.BackupButton_Click);
			// 
			// excel_checkbox
			// 
			this.excel_checkbox.AutoSize = true;
			this.excel_checkbox.Location = new System.Drawing.Point(91, 738);
			this.excel_checkbox.Name = "excel_checkbox";
			this.excel_checkbox.Size = new System.Drawing.Size(63, 30);
			this.excel_checkbox.TabIndex = 10;
			this.excel_checkbox.Text = "Excel\nBackup";
			this.excel_checkbox.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.groupBox1.Controls.Add(this.comment_label);
			this.groupBox1.Controls.Add(this.comment);
			this.groupBox1.Controls.Add(this.lastcycle_label);
			this.groupBox1.Controls.Add(this.Impressum);
			this.groupBox1.Controls.Add(this.lastcleanL_label);
			this.groupBox1.Controls.Add(this.lastcleanR_label);
			this.groupBox1.Controls.Add(this.dump);
			this.groupBox1.Controls.Add(this.groupBox3);
			this.groupBox1.Controls.Add(this.status);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.mouse_y);
			this.groupBox1.Controls.Add(this.mouse_x);
			this.groupBox1.Controls.Add(this.chart1);
			this.groupBox1.Controls.Add(this.data_manual_box);
			this.groupBox1.Controls.Add(this.data_auto_box);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(848, 3);
			this.groupBox1.Name = "groupBox1";
			this.tableLayoutPanel1.SetRowSpan(this.groupBox1, 2);
			this.groupBox1.Size = new System.Drawing.Size(1053, 1018);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Koordinaten";
			// 
			// comment_label
			// 
			this.comment_label.AutoSize = true;
			this.comment_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.comment_label.Location = new System.Drawing.Point(290, 683);
			this.comment_label.Name = "comment_label";
			this.comment_label.Size = new System.Drawing.Size(106, 22);
			this.comment_label.TabIndex = 36;
			this.comment_label.Text = "Kommentar:";
			// 
			// comment
			// 
			this.comment.AcceptsReturn = true;
			this.comment.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.comment.Location = new System.Drawing.Point(294, 706);
			this.comment.Multiline = true;
			this.comment.Name = "comment";
			this.comment.Size = new System.Drawing.Size(379, 46);
			this.comment.TabIndex = 35;
			this.comment.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EnterComment);
			// 
			// data_manual_box
			// 
			this.data_manual_box.Controls.Add(this.rastbar_manual_label);
			this.data_manual_box.Controls.Add(this.manu_manual_label);
			this.data_manual_box.Controls.Add(this.pallette_manual_label);
			this.data_manual_box.Controls.Add(this.un_manual_label);
			this.data_manual_box.Controls.Add(this.schild_manual_label);
			this.data_manual_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
			this.data_manual_box.Location = new System.Drawing.Point(23, 758);
			this.data_manual_box.Name = "data_manual_box";
			this.data_manual_box.Size = new System.Drawing.Size(373, 205);
			this.data_manual_box.TabIndex = 34;
			this.data_manual_box.TabStop = false;
			this.data_manual_box.Text = "Daten (Manuell)";
			// 
			// manu_manual_label
			// 
			this.manu_manual_label.AutoSize = true;
			this.manu_manual_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.manu_manual_label.Location = new System.Drawing.Point(16, 23);
			this.manu_manual_label.Name = "manu_manual_label";
			this.manu_manual_label.Size = new System.Drawing.Size(92, 22);
			this.manu_manual_label.TabIndex = 23;
			this.manu_manual_label.Text = "Hersteller:";
			// 
			// pallette_manual_label
			// 
			this.pallette_manual_label.AutoSize = true;
			this.pallette_manual_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.pallette_manual_label.Location = new System.Drawing.Point(17, 54);
			this.pallette_manual_label.Name = "pallette_manual_label";
			this.pallette_manual_label.Size = new System.Drawing.Size(71, 22);
			this.pallette_manual_label.TabIndex = 11;
			this.pallette_manual_label.Text = "Palette:";
			// 
			// un_manual_label
			// 
			this.un_manual_label.AutoSize = true;
			this.un_manual_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.un_manual_label.Location = new System.Drawing.Point(18, 117);
			this.un_manual_label.Name = "un_manual_label";
			this.un_manual_label.Size = new System.Drawing.Size(41, 22);
			this.un_manual_label.TabIndex = 25;
			this.un_manual_label.Text = "UN:";
			// 
			// schild_manual_label
			// 
			this.schild_manual_label.AutoSize = true;
			this.schild_manual_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.schild_manual_label.Location = new System.Drawing.Point(16, 86);
			this.schild_manual_label.Name = "schild_manual_label";
			this.schild_manual_label.Size = new System.Drawing.Size(116, 22);
			this.schild_manual_label.TabIndex = 26;
			this.schild_manual_label.Text = "Schildgröße: ";
			// 
			// lastcycle_label
			// 
			this.lastcycle_label.AutoSize = true;
			this.lastcycle_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.lastcycle_label.Location = new System.Drawing.Point(455, 768);
			this.lastcycle_label.Name = "lastcycle_label";
			this.lastcycle_label.Size = new System.Drawing.Size(126, 22);
			this.lastcycle_label.TabIndex = 20;
			this.lastcycle_label.Text = "Letzter Zyklus:";
			// 
			// Impressum
			// 
			this.Impressum.BackgroundImage = global::SPSCtrl.Properties.Resources.logo;
			this.Impressum.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.Impressum.Location = new System.Drawing.Point(679, 700);
			this.Impressum.Name = "Impressum";
			this.Impressum.Size = new System.Drawing.Size(140, 55);
			this.Impressum.TabIndex = 15;
			this.Impressum.UseVisualStyleBackColor = true;
			this.Impressum.Click += new System.EventHandler(this.Impressum_Click);
			// 
			// lastcleanL_label
			// 
			this.lastcleanL_label.AutoSize = true;
			this.lastcleanL_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.lastcleanL_label.Location = new System.Drawing.Point(455, 794);
			this.lastcleanL_label.Name = "lastcleanL_label";
			this.lastcleanL_label.Size = new System.Drawing.Size(164, 22);
			this.lastcleanL_label.TabIndex = 21;
			this.lastcleanL_label.Text = "Letzte Reinigung L:";
			// 
			// lastcleanR_label
			// 
			this.lastcleanR_label.AutoSize = true;
			this.lastcleanR_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.lastcleanR_label.Location = new System.Drawing.Point(455, 820);
			this.lastcleanR_label.Name = "lastcleanR_label";
			this.lastcleanR_label.Size = new System.Drawing.Size(167, 22);
			this.lastcleanR_label.TabIndex = 22;
			this.lastcleanR_label.Text = "Letzte Reinigung R:";
			// 
			// dump
			// 
			this.dump.Location = new System.Drawing.Point(794, 955);
			this.dump.Name = "dump";
			this.dump.Size = new System.Drawing.Size(104, 23);
			this.dump.TabIndex = 34;
			this.dump.Text = "Schilder speichern";
			this.dump.UseVisualStyleBackColor = true;
			this.dump.Visible = false;
			this.dump.Click += new System.EventHandler(this.dump_Click);
			// 
			// data_auto_box
			// 
			this.data_auto_box.Controls.Add(this.rastbar_label);
			this.data_auto_box.Controls.Add(this.schildsizeBack_label);
			this.data_auto_box.Controls.Add(this.manu_label);
			this.data_auto_box.Controls.Add(this.pallet_label);
			this.data_auto_box.Controls.Add(this.un_label);
			this.data_auto_box.Controls.Add(this.schildsizeFront_label);
			this.data_auto_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
			this.data_auto_box.Location = new System.Drawing.Point(23, 759);
			this.data_auto_box.Name = "data_auto_box";
			this.data_auto_box.Size = new System.Drawing.Size(373, 205);
			this.data_auto_box.TabIndex = 33;
			this.data_auto_box.TabStop = false;
			this.data_auto_box.Text = "Daten (Automatisch)";
			// 
			// schildsizeBack_label
			// 
			this.schildsizeBack_label.AutoSize = true;
			this.schildsizeBack_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.schildsizeBack_label.Location = new System.Drawing.Point(16, 119);
			this.schildsizeBack_label.Name = "schildsizeBack_label";
			this.schildsizeBack_label.Size = new System.Drawing.Size(219, 22);
			this.schildsizeBack_label.TabIndex = 30;
			this.schildsizeBack_label.Text = "Schildgröße (Hinterseite): ";
			// 
			// manu_label
			// 
			this.manu_label.AutoSize = true;
			this.manu_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.manu_label.Location = new System.Drawing.Point(16, 23);
			this.manu_label.Name = "manu_label";
			this.manu_label.Size = new System.Drawing.Size(92, 22);
			this.manu_label.TabIndex = 23;
			this.manu_label.Text = "Hersteller:";
			// 
			// pallet_label
			// 
			this.pallet_label.AutoSize = true;
			this.pallet_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.pallet_label.Location = new System.Drawing.Point(16, 54);
			this.pallet_label.Name = "pallet_label";
			this.pallet_label.Size = new System.Drawing.Size(75, 22);
			this.pallet_label.TabIndex = 24;
			this.pallet_label.Text = "Pallette:";
			// 
			// un_label
			// 
			this.un_label.AutoSize = true;
			this.un_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.un_label.Location = new System.Drawing.Point(16, 148);
			this.un_label.Name = "un_label";
			this.un_label.Size = new System.Drawing.Size(41, 22);
			this.un_label.TabIndex = 25;
			this.un_label.Text = "UN:";
			// 
			// schildsizeFront_label
			// 
			this.schildsizeFront_label.AutoSize = true;
			this.schildsizeFront_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.schildsizeFront_label.Location = new System.Drawing.Point(16, 87);
			this.schildsizeFront_label.Name = "schildsizeFront_label";
			this.schildsizeFront_label.Size = new System.Drawing.Size(225, 22);
			this.schildsizeFront_label.TabIndex = 26;
			this.schildsizeFront_label.Text = "Schildgröße (Vorderseite): ";
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox3.Controls.Add(this.yliveright_label);
			this.groupBox3.Controls.Add(this.xliveright_label);
			this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.groupBox3.Location = new System.Drawing.Point(255, 17);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(141, 70);
			this.groupBox3.TabIndex = 32;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Düse Rückseite";
			// 
			// yliveright_label
			// 
			this.yliveright_label.AutoSize = true;
			this.yliveright_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.yliveright_label.Location = new System.Drawing.Point(43, 48);
			this.yliveright_label.Name = "yliveright_label";
			this.yliveright_label.Size = new System.Drawing.Size(21, 18);
			this.yliveright_label.TabIndex = 23;
			this.yliveright_label.Text = "Y:";
			// 
			// xliveright_label
			// 
			this.xliveright_label.AutoSize = true;
			this.xliveright_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.xliveright_label.Location = new System.Drawing.Point(43, 22);
			this.xliveright_label.Name = "xliveright_label";
			this.xliveright_label.Size = new System.Drawing.Size(22, 18);
			this.xliveright_label.TabIndex = 22;
			this.xliveright_label.Text = "X:";
			// 
			// status
			// 
			this.status.AutoSize = true;
			this.status.BackColor = System.Drawing.SystemColors.Window;
			this.status.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
			this.status.Location = new System.Drawing.Point(19, 681);
			this.status.Name = "status";
			this.status.Size = new System.Drawing.Size(65, 24);
			this.status.TabIndex = 32;
			this.status.Text = "Status:";
			this.status.TextChanged += new System.EventHandler(this.AutoChooseProduct);
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.groupBox2.Controls.Add(this.yliveleft_label);
			this.groupBox2.Controls.Add(this.xliveleft_label);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.groupBox2.Location = new System.Drawing.Point(91, 17);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(141, 70);
			this.groupBox2.TabIndex = 31;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Düse Vorderseite";
			// 
			// yliveleft_label
			// 
			this.yliveleft_label.AutoSize = true;
			this.yliveleft_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.yliveleft_label.Location = new System.Drawing.Point(39, 48);
			this.yliveleft_label.Name = "yliveleft_label";
			this.yliveleft_label.Size = new System.Drawing.Size(21, 18);
			this.yliveleft_label.TabIndex = 19;
			this.yliveleft_label.Text = "Y:";
			// 
			// xliveleft_label
			// 
			this.xliveleft_label.AutoSize = true;
			this.xliveleft_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.xliveleft_label.Location = new System.Drawing.Point(38, 22);
			this.xliveleft_label.Name = "xliveleft_label";
			this.xliveleft_label.Size = new System.Drawing.Size(22, 18);
			this.xliveleft_label.TabIndex = 18;
			this.xliveleft_label.Text = "X:";
			// 
			// mouse_y
			// 
			this.mouse_y.AutoSize = true;
			this.mouse_y.BackColor = System.Drawing.SystemColors.Window;
			this.mouse_y.Location = new System.Drawing.Point(723, 22);
			this.mouse_y.Name = "mouse_y";
			this.mouse_y.Size = new System.Drawing.Size(0, 13);
			this.mouse_y.TabIndex = 11;
			// 
			// mouse_x
			// 
			this.mouse_x.AutoSize = true;
			this.mouse_x.BackColor = System.Drawing.SystemColors.Window;
			this.mouse_x.Location = new System.Drawing.Point(622, 21);
			this.mouse_x.Name = "mouse_x";
			this.mouse_x.Size = new System.Drawing.Size(0, 13);
			this.mouse_x.TabIndex = 10;
			// 
			// chart1
			// 
			this.chart1.BorderlineColor = System.Drawing.Color.Empty;
			chartArea1.AxisX.Crossing = -1.7976931348623157E+308D;
			chartArea1.AxisX.Interval = 100D;
			chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
			chartArea1.AxisX.IsLabelAutoFit = false;
			chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
			chartArea1.AxisX.Maximum = 1000D;
			chartArea1.AxisX.Minimum = 0D;
			chartArea1.AxisY.Interval = 100D;
			chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea1.AxisY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
			chartArea1.AxisY.IsLabelAutoFit = false;
			chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
			chartArea1.AxisY.Maximum = 1200D;
			chartArea1.AxisY.Minimum = 0D;
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			this.chart1.Location = new System.Drawing.Point(11, 16);
			this.chart1.Name = "chart1";
			this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
			this.chart1.Size = new System.Drawing.Size(817, 657);
			this.chart1.TabIndex = 14;
			this.chart1.Text = "chart1";
			// 
			// coorBox
			// 
			this.coorBox.Controls.Add(this.discardbutton);
			this.coorBox.Controls.Add(this.xis_list);
			this.coorBox.Controls.Add(this.savebutton);
			this.coorBox.Controls.Add(this.yis_list);
			this.coorBox.Controls.Add(this.ytar_list);
			this.coorBox.Controls.Add(this.xtar_list);
			this.coorBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.coorBox.Location = new System.Drawing.Point(360, 3);
			this.coorBox.Name = "coorBox";
			this.tableLayoutPanel1.SetRowSpan(this.coorBox, 2);
			this.coorBox.Size = new System.Drawing.Size(482, 1018);
			this.coorBox.TabIndex = 14;
			this.coorBox.TabStop = false;
			this.coorBox.Text = "Koordinaten";
			// 
			// discardbutton
			// 
			this.discardbutton.Enabled = false;
			this.discardbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
			this.discardbutton.ForeColor = System.Drawing.Color.Red;
			this.discardbutton.Location = new System.Drawing.Point(235, 16);
			this.discardbutton.Name = "discardbutton";
			this.discardbutton.Size = new System.Drawing.Size(104, 42);
			this.discardbutton.TabIndex = 6;
			this.discardbutton.Text = "Änderungen verwerfen";
			this.toolTip1.SetToolTip(this.discardbutton, "Setzt alle Änderungen auf die Ist Werte zurück.");
			this.discardbutton.UseVisualStyleBackColor = true;
			this.discardbutton.Click += new System.EventHandler(this.Discardbutton_Click);
			// 
			// xis_list
			// 
			this.xis_list.BackColor = System.Drawing.SystemColors.ControlLight;
			this.xis_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.xis});
			this.xis_list.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.xis_list.GridLines = true;
			this.xis_list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.xis_list.HideSelection = false;
			this.xis_list.LabelWrap = false;
			this.xis_list.Location = new System.Drawing.Point(12, 65);
			this.xis_list.MultiSelect = false;
			this.xis_list.Name = "xis_list";
			this.xis_list.Size = new System.Drawing.Size(85, 724);
			this.xis_list.TabIndex = 0;
			this.xis_list.UseCompatibleStateImageBehavior = false;
			this.xis_list.View = System.Windows.Forms.View.Details;
			this.xis_list.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListMousePosition);
			// 
			// xis
			// 
			this.xis.Text = "X Ist";
			this.xis.Width = 80;
			// 
			// savebutton
			// 
			this.savebutton.Enabled = false;
			this.savebutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
			this.savebutton.ForeColor = System.Drawing.Color.Green;
			this.savebutton.Location = new System.Drawing.Point(6, 17);
			this.savebutton.Name = "savebutton";
			this.savebutton.Size = new System.Drawing.Size(104, 42);
			this.savebutton.TabIndex = 5;
			this.savebutton.Text = "Änderungen speichern";
			this.savebutton.UseVisualStyleBackColor = true;
			this.savebutton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// yis_list
			// 
			this.yis_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.yis});
			this.yis_list.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.yis_list.GridLines = true;
			this.yis_list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.yis_list.HideSelection = false;
			this.yis_list.LabelWrap = false;
			this.yis_list.Location = new System.Drawing.Point(98, 65);
			this.yis_list.MultiSelect = false;
			this.yis_list.Name = "yis_list";
			this.yis_list.Size = new System.Drawing.Size(85, 724);
			this.yis_list.TabIndex = 1;
			this.yis_list.UseCompatibleStateImageBehavior = false;
			this.yis_list.View = System.Windows.Forms.View.Details;
			this.yis_list.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListMousePosition);
			// 
			// yis
			// 
			this.yis.Text = "Y Ist";
			this.yis.Width = 80;
			// 
			// ytar_list
			// 
			this.ytar_list.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.ytar_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ytar});
			this.ytar_list.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ytar_list.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.ytar_list.GridLines = true;
			this.ytar_list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.ytar_list.HideSelection = false;
			this.ytar_list.LabelEdit = true;
			this.ytar_list.LabelWrap = false;
			this.ytar_list.Location = new System.Drawing.Point(294, 65);
			this.ytar_list.MultiSelect = false;
			this.ytar_list.Name = "ytar_list";
			this.ytar_list.Size = new System.Drawing.Size(85, 724);
			this.ytar_list.TabIndex = 3;
			this.ytar_list.UseCompatibleStateImageBehavior = false;
			this.ytar_list.View = System.Windows.Forms.View.Details;
			this.ytar_list.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.CheckNumericValue);
			this.ytar_list.ItemActivate += new System.EventHandler(this.ClickEditLabel);
			this.ytar_list.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListMousePosition);
			// 
			// ytar
			// 
			this.ytar.Text = "Y Soll";
			this.ytar.Width = 80;
			// 
			// xtar_list
			// 
			this.xtar_list.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.xtar_list.BackColor = System.Drawing.SystemColors.ControlLight;
			this.xtar_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.xtar});
			this.xtar_list.Cursor = System.Windows.Forms.Cursors.Hand;
			this.xtar_list.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.xtar_list.GridLines = true;
			this.xtar_list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.xtar_list.HideSelection = false;
			this.xtar_list.LabelEdit = true;
			this.xtar_list.LabelWrap = false;
			this.xtar_list.Location = new System.Drawing.Point(208, 65);
			this.xtar_list.MultiSelect = false;
			this.xtar_list.Name = "xtar_list";
			this.xtar_list.ShowGroups = false;
			this.xtar_list.Size = new System.Drawing.Size(85, 724);
			this.xtar_list.TabIndex = 2;
			this.xtar_list.UseCompatibleStateImageBehavior = false;
			this.xtar_list.View = System.Windows.Forms.View.Details;
			this.xtar_list.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.CheckNumericValue);
			this.xtar_list.ItemActivate += new System.EventHandler(this.ClickEditLabel);
			this.xtar_list.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListMousePosition);
			// 
			// xtar
			// 
			this.xtar.Text = "X Soll";
			this.xtar.Width = 80;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			// 
			// Column1
			// 
			this.Column1.Name = "Column1";
			// 
			// Column2
			// 
			this.Column2.Name = "Column2";
			// 
			// Column3
			// 
			this.Column3.Name = "Column3";
			// 
			// drawtimer
			// 
			this.drawtimer.Interval = 5;
			this.drawtimer.Tick += new System.EventHandler(this.Timer_Tick);
			// 
			// fileSystemWatcher1
			// 
			this.fileSystemWatcher1.EnableRaisingEvents = true;
			this.fileSystemWatcher1.SynchronizingObject = this;
			// 
			// rastbar_label
			// 
			this.rastbar_label.AutoSize = true;
			this.rastbar_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.rastbar_label.Location = new System.Drawing.Point(17, 174);
			this.rastbar_label.Name = "rastbar_label";
			this.rastbar_label.Size = new System.Drawing.Size(83, 22);
			this.rastbar_label.TabIndex = 31;
			this.rastbar_label.Text = "Rastbar: ";
			// 
			// rastbar_manual_label
			// 
			this.rastbar_manual_label.AutoSize = true;
			this.rastbar_manual_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.rastbar_manual_label.Location = new System.Drawing.Point(18, 149);
			this.rastbar_manual_label.Name = "rastbar_manual_label";
			this.rastbar_manual_label.Size = new System.Drawing.Size(83, 22);
			this.rastbar_manual_label.TabIndex = 32;
			this.rastbar_manual_label.Text = "Rastbar: ";
			// 
			// MainGUI
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.ClientSize = new System.Drawing.Size(1904, 1045);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainGUI";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SPS Control";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.tableLayoutPanel1.ResumeLayout(false);
			this.conSettingsBox.ResumeLayout(false);
			this.conSettingsBox.PerformLayout();
			this.selectionBox.ResumeLayout(false);
			this.selectionBox.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.data_manual_box.ResumeLayout(false);
			this.data_manual_box.PerformLayout();
			this.data_auto_box.ResumeLayout(false);
			this.data_auto_box.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.coorBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
			this.ResumeLayout(false);

        }

        /// <summary>
        /// Setzt nach einem Fehler die Hintergrundfarbe der Console wieder zurück.
        /// </summary>
        private void ResetConsole(object sender, MouseEventArgs e)
        {
            consoleOutput.BackColor = Color.Green;
            consoleOutput.ForeColor = Color.Black;
        }

        private void EnterComment(object sender, KeyEventArgs e)
        {
            SetComment(this.productlist.SelectedIndex, comment.Text);
        }

        private void EnterIP(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Connectbutton_Click(sender, e);
            }
        }

        /// <summary>
        /// Stellt bei Eingabe einer IP-Adresse die Verbindung zu einer SPS mit Standarteinstellungen her.
        /// </summary>
        private void ConnectToSPS(string ip)
        {
            this.SPSController = new SPSBackend(ip);

            if (SPSController.IsConnected)
            {
                this.connectbutton.Enabled = false;
                this.ip_text.Enabled = false;

                SendToConsole("SPS Verbindung hergestellt\n");

                this.selectionBox.Visible = true;
                this.consoleOutput.BackColor = Color.Green;
                this.productlist.Enabled = true;
                this.backupButton.Enabled = true;

                this.productlist.SelectedIndex = 0;
                autochoose.Enabled = true;
                restoreButton.Enabled = true;
                savebutton.Enabled = true;
                discardbutton.Enabled = true;

                if(products.Count == 208)
                    BackupLists(true); //Automatisches Backup, wenn verbunden und Produktliste gefüllt.
            }
            else
            {
                SendToConsole("Verbindungsfehler!\n");
                this.consoleOutput.BackColor = Color.Red;
                this.productlist.Enabled = false;
            }
        }

        /// <summary>
        /// Wird aufgerufen wenn sich der TExt im IP-Feld ändert
        /// </summary>
        private void Ip_TextChanged(object sender, EventArgs e)
        {
            Clear_XYLists();
            connectbutton.Enabled = true;
            drawtimer.Enabled = false;
        }

        /// <summary>
        /// Wird aufgerufen wenn sich der 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Productlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductListChanged();
        }

        /// <summary>
        /// Gibt einen übergebenden String im Konsolenfenster der Andwendung aus
        /// </summary>
        public void SendToConsole(string output)
        {
            this.consoleOutput.AppendText(output);
            this.consoleOutput.ScrollToCaret();
        }

        /// <summary>
        /// Mit jedem Tick werden Graphen und Metadaten (bei Änderung) neu gezeichnet. 
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckBack();
            RedrawChart();
            RedrawLiveNozzle();
            RedrawLiveLabels();
        }

        /// <summary>
        /// Setzt den boolschen Wert 'back', wenn das Hintere Schild gereinigt wird.
        /// </summary>
        /// <returns></returns>
        private bool CheckBack()
        {
            back = (SPSController.state == 1 || SPSController.state == 2) ? true : false;
            return back;
        }
    }
}
