using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class RobotPlayer : Player
    {
        public int Level { get; set; } = 5;
        Dictionary<MyColor, HashSet<Field>> OneColorNeighbourFields = new Dictionary<MyColor, HashSet<Field>>();
        public RobotPlayer(string Name, MyColor Color) : base(Name, Color)
        {
            IsRobot = true;
        }
        public Dictionary<MyColor, HashSet<Field>> CountNegihbourColors(ObservableCollection<Field> AllFields, ObservableCollection<MyColor> FreeColors)
        {
            OneColorNeighbourFields = new Dictionary<MyColor, HashSet<Field>>();
            HashSet<Field> neighbours = new HashSet<Field>();
            FindOwnFields(AllFields);
            OwnFields.ForEach(f =>
            {
                var fieldNeighbours = f.FindNeighbours(AllFields);
                fieldNeighbours.RemoveAll(field => OwnFields.Contains(field));
                fieldNeighbours.ForEach(fn => neighbours.Add(fn));
            });
            foreach (var neighbour in neighbours)
            {
                if (OneColorNeighbourFields.ContainsKey(neighbour.Colors.OwnColor))
                {
                    neighbour.FindOneColorAreaFromHere(AllFields).ForEach(neigh => OneColorNeighbourFields[neighbour.Colors.OwnColor].Add(neigh));
                }
                else if (FreeColors.Contains(neighbour.Colors.OwnColor))
                {
                    OneColorNeighbourFields.Add(neighbour.Colors.OwnColor, new HashSet<Field>(neighbour.FindOneColorAreaFromHere(AllFields)));
                }
            }
            return OneColorNeighbourFields;
        }
        public MyColor ChooseColor(ObservableCollection<Field> AllFields, ObservableCollection<MyColor> FreeColors)
        {
            int MaxCount = 0;
            MyColor MaxColor = FreeColors[0];
            Dictionary<MyColor, HashSet<Field>> ColorNeighbours = CountNegihbourColors(AllFields, FreeColors);
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
        public void ChangeColor(ObservableCollection<Field> AllFields, ObservableCollection<MyColor> FreeColors)
        {
            base.ChangeColor(ChooseColor(AllFields, FreeColors), AllFields);
        }
    }
}
