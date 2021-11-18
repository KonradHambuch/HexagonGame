using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
        public List<Field> FindNeighbours(ObservableCollection<Field> AllFields)
        {
            return AllFields.Where(f => Math.Abs(f.CoordX + f.CoordY - CoordX - CoordY) <= 1 && Math.Abs(f.CoordX - CoordX) <= 1 && Math.Abs(f.CoordY - CoordY) <= 1).ToList();
        }
        public List<Field> FindOneColorAreaFromHere(ObservableCollection<Field> AllFields)
        {
            List<Field> Acc = new List<Field>();
            Acc.Add(this);
            FindOneColorAreaFromHere(AllFields, Colors.OwnColor, Acc);
            return Acc;
        }
        private void FindOneColorAreaFromHere(ObservableCollection<Field> AllFields, MyColor FieldColor, List<Field> Accumulator)
        {
            List<Field> NeighbourFields = FindNeighbours(AllFields);
            var ColorableNeighbours = AllFields.Where(f => NeighbourFields.Contains(f) && f.Colors.OwnColor == FieldColor && !Accumulator.Contains(f)).ToList();
            if (ColorableNeighbours.Count == 0) return;
            ColorableNeighbours.ForEach(f =>
            {
                if (!Accumulator.Contains(f)) Accumulator.Add(f);
                f.FindOneColorAreaFromHere(AllFields, FieldColor, Accumulator);
            });
        }
    }
}
