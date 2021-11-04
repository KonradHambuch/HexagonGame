using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace HexagonGame.Core.Models
{
    public class Field : BindableBase
    {
        public bool IsMarked { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public PolygonItem PolygonItem { get; set; } = new PolygonItem();
        private FieldColors colors;
        public FieldColors Colors
        {
            get => colors;
            set
            {
                SetProperty(ref colors, value);
            }
        }
        public void Mark(MyColor PlayerColor)
        {
            IsMarked = true;
            Colors.MarkColor = PlayerColor;
        }
        public Field()
        {
            Colors = new FieldColors();
        }
    }
}
