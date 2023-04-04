using Final_Game_Revision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Final_Game_Revision
{
    internal class AI_Manager
    {
        public List<AI_Bot> Bots = new();
        private bool inGame = true;
        private int frequency = 200; //ms for ai to update
        private Thread AI_Thread;
        private string[,] mazeCopy = Program.defaultMaze();
        private int amount;

        public AI_Manager(int amount = 3)
        {
            this.amount = amount;

            for (int i = 0; i < amount; i++)
            {
                AI_Bot bot = new AI_Bot(8 + i, 9);
                Bots.Add(bot);
            }
            AI_Thread = new Thread(AI);
            AI_Thread.Start();
        }

        private void AI()
        {
            do
            {
                //loops through each AI character, and generates movement
                for (int i = 0; i < Bots.Count; i++)
                {
                    //call AI movement function, receive movement info that has been validated
                    Bots[i].SetDirection(AI_Movement(i));
                    Bots[i].MoveDirection();
                    Thread.Sleep(frequency / amount);
                }
            } while (inGame);
            Thread.Sleep(30);
        }

        private int AI_Movement(int i)
        {
            int randomNumber;
            int Attempts = 0;
            Random r = new Random();

            AI_Bot tempBot = Bots[i];
            //we cache this as we dont want to be constantly fetching from array,
            //and we dont want to modify the array yet
            int lastDirection = tempBot.GetDirection();


            int tempX = tempBot.getX();
            int tempY = tempBot.getY();
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
                bool validMove = MovementIsVaild(i, newCol, newRow);

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

        private bool MovementIsVaild(int ID, int x, int y)
        {
            //check if path is clear on maze   

            if (mazeCopy[y, x] != " ")
            {
                return false;
            }

            //check for coliding with other players or AI
            for (int i = 0; i < Bots.Count(); i++)
            {
                if (i == ID)
                {//we arent comparing against itself
                    continue;
                }
                if (Bots[i].Collision(x, y))
                {//if we see a collision, we return false
                    return false;
                }

            }

            for (int i = 0; i < Program.pManager.allPlayers.Count; i++)
            {
                if (Program.pManager.allPlayers[i].Location(x, y))
                {
                    return false;
                }

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

        public bool playerCollision(int x, int y)
        {
            for (int i = 0; i < Bots.Count; i++)
            {
                if (Bots[i].Collision(x, y))
                {
                    Bots[i].Respawn();
                    return true;
                }
            }
            return false;
        }

        public bool isAI(int x, int y)
        {
            for (int i = 0; i < Bots.Count; i++)
            {
                if (Bots[i].Collision(x, y))
                {
                    return true;
                }
            }
            return false;
        }



    }
}
