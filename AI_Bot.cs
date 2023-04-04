using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Game_Revision
{
    internal class AI_Bot
    {
        private int Direction = 0;

        private int X;
        private int Y;


        public AI_Bot(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public int GetDirection()
        {
            return Direction;
        }

        public void SetDirection(int Direction)
        {
            this.Direction = Direction;
        }

        public void MoveDirection()
        {
            if (Direction == 1)
            {
                Y--;
            }
            else if (Direction == 2)
            {
                Y++;
            }
            else if (Direction == 3)
            {
                X--;
            }
            else if (Direction == 4)
            {
                X++;
            }

            if (X < 0)
            {
                X = 18;
            }
            else if (X > 18)
            {
                X = 0;
            }

        }

        public bool Collision(int x, int y)
        {
            if (x == X && y == Y)
                return true;
            else
                return false;
        }

        public int getX()
        {
            return X;
        }
        public int getY()
        {
            return Y;
        }

        public void Respawn()
        {
            X = 9;
            Y = 9;
        }



    }
}
