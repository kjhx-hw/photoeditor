using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoEditor
{
    public partial class ProgressScreen : Form
    {
        public ProgressScreen(int minimum, int maximum)
        {
            InitializeComponent();
            this.progressBar1.Minimum = minimum;
            this.progressBar1.Maximum = maximum;
        }

        public void UpdateProgressBar(int index)
        {
            if (this.progressBar1.Value == this.progressBar1.Maximum)
                this.progressBar1.Visible = true;
            else
                this.progressBar1.Value += 1;
            this.progressBar1.Visible = true;
            this.Show();
        }

        private void ProgressScreen_Load(object sender, EventArgs e)
        {
           
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
