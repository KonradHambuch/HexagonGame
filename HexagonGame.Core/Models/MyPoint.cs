using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class MyPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public MyPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
