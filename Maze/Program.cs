using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

/*
Sample Maze file content:
1S11111111
1000011111
1011000001
1010011101
1111011111
1111011111
1000000011
1111011011
1000010011
1F11111111
*/

namespace SomethingDigital_Maze
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Requires maze file as argument. \nPlease run the program with filename.");
                //System.Environment.Exit(1);
            }
            string filename_MAZE = args[0].ToString();
            int len = 0;
            int br = 0;
            string[,] mazeArray;
            Stack PATH = new Stack();
            int[,] indexS = new int[1, 2];
            int[,] indexF = new int[1, 2];
            List<string> mazeLines = new List<string>();
            try
            {
                StreamReader str = new StreamReader(filename_MAZE);
                string line;
                while ((line = str.ReadLine()) != null)
                {
                    mazeLines.Add(line.ToString().ToUpper());   //to solve case sensitivity of S and F
                    len++;
                }
                str.Close();    //closing Reader
                if (len > 0)
                {
                    br = mazeLines[0].Length;
                }
                else
                {
                    Console.WriteLine("No data present in Maze file to read.");
                    //System.Environment.Exit(1);
                }
                mazeArray = GetMazeArray(mazeLines, ref indexS, ref indexF);
                if (mazeArray == null)
                {
                    Console.WriteLine("Issue in Getting this MAZE: Contact Developers.");
                    //System.Environment.Exit(1);
                }

                PATH = GetPathForMaze(mazeArray, indexS, indexF);
                if (PATH.Count == 0)
                {
                    Console.WriteLine("No exit path exists to this maze.");
                }
                else
                {
                    Console.WriteLine("Following is path (coordinates) from Start (\"S\") to Finish (\"F\")");
                    Stack ForwardPath = new Stack();
                    int pathLength = PATH.Count;
                    while (PATH.Count != 0)
                    {
                        ForwardPath.Push((int[,])PATH.Pop());   //To get forward path as PATH contains reversed path...
                    }
                    int[,] pathCoord = new int[pathLength, 2];
                    int[,] tempCoord = new int[1, 2];

                    for (int i = 0; i < pathLength; i++)
                    {
                        tempCoord = (int[,])ForwardPath.Pop();
                        pathCoord[i, 0] = tempCoord[0, 0];
                        pathCoord[i, 1] = tempCoord[0, 1];
                        Console.WriteLine("(" + pathCoord[i, 0] + "," + pathCoord[i, 1] + ")");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            Console.ReadLine();  //to stop from console window from closing..
        }

        /*Converts maze read from file as lines to a 2D array and returns string[,]*/
        static string[,] GetMazeArray(List<string> mazeLines, ref int[,] indexS, ref int[,] indexF)
        {
            int len = 0, breadth = 0;
            string[,] returnMaze = null;
            int itemNumber = 0;
            int charNumber;
            try
            {
                len = mazeLines.Count;
                breadth = mazeLines[0].Length;
                returnMaze = returnMaze = new string[len, breadth];
                foreach (var item in mazeLines)
                {
                    charNumber = 0;
                    foreach (char c in item)
                    {
                        returnMaze[itemNumber, charNumber] = c.ToString();
                        if (c.ToString().Equals("S"))
                        {
                            indexS[0, 0] = itemNumber;
                            indexS[0, 1] = charNumber;
                        }
                        if (c.ToString().Equals("F"))
                        {
                            indexF[0, 0] = itemNumber;
                            indexF[0, 1] = charNumber;
                        }
                        charNumber++;
                    }
                    itemNumber++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            return returnMaze;
        }
        /*Function takes an array of type string containg elements of maze and returns reversed solution path as a stack*/
        static Stack GetPathForMaze(string[,] mazeArray, int[,] indexS, int[,] indexF)
        {
            //Stack returnStack = new Stack();
            Stack adjacentElems = new Stack();
            Stack pathStack = new Stack();

            int row = mazeArray.GetLength(0);   //number of rows
            int col = mazeArray.GetLength(1);   //number of cols
            int i = 0, j = 0, startIndex = 0;

            //to track the neighbours visited adjacent to the point in context.
            int[,] adjancencyMatrix = new int[row, col];
            for (i = 0; i < row; i++)
            {
                for (j = 0; j < col; j++)
                {
                    adjancencyMatrix[i, j] = 0;
                }
            }

            try
            {
                //to handle floating starting point across top boundary..
                for (int k = 0; k < col; k++)
                {
                    if (mazeArray[0, k].Equals("S"))
                    {
                        startIndex = k;
                        break;
                    }
                }


                int[,] coordinates = new int[1, 2];
                int[,] coord = new int[1, 2];
                //coordinates[0, 0] = 0;
                //coordinates[0, 1] = startIndex;
                coordinates[0, 0] = indexS[0, 0];
                coordinates[0, 1] = indexS[0, 1];
                adjacentElems.Push(coordinates);
                adjancencyMatrix[coordinates[0, 0], coordinates[0, 1]] = 1;
                int movement;   //used to track if any useful movement in maze
                while (adjacentElems.Count > 0)
                {
                    movement = 0;
                    coord = (int[,])adjacentElems.Peek();
                    if (!pathStack.Contains(coord))
                        pathStack.Push(coord);
                    //checking element below
                    if (((coord[0, 0] + 1) < row) && (mazeArray[(coord[0, 0] + 1), coord[0, 1]].Equals("0") || mazeArray[(coord[0, 0] + 1), coord[0, 1]].Equals("F")) && (adjancencyMatrix[(coord[0, 0] + 1), coord[0, 1]] == 0))
                    {
                        int[,] tempCoord = new int[1, 2];
                        tempCoord = (int[,])coord.Clone();
                        tempCoord[0, 0] += 1;
                        adjacentElems.Push(tempCoord);
                        adjancencyMatrix[tempCoord[0, 0], tempCoord[0, 1]] = 1; //marking visited..
                        movement = 1;
                        if (mazeArray[(tempCoord[0, 0]), tempCoord[0, 1]].Equals("F"))
                        {
                            pathStack.Push(tempCoord);
                            //pathQueue.Enqueue("F");
                            break;
                        }
                    }

                    //checking element right
                    if (((coord[0, 1] + 1) < col) && (mazeArray[coord[0, 0], (coord[0, 1] + 1)].Equals("0") || mazeArray[coord[0, 0], (coord[0, 1] + 1)].Equals("F")) && (adjancencyMatrix[coord[0, 0], (coord[0, 1] + 1)] == 0))
                    {
                        int[,] tempCoord = new int[1, 2];
                        tempCoord = (int[,])coord.Clone();
                        tempCoord[0, 1] += 1;
                        adjacentElems.Push(tempCoord);
                        adjancencyMatrix[tempCoord[0, 0], tempCoord[0, 1]] = 1;
                        movement = 1;
                        if (mazeArray[(tempCoord[0, 0]), tempCoord[0, 1]].Equals("F"))
                        {
                            pathStack.Push(tempCoord);
                            //pathQueue.Enqueue("F");
                            break;
                        }

                    }
                    //checking element left
                    if (((coord[0, 1] - 1) >= 0) && (mazeArray[coord[0, 0], (coord[0, 1] - 1)].Equals("0") || mazeArray[coord[0, 0], (coord[0, 1] - 1)].Equals("F")) && (adjancencyMatrix[coord[0, 0], (coord[0, 1] - 1)] == 0))
                    {
                        int[,] tempCoord = new int[1, 2];
                        tempCoord = (int[,])coord.Clone();
                        tempCoord[0, 1] -= 1;
                        adjacentElems.Push(tempCoord);
                        adjancencyMatrix[tempCoord[0, 0], tempCoord[0, 1]] = 1;
                        movement = 1;
                        if (mazeArray[(tempCoord[0, 0]), tempCoord[0, 1]].Equals("F"))
                        {
                            pathStack.Push(tempCoord);
                            //pathQueue.Enqueue("F");
                            break;
                        }
                    }
                    //checking element above
                    if (((coord[0, 0] - 1) >= 0) && (mazeArray[(coord[0, 0] - 1), coord[0, 1]].Equals("0") || mazeArray[(coord[0, 0] - 1), coord[0, 1]].Equals("F")) && (adjancencyMatrix[(coord[0, 0] - 1), coord[0, 1]] == 0))
                    {
                        int[,] tempCoord = new int[1, 2];
                        tempCoord = (int[,])coord.Clone();
                        tempCoord[0, 0] -= 1;
                        adjacentElems.Push(tempCoord);
                        adjancencyMatrix[tempCoord[0, 0], tempCoord[0, 1]] = 1;
                        movement = 1;
                        if (mazeArray[(tempCoord[0, 0]), tempCoord[0, 1]].Equals("F"))
                        {
                            pathStack.Push(tempCoord);
                            //pathQueue.Enqueue("F");
                            break;
                        }
                    }
                    //if nothing added, last added needs to be poped. pop the last elem 
                    if (movement == 0)
                    {
                        adjacentElems.Pop();
                        pathStack.Pop();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            return pathStack;
        }
    }
}
