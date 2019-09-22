﻿using System;
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
        private Bitmap transformedBitmap { get; set; }
        public EditorForm(string photoLocation)
        {
            InitializeComponent();
            transformedBitmap = new Bitmap(@photoLocation);
            //photoBox.Image = System.Drawing.Image.FromFile(photoLocation);
            photoBox.Image = transformedBitmap;
        }


        async private Task invertPhoto()
        {
            await Task.Run(() => 
            {
                for (int y = 0; y < transformedBitmap.Height; y++)
                {
                    for (int x = 0; x < transformedBitmap.Width; x++)
                    {
                        Color color = transformedBitmap.GetPixel(x, y);
                        int newRed = Math.Abs(color.R - 255);
                        int newGreen = Math.Abs(color.G - 255);
                        int newBlue = Math.Abs(color.B - 255);
                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        transformedBitmap.SetPixel(x, y, newColor);
                    }
                }
            });
        }

        async private Task colorChanger(float red, float green, float blue)
        {
            await Task.Run(() =>
            {
                for (int y = 0; y < transformedBitmap.Height; y++)
                {
                    for (int x = 0; x < transformedBitmap.Width; x++)
                    {
                        Color color = transformedBitmap.GetPixel(x, y);
                        int newRed = Math.Abs(color.R - 255);
                        int newGreen = Math.Abs(color.G - 255);
                        int newBlue = Math.Abs(color.B - 255);
                        float total = (newRed + newGreen + newBlue) / 3;
                        total /= 255;
                        Color newColor = Color.FromArgb((int)(red * total), (int)(green * total), (int)(total * blue));
                        transformedBitmap.SetPixel(x, y, newColor);
                    }
                }
            });
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        async private void ColorPicker(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                await colorChanger(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
                photoBox.Image = transformedBitmap;
            }
        }

        async private void TrackBar1_Scroll(object sender, EventArgs e)
        {
           
        }

        async private void InvertButton(object sender, EventArgs e)
        {
            await invertPhoto();
            photoBox.Image = transformedBitmap;
        }
    }
}
