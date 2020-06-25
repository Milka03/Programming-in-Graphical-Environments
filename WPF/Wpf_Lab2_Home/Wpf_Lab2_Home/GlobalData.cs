using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
//using System.Windows.Forms;
using System.Globalization;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Wpf_Lab2_Home
{

    public class myData
    {
        public string Name { get; set; }
        public bool bold = true;
        public bool isDirectory = false;
        public bool rightClick=false;

    }

    public class myDirectory : myData
    {
        public static int counter = 0;
        //public List<MyTreeItem> subitems = new List<MyTreeItem>();
        public DirectoryPage myPAGE { get; set; }

        public myDirectory()
        {
            counter++;
            isDirectory = true;
            Name = "New Directory";
        }
        public int Counter
        {
            get { return counter; }
            set { counter = value; }
        }
    }

    public class myImage : myData
    {
        public static int counter = 0;
        public ImagePage myPAGE { get; set; }

        public myImage()
        {
            counter++;
            isDirectory = false;
            bold = false;
            Name = "New Image";
        }

        public int Counter
        {
            get { return counter; }
            set { counter = value; }
        }
    }

    public class myPassword : myData
    {
        public static int counter = 0;
        public List<PasswordItem> listOfPasswords { get; set; }
        public PasswordPage myPAGE { get; set; }
     
        //public ContentControl CurrentView { get; set; }
        public myPassword()
        {
            counter++;
            isDirectory = false;
            bold = false;
            Name = "New Passwords";
            listOfPasswords = new List<PasswordItem>();
        }

        public int Counter
        {
            get { return counter; }
            set { counter = value; }
        }
    }



    public class GlobalData : INotifyPropertyChanged
    {

        private myData current;
        public event PropertyChangedEventHandler PropertyChanged;

        private Visibility isVisibleForm = Visibility.Hidden;
        private Visibility isVisibleViewer = Visibility.Hidden;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public myData Current
        {
            get
            {
                return this.current;
            }
            set
            {
                if(value!= this.current)
                {
                    this.current = value;
                    NotifyPropertyChanged();
                }

            }
        }

        public Visibility IsVisibleForm
        {
            get
            {
                return this.isVisibleForm;
            }
            set
            {
                if (value != this.isVisibleForm)
                {
                    this.isVisibleForm = value;
                    NotifyPropertyChanged();
                }

            }
        }

        public Visibility IsVisibleViewer
        {
            get
            {
                return this.isVisibleViewer;
            }
            set
            {
                if (value != this.isVisibleViewer)
                {
                    this.isVisibleViewer = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public void Update()
        {
            NotifyPropertyChanged();
        }

    }
}
