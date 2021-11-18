using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HexagonGame.Core.Models
{
    public class Player : BindableBase
    {
        public bool IsRobot { get; set; } = false;
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set => SetProperty(ref isActive, value);
        }
        public List<Field> OwnFields { get; set; } = new List<Field>();
        public int StartCoordX { get; set; }
        public int StartCoordY { get; set; }
        public MyColor Color { get; set; }
        
        public Player(string Name, MyColor Color)
        {
            this.Name = Name;
            this.Color = Color;
            OwnFields = new List<Field>();
        }        
        public List<Field> FindOwnFields(ObservableCollection<Field> AllFields)
        {
            OwnFields.Clear();
            Field StartField = AllFields.First(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY);
            OwnFields = StartField.FindOneColorAreaFromHere(AllFields);
            return OwnFields;
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
