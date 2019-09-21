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

        private void switchViewMode(int v) {
            detailsToolStripMenuItem.Checked = false;
            smallToolStripMenuItem.Checked = false;
            largeToolStripMenuItem.Checked = false;

            if (v == 0) {
                largeToolStripMenuItem.Checked = true;
            } else if (v == 1) {
                smallToolStripMenuItem.Checked = true;
            } else {
                detailsToolStripMenuItem.Checked = true;
            }
        }

        private void largeToolStripMenuItem_Click(object sender, EventArgs e) {
            switchViewMode(0);
        }

        private void smallToolStripMenuItem_Click(object sender, EventArgs e) {
            switchViewMode(1);
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e) {
            switchViewMode(2);
        }
    }
}
