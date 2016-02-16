using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantcast_HoneyWord
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                /*For generating test cases and tracking performance*/
                //GenerateHoneyWords();
                //Stopwatch sw = Stopwatch.StartNew();

                int levels;
                List<string> honeycomb = new List<string>();    //for storing honeycomb structure content read from file
                HashSet<string> prefix = new HashSet<string>(); //holding all possible substrings starting at index 0 for all words available in dictionary.
                HashSet<string> Dict = new HashSet<string>();   //for storing the words provided as input in dictionary
                SortedSet<string> Found = new SortedSet<string>();  //for holding the words found in honeycomb which are in dictionary
                int i, j, m, x, y;

                if (args.Length < 2)    //checking for console input 
                {
                    Console.WriteLine("Requires 2 arguments. Please run program with <honeycomb_file, dictionary_file> as arguments");
                    Console.ReadLine();
                    System.Environment.Exit(1);
                }
                StreamReader sc = new StreamReader(args[0].ToString());
                string line;
                if (int.TryParse(sc.ReadLine(), out levels))    //reading honeycomb file
                {
                    if (levels > 0)
                    {
                        while ((line = sc.ReadLine()) != null)
                        {
                            honeycomb.Add(line);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid number of levels in honeycomb file. Exiting.");
                        Console.ReadLine();
                        System.Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("Error in reading number of levels from honeycomb file. Exiting.");
                    Console.ReadLine();
                    System.Environment.Exit(1);
                }
                sc.Close();

                sc = new StreamReader(args[1].ToString());
                while ((line = sc.ReadLine()) != null)  //reading dictionary file
                {
                    Dict.Add(line);
                }
                sc.Close();

                foreach (var item in Dict)  //filling the prefix set
                {
                    for (i = item.Length; i > 0; i--)
                    {
                        if (!prefix.Contains(item.Substring(0, i)))
                            prefix.Add(item.Substring(0, i));
                    }
                }

                string[,] honeyGrid = new string[2 * levels - 1, 2 * levels - 1];
                int xCoord = levels - 1, yCoord = levels - 1;
                honeyGrid[xCoord, yCoord] = honeycomb[0][0].ToString(); //centermost element of honeycomb
                for (i = 1; i < honeycomb.Count; i++)   //creating array for holding and traversing honeycomb
                {
                    m = i;
                    j = 0;
                    x = xCoord - i; //always to start from top in clockwise pattern..
                    y = yCoord;
                    while (y < yCoord + i + 1)  //upper left to right
                    {
                        honeyGrid[x, y] = honeycomb[i][j++].ToString();
                        y++;
                    }
                    x++;
                    y--;
                    while (x < xCoord + 1)  //right to bottom
                    {
                        honeyGrid[x++, y] = honeycomb[i][j++].ToString();
                    }
                    y--;
                    while (y > yCoord - 1)  //diagonally left
                    {
                        honeyGrid[x++, y--] = honeycomb[i][j++].ToString();
                    }
                    x--;
                    while (y > yCoord - i - 1)  //lower right to left
                    {
                        honeyGrid[x, y--] = honeycomb[i][j++].ToString();
                    }
                    y++;
                    x--;
                    while (x > xCoord - 1)  //bottom left to up
                    {
                        honeyGrid[x--, y] = honeycomb[i][j++].ToString();
                    }
                    y++;
                    while (y < yCoord)  //diagonally right
                    {
                        honeyGrid[x--, y++] = honeycomb[i][j++].ToString();
                    }
                }

                DFS(ref honeyGrid, ref Dict, ref prefix, ref Found, levels);    //call to solve all words possible inside dictionary.

                foreach (var item in Found)
                {
                    Console.WriteLine(item);
                }
                //for performance check...
                //sw.Stop();
                //Console.WriteLine(sw.ElapsedMilliseconds);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        static void DFS(ref string[,] honeyGrid, ref HashSet<string> Dict, ref HashSet<string> prefix, ref SortedSet<string> Found, int levels)
        {
            try
            {
                int x = 2 * levels - 1;
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < x; j++)
                    {
                        if (honeyGrid[i, j] != null)
                        {
                            SearchForWords(ref honeyGrid, ref Dict, ref prefix, ref Found, levels, i, j);   //calling for all possible non null positions as start position
                        }
                    }
                }
            }
            catch (Exception)
            { throw; }
        }

        static void SearchForWords(ref string[,] honeyGrid, ref HashSet<string> Dict, ref HashSet<string> prefix, ref SortedSet<string> Found, int levels, int xc, int yc)
        {
            try
            {
                int x = 2 * levels - 1;
                bool[,] visited = new bool[x, x];   //maintaining a visited array to prevent getting into infinite loops
                StringBuilder word = new StringBuilder();
                word.Clear();
                PerformDFS(ref honeyGrid, ref Dict, ref prefix, ref Found, ref visited, levels, xc, yc, word);
            }
            catch (Exception)
            { throw; }
        }

        static void PerformDFS(ref string[,] honeyGrid, ref HashSet<string> Dict, ref HashSet<string> prefix, ref SortedSet<string> Found, ref bool[,] visited, int levels, int xc, int yc, StringBuilder word)
        {
            StringBuilder temp = new StringBuilder();
            try
            {
                temp.Append(string.Copy(word.ToString()));
                temp.Append(honeyGrid[xc, yc]);
                if (prefix.Contains(temp.ToString()))
                {
                    visited[xc, yc] = true;
                    if (Dict.Contains(temp.ToString()))
                    {
                        if (!Found.Contains(temp.ToString()))   //to prevent duplicate entries found in different paths...
                            Found.Add(temp.ToString());
                    }
                    if (xc - 1 >= 0 && honeyGrid[xc - 1, yc] != null && !visited[xc - 1, yc])    //can move up
                    {
                        PerformDFS(ref honeyGrid, ref Dict, ref prefix, ref Found, ref visited, levels, xc - 1, yc, temp);
                    }
                    if (xc - 1 >= 0 && (yc + 1 < 2 * levels - 1) && honeyGrid[xc - 1, yc + 1] != null && !visited[xc - 1, yc + 1])  //can move diagonally up
                    {
                        PerformDFS(ref honeyGrid, ref Dict, ref prefix, ref Found, ref visited, levels, xc - 1, yc + 1, temp);
                    }
                    if ((yc + 1 < 2 * levels - 1) && honeyGrid[xc, yc + 1] != null && !visited[xc, yc + 1])  //can move right
                    {
                        PerformDFS(ref honeyGrid, ref Dict, ref prefix, ref Found, ref visited, levels, xc, yc + 1, temp);
                    }
                    if ((xc + 1 < 2 * levels - 1) && honeyGrid[xc + 1, yc] != null && !visited[xc + 1, yc])  //can move down
                    {
                        PerformDFS(ref honeyGrid, ref Dict, ref prefix, ref Found, ref visited, levels, xc + 1, yc, temp);
                    }
                    if ((xc + 1 < 2 * levels - 1) && (yc - 1 >= 0) && honeyGrid[xc + 1, yc - 1] != null && !visited[xc + 1, yc - 1])  //can move diagonally down
                    {
                        PerformDFS(ref honeyGrid, ref Dict, ref prefix, ref Found, ref visited, levels, xc + 1, yc - 1, temp);
                    }
                    if (yc - 1 >= 0 && honeyGrid[xc, yc - 1] != null && !visited[xc, yc - 1])  //can move left
                    {
                        PerformDFS(ref honeyGrid, ref Dict, ref prefix, ref Found, ref visited, levels, xc, yc - 1, temp);
                    }
                    visited[xc, yc] = false;
                }
            }
            catch (Exception)
            { throw; }
        }
        /*static void GenerateHoneyWords()
        {
            int level, limit;
            List<string> dic = new List<string>();
            string[] alphabets = { "A","B", "C", "D" , "E", "F", "G", "H" , "I","J", "K", "L" , "M", "N", "O", "P",
            "Q","R", "S", "T" , "U", "V", "W", "X","Y"  ,"Z" };
            Random r = new Random(30);
            StringBuilder sb = new StringBuilder();

            Console.WriteLine("Enter the LEVEL: ");
            level = int.Parse(Console.ReadLine());

            StreamWriter sw = new StreamWriter("honeyword.txt");
            sw.WriteLine(level.ToString());
            sw.WriteLine(alphabets[r.Next(0, 25)]);
            for (int i = 1; i < level - 1; i++)
            {
                sb.Clear();
                limit = i * 6;
                for (int j = 0; j < limit; j++)
                {
                    sb.Append(alphabets[r.Next(0, 25)]);
                }
                sw.WriteLine(sb.ToString());
            }
            sw.Close();
        }*/
    }
}
