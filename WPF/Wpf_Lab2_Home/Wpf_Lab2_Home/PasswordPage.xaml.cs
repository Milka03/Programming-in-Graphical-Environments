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
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.ComponentModel;

namespace Wpf_Lab2_Home
{
    /// <summary>
    /// Interaction logic for PasswordPage.xaml
    /// </summary>
    public partial class PasswordPage : Page
    {
        public List<PasswordItem> xp { get; set; }

        public PasswordItem LastItem;
        public CollectionView collectionView;
        public PropertyGroupDescription groupDescription;
        public int ID { get; set; }
        public PasswordPage(int name)
        {
            InitializeComponent();
            ID = name;
            xp = new List<PasswordItem>();
            PasswordList.ItemsSource = xp.ToArray();

            collectionView = (CollectionView)CollectionViewSource.GetDefaultView(PasswordList.ItemsSource);
            groupDescription = new PropertyGroupDescription("FirstLetter");
            collectionView.GroupDescriptions.Add(groupDescription);
            //collectionView.Filter += UserFilter;
            PasswordList.Items.SortDescriptions.Add(new SortDescription("NameBox", ListSortDirection.Ascending));
        }

        private void AddPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordItem viewItem = new PasswordItem(System.DateTime.Now);
            viewItem.NameBox = "Account Name";
            //viewItem.Height = 80;
            //viewItem.Content = viewItem.NameBox;

            PasswordList.SelectedItem = viewItem;
            xp.Add(viewItem);
            PasswordList.ItemsSource = xp.ToArray();
            PasswordList.Items.Refresh();
            //((this.DataContext as GlobalData).Current as myPassword).listOfPasswords.Add(viewItem);
            (this.DataContext as GlobalData).IsVisibleForm = Visibility.Visible;
            (this.DataContext as GlobalData).IsVisibleViewer = Visibility.Hidden;
            this.FormEditor.Visibility = Visibility.Visible;
            this.FormViewer.Visibility = Visibility.Hidden;
            (this.DataContext as GlobalData).NotifyPropertyChanged("isVisibleForm");

            if (SelectbtnImage != null) SelectbtnImage.Source = null;
            this.AddButton.IsEnabled = false;
            this.ListSearchBox.IsEnabled = false;
            LastItem = viewItem;
            PasswordList.Focusable = false;
            PasswordList.Items.SortDescriptions.Add(new SortDescription("NameBox", ListSortDirection.Ascending));
            collectionView = (CollectionView)CollectionViewSource.GetDefaultView(PasswordList.ItemsSource);
            //groupDescription = new PropertyGroupDescription("NameBox");
            collectionView.GroupDescriptions.Add(groupDescription);
        }

        private void SelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            PasswordItem pass = PasswordList.SelectedItem as PasswordItem;
            if (pass == null) pass = LastItem;
            //PasswordList.Items.Add(pass);

