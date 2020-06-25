using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.IO;

namespace Wpf_Lab2_Home
{
    /// <summary>
    /// Interaction logic for ImagePage.xaml
    /// </summary>
    public partial class ImagePage : Page
    {
        public int ID { get; set; }
        public ImagePage(int name)
        {
            InitializeComponent();
            ID = name;
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog op = new SaveFileDialog();
            op.Title = "Select image";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                var encoder = new JpegBitmapEncoder(); 
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(myImage.Source.ToString(), UriKind.RelativeOrAbsolute);
                bi.EndInit();
                encoder.Frames.Add(BitmapFrame.Create(bi));
                using (var stream = op.OpenFile())
                {
                    encoder.Save(stream);
                }
            }
        }
    }
}
