using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HexagonGame.Core.Models
{
    public class Player : BindableBase
    {
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public List<Field> OwnFields { get; set; } = new List<Field>();
        public int StartCoordX { get; set; }
        public int StartCoordY { get; set; }
        public MyColor Color { get; set; }
        Dictionary<MyColor, HashSet<Field>> OneColorNeighbourFields = new Dictionary<MyColor, HashSet<Field>>();
        public Player(string Name, int StartCoordX, int StartCoordY, MyColor Color)
        {
            this.Name = Name;
            this.StartCoordX = StartCoordX;
            this.StartCoordY = StartCoordY;
            this.Color = Color;
            OwnFields = new List<Field>();
        }
        List<Field> FindNeighboursOfField(Field field, ObservableCollection<Field> AllFields)
        {
            return AllFields.Where(f => Math.Abs(f.CoordX + f.CoordY - field.CoordX - field.CoordY) <= 1 && Math.Abs(f.CoordX - field.CoordX) <= 1 && Math.Abs(f.CoordY - field.CoordY) <= 1).ToList();
        }
        public List<Field> FindOneColorArea(int StartCoordX, int StartCoordY, ObservableCollection<Field> AllFields)
        {
            List<Field> Acc = new List<Field>();
            Field StartField = AllFields.First(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY);
            FindOneColorArea(StartCoordX, StartCoordY, AllFields, StartField.Colors.OwnColor, Acc);
            return Acc;
        }
        private void FindOneColorArea(int StartCoordX, int StartCoordY, ObservableCollection<Field> AllFields, MyColor FieldColor, List<Field> Accumulator)
        {
            Field StartField = AllFields.First(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY);
            Accumulator.Add(StartField);
            List<Field> NeighbourFields = FindNeighboursOfField(StartField, AllFields);
            var ColorableNeighbours = AllFields.Where(f => NeighbourFields.Contains(f) && f.Colors.OwnColor == FieldColor && !Accumulator.Contains(f)).ToList();
            if (ColorableNeighbours.Count == 0) return;
            ColorableNeighbours.ForEach(f =>
            {
                Accumulator.Add(f);
                FindOneColorArea(f.CoordX, f.CoordY, AllFields, FieldColor, Accumulator);
            });
        }
        public List<Field> FindOwnFields(ObservableCollection<Field> AllFields)
        {
            OwnFields.Clear();
            Field StartField = AllFields.First(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY);
            FindOneColorArea(StartCoordX, StartCoordY, AllFields, StartField.Colors.OwnColor, OwnFields);
            return OwnFields;
        }
        public Dictionary<MyColor, HashSet<Field>> CountNegihbourColors(ObservableCollection<Field> AllFields)
        {
            OneColorNeighbourFields = new Dictionary<MyColor, HashSet<Field>>();
            var PossibleColors = new HashSet<MyColor>();
            HashSet<Field> neighbours = new HashSet<Field>();
            FindOwnFields(AllFields);
            OwnFields.ForEach(f =>
            {
                var fieldNeighbours = FindNeighboursOfField(f, AllFields);
                fieldNeighbours.RemoveAll(field => AllFields.Contains(field));
                fieldNeighbours.ForEach(fn => neighbours.Add(fn));
            });
            foreach (var neighbour in neighbours)
            {
                PossibleColors.Add(neighbour.Colors.OwnColor);
            }
            foreach (var neighbour in neighbours)
            {
                if (OneColorNeighbourFields.ContainsKey(neighbour.Colors.OwnColor))
                {
                    FindOneColorArea(neighbour.CoordX, neighbour.CoordY, AllFields).ForEach(neigh => OneColorNeighbourFields[neighbour.Colors.OwnColor].Add(neigh));
                }
                else
                {
                    OneColorNeighbourFields.Add(neighbour.Colors.OwnColor, new HashSet<Field>(FindOneColorArea(neighbour.CoordX, neighbour.CoordY, AllFields)));
                }
            }
            return OneColorNeighbourFields;
        }
        public void ChangeColor(MyColor NewColor, ObservableCollection<Field> AllFields)
        {
            FindOwnFields(AllFields);
            OwnFields.ForEach(f => f.Colors.OwnColor = NewColor);
            FindOwnFields(AllFields);
            OwnFields.ForEach(f => f.Mark(Color));
        }
    }
}
