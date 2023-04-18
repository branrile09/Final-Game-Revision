
using System.ComponentModel;
using System.Diagnostics;

namespace Final_Game_Revision
{
    internal class Program
    {
        public static string[,] maze = defaultMaze();
        public static int[,] charPositions = defaultPositions();

        public static int player0Score = 0;
        public static int player1Score = 0;

        public static bool InGame = false;
        public static AI_Manager aiManager;
        public static Player_Manager pManager;


        static void Main(string[] args)
        {
            //declare and start a new thread that will manage the AI characters
            int playerCount = 0;
            do
            {
                Console.WriteLine("how many players?(0-2)");
                try
                {
                    playerCount = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch
                { 
                
                }
               
            } while (true);

            Console.Clear();
            if (playerCount > 2)
            {
                playerCount = 2;
            }
            else if (playerCount < 0)
            {
                playerCount = 0;
            }


            Multiplayer(playerCount);
            //SinglePlayer();

        }
               


        //we need to get multiplayer to work
        static void Multiplayer(int playerCount)
        {
            InGame = true;
            aiManager = new AI_Manager();
            pManager = new Player_Manager(playerCount);

            Thread GameRender = new Thread(() => DisplayMazeThread(120));
            GameRender.Start();


            do
            {


            } while (pManager.getPlayerScore(0) < 20 && pManager.getPlayerScore(1) < 20);

            int i = 1;
            if (pManager.getPlayerScore(1) == 20)
            {
                i = 2;
            }


            InGame = false;
            aiManager.Stop();
            pManager.Stop();
            GameRender.Join();

            Console.Clear();
            Console.WriteLine($"Player{i} Won");
            Thread.Sleep(2000);

        }

        //create a thread to display the maze
        static void DisplayMazeThread(int fps = 60)
        {
            int delay = 1000 / fps;

            do
            {
                DisplayMaze();
                Thread.Sleep(delay);
            } while (InGame);
            Thread.Sleep(150);


        }

        static int[,] defaultPositions()
        {

            //charPositions[charID, X/Y]; 
            int[,] charPositions = {
            {1,1}, // player  (charPositions[0,0] & charPositions[0,1])
            {1,2}, // AI1 (charPositions[1,0] & charPositions[1,1])            
            };


            return charPositions;
        }


        static void DisplayMaze()
        {

            //we are re-writing over entire screen to reduce flicker
            //since we are doing this, we can only have 1 instance of writing
            //this is why we need mutex
            Console.SetCursorPosition(0, 0);
            //displays score
            Console.WriteLine("Score:" + pManager.getPlayerScore(0) + "   Score:" + pManager.getPlayerScore(1));

            //display the maze with the players and AI current position
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    if (pManager.playerLocation(0, col, row))
                    {
                        Console.Write("1");
                    }
                    else if (pManager.playerLocation(1, col, row))
                    {
                        Console.Write("2");
                    }
                    else if (aiManager.isAI(col, row))
                    {
                        Console.Write("&");
                    }
                    else
                    {
                        Console.Write(maze[row, col]);
                    }

                }

                Console.WriteLine();
            }
            Console.Write("use arrows or WASD to move");
        }


        static public string[,] defaultMaze()
        {

            string[,] maze = {
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#","#", "#", "#", "#", "#", "#", "#", "#", "#" },//
                { "#", " ", " ", " ", " ", " ", " ", " ", " ", "#"," ", " ", " ", " ", " ", " ", " ", " ", "#" },//
                { "#", " ", "#", "#", " ", "#", "#", "#", " ", "#"," ", "#", "#", "#", " ", "#", "#", " ", "#" },//
                { "#", " ", " ", " ", " ", " ", " ", " ", " ", " "," ", " ", " ", " ", " ", " ", " ", " ", "#" },//
                { "#", " ", "#", "#", " ", "#", " ", "#", "#", "#","#", "#", " ", "#", " ", "#", "#", " ", "#" },//
                { "#", " ", " ", " ", " ", "#", " ", " ", " ", "#"," ", " ", " ", "#", " ", " ", " ", " ", "#" },//
                { "#", "#", "#", "#", " ", "#", "#", "#", " ", "#"," ", "#", "#", "#", " ", "#", "#", "#", "#" },//
                { "#", "#", "#", "#", " ", "#", " ", " ", " ", " "," ", " ", " ", "#", " ", "#", "#", "#", "#" },//
                { "#", "#", "#", "#", " ", "#", " ", "#", "#", " ","#", "#", " ", "#", " ", "#", "#", "#", "#" },//
                { " ", " ", " ", " ", " ", " ", " ", "#", " ", " "," ", "#", " ", " ", " ", " ", " ", " ", " " },//
                { "#", "#", "#", "#", " ", "#", " ", "#", "#", "#","#", "#", " ", "#", " ", "#", "#", "#", "#" },//
                { "#", "#", "#", "#", " ", "#", " ", " ", " ", " "," ", " ", " ", "#", " ", "#", "#", "#", "#" },//
                { "#", "#", "#", "#", " ", "#", "#", "#", " ", "#"," ", "#", "#", "#", " ", "#", "#", "#", "#" },//
                { "#", " ", " ", " ", " ", "#", " ", " ", " ", "#"," ", " ", " ", "#", " ", " ", " ", " ", "#" },//
                { "#", " ", "#", "#", " ", "#", " ", "#", "#", "#","#", "#", " ", "#", " ", "#", "#", " ", "#" },//
                { "#", " ", " ", " ", " ", " ", " ", " ", " ", " "," ", " ", " ", " ", " ", " ", " ", " ", "#" },//
                { "#", " ", "#", "#", " ", "#", "#", "#", " ", "#"," ", "#", "#", "#", " ", "#", "#", " ", "#" },//
                { "#", " ", " ", " ", " ", " ", " ", " ", " ", "#"," ", " ", " ", " ", " ", " ", " ", " ", "#" },//                
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#","#", "#", "#", "#", "#", "#", "#", "#", "#" },//     
            };
            return maze;

        }




    }
}
