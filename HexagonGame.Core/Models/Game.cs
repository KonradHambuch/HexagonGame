using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HexagonGame.Core.Models
{
    public delegate void GameEndedHandler();
    public class Game : BindableBase
    {
        public event GameEndedHandler GameEnded;
        private Player activePlayer;
        public int Size { get; set; }
        private bool realPlayerTakingTurn;
        public bool RealPlayerTakingTurn 
        {
            get => realPlayerTakingTurn;
            set => SetProperty(ref realPlayerTakingTurn, value);
        }
        public ObservableCollection<Field> Fields { get; set; } = new ObservableCollection<Field>();
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();

        public List<MyColor> PossibleColors = new List<MyColor>() { new MyColor(255, 230, 25, 75), new MyColor(255, 60, 180, 75), new MyColor(255, 255, 225, 25), new MyColor(255, 0, 130, 200), new MyColor(255, 245, 130, 48), new MyColor(255, 145, 30, 180), new MyColor(255, 70, 240, 240), new MyColor(255, 240, 50, 230), new MyColor(255, 210, 245, 60), new MyColor(255, 250, 190, 212), new MyColor(255, 0, 128, 128), new MyColor(255, 220, 190, 255), new MyColor(255, 170, 110, 40), new MyColor(255, 255, 250, 200), new MyColor(255, 128, 0, 0), new MyColor(255, 170, 255, 195), new MyColor(255, 128, 128, 0), new MyColor(255, 255, 215, 180), new MyColor(255, 0, 0, 128), new MyColor(255, 128, 128, 128), new MyColor(255, 255, 255, 255) };
        public ObservableCollection<MyColor> FieldColors { get; set; } = new ObservableCollection<MyColor>();
        public ObservableCollection<MyColor> FreeColors { get; set; } = new ObservableCollection<MyColor>();
        public Player ActivePlayer
        {
            get => activePlayer;
            set => SetProperty(ref activePlayer, value);
        }
        public Player Winner { get; set; }

        public Game(GameSettings GameSettings)
        {
            Size = GameSettings.Size;
            Players = GameSettings.Players;
            foreach(var color in PossibleColors.Take(GameSettings.NumberOfColors))
            {
                FieldColors.Add(color);
            }
            CreateHexagons(GameSettings.Size, GameSettings.Width, GameSettings.Height);
            foreach (var player in Players)
            {
                (int X, int Y) = PickRandomUnoccupiedField();
                player.StartCoordX = X;
                player.StartCoordY = Y;
                player.FindOwnFields(Fields);
                foreach (var field in player.OwnFields)
                {
                    field.Mark(player.Color);
                }
            }
            ActivateNextPlayer();  
            FindFreeColors();            
        }        
        public MyColor PickRandomColor()
        {
            Random rnd = new Random();
            var idx = rnd.Next(0, FieldColors.Count);
            return FieldColors[idx];
        }
        public (int, int) PickRandomUnoccupiedField()
        {
            Random rnd = new Random();
            int X = rnd.Next(2 * (Size - 1)) - Size + 1;
            int Y = rnd.Next(2 * (Size - 1)) - Size + 1;
            if (X * Y > 0 && Math.Abs(X) + Math.Abs(Y) >= Size || Players.Any(p => p.OwnFields.Any(f => f.CoordX == X && f.CoordY == Y))) return PickRandomUnoccupiedField();
            else return (X, Y);
        }
        public void ActivateNextPlayer()
        {
            if (ActivePlayer == null) ActivePlayer = Players[0];            
            else ActivePlayer = Players.IndexOf(ActivePlayer) == Players.Count - 1 ? Players[0] : Players[Players.IndexOf(ActivePlayer) + 1];
            RealPlayerTakingTurn = ActivePlayer is RobotPlayer ? false : true;
            ActivePlayer.IsActive = true;
            foreach (var player in Players)
            {
                if (player != ActivePlayer) player.IsActive = false;
            }
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
        public async void PlayerChooseColor(MyColor color)
        {
            ActivePlayer.ChangeColor(color, Fields);
            ActivateNextPlayer();
            FindFreeColors();
            TryDetectEndOfGame();
            if (ActivePlayer is RobotPlayer)
            {
                RealPlayerTakingTurn = false;
                await TakeRobotTurns();                              
            }
            
        }
        public async Task TakeRobotTurns()
        {
            while(ActivePlayer is RobotPlayer)
            {
                await Task.Delay(200);
                RobotPlayer Robot = (RobotPlayer)ActivePlayer;
                Robot.ChangeColor(Fields, FreeColors);
                ActivateNextPlayer();
                FindFreeColors();
                TryDetectEndOfGame();               
            }
            RealPlayerTakingTurn = true;
        }
        public void TryDetectEndOfGame()
        {
            if (Fields.All(f => Players.Any(p => p.OwnFields.Contains(f))))
            {
                Winner = Players.Aggregate((mostFields, next) => next.OwnFields.Count > mostFields.OwnFields.Count ? next : mostFields);
                GameEnded?.Invoke();
                GameEnded = null;
            }
        }
    }
}
