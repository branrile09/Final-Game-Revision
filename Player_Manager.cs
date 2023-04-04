using Final_Game_Revision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Game_Revision
{
    internal class Player_Manager
    {
        public List<Player> allPlayers = new List<Player>();
        private Thread PMThread;
        private bool inGame = true;
        private bool AI = false;
        private int playerCount;
        private PlayerAI p2AI;
        private PlayerAI p1AI;

        public Player_Manager(int i)
        {
            this.playerCount = i;
            Player Player1 = new Player(1, 1);
            Player Player2 = new Player(1, 2);
            allPlayers.Add(Player1);
            allPlayers.Add(Player2);

            if (i < 2)
            {
                AI = true;
                p2AI = new PlayerAI(1);
            }
            if (i < 1)
            {
                p1AI = new PlayerAI(0);
            }


            PMThread = new Thread(PlayerManagement);
            PMThread.Start();
        }

        void PlayerManagement()
        {
            do
            {
                if (playerCount == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                int move = KeyPressedPlayer();
                int player = 0;
                if (move > 4)
                {
                    if (!AI)
                    {
                        player++;
                    }
                    move -= 4;
                }
                allPlayers[player].MoveDirection(move);
                ScorePointCheck();

            } while (inGame);
            Thread.Sleep(30);

        }



        int KeyPressedPlayer()
        {
            do
            {
                //cachine the variable
                ConsoleKey keypressed = Console.ReadKey(true).Key;
                //compare what the keystroke is
                if (keypressed == ConsoleKey.UpArrow)
                {
                    return 1;
                }
                else if (keypressed == ConsoleKey.DownArrow)
                {
                    return 2;
                }
                else if (keypressed == ConsoleKey.LeftArrow)
                {
                    return 3;
                }
                else if (keypressed == ConsoleKey.RightArrow)
                {
                    return 4;
                }
                else if (keypressed == ConsoleKey.W)
                {
                    return 5;
                }
                else if (keypressed == ConsoleKey.S)
                {
                    return 6;
                }
                else if (keypressed == ConsoleKey.A)
                {
                    return 7;
                }
                else if (keypressed == ConsoleKey.D)
                {
                    return 8;
                }
                else
                { return 0; }

            }
            while (false);
        }

        public void Stop()
        {
            if (AI)
            {
                if (playerCount == 0)
                {
                    p1AI.Stop();
                }
                p2AI.Stop();
            }


            inGame = false;
            if (PMThread.IsAlive)
            {
                PMThread.Join();
            }

        }

        public void ScorePointCheck()
        {
            for (int i = 0; i < allPlayers.Count(); i++)
            {
                int playerX = allPlayers[i].getX();
                int playerY = allPlayers[i].getY();
                //collision check            
                if (Program.aiManager.playerCollision(playerX, playerY))
                {
                    allPlayers[i].Score();
                }
            }

        }

        public int getPlayerScore(int i)
        {
            return allPlayers[i].GetScore();

        }

        public bool playerLocation(int i, int x, int y)
        {

            if (allPlayers[i].Location(x, y))
            {
                return true;
            }
            return false;

        }



    }
}
