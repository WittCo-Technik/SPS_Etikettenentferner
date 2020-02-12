using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPSCtrl
{
    public partial class Form1 : Form
    {
        private SPSClass SPSController;
        private int listlength;
        public Form1()
        {
            this.SPSController = new SPSClass("192.168.0.154", 0, 0);
            //this.SPSController.readDBEntry(0);
            this.SPSController.DebugRead(); //Befüllt den SPSController mit Zufallszahlen zu Testzwecken
            this.listlength = SPSClass.arraysize;

            InitializeComponent();
            InitListView();
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.xis_list = new System.Windows.Forms.ListView();
            this.xis = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.yis_list = new System.Windows.Forms.ListView();
            this.yis = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.xtar_list = new System.Windows.Forms.ListView();
            this.xtar = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ytar_list = new System.Windows.Forms.ListView();
            this.ytar = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.37263F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.62737F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1101, 687);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.xis_list);
            this.flowLayoutPanel1.Controls.Add(this.yis_list);
            this.flowLayoutPanel1.Controls.Add(this.xtar_list);
            this.flowLayoutPanel1.Controls.Add(this.ytar_list);
            this.flowLayoutPanel1.Controls.Add(this.numericUpDown1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 87);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(544, 597);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // xis_list
            // 
            this.xis_list.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.xis_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.xis});
            this.xis_list.GridLines = true;
            this.xis_list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.xis_list.HideSelection = false;
            this.xis_list.LabelEdit = true;
            this.xis_list.LabelWrap = false;
            this.xis_list.Location = new System.Drawing.Point(3, 3);
            this.xis_list.MultiSelect = false;
            this.xis_list.Name = "xis_list";
            this.xis_list.Scrollable = false;
            this.xis_list.Size = new System.Drawing.Size(82, 566);
            this.xis_list.TabIndex = 0;
            this.xis_list.UseCompatibleStateImageBehavior = false;
            this.xis_list.View = System.Windows.Forms.View.Details;
            // 
            // xis
            // 
            this.xis.Text = "X Ist";
            this.xis.Width = 80;
            // 
            // yis_list
            // 
            this.yis_list.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.yis_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.yis});
            this.yis_list.GridLines = true;
            this.yis_list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.yis_list.HideSelection = false;
            this.yis_list.LabelEdit = true;
            this.yis_list.LabelWrap = false;
            this.yis_list.Location = new System.Drawing.Point(91, 3);
            this.yis_list.MultiSelect = false;
            this.yis_list.Name = "yis_list";
            this.yis_list.Scrollable = false;
            this.yis_list.Size = new System.Drawing.Size(82, 566);
            this.yis_list.TabIndex = 1;
            this.yis_list.UseCompatibleStateImageBehavior = false;
            this.yis_list.View = System.Windows.Forms.View.Details;
            // 
            // yis
            // 
            this.yis.Text = "Y Ist";
            this.yis.Width = 80;
            // 
            // xtar_list
            // 
            this.xtar_list.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.xtar_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.xtar});
            this.xtar_list.GridLines = true;
            this.xtar_list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.xtar_list.HideSelection = false;
            this.xtar_list.LabelEdit = true;
            this.xtar_list.LabelWrap = false;
            this.xtar_list.Location = new System.Drawing.Point(179, 3);
            this.xtar_list.MultiSelect = false;
            this.xtar_list.Name = "xtar_list";
            this.xtar_list.Scrollable = false;
            this.xtar_list.Size = new System.Drawing.Size(82, 566);
            this.xtar_list.TabIndex = 2;
            this.xtar_list.UseCompatibleStateImageBehavior = false;
            this.xtar_list.View = System.Windows.Forms.View.Details;
            // 
            // xtar
            // 
            this.xtar.Text = "X Soll";
            this.xtar.Width = 80;
            // 
            // ytar_list
            // 
            this.ytar_list.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.ytar_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ytar});
            this.ytar_list.GridLines = true;
            this.ytar_list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ytar_list.HideSelection = false;
            this.ytar_list.LabelEdit = true;
            this.ytar_list.LabelWrap = false;
            this.ytar_list.Location = new System.Drawing.Point(267, 3);
            this.ytar_list.MultiSelect = false;
            this.ytar_list.Name = "ytar_list";
            this.ytar_list.Scrollable = false;
            this.ytar_list.Size = new System.Drawing.Size(82, 566);
            this.ytar_list.TabIndex = 3;
            this.ytar_list.UseCompatibleStateImageBehavior = false;
            this.ytar_list.View = System.Windows.Forms.View.Details;
            // 
            // ytar
            // 
            this.ytar.Text = "Y Soll";
            this.ytar.Width = 80;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(355, 3);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 4;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1101, 687);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void InitListView()
        {
            ClearLists();

            for (int i = 0; i < listlength; i++) //Liest Koordianten aus SPSController in Tabellen ein
            {
                //Ist
                xis_list.Items.Add(new ListViewItem(String.Format("{0}", SPSController.GetXVal(i)), 0));
                yis_list.Items.Add(new ListViewItem(String.Format("{0}", SPSController.GetYVal(i)), 0));

                //Soll
                xtar_list.Items.Add(new ListViewItem(String.Format("{0}", SPSController.GetXVal(i)), 0));
                ytar_list.Items.Add(new ListViewItem(String.Format("{0}", SPSController.GetYVal(i)), 0));
            }
        }

        private void ClearLists()
        {
            xis_list.Items.Clear();
            yis_list.Items.Clear();
            xtar_list.Items.Clear();
            ytar_list.Items.Clear();
        }
        private void UpdateEditedListView(ListView listx, ListView listy)
        {
            ListViewItem[] itemBufferX = new ListViewItem[listlength]; //Arrays zum Speichern der Listenelemente
            ListViewItem[] itemBufferY = new ListViewItem[listlength]; //

            listx.Items.CopyTo(itemBufferX, 0); //Kopiert Inhalt der Liste in Array
            listy.Items.CopyTo(itemBufferY, 0); //


            for (int i = 0; i < listlength; i++) //Schreibt die Tabellenwerte in den Short-Buffer(w_XKoor, w_YKoor) des SPSController Objekts
            {
                SPSController.SetX(short.Parse(itemBufferX[i].Text), i);
                SPSController.SetY(short.Parse(itemBufferY[i].Text), i);
            }

            SPSController.DebugWrite();
            InitListView();

            //SPSController.writeDBentry(0);
            //SPSController.readDBEntry(0);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            UpdateEditedListView(xtar_list, ytar_list);
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int test = (int)numericUpDown1.Value;
            SPSController.DebugRead();
            InitListView();
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }
    }
}
