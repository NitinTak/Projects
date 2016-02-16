using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WordSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            string WORD = "digital";
            Dictionary<string, List<string>> dict_permutation = new Dictionary<string, List<string>>(); //dictionaty used to speed up in permutation, dynamic programming for faster lookup
            List<string> list_permutation = new List<string>(); //containing all permutations, to be printed in the end.
            List<string> list_combination = new List<string>(); //containing all possible word combinations satisfying the constraints in the Q.
            try
            {
                for (int i = 3; i < 8; i++) //since the length can be only >= 3.
                {
                    List<string> temp_combination = new List<string>();
                    temp_combination = nCombination(WORD, i);   //getting all the word combinations possible for word length in range[3,7]
                    foreach (var item in temp_combination)
                    {
                        if (!list_combination.Contains(item))   //removing the duplicate entries caused due ti presence of letter i 2times in word "digital". Saves lot of cycles later while permutation.
                            list_combination.Add(item);
                    }
                }
                //Console.WriteLine(list_combination.Count + " total possible combinations with length >=3 exist.");
                foreach (var item in list_combination)
                {
                    List<string> temp_permutation = new List<string>();
                    temp_permutation = Permutation(item, ref dict_permutation);   //getting all the permutations for all valid word combinations 
                    foreach (var perm in temp_permutation)
                    {
                        if (!list_permutation.Contains(perm))
                            list_permutation.Add(perm);
                    }
                }
                Console.WriteLine(list_permutation.Count + " total possible unique letter combinations with length >= 3 exist for the word 'digital'.\nWriting to file (Filename: SD_digital_WordSplitter.txt) in current working project directory...");
                StreamWriter file_out = new StreamWriter("SD_digital_WordSplitter.txt");    //for writing the output(all the words) to a file for better comprehension.
                foreach (var item in list_permutation)
                {
                    file_out.WriteLine(item);
                }
                file_out.Close();   //closing the output file
                Console.WriteLine("File write completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            Console.ReadLine();
        }

        static List<string> Permutation(string s, ref Dictionary<string, List<string>> dict_permutation)
        {
            List<string> ret = new List<string>();
            try
            {
                if (s.Length == 1)
                {
                    ret.Add(s);
                    if (!dict_permutation.ContainsKey(s))
                    {
                        dict_permutation.Add(s, ret);
                    }
                    return ret;
                }
                else if (dict_permutation.ContainsKey(SortString(s)))
                {
                    return dict_permutation[SortString(s)];
                }
                else
                {
                    StringBuilder sa = new StringBuilder();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < s.Length; i++)
                    {
                        List<string> tempList = new List<string>();
                        sa.Clear();
                        sb.Clear();
                        sa.Append(s[i].ToString());
                        for (int j = 0; j < s.Length; j++)
                        {
                            if (i != j)
                                sb.Append(s[j].ToString());
                        }
                        if (dict_permutation.ContainsKey(SortString(sb.ToString())))
                        {
                            tempList = dict_permutation[SortString(sb.ToString())];
                        }
                        else
                        {
                            tempList = Permutation(sb.ToString(), ref dict_permutation);
                        }
                        foreach (var item in tempList)
                        {
                            ret.Add(sa.ToString() + item);
                        }
                    }
                    if (!dict_permutation.ContainsKey(SortString(s)))
                    {
                        dict_permutation.Add(SortString(s), ret);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            return ret;
        }

        static string SortString(string s)
        {
            if (s.Length <= 1)
                return s;
            char[] t = s.ToCharArray();
            Array.Sort(t);
            return new string(t);
        }

        static List<string> nCombination(string s, int lenOfWord)
        {
            //as per req, s will be 7 and lenOfWord will range from 3-7, so n can take values: 0,1,2,3,4
            List<string> ret = new List<string>();
            int n = s.Length - lenOfWord;
            try
            {
                if (n < 0)
                {
                    return null;
                }
                else if (n == 0)
                {
                    ret.Add(s);
                    return ret;
                }
                else if (n == 1)    //6 loops
                {
                    StringBuilder sa = new StringBuilder();
                    //List<string> tempList = new List<string>();
                    for (int i = 0; i < s.Length; i++)
                    {
                        for (int j = i + 1; j < s.Length; j++)
                        {
                            for (int k = j + 1; k < s.Length; k++)
                            {
                                for (int l = k + 1; l < s.Length; l++)
                                {
                                    for (int m = l + 1; m < s.Length; m++)
                                    {
                                        for (int p = m + 1; p < s.Length; p++)
                                        {
                                            sa.Clear();
                                            sa.Append(s[i].ToString() + s[j].ToString() + s[k].ToString() + s[l].ToString() + s[m].ToString() + s[p].ToString());
                                            ret.Add(SortString(sa.ToString()));

                                        }
                                    }
                                }
                            }
                        }
                    }
                    return ret;
                }
                else if (n == 2)    //5 loops
                {
                    StringBuilder sa = new StringBuilder();
                    //List<string> tempList = new List<string>();
                    for (int i = 0; i < s.Length; i++)
                    {
                        for (int j = i + 1; j < s.Length; j++)
                        {
                            for (int k = j + 1; k < s.Length; k++)
                            {
                                for (int l = k + 1; l < s.Length; l++)
                                {
                                    for (int m = l + 1; m < s.Length; m++)
                                    {
                                        sa.Clear();
                                        sa.Append(s[i].ToString() + s[j].ToString() + s[k].ToString() + s[l].ToString() + s[m].ToString());
                                        ret.Add(SortString(sa.ToString()));
                                    }
                                }
                            }
                        }
                    }
                    return ret;
                }
                else if (n == 3)    //4 loops
                {
                    StringBuilder sa = new StringBuilder();
                    //List<string> tempList = new List<string>();
                    for (int i = 0; i < s.Length; i++)
                    {
                        for (int j = i + 1; j < s.Length; j++)
                        {
                            for (int k = j + 1; k < s.Length; k++)
                            {
                                for (int l = k + 1; l < s.Length; l++)
                                {
                                    sa.Clear();
                                    sa.Append(s[i].ToString() + s[j].ToString() + s[k].ToString() + s[l].ToString());
                                    ret.Add(SortString(sa.ToString()));
                                }
                            }
                        }
                    }
                    return ret;
                }
                else if (n == 4)    //3 loops
                {
                    StringBuilder sa = new StringBuilder();
                    //List<string> tempList = new List<string>();
                    for (int i = 0; i < s.Length; i++)
                    {
                        for (int j = i + 1; j < s.Length; j++)
                        {
                            for (int k = j + 1; k < s.Length; k++)
                            {
                                sa.Clear();
                                sa.Append(s[i].ToString() + s[j].ToString() + s[k].ToString());
                                ret.Add(SortString(sa.ToString()));
                            }
                        }
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            Console.ReadLine();
            return ret;
        }
    }
}
