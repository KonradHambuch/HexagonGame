using Prism.Mvvm;
using System;
using System.Collections.Generic;

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
        public List<Field> Fields { get; set; } = new List<Field>();
        public int StartCoordX { get; set; }
        public int StartCoordY { get; set; }
        public MyColor Color { get; set; }

        public Player(string Name, int StartCoordX, int StartCoordY, MyColor Color)
        {
            this.Name = Name;
            this.StartCoordX = StartCoordX;
            this.StartCoordY = StartCoordY;
            this.Color = Color;
            Fields = new List<Field>();
        }
        public void TakeTurn()
        {

        }
    }
}
