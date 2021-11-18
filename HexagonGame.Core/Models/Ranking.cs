using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class Ranking : BindableBase
    {
        private string name;
        private int score;
        public string Name 
        {   
            get => name;
            set => SetProperty(ref name, value);
        }
        public int Score 
        { 
            get => score;
            set => SetProperty(ref score, value);
        }
    }
}
