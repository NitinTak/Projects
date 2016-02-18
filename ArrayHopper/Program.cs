using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP_ArrayHopper
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> input = new List<int>();
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Please provide path to file as commald line argument. Exiting.");
                    Console.ReadLine();
                    System.Environment.Exit(1);
                }
                else
                {
                    StreamReader sr = new StreamReader(args[0]);
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        input.Add(int.Parse(line));
                    }
                    sr.Close();
                }
                //                Console.WriteLine(input.Count);
                Console.WriteLine(MinHop(ref input));
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ":" + ex.StackTrace);
            }
        }
        static string MinHop(ref List<int> input)
        {
            /*
            5
            6
            0
            4
            2
            4
            1
            0
            0
            4

            1
            3
            6
            3
            2
            3
            6
            8
            9
            5

            */
            StringBuilder ret = new StringBuilder();
            bool found = false;
            int largestIndex;
            int hopsAvailable, k;
            ret.Clear();
            try
            {
                AppendHop(ref ret, "0");
                for (int i = 0; i < input.Count; i++)
                {
                    hopsAvailable = input[i];
                    if (i + hopsAvailable > input.Count - 1)
                    {
                        AppendHop(ref ret, "out");
                        found = true;
                        break;
                    }
                    if (input[i] > 0)
                    {
                        k = i + 1;
                        largestIndex = k;
                    }
                    else
                    {
                        return "failure";
                    }
                    while (k <= i + hopsAvailable)
                    {
                        if (input[largestIndex] <= input[k] + (k - largestIndex))
                        {
                            largestIndex = k;
                        }
                        k++;
                    }
                    AppendHop(ref ret, largestIndex.ToString());
                    i = largestIndex - 1;
                }
                if (!found)
                {
                    return "failure";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret.ToString();
        }

        static void AppendHop(ref StringBuilder sb, string n)
        {
            try
            {
                if (n.Equals("out"))
                    sb.Append(n);
                else
                    sb.Append(n + ", ");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
