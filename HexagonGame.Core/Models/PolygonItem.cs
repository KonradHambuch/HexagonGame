using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class PolygonItem : BindableBase
    {
        public ObservableCollection<MyPoint> Points { get; set; } = new ObservableCollection<MyPoint>();
        private string pointsString;
        public string PointsString
        {
            get => pointsString;
            set => SetProperty(ref pointsString, value);
        }
        public PolygonItem()
        {
            Points.CollectionChanged += new NotifyCollectionChangedEventHandler(SetPointsString);
        }
        public void SetPointsString(object sender, NotifyCollectionChangedEventArgs e)
        {
            string acc = "";
            foreach (var Point in Points)
            {
                acc = string.Join(",", (new string[] { acc, Point.X.ToString(), Point.Y.ToString() }).Where(s => !string.IsNullOrEmpty(s)));
            }
            PointsString = acc;
        }
    }
}
