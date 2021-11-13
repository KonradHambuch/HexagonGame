using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace HexagonGame.Converters
{
    class IsActiveBoolToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if((bool)value)
            {
                return new SolidColorBrush(Colors.Aqua);
            }
            else
            {
                return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
