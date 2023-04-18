using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Game_Revision
{
    internal class Player
    {
        private int X;
        private int Y;
        private int score = 0;

        public Player(int X, int Y)
        { 
            this.X = X;
            this.Y = Y;
        }

        public void Score()
        {
            score++;        
        }


        public void MoveDirection(int direction)
        {
            int newx = X;
            int newy = Y;

            if (direction == 1)
            {
                newy--;
            }
            else if (direction == 2)
            {
                newy++;
            }
            else if (direction == 3)
            {
                newx--;
            }
            else if (direction == 4)
            {
                newx++;
            }

            if (newx < 0)
            {
                newx = 18;
            }
            else if (newx > 18)
            {
                newx = 0;
            }

            if (Program.maze[newy, newx] == " ")
            {
                X = newx;
                Y = newy;
            }

        }

        public int getX()
        {
            return X;        
        }
        public int getY()
        {
            return Y;        
        }

        public int GetScore()
        {
            return score;
        }

        public bool Location(int X, int Y)
        { 
            return this.X == X && this.Y == Y;        
        }


    }
}
