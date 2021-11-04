using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HexagonGame.Core.Models;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Windows.Foundation;
using Windows.UI;

namespace HexagonGame.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public INavigationService NavigationService { get; set; }
        
        private int numberOfPlayers = 0;
        public int NumberOfPlayers
        {
            get => numberOfPlayers;
            set
            {
                SetProperty(ref numberOfPlayers, value);
                NumberOfColors = NumberOfPlayers + 3;
                Players.Clear();
                for (int i = 0; i < value; i++)
                {
                    Players.Add(new Player("Játékos" + i, 0, 0, PickRandomColor()));
                }
            }
        }
        private int numberOfColors = 0;
        public int NumberOfColors
        {
            get => numberOfColors;
            set
            {
                SetProperty(ref numberOfColors, value);
                ColorList.Clear();
                Random random = new Random();
                for (int i = 0; i < value; i++)
                {
                    ColorList.Add(new MyColor((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
                }
            }
        }
        public int Width { get; set; } = 40;
        public int Height { get; set; } = 36;
        private Player activePlayer;
        public Player ActivePlayer
        {
            get => activePlayer;
            set => SetProperty(ref activePlayer, value);
        }
        public ObservableCollection<MyColor> ColorList { get; set; } = new ObservableCollection<MyColor>();
        public ObservableCollection<Field> Fields { get; set; } = new ObservableCollection<Field>();
        public List<Field> VisitedFields { get; set; } = new List<Field>();
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        Dictionary<MyColor, HashSet<Field>> OneColorNeighbourFields = new Dictionary<MyColor, HashSet<Field>>();
        private int size = 7;
        public int Size
        {
            get => size;
            set => SetProperty(ref size, value);
        }
        public DelegateCommand RetryCommand { get; set; }
        public DelegateCommand<object> ChangeColorCommand { get; set; }
        public DelegateCommand StartGameCommand { get; set; }
        public GameViewModel()
        {
            RetryCommand = new DelegateCommand(Retry);
            ChangeColorCommand = new DelegateCommand<object>(ChangeColorCommandFunc);
            StartGameCommand = new DelegateCommand(StartGame);
            NumberOfPlayers = 1;
            NumberOfColors = 10;
            ActivePlayer = Players[0];
            CreateHexagons();
            foreach (var player in Players)
            {
                FindFieldsOfPlayer(player);
                foreach (var field in player.Fields)
                {
                    field.Mark(player.Color);
                }
            }
        }

        public void CreateHexagons()
        {
            Point start = new Point(50 + 0.75 * (Size - 1) * Width, (2 * Size - 1) / 2 * Height + 50);
            double[] x_delta = new double[] { 0, Width / 4, 3.0 / 4 * Width, Width, 3.0 / 4 * Width, Width / 4 };
            double[] y_delta = new double[] { 0, -1.0 / 2 * Height, -1.0 / 2 * Height, 0, 1.0 / 2 * Height, 1.0 / 2 * Height };
            for (int i = -Size + 1; i < Size; i++)
            {
                for (int j = -Size + 1; j < Size; j++)
                {
                    if (i * j > 0 && Math.Abs(i) + Math.Abs(j) >= Size) continue;
                    Point LeftCorner = new Point(start.X + Width * 0.75 * i, start.Y + Height * (0.5 * i + 1 * j));
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

        public MyColor PickRandomColor(int NumberOfColors = 6)
        {
            Random rnd = new Random();
            var idx = rnd.Next(0, Math.Min(ColorList.Count, NumberOfColors));
            return ColorList[idx];
        }

        public void ChangeColor(Player p, MyColor NewColor)
        {
            FindFieldsOfPlayer(p);
            p.Fields.ForEach(f => f.Colors.OwnColor = NewColor);
            FindFieldsOfPlayer(p);
            p.Fields.ForEach(f => f.Mark(p.Color));
        }
        public List<Field> FindOneColorArea(int StartCoordX, int StartCoordY)
        {
            List<Field> Acc = new List<Field>();
            Field StartField = Fields.First(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY);
            FindOneColorArea(StartCoordX, StartCoordY, StartField.Colors.OwnColor, Acc);
            return Acc;
        }
        private void FindOneColorArea(int StartCoordX, int StartCoordY, MyColor FieldColor, List<Field> Accumulator)
        {
            Field StartField = Fields.First(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY);
            Accumulator.Add(StartField);
            List<Field> NeighbourFields = FindNeighboursOfField(StartField);
            var ColorableNeighbours = Fields.Where(f => NeighbourFields.Contains(f) && f.Colors.OwnColor == FieldColor && !Accumulator.Contains(f)).ToList();
            if (ColorableNeighbours.Count == 0) return;
            ColorableNeighbours.ForEach(f =>
            {
                Accumulator.Add(f);
                FindOneColorArea(f.CoordX, f.CoordY, FieldColor, Accumulator);
            });
        }
        public List<Field> FindFieldsOfPlayer(Player player)
        {
            player.Fields.Clear();
            Field StartField = Fields.First(f => f.CoordX == player.StartCoordX && f.CoordY == player.StartCoordY);
            FindOneColorArea(player.StartCoordX, player.StartCoordY, StartField.Colors.OwnColor, player.Fields);
            return player.Fields;
        }

        List<Field> FindNeighboursOfField(Field field)
        {
            return Fields.Where(f => Math.Abs(f.CoordX + f.CoordY - field.CoordX - field.CoordY) <= 1 && Math.Abs(f.CoordX - field.CoordX) <= 1 && Math.Abs(f.CoordY - field.CoordY) <= 1).ToList();
        }
        public Dictionary<MyColor, HashSet<Field>> CountNegihbourColors(Player p)
        {
            OneColorNeighbourFields = new Dictionary<MyColor, HashSet<Field>>();
            var PossibleColors = new HashSet<MyColor>();
            HashSet<Field> neighbours = new HashSet<Field>();
            FindFieldsOfPlayer(p);
            p.Fields.ForEach(f =>
            {
                var fieldNeighbours = FindNeighboursOfField(f);
                fieldNeighbours.RemoveAll(field => p.Fields.Contains(field));
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
                    FindOneColorArea(neighbour.CoordX, neighbour.CoordY).ForEach(neigh => OneColorNeighbourFields[neighbour.Colors.OwnColor].Add(neigh));
                }
                else
                {
                    OneColorNeighbourFields.Add(neighbour.Colors.OwnColor, new HashSet<Field>(FindOneColorArea(neighbour.CoordX, neighbour.CoordY)));
                }
            }
            return OneColorNeighbourFields;
        }
        public MyColor RobotTurn(RobotPlayer robot)
        {
            int MaxCount = 0;
            MyColor MaxColor = new MyColor();
            Dictionary<MyColor, HashSet<Field>> ColorNeighbours = CountNegihbourColors(robot);
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

        public void Retry()
        {
            Fields.Clear();
            CreateHexagons();
            foreach (var player in Players)
            {
                FindFieldsOfPlayer(player);
                foreach (var field in player.Fields)
                {
                    field.Mark(player.Color);
                }
            }
        }
        public void ChangeColorCommandFunc(object color)
        {
            ChangeColor(ActivePlayer, (MyColor)color);
            ActivePlayer = Players.IndexOf(ActivePlayer) == Players.Count - 1 ? Players[0] : Players[Players.IndexOf(ActivePlayer)];
        }
        public void StartGame()
        {
            _navigationService.Navigate(PageTokens.GamePage, null);
            CreateHexagons();
            ActivePlayer = Players[0];

            //Players.Add(new Player("Koni", 0, 6, Colors.Blue));
            //Players.Add(new RobotPlayer("Robot", 0, -6, Colors.Red, 1));

            foreach (var player in Players)
            {
                FindFieldsOfPlayer(player);
                foreach (var field in player.Fields)
                {
                    field.Mark(player.Color);
                }
            }
        }
    }
}
