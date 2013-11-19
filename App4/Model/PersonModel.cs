using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App4.Model
{
    public class Position
    {
        public int x;
        public int y;

        public Position(int i, int j){
            x = i;
            y = j;
        }

        public Position(){
            x = 0;
            y = 0;
        }

        public Position copy(){
            Position temp = new Position(this.x, this.y);

            return temp;
        }

        public bool isEqualTo(Position pos){
            if(pos.x == this.x && pos.y == this.y){
                return true;
            }
            return false;
        }

        public bool isEnd()
        {
            if (this.x == MapData.width - 2 && this.y == MapData.height - 2)
            {
                return true;
            }
            return false;
        }
    }

    public enum Direction
    {
        LEFT, UP, RIGHT, DOWN
    }

    public class PersonModel
    {
        // the position of the beginning location
        public Position curPos = new Position(1, 1);

        public MapData map;

        public PersonModel(ref MapData map){
            this.map = map;
        }
        /// <summary>
        /// Judge whether this direction pos is enabled to go. If can, moved.
        /// </summary>
        /// <param name="dirType"></param>
        /// <returns></returns>
        public bool toNextPos(Direction dirType)
        {
            Position nextPos = this.curPos.copy();
            switch (dirType)
            {
                case Direction.LEFT:
                    nextPos.y--;
                    break;
                case Direction.UP:
                    nextPos.x--;
                    break;
                case Direction.DOWN:
                    nextPos.x++;
                    break;
                case Direction.RIGHT:
                    nextPos.y++;
                    break;
            }

            if (canMove(ref nextPos))
            {
                this.curPos = nextPos;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Judge whether pos is enabled to go
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool canMove(ref Position pos)
        {
            // if next pos is border
            if (pos.x == 0 || pos.x == MapData.row - 1 
                || pos.y == MapData.column - 1 || pos.y == 0)
            {
                return false;
            }

            if (this.map.model.mapArray[pos.x][pos.y] == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // judge whether the person arrived the end pos.
        public bool isEnd(){
            if(curPos.x == MapData.row - 2 && curPos.y == MapData.column - 2)
            {
                return true;
            }
            return false;
        }
    }
}
