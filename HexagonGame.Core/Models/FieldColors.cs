using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class FieldColors: BindableBase
    {
        private MyColor ownColor;
        private MyColor markColor;
        public MyColor OwnColor 
        {
            get => ownColor;
            set
            {
                SetProperty(ref ownColor, value);
            }
        }
        public MyColor MarkColor
        {
            get => markColor;
            set => SetProperty(ref markColor, value);
        }
        public FieldColors()
        {
            OwnColor = new MyColor();
            MarkColor = new MyColor();
        }
    }
}
