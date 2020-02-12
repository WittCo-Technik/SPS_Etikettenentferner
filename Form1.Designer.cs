using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sharp7;

namespace SPSCtrl
{
    partial class Form1
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
        private FlowLayoutPanel flowLayoutPanel1;
        private ListView xis_list;
        private ListView yis_list;
        private ListView xtar_list;
        private ListView ytar_list;
        private NumericUpDown numericUpDown1;
        private ColumnHeader yis;
        private ColumnHeader xtar;
        private ColumnHeader ytar;
        private ColumnHeader xis;
    }
}

