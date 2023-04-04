using Final_Game_Revision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Final_Game_Revision
{
    internal class PlayerAI
    {

        private Thread AI_Thread;
        private int frequency = 200;
        int lastDirection = 0;

        int playerID;

        bool inGame = true;

        public PlayerAI(int id)
        {
            this.playerID = id;

            AI_Thread = new Thread(initialize);
            AI_Thread.Start();

        }

        private void initialize()
        {

            do
            {
                int move = AI_Movement();
                lastDirection = move;
                Program.pManager.allPlayers[playerID].MoveDirection(move);
                Program.pManager.ScorePointCheck();
                Thread.Sleep(frequency);
            } while (inGame);
            Thread.Sleep(50);

        }


        private int AI_Movement()
        {

            int randomNumber;
            int Attempts = 0;
            Random r = new Random();

            Player playerRef = Program.pManager.allPlayers[playerID];
            //we cache this as we dont want to be constantly fetching from array,
            //and we dont want to modify the array yet       

            int tempX = playerRef.getX();
            int tempY = playerRef.getY();
            do
            {
                //generate random number
                randomNumber = r.Next(1, 5);
                // grabs new positions
                int newCol = tempX;
                int newRow = tempY;
                bool oppositeDirection = false; //for checking directional 

                //----------up
                if (randomNumber == 1)
                {
                    newRow--;
                    if (lastDirection == 2)
                    {
                        oppositeDirection = true;
                    }
                }
                //------------down
                else if (randomNumber == 2)
                {
                    newRow++;
                    if (lastDirection == 1)
                    {
                        oppositeDirection = true;
                    }
                }
                //--------------------left
                else if (randomNumber == 3)
                {
                    newCol--;
                    if (lastDirection == 4)
                    {
                        oppositeDirection = true;
                    }
                }
                //-------------right
                else if (randomNumber == 4)
                {
                    newCol++;
                    if (lastDirection == 3)
                    {
                        oppositeDirection = true;
                    }
                }

                if (newCol < 0)
                {
                    newCol = 18;
                }
                else if (newCol > 18)
                {
                    newCol = 0;
                }


                //check if tthe move is Valid
                bool validMove = MovementIsVaild(newCol, newRow);

                bool moveInSameDirection = validMove && randomNumber == lastDirection; //move forward
                bool movePerpendicular = validMove && !oppositeDirection; //moves that are perpendicular
                bool forcedRotation = validMove && oppositeDirection && Attempts > 14; //forced rotation needs to take lower precedence over the other 2 conditions
                bool Stuck = !validMove && Attempts > 28; //we need to try a fair effort before calling it quits


                if (moveInSameDirection || movePerpendicular || forcedRotation)
                {
                    return randomNumber;
                }
                else if (Stuck)
                {
                    return 0; //wait for other player to move out of the way
                }
                Attempts++;

            } while (true);


        }

        private bool MovementIsVaild(int x, int y)
        {
            //check if path is clear on maze   

            if (Program.maze[y, x] != " ")
            {
                return false;
            }
            //if  no collisions detected, we return true
            return true;

        }

        public void Stop()
        {
            inGame = false;
            if (AI_Thread.IsAlive)
            {
                AI_Thread.Join();
            }

        }


    }
}
