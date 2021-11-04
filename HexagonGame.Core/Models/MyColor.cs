using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class MyColor
    {
        public int A { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public MyColor()
        {
            
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
