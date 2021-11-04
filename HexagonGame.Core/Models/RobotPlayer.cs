using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace HexagonGame.Core.Models
{
    public class RobotPlayer : Player
    {
        public int Level { get; set; }
        public RobotPlayer(string Name, int StartCoordX, int StartCoordY, MyColor Color, int Level) : base(Name, StartCoordX, StartCoordY, Color)
        {
            this.Level = Level;
        }
    }
}
