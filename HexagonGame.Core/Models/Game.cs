using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace HexagonGame.Core.Models
{
    public class Game : BindableBase
    {
        private Player activePlayer;
        public ObservableCollection<Field> Fields { get; set; } = new ObservableCollection<Field>();
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        public ObservableCollection<MyColor> FieldColors { get; set; } = new ObservableCollection<MyColor>();
        public ObservableCollection<MyColor> FreeColors { get; set; } = new ObservableCollection<MyColor>();
        public Player ActivePlayer
        {
            get => activePlayer;
            set => SetProperty(ref activePlayer, value);
        }

        public Game(GameSettings GameSettings)
        {
            Players = GameSettings.Players;
            FieldColors = GameSettings.ColorList;
            CreateHexagons(GameSettings.Size, GameSettings.Width, GameSettings.Height);
            ActivePlayer = Players[0];
            foreach (var player in GameSettings.Players)
            {
                player.FindOwnFields(Fields);
                foreach (var field in player.OwnFields)
                {
                    field.Mark(player.Color);
                }
            }
            FindFreeColors();            
        }
        public MyColor PickRandomColor(int NumberOfColors = 6)
        {
            Random rnd = new Random();
            var idx = rnd.Next(0, Math.Min(FieldColors.Count, NumberOfColors));
            return FieldColors[idx];
        }
        public void ActivateNextPlayer()
        {
            ActivePlayer = Players.IndexOf(ActivePlayer) == Players.Count - 1 ? Players[0] : Players[Players.IndexOf(ActivePlayer) + 1];
        }
        public void CreateHexagons(int Size, double Width, double Height)
        {
            MyPoint start = new MyPoint(50 + 0.75 * (Size - 1) * Width, (2 * Size - 1) / 2 * Height + 50);
            double[] x_delta = new double[] { 0, Width / 4, 3.0 / 4 * Width, Width, 3.0 / 4 * Width, Width / 4 };
            double[] y_delta = new double[] { 0, -1.0 / 2 * Height, -1.0 / 2 * Height, 0, 1.0 / 2 * Height, 1.0 / 2 * Height };
            for (int i = -Size + 1; i < Size; i++)
            {
                for (int j = -Size + 1; j < Size; j++)
                {
                    if (i * j > 0 && Math.Abs(i) + Math.Abs(j) >= Size) continue;
                    MyPoint LeftCorner = new MyPoint(start.X + Width * 0.85 * i, start.Y + Height * (0.6 * i + 1.15 * j));
                    var NewField = new Field();
                    NewField.CoordX = i;
                    NewField.CoordY = j;
                    NewField.Colors.OwnColor = PickRandomColor();
                    for (int k = 0; k < x_delta.Length; k++)
                    {
                        NewField.PolygonItem.Points.Add(new MyPoint(LeftCorner.X + x_delta[k], LeftCorner.Y + y_delta[k]));
                    }
                    Fields.Add(NewField);
                }
            }
        }
        public void FindFreeColors()
        {
            FreeColors.Clear();
            foreach (var color in FieldColors)
            {
                if (Players.Any(p => p.OwnFields[0].Colors.OwnColor == color)) continue;
                FreeColors.Add(color);
            }
        }
        public void TakeRobotTurns()
        {
            while(ActivePlayer is RobotPlayer)
            {
                Thread.Sleep(2000);
                RobotPlayer Robot = (RobotPlayer)ActivePlayer;
                Robot.ChangeColor(Fields);
                ActivateNextPlayer();
            }
        }

    }
}
