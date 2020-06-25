using System;
using System.Collections.Generic;
using System.ComponentModel;
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
//using Wpf_Lab2.Pages;
//using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;

namespace Wpf_Lab2_Home
{
    /// <summary>
    /// Interaction logic for MainPage1.xaml
    /// </summary>
    public partial class MainPage1 : Page
    {
        MainWindow main;
        GlobalData globalData;
        List<ImagePage> imagePages;
        List<DirectoryPage> directoryPages;
        List<PasswordPage> passwordPages;
        //public string MyImageSource { get; set; }

        public TreeViewItem LastTitem { get; set; }
        public bool isInEditMode { get; set; }
        public MainPage1()
        {
            InitializeComponent();
            main = (MainWindow)App.Current.MainWindow;
            globalData = new GlobalData();
            imagePages = new List<ImagePage>();
            directoryPages = new List<DirectoryPage>();
            passwordPages = new List<PasswordPage>();

            this.DataContext = main;
            treevMenu.Items.Clear();
            isInEditMode = false;
        }
        
        
        private void Trv_SelectedItemChanged(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem it = (TreeViewItem)(sender);
            string s = it.Header.ToString();
            globalData.Current = (myData)it.Tag;
            it.Focus();
            LastTitem = it;

            if (it.Tag is myImage)
            {
                (it.Tag as myImage).myPAGE.ImageGrid.Visibility = Visibility.Visible;
                RightPanel.NavigationService.Navigate((it.Tag as myImage).myPAGE);
            }
            if (it.Tag is myDirectory)
            {
                (it.Tag as myDirectory).myPAGE.DirTextBlock.Text = s + "(" + it.Items.Count + ")";
                RightPanel.NavigationService.Navigate((it.Tag as myDirectory).myPAGE);

            }
            if (it.Tag is myPassword)
            {
                RightPanel.NavigationService.Navigate((it.Tag as myPassword).myPAGE);
            }

        }
       
       
        private void MenuLogout_Click(object sender, RoutedEventArgs e)
        {
            //main.LockPage
            directoryPages.Clear();
            passwordPages.Clear();
            imagePages.Clear();
            main.Framestart.NavigationService.Navigate(new StartPage());
        }



        //--------------------- Adding TreeViewItems ------------------------------
        private void NewDirectory_Click(object sender, RoutedEventArgs e)
        {
            myDirectory dir = new myDirectory();
            TreeViewItem item = new TreeViewItem();
            item.Header = dir.Name;
            
            item.FontWeight = FontWeights.Bold;
            item.ContextMenu = this.Resources["DirectoryMenu"] as ContextMenu;

            MenuItem mi = new MenuItem();
            mi.Header = dir.Name;
            mi.IsEnabled = false;
            item.ContextMenu.Items[0] = mi;
            TreeViewItem selected = (TreeViewItem)treevMenu.SelectedItem;
            if (selected != null) selected.Items.Add(item);
            else treevMenu.Items.Add(item);

            DirectoryPage pd = new DirectoryPage(dir.Counter);
            dir.myPAGE = pd;
            globalData.Current = dir;
            pd.DataContext = globalData;
            directoryPages.Add(pd);
            item.Tag = dir;

            LastTitem = item;
        }

        private void NewPasswords_Click(object sender, RoutedEventArgs e)
        {
            myPassword pas = new myPassword();
            TreeViewItem item = new TreeViewItem();
            item.Header = pas.Name;
            item.Tag = pas;
            item.FontWeight = FontWeights.Normal;
            item.FontStyle = FontStyles.Italic;

            item.ContextMenu = this.Resources["ImagePasswordMenu"] as ContextMenu;
            MenuItem mi = new MenuItem();
            mi.Header = pas.Name;
            mi.IsEnabled = false;
            item.ContextMenu.Items[0] = mi;

            TreeViewItem selected = (TreeViewItem)treevMenu.SelectedItem;
            if (selected != null)
            {
                string s = selected.Header.ToString();
                if (selected.Tag is myDirectory) selected.Items.Add(item);
            }
            else treevMenu.Items.Add(item);

            PasswordPage pp = new PasswordPage(pas.Counter);
            pas.myPAGE = pp;
            globalData.Current = pas;
            pp.DataContext = globalData;
            passwordPages.Add(pp);
            //RightPanel.NavigationService.Navigate(p2);
            LastTitem = item;
        }

        private void NewImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select image";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                myImage img = new myImage();
                TreeViewItem item = new TreeViewItem();
                item.Header = img.Name;
                item.Tag = img;
                item.FontWeight = FontWeights.Normal;
                item.FontStyle = FontStyles.Italic;
                
                item.ContextMenu = this.Resources["ImagePasswordMenu"] as ContextMenu;
                MenuItem mi = new MenuItem();
                mi.Header = img.Name;
                mi.IsEnabled = false;
                item.ContextMenu.Items[0] = mi;

                TreeViewItem selected = (TreeViewItem)treevMenu.SelectedItem;
                if (selected != null)
                {
                    string s = selected.Header.ToString();
                    if (selected.Tag is myDirectory) selected.Items.Add(item);
                }
                else treevMenu.Items.Add(item);

                ImagePage p = new ImagePage(img.Counter);
                p.myImage.Source = new BitmapImage(new Uri(op.FileName));
                p.ImageGrid.Visibility = Visibility.Collapsed;
                img.myPAGE = p;
                globalData.Current = img;
                p.DataContext = globalData;
                imagePages.Add(p);
                //System.Windows.MessageBox.Show("Added to list " + imagePages.Count );
                LastTitem = item;
            }
        }
   

        //--------------------- Rename TreeViewItem --------------------- not finished
        //TextBox tempTextBox;
        //TreeViewItem titem;
        private void RenameDirectory_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem titem = (TreeViewItem)treevMenu.SelectedItem;
            if (titem != null)
            {
                isInEditMode = true;
                // the current node has focus
                titem.Focus();
                // no longer handle the operating system
                e.Handled = true;
            }            
        }
       
        private void renametextbox_LostFous(object sender, RoutedEventArgs e)
        {
            //tempTextBox.Visibility = Visibility.Collapsed;
        }

        private void ConfirmChangesByEnter(object sender, KeyEventArgs e)
        {

        }

        //---------------------- Delete TreeViewItem --------------------------
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)treevMenu.SelectedItem;
            if (item != null)
            {
                TreeViewItem parent = GetParentObject(item);
                if (parent == null) treevMenu.Items.Remove(item);
                else parent.Items.Remove(item);
            }
        }

       
        public TreeViewItem GetParentObject(TreeViewItem obj)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is TreeViewItem)
                {
                    return (TreeViewItem)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right && !(e.OriginalSource is TextBlock))
            {
                TreeViewItem item = (TreeViewItem)treevMenu.SelectedItem;
                if (item != null)
                {
                    treevMenu.Focus();
                    item.IsSelected = false;
                }
            }
        }

        
    }
}
