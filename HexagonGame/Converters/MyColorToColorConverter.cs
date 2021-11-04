using HexagonGame.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace HexagonGame.Converters
{
    class MyColorToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            MyColor myColor = (MyColor)value;
            return Color.FromArgb((byte)myColor.A, (byte)myColor.R, (byte)myColor.G, (byte)myColor.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Color color = (Color)value;
            return new MyColor(color.A, color.R, color.G, color.B);
        }
    }
}
