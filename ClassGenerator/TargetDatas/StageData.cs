using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator.TargetDatas
{
    public class StageData
    {
        public int[,] _CellDatas;
        public int[,] _FloorDatas;
        public int[,] _DirDatas;
    }

    public enum CellType { 
        None,
        Wall,
        Breakable,
        Gimmick,
        Enemy,
        Start,
        Goal,
        MAX
    }

    public enum Direction { 
        Up,
        Right,
        Down,
        Left,
        UNDIF

    }
}
