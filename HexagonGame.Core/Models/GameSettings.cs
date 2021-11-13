using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class GameSettings : BindableBase
    {        
        private int numberOfColors = 0;
        public int NumberOfColors
        {
            get => numberOfColors;
            set
            {
                SetProperty(ref numberOfColors, value);
                ColorList.Clear();                
                for (int i = 0; i < value; i++)
                {
                    AddRandomColor();
                }
            }
        }
        private int size = 7;
        public int Size
        {
            get => size;
            set => SetProperty(ref size, value);
        }
        public int Width { get; set; } = 40;
        public int Height { get; set; } = 36;
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        public List<MyColor> PlayerColors = new List<MyColor>()
        {
            new MyColor(255,255,0,0), //red
            new MyColor(255,0,255,0), //green
            new MyColor(255,255,128,0), //orange
            new MyColor(255,0,0,255), //blue
            new MyColor(255, 188,0,255), //purple
            new MyColor(255,255,255,128) //yellowish
        };
        public ObservableCollection<MyColor> ColorList { get; set; } = new ObservableCollection<MyColor>();

        public GameSettings()
        {
            Players.CollectionChanged += ResetColorNumber;
        }
        public void AddRandomColor()
        {
            Random random = new Random();
            MyColor NewColor = new MyColor(((byte)random.Next(3) + 5) * 31, (byte)random.Next(13) * 20, (byte)random.Next(13) * 20, (byte)random.Next(13) * 20);
            if (ColorList.Contains(NewColor))
            {
                AddRandomColor();
            }
            else
            {
                ColorList.Add(new MyColor((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
                return;
            }
        }
        public bool NewPlayer()
        {
            (int X, int Y) = PickRandomUnoccupiedField();
            Players.Add(new Player("Játékos" + Players.Count, X, Y, PlayerColors[Players.Count]));
            return Players.Count >= 6 ? false : true;
        }
        public bool NewRobotPlayer()
        {
            (int X, int Y) = PickRandomUnoccupiedField();
            Players.Add(new RobotPlayer("Robot" + Players.Count, X, Y, PlayerColors[Players.Count]));
            return Players.Count >= 6 ? false : true;
        }
        public void ResetColorNumber(object sender, NotifyCollectionChangedEventArgs e)
        {
            NumberOfColors = Players.Count + 3;
        }
        public (int, int) PickRandomUnoccupiedField()
        {
            Random rnd = new Random();
            int X = rnd.Next(2 * (Size - 1)) - Size + 1;
            int Y = rnd.Next(2 * (Size - 1)) - Size + 1;
            if (X * Y > 0 && Math.Abs(X) + Math.Abs(Y) >= Size || Players.Any(p => p.StartCoordX == X && p.StartCoordY == Y)) return PickRandomUnoccupiedField();
            else return (X, Y);
        }        
    }
}
