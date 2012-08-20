using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace WPFIntf
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value,Type targetType,
                              object parameter,CultureInfo culture)
        {
             if(targetType == typeof(string) && parameter is string)
             {
                return String.Format(parameter as string,value);
             }
             return value;
        }
        
        public object ConvertBack(object value,Type targetType,
                                  object parameter,CultureInfo culture)
        {
            return value;
        }
    }
}
