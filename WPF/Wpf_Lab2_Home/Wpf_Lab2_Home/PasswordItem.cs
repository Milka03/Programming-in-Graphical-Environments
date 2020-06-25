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
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Wpf_Lab2_Home
{
    public class PasswordItem : DependencyObject, INotifyPropertyChanged
    {
        private BitmapImage icon;
        private string nameBox = String.Empty;
        private string email = String.Empty;
        private string login = String.Empty;
        private string password = String.Empty;
        private string website = String.Empty;
        private string notes = String.Empty;
        private DateTime creationDate;
        private DateTime editDate;
        private string firstLetter;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public PasswordItem(DateTime date)
        {
            nameBox = "Account Name";
            firstLetter = "A";
            creationDate = date;
            editDate = date;
        }

        public void UpdateValues(string fieldName, string val)
        {
            switch (fieldName)
            {
                case "NameBox": NameBox = val; break;
                case "EmailBox": Email = val; break;
                case "LoginBox": Login = val; break;
                case "PassBox": Password = val; break;
                case "WebsiteBox": Website = val; break;
                case "NotesBox": Notes = val; break;
                default: break;
            }
            NotifyPropertyChanged();
        }

        public BitmapImage Icon
        {
            get { return this.icon; }
            set
            {
                if (value != this.icon)
                {
                    this.icon = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string NameBox
        {
            get { return this.nameBox; }
            set
            {
                if (value != this.nameBox)
                {
                    this.nameBox = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Email
        {
            get { return this.email; }
            set
            {
                if (value != this.email)
                {
                    this.email = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Login 
        {
            get { return this.login; }
            set
            {
                if (value != this.login)
                {
                    this.login = value;
                    NotifyPropertyChanged();
                }
            } 
        }
        public string Password 
        {
            get { return this.password; }
            set
            {
                if (value != this.password)
                {
                    this.password = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Website 
        {
            get { return this.website; }
            set
            {
                if (value != this.website)
                {
                    this.website = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Notes 
        {
            get { return this.notes; }
            set
            {
                if (value != this.notes)
                {
                    this.notes = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime CreationDate
        {
            get { return this.creationDate; }
           
        }

        public DateTime EditDate
        {
            get { return this.editDate; }
            set
            {
                if (value != this.editDate)
                {
                    this.editDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string FirstLetter
        {
            get { return this.firstLetter; }
            set
            {
                if (value != this.firstLetter)
                {
                    this.firstLetter = value;
                    NotifyPropertyChanged();
                }
            }
        }

    }


    
}
