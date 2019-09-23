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
using System.IO;

namespace PhotoEditor
{
    public partial class EditorForm : Form
    {

        bool cancel { get; set; } = false;
        private CancellationTokenSource cancellationTokenSource;

        private Bitmap transformedBitmap { get; set; }
        private Bitmap currentPhoto { get; set; }
        string thing;
        public EditorForm(string photoLocation)
        {
            InitializeComponent();

            byte[] bytes = System.IO.File.ReadAllBytes(photoLocation);
            MemoryStream ms = new MemoryStream(bytes);
            Image image = Image.FromStream(ms);


            transformedBitmap = new Bitmap(image);
            photoBox.Image = transformedBitmap;
            currentPhoto = new Bitmap(image);
            thing = photoLocation;
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
                for (int y = 0; y < transformedBitmap.Height && !token.IsCancellationRequested && !loadingScreen.CurrentState(); y++)
                {
                    for (int x = 0; x < transformedBitmap.Width; x++)
                    {
                        Color color = transformedBitmap.GetPixel(x, y);
                        int newRed = color.R - amount;
                        int newGreen = color.G - amount;
                        int newBlue = color.B - amount;

                        if (token.IsCancellationRequested)
                        {
                            this.cancel = true;
                            break;
                        }
                        if (loadingScreen.CurrentState())
                        {
                            this.cancel = true;
                            break;
                        }


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
                if (loadingScreen.CurrentState())
                    this.cancel = true;
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
            Enabled = false;

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

                        if (token.IsCancellationRequested)
                        {
                            this.cancel = true;
                            break;
                        }
                        if (loadingScreen.CurrentState())
                        {
                            this.cancel = true;
                            break;
                        }

                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        transformedBitmap.SetPixel(x, y, newColor);
                    }
                    Invoke((Action)delegate () { loadingScreen.UpdateProgressBar(1); });

                }
                if (loadingScreen.CurrentState())
                    this.cancel = true;
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
                        {
                            this.cancel = true;
                            break;
                        }
                        if (loadingScreen.CurrentState())
                        {
                            this.cancel = true;
                            break;
                        }
                        Color newColor = Color.FromArgb((int)(red * total), (int)(green * total), (int)(total * blue));
                        transformedBitmap.SetPixel(x, y, newColor);
                    }
                    
                }
                if (loadingScreen.CurrentState())
                    this.cancel = true;
                Invoke((Action)delegate () { loadingScreen.Close(); });
            });
        }


        async private void ColorPicker(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                
                Enabled = false;
                cancel = false;
                await ColorChanger(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
                if(cancel != true)
                {
                    photoBox.Image = transformedBitmap;
                    currentPhoto = transformedBitmap;
                }
                else
                    photoBox.Image = currentPhoto;
                Enabled = true;
                Activate();
                
            }
        }


        async private void InvertButton(object sender, EventArgs e)
        {
            cancel = false;
            await InvertPhoto();
            if (cancel != true)
            {
                photoBox.Image = transformedBitmap;
                currentPhoto = transformedBitmap;
            }
            else
                photoBox.Image = currentPhoto;
        }

        async private void TrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            cancel = false;
            await Slider();
            if(cancel != true)
            {
                photoBox.Image = transformedBitmap;
                currentPhoto = transformedBitmap;
            }
            else
                photoBox.Image = currentPhoto;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine(thing);
            currentPhoto.Save(thing, ImageFormat.Jpeg);
            this.Close();
        }

        private void Button5_Click(object sender, EventArgs e)
        {

            //https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/how-to-save-files-using-the-savefiledialog-component
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

           
            if (saveFileDialog1.FileName != "")
            {
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        currentPhoto.Save(saveFileDialog1.FileName,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        currentPhoto.Save(saveFileDialog1.FileName,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        currentPhoto.Save(saveFileDialog1.FileName,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }
            }
            }
        }
}
