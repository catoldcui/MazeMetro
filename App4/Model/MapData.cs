using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App4.Model
{
    public sealed partial class MapData
    {
        public static int width = 20;
        public static int height = 10;
        public int[] mapNumArr = new int[width * height];

        public MapData()
        {
            SetMapNum();
        }

        private void SetMapNum()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    mapNumArr[i * width + j] = 0;
                }
            }
        }

        public int GetMapItemData(int i, int j)
        {
            return mapNumArr[i * width + j];
        }
    }
}
