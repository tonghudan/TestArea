using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SoftPainter.History.Util
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            //Boolean verValue = (Boolean)values;

            //if (!verValue)
            //{
            //    return Brushes.LightGray;
            //}
            //else
            //{
            //    return Brushes.Green;
            //}

            Boolean verValue = (Boolean)values;

            if (!verValue)
            {
                return "未喷涂";
            }
            else
            {
                return "完成";
            }
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
