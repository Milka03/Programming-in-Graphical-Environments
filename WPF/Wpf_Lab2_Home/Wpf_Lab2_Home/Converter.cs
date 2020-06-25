using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Controls;

namespace Wpf_Lab2_Home
{
    class FormVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value;
            if (!(value is GlobalData)) return null;
            var x = (value as GlobalData).IsVisibleForm;
            return (value as GlobalData).IsVisibleForm;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ViewerVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value;
            if (!(value is GlobalData)) return null;
            var x = (value as GlobalData).IsVisibleForm;

            return (value as GlobalData).IsVisibleViewer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    //------------------- Password converters -----------------------
    public class ProgressForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string)value;
            PasswordStrength progress = PasswordStrengthUtils.CalculatePasswordStrength(s);
            Brush foreground = Brushes.Gray;

            switch (progress)
            {
                case PasswordStrength.VeryStrong:
                    foreground = Brushes.LimeGreen;
                    break;
                case PasswordStrength.Strong:
                    foreground = Brushes.LimeGreen;
                    break;
                case PasswordStrength.Average:
                    foreground = Brushes.Orange;
                    break;
                case PasswordStrength.Weak:
                    foreground = Brushes.OrangeRed;
                    break;
                case PasswordStrength.VeryWeak:
                    foreground = Brushes.Red;
                    break;
                case PasswordStrength.Invalid:
                    foreground = Brushes.Gray;
                    break;
                default:
                    foreground = Brushes.Gray;
                    break;
            }
            return foreground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ProgressValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string)value;
            PasswordStrength progress = PasswordStrengthUtils.CalculatePasswordStrength(s);
            int val = 0;

            switch (progress)
            {
                case PasswordStrength.VeryStrong:
                    val = 5;
                    break;
                case PasswordStrength.Strong:
                    val = 4;
                    break;
                case PasswordStrength.Average:
                    val = 3;
                    break;
                case PasswordStrength.Weak:
                    val = 2;
                    break;
                case PasswordStrength.VeryWeak:
                    val = 1;
                    break;
                case PasswordStrength.Invalid:
                    val = 0;
                    break;
                default:
                    val = 0;
                    break;
            }
            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class PassStrengthNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string)value;
            PasswordStrength progress = PasswordStrengthUtils.CalculatePasswordStrength(s);
            string result = "";

            switch (progress)
            {
                case PasswordStrength.VeryStrong:
                    result = "Very Strong";
                    break;
                case PasswordStrength.Strong:
                    result = "Strong";
                    break;
                case PasswordStrength.Average:
                    result = "Average";
                    break;
                case PasswordStrength.Weak:
                    result = "Weak";
                    break;
                case PasswordStrength.VeryWeak:
                    result = "Very Weak";
                    break;
                case PasswordStrength.Invalid:
                    result = "Invalid";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //--------------------------------------------------------------------

    public class GlobalDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value;
            if (!(value is myPassword)) { return null;  }
            //MessageBox.Show("l: " + (value as myPassword).listOfPasswords.Count);
            return (value as myPassword).listOfPasswords.ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PasswordItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (CollectionViewGroup)value;
            if (!(v is CollectionViewGroup)) { return null; }
            var tmp = (value as CollectionViewGroup).Items;
            var z = tmp[0];
            return z;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
   public class PasswordDisplayConverter : IValueConverter
   {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tmp = "";
            if (!(value is string)) { return null; }
            for (int i = 0; i < (value as string).Length; i++)
            {
                tmp += "\u25CF";
            }
            return tmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
   }


    public class GroupStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (CollectionViewGroup)value;
            if (!(v is CollectionViewGroup)) { return null; }
            var tmp = (value as CollectionViewGroup).Items;
            var z = tmp[0];
            return (z as PasswordItem).FirstLetter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //----------------------- Resolution/DPI Converters-----------------------------------------
    public class ResolutionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tmp = "Resolution: ";
            if (!(value is BitmapImage) || value == null) return null;
            int a = (int)(value as BitmapImage).Width;
            tmp += a;
            tmp += "x";
            int b = (int)(value as BitmapImage).Height;
            tmp += b;
            return tmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DPIConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tmp = "DPI: ";
            if (!(value is BitmapImage) || value == null) return null;
            tmp += (value as BitmapImage).DpiX;
            tmp += "x";
            tmp += (value as BitmapImage).DpiY;
            return tmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tmp = "Format: ";
            if (!(value is BitmapImage) || value == null) return null;
            tmp += (value as BitmapImage).Format;
            return tmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //---------------------------------------------------------------------------

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) return null;
            if (String.IsNullOrEmpty(value as string)) return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    //-------------------- Not used
    public class TreeItemNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is myData)) return null;
            string s = (value as myData).Name;
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
   

