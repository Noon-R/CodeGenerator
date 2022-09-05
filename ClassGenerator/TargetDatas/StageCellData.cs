using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator.TargetDatas
{
    public class StageCellData
    {
        public int[,] _CellDatas;
        public int[,] _FloorDatas;
        public int[,] _DirDatas;
    }

    public enum CellType { 
        None,
        Start,
        Goal,
        Wall,
        Enemy,
        Obstacle,
        Breakable,
        Slope,
        Gimmick,
        
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
