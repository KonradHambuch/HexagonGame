using HexagonGame.Core.Models;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace HexagonGame.Converters
{
    class ColorsToRadialGradientBrushConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            FieldColors colors = (FieldColors) value;
            RadialGradientBrush radialGradient = new RadialGradientBrush();

            radialGradient.GradientOrigin = new Point(0.5, 0.5);

            radialGradient.Center = new Point(0.5, 0.5);

            radialGradient.RadiusX = 0.5;
            radialGradient.RadiusY = 0.5;
            radialGradient.GradientStops.Add(new GradientStop() { Color = Color.FromArgb((byte)colors.MarkColor.A, (byte)colors.MarkColor.R, (byte)colors.MarkColor.G, (byte)colors.MarkColor.B), Offset = 0.0 });
            radialGradient.GradientStops.Add(new GradientStop() { Color = Colors.Black, Offset = 0.5 });
            radialGradient.GradientStops.Add(new GradientStop() { Color = Color.FromArgb((byte)colors.OwnColor.A, (byte)colors.OwnColor.R, (byte)colors.OwnColor.G, (byte)colors.OwnColor.B), Offset = 1 });

         
            return radialGradient;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
