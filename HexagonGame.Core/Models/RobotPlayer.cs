using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class RobotPlayer : Player
    {
        public int Level { get; set; }
        public RobotPlayer(string Name, int StartCoordX, int StartCoordY, MyColor Color) : base(Name, StartCoordX, StartCoordY, Color)
        {
            
        }
        public MyColor ChooseColor(ObservableCollection<Field> AllFields)
        {
            int MaxCount = 0;
            MyColor MaxColor = new MyColor();
            Dictionary<MyColor, HashSet<Field>> ColorNeighbours = CountNegihbourColors(AllFields);
            foreach (var entry in ColorNeighbours)
            {
                if (entry.Value.Count > MaxCount)
                {
                    MaxCount = entry.Value.Count;
                    MaxColor = entry.Key;
                }
            }
            return MaxColor;
        }        
        public void ChangeColor(ObservableCollection<Field> AllFields)
        {
            base.ChangeColor(ChooseColor(AllFields), AllFields);
        }
    }
}
