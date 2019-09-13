using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoEditor {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutDialog d = new AboutDialog();
            d.ShowDialog();
        }
    }
}