            if (pass != null)
            {
                LastItem = pass;
                this.FormViewer.Visibility = Visibility.Visible;
                this.FormEditor.Visibility = Visibility.Hidden;
                (this.DataContext as GlobalData).NotifyPropertyChanged("Current");
            }
        }

        private void MouseOnFormHandler(object sender, MouseButtonEventArgs e)
        {
            if (PasswordList.SelectedItem != null) PasswordList.Focus();
        }

        //----------------------- Search Box and searching ---------------------------
        private void SearchBoxClick(object sender, RoutedEventArgs e)
        {
            TextBox b = (TextBox)sender;
            b.Text = "";
            b.Foreground = Brushes.Black;
        }

        private void SearchBoxLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox b = (TextBox)sender;
            b.Text = "Search...";
            b.Foreground = Brushes.DarkGray;
            if (PasswordList != null) PasswordList.Focus();
        }

        private void SearchTextChaged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if(this.FormViewer != null)
                this.FormViewer.Visibility = Visibility.Hidden;
            if(PasswordList != null)
                CollectionViewSource.GetDefaultView(PasswordList.ItemsSource).Refresh();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(this.ListSearchBox.Text) || this.ListSearchBox.Text == "Search...")
                return true;
            if (this.FormEditor.Visibility == Visibility.Visible) return true;
            else
            {
                bool result = ((item as PasswordItem).NameBox.StartsWith(ListSearchBox.Text, StringComparison.OrdinalIgnoreCase));
                if (result == false) this.FormViewer.Visibility = Visibility.Hidden;
                return result;
            }
               
        }

        //----------------------- BUTTON CLICKS ----------------------
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordItem pass = PasswordList.SelectedItem as PasswordItem;
            if (pass == null) pass = LastItem;

            if (pass != null)
            {
                //pass.UpdateValues(box.Name, box.Text);
                pass.NameBox = NameBox.Text;
                pass.Email = EmailBox.Text;
                pass.Login = LoginBox.Text;
                pass.Password = PassBox.Text;
                pass.Website = WebsiteBox.Text;
                pass.Notes = NotesBox.Text;
                pass.EditDate = DateTime.Now;
                pass.FirstLetter = NameBox.Text[0].ToString();
                pass.Icon = (BitmapImage)SelectbtnImage.Source;
                PasswordList.Items.Refresh();

                (this.DataContext as GlobalData).IsVisibleViewer = Visibility.Visible;
                (this.DataContext as GlobalData).IsVisibleForm = Visibility.Hidden;
                (this.DataContext as GlobalData).NotifyPropertyChanged("Current");

                this.FormEditor.Visibility = Visibility.Hidden;
                this.FormViewer.Visibility = Visibility.Visible;
                this.AddButton.IsEnabled = true;
                this.ListSearchBox.IsEnabled = true;
                LastItem = pass;
                PasswordList.Focusable = true;
                PasswordList.Items.SortDescriptions.Add(new SortDescription("NameBox", ListSortDirection.Ascending));
                collectionView = (CollectionView)CollectionViewSource.GetDefaultView(PasswordList.ItemsSource);
                collectionView.Filter += UserFilter;
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            (this.DataContext as GlobalData).IsVisibleViewer = Visibility.Visible;
            (this.DataContext as GlobalData).IsVisibleForm = Visibility.Hidden;
            (this.DataContext as GlobalData).NotifyPropertyChanged();
            this.FormEditor.Visibility = Visibility.Hidden;
            this.FormViewer.Visibility = Visibility.Visible;
            PasswordList.SelectedItem = LastItem;
            this.AddButton.IsEnabled = true;
            this.ListSearchBox.IsEnabled = true;
            PasswordList.Focusable = true;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as GlobalData).IsVisibleViewer = Visibility.Hidden;
            (this.DataContext as GlobalData).IsVisibleForm = Visibility.Visible;
            (this.DataContext as GlobalData).NotifyPropertyChanged();

            this.FormEditor.Visibility = Visibility.Visible;
            this.FormViewer.Visibility = Visibility.Hidden;
            PasswordItem pas = (PasswordItem)PasswordList.SelectedItem;
            if (pas == null) pas = LastItem;
            if (pas.Icon != null) SelectbtnImage.Source = pas.Icon;
            if (pas.Icon == null) SelectbtnImage.Source = null;
            this.AddButton.IsEnabled = false;
            this.ListSearchBox.IsEnabled = false;
            PasswordList.Focusable = false;
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            PasswordItem pas = PasswordList.SelectedItem as PasswordItem;
            if (pas == null) pas = LastItem;
            ((this.DataContext as GlobalData).Current as myPassword).listOfPasswords.Remove(pas);
            (this.DataContext as GlobalData).NotifyPropertyChanged("Current");

            this.xp.Remove(pas);
            PasswordList.ItemsSource = xp.ToArray();
            PasswordList.Items.Refresh();
            this.FormEditor.Visibility = Visibility.Hidden;
            this.FormViewer.Visibility = Visibility.Hidden;
            this.AddButton.IsEnabled = true;
            this.ListSearchBox.IsEnabled = true;
            PasswordList.Focusable = true;

            PasswordList.Items.SortDescriptions.Add(new SortDescription("NameBox", ListSortDirection.Ascending));
            collectionView = (CollectionView)CollectionViewSource.GetDefaultView(PasswordList.ItemsSource);
            //groupDescription = new PropertyGroupDescription("NameBox");
            collectionView.GroupDescriptions.Add(groupDescription);
            collectionView.Filter += UserFilter;
        }

        //---------------------------------------------------------------------------------
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select image";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                SelectbtnImage.Source = new BitmapImage(new Uri(op.FileName));
                //PasswordItem pass = PasswordList.SelectedItem as PasswordItem; ->Previoulsy for lab part
                //if (pass == null) PasswordList.SelectedItem = pass;
                //pass.Icon = new BitmapImage(new Uri(op.FileName));

                (this.DataContext as GlobalData).NotifyPropertyChanged("Current");
            }
        }

        private void FormTextChangedHandler(object sender, TextChangedEventArgs e)
        {

        }

        //--------------------- Viewer functionalities -------------------

        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            PasswordItem pas = PasswordList.SelectedItem as PasswordItem;
            if (pas == null) pas = LastItem;
            string tmp = "";
            if (b.Name == "Copybtn1") tmp = pas.Email;
            if (b.Name == "Copybtn2") tmp = pas.Login;
            if (b.Name == "Copybtn3") tmp = pas.Password;
            Clipboard.SetText(tmp);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            string s = LastItem.Website;
            try
            {
                System.Diagnostics.Process.Start(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            e.Handled = true;
        }

        private void EmailHyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            if (hyperlink == null) return;
            if (Regex.IsMatch(hyperlink.NavigateUri.ToString(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                string address = string.Concat("mailto:", hyperlink.NavigateUri.ToString());
                try { System.Diagnostics.Process.Start(address); }
                catch { MessageBox.Show("That e-mail address is invalid.", "E-mail error"); }
            }

            //try
            //{
            //    if (Regex.IsMatch(hyperlink.NavigateUri.ToString(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")) {}
            //    else { MessageBox.Show("Invalid e-mail format"); }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

    }
}
