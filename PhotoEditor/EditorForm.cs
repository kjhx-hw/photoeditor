using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace PhotoEditor
{
    public partial class EditorForm : Form
    {
        

        private CancellationTokenSource cancellationTokenSource;

        private Bitmap transformedBitmap { get; set; }
        public EditorForm(string photoLocation)
        {
            InitializeComponent();
            transformedBitmap = new Bitmap(@photoLocation);
            photoBox.Image = transformedBitmap;
        }

        async private Task Slider()
        {
            ProgressScreen loadingScreen = new ProgressScreen(0, transformedBitmap.Height);
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            await Task.Run(() =>
            {
                
                Enabled = false;
                int amount=0;
                Invoke((Action) delegate () { amount = Convert.ToInt32(2 * (50 - trackBar1.Value) * 0.01 * 255); });
                for (int y = 0; y < transformedBitmap.Height && !token.IsCancellationRequested; y++)
                {
                    for (int x = 0; x < transformedBitmap.Width; x++)
                    {
                        Color color = transformedBitmap.GetPixel(x, y);
                        int newRed = color.R - amount;
                        int newGreen = color.G - amount;
                        int newBlue = color.B - amount;

                        if (token.IsCancellationRequested)
                            break;
                        

                        if (newRed < 0)
                            newRed = 0;
                        else if (newRed > 255)
                            newRed = 255;

                        if (newGreen < 0)
                            newGreen = 0;
                        else if (newGreen > 255)
                            newGreen = 255;

                        if (newBlue < 0)
                            newBlue = 0;
                        else if (newBlue > 255)
                            newBlue = 255;

                        
                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        transformedBitmap.SetPixel(x, y, newColor);
                    }

                    Invoke((Action)delegate () { loadingScreen.UpdateProgressBar(1); });
                }
                Invoke((Action)delegate () { loadingScreen.Close(); });

            });
            Enabled = true;
            Activate();
        }

        async private Task InvertPhoto()
        {
            ProgressScreen loadingScreen = new ProgressScreen(0, transformedBitmap.Height);
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            await Task.Run(() => 
            {
                Enabled = false;   
                for (int y = 0; y < transformedBitmap.Height && !token.IsCancellationRequested; y++)
                {
                    for (int x = 0; x < transformedBitmap.Width; x++)
                    {
                        Color color = transformedBitmap.GetPixel(x, y);
                        int newRed = Math.Abs(color.R - 255);
                        int newGreen = Math.Abs(color.G - 255);
                        int newBlue = Math.Abs(color.B - 255);

                        if (token.IsCancellationRequested)
                            break;

                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        transformedBitmap.SetPixel(x, y, newColor);
                    }
                    Invoke((Action)delegate () { loadingScreen.UpdateProgressBar(1); });
                }
                Invoke((Action)delegate () { loadingScreen.Close(); });
            });
            Enabled = true;
            Activate();
        }

        async private Task ColorChanger(float red, float green, float blue)
        {
            ProgressScreen loadingScreen = new ProgressScreen(0, transformedBitmap.Height);
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            await Task.Run(() =>
            {
                  
                
                for (int y = 0; y < transformedBitmap.Height && !token.IsCancellationRequested && !loadingScreen.CurrentState(); y++)
                {
                    for (int x = 0; x < transformedBitmap.Width; x++)
                    {
                        Color color = transformedBitmap.GetPixel(x, y);
                        int newRed = Math.Abs(color.R - 255);
                        int newGreen = Math.Abs(color.G - 255);
                        int newBlue = Math.Abs(color.B - 255);
                        float total = (newRed + newGreen + newBlue) / 3;
                        total /= 255;
                        if (token.IsCancellationRequested)
                            break;
                        if (loadingScreen.CurrentState())
                            break;
                        Color newColor = Color.FromArgb((int)(red * total), (int)(green * total), (int)(total * blue));
                        transformedBitmap.SetPixel(x, y, newColor);
                    }
                    Invoke((Action)delegate () { loadingScreen.UpdateProgressBar(1); });
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(25);
                        for(int j = 0; j < 100; j++)
                        {
                            Application.DoEvents();
                        }
                    }
                }
                Invoke((Action)delegate () { loadingScreen.Close(); });

            });
        }


        async private void ColorPicker(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                
                Enabled = false;
                await ColorChanger(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
                photoBox.Image = transformedBitmap;
                Enabled = true;
                Activate();
                
            }
        }


        async private void InvertButton(object sender, EventArgs e)
        {
          
            await InvertPhoto();
           
            photoBox.Image = transformedBitmap;
        }

        async private void TrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            
            await Slider();
            photoBox.Image = transformedBitmap;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            photoBox.Image.Save("myohoto.jpg", ImageFormat.Jpeg);
        }
    }
}
