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
    public partial class EditorForm : Form
    {
        public EditorForm(string photoLocation)
        {
            InitializeComponent();
            photoBox.Image = System.Drawing.Image.FromFile(photoLocation);
        }

        async private Task invertPhoto()
        {
            await Task.Run(() => 
            {
           
            });
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        async private void TrackBar1_Scroll(object sender, EventArgs e)
        {

        }
    }
}
