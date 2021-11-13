using HexagonGame.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace HexagonGame.Converters
{
    class PlayerToIsRobotConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is RobotPlayer ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
