using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class RobotPlayer : Player
    {
        public int Level { get; set; } = 3;
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
            MyColor ChoosenColor = null;
            Dictionary<MyColor, HashSet<Field>> ColorNeighbours = CountNegihbourColors(AllFields, FreeColors);
            if (ColorNeighbours.Count == 0) return FreeColors[0];
            switch (Level)
            {
                case 1:
                    // code block

                    ChoosenColor = ColorNeighbours.OrderBy(kvp => kvp.Value.Count).First().Key;

                    break;
                case 2:
                    // code block

                    var rand = new Random();
                    if (rand.Next(2) > 0)
                    {
                        ChoosenColor = ColorNeighbours.OrderBy(kvp => kvp.Value.Count).First().Key;
                    }
                    else
                    {
                        ChoosenColor = ColorNeighbours.OrderByDescending(kvp => kvp.Value.Count).First().Key;
                    }

                    break;
                default:
                    // code block
                    //MyColor MaxColor = FreeColors[0];
                    //int MaxCount = 0;

                    ChoosenColor = ColorNeighbours.OrderByDescending(kvp => kvp.Value.Count).First().Key;

                    //foreach (var entry in ColorNeighbours)
                    //{
                    //    if (entry.Value.Count > MaxCount)
                    //    {
                    //        MaxCount = entry.Value.Count;
                    //        MaxColor = entry.Key;
                    //    }
                    //}

                    break;
            }
            return ChoosenColor;
        }        
        public void ChangeColor(ObservableCollection<Field> AllFields, ObservableCollection<MyColor> FreeColors)
        {
            base.ChangeColor(ChooseColor(AllFields, FreeColors), AllFields);
        }
    }
}
