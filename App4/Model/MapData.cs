using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App4.Model
{
    public sealed partial class MapData
    {
        public static int width = 1025;
        public static int height = 525;
        public static int column = width / GamePage.sizeOfBlock;
        public static int row = height / GamePage.sizeOfBlock;
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