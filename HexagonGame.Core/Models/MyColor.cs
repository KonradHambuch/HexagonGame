using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class MyColor : BindableBase
    {
        private int a;
        private int r;
        private int g;
        private int b;
        public int A 
        {
            get => a;
            set => SetProperty(ref a, value);
        }
        public int R
        {
            get => r;
            set => SetProperty(ref r, value);
        }
        public int G
        {
            get => g;
            set => SetProperty(ref g, value);
        }
        public int B
        {
            get => b;
            set => SetProperty(ref b, value);
        }
        public MyColor()
        {
            A = B = R = G = 0;
        }
        public MyColor(int a, int r, int g, int b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
    }
}
