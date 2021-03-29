using Conex1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Conex1
{
    public class Player
    {
        public int Game { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public int nCircles { get; set; }
    }
}