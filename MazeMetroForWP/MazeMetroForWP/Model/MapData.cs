﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeMetroForWP.Model
{
    public sealed partial class MapData
    {
        public static int column = 27;
        public static int row = 15;
        public MazeModel model;
        public MapData()
        {
            int modelH = column / 2;
            int modelR = row / 2;
            model = new MazeModel(modelH, modelR);
            model.setArray();
        }

        public int GetMapItemData(int i, int j)
        {
            return model.mapArray[i][j];
        }
    }
}