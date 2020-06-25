using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;

//---------------- Not used ---------------------
namespace Wpf_Lab2_Home
{
    class TreeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MyImageDataTemplate { get; set; }
        public DataTemplate MyPasswordDataTemplate { get; set; }
        public DataTemplate MyDirectoryDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // Null value can be passed by IDE designer
            if (item == null) return null;

            var s = ((string)item);

            // Select one of the DataTemplate objects, based on the 
            // value of the selected item in the TreeView.
            if (s.Contains("Image"))
            {
                return MyImageDataTemplate;
            }
            if (s.Contains("Directory"))
            {
                return MyDirectoryDataTemplate;
            }
            if (s.Contains("Password"))
            {
                return MyPasswordDataTemplate;
            }
            else return null;
         
        }

    }
}
