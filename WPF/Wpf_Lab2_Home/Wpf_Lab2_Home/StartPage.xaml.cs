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
using System.IO;
using WPF_Project;

namespace Wpf_Lab2_Home
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        MainWindow main;
        public StartPage()
        {
            InitializeComponent();
            main = (MainWindow)App.Current.MainWindow;
        }

        private void UnlockButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("../../Passwords.bin"))
            {
                byte[] data = DataEncryption.Decrypt(PasswordInput.Password, File.ReadAllBytes("../../Passwords.bin"));
                MainPage1 p1 = new MainPage1();
                main.Framestart.NavigationService.Navigate(p1);
            }
            else
            {
                MainPage1 p = new MainPage1();
                main.Framestart.NavigationService.Navigate(p);
            }
            //MainPage1 p1 = new MainPage1();
            //main.Framestart.NavigationService.Navigate(p1);
        }
    }
}
