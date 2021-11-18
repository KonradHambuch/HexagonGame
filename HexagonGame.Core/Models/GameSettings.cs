using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class GameSettings : BindableBase
    {

        private bool allValid;
        public bool AllValid
        {
            get => allValid;
            set
            {
                SetProperty(ref allValid, value);
            }
        }
        private string sizeError;
        public string SizeError
        {
            get => sizeError;
            set
            {
                SetProperty(ref sizeError, value);
                AllValid = SizeError == null && ColorNumberError == null;
            }
        }
        private string colorNumberError;
        public string ColorNumberError
        {
            get => colorNumberError;
            set
            {
                SetProperty(ref colorNumberError, value);
                AllValid = SizeError == null && ColorNumberError == null;
            }
        }
        private int numberOfColors = 0; 
        public int NumberOfColors
        {
            get => numberOfColors;
            set
            {
                if(ValidateColorNumber(value)) SetProperty(ref numberOfColors, value);                   
            }
        }
        private int size = 7;
        public int Size
        {
            get => size;
            set
            {
                if (ValidateSize(value))
                {
                    SetProperty(ref size, value);
                    Width = 550 / ((Size * 2) - 1);
                    Height = 520 / ((Size * 2) - 1);
                }
            }
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

        public GameSettings()
        {
            Players.CollectionChanged += ResetColorNumber;

            NewPlayer();
        }        
        public bool NewPlayer()
        {
            Players.Add(new Player("Játékos" + Players.Count, PlayerColors[Players.Count]));
            return Players.Count >= 6 ? false : true;
        }
        public bool NewRobotPlayer()
        {            
            Players.Add(new RobotPlayer("Robot" + Players.Count, PlayerColors[Players.Count]));
            return Players.Count >= 6 ? false : true;
        }
        public void ResetColorNumber(object sender, NotifyCollectionChangedEventArgs e)
        {
            NumberOfColors = Players.Count + 3;
        }         
        public bool ValidateSize(int validateableSize)
        {
            if (validateableSize < 3 || validateableSize > 20)
            {
                SizeError = "A pálya mérete 3 és 20 közötti legyen.";
                return false;
            }
            SizeError = null;
            return true;
        }
        public bool ValidateColorNumber(int validateableColorNumber)
        {            
            if (validateableColorNumber < Players.Count + 3)
            {
                ColorNumberError = "A színek száma legalább a játékosok száma + 3.";
                return false;
            }
            if (validateableColorNumber < 4 || validateableColorNumber > 20)
            {
                ColorNumberError = "A színek száma 4 és 20 közötti legyen.";
                return false ;
            }
            ColorNumberError = null;
            return true;
        }
    }
}
