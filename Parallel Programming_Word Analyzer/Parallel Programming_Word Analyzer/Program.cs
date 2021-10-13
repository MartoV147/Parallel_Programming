using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Parallel_Programming_Word_Analyzer
{
    class Program
    {
        #region Methods

        public static string[] SplitString(string textToSplit) 
        {
            string[] separators = new string[] { " ", "  ", "\t", ".", ",", "-", "=" };

            string[] words = textToSplit.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            words = words.Select(w => TrimString(w)).ToArray();
            return words;
        }
        
        public static string TrimString(string word) 
        {
            var regex = new Regex(@"[ \[\*_\-$!.:,\];…—'?\n\t]");
            return regex.Replace(word, "").Trim();
        }

        public static List<string> ReadTextFromFile(string filepath) 
        {
            string text = File.ReadAllText(filepath, Encoding.UTF8);
            string[] words = SplitString(text);
            List<string> listOfWord = new List<string>();
            foreach (var word in words)
            {
                string wordToAdd = TrimString(word);

                if (!string.IsNullOrWhiteSpace(wordToAdd))
                {
                    listOfWord.Add(wordToAdd);
                }
            }

            return listOfWord;

        }

        public static void FindWordsCount(List<string> words)
        {
            int numOfWords = words.Count;
            Console.WriteLine("Number of words = " + numOfWords);
        }

        public static void FindShortestWord(List<string> words) 
        {

            if (words.Count != 0)
            {
                string shortestWord = words[0];

                for (int i = 1; i < words.Count; i++)
                {
                    if (words[i].Length < shortestWord.Length)
                    {
                        shortestWord = words[i];
                    }
                }

                Console.WriteLine("The Shortest word is " + shortestWord);
            }
            else Console.WriteLine("Invalid input");
        }

        public static void FindLongestWord(List<string> words)
        {
            if (words.Count != 0)
            {
                string longestWord = words[0];

                for (int i = 1; i < words.Count; i++)
                {
                    if (words[i].Length > longestWord.Length)
                    {
                        longestWord = words[i];
                    }
                }
                Console.WriteLine("The Longest word is " + longestWord);
            }
            else Console.WriteLine("Invalid input");
        }

        public static void AvgWordLength(List<string> words) 
        {
            float avgWordLength = 0;
            foreach (var word in words)
            {
                avgWordLength += (float)word.Length;
            }

            Console.WriteLine("Average word length is " + avgWordLength / words.Count);
        }

        public static void FiveMostCommonWords(List<string> words) 
        {
            var grouped = words.GroupBy(item => item).ToList();
            List<string> sorted = grouped.OrderByDescending(group => group.Count()).Take(5).Select(x => x.Key).ToList();

            Console.WriteLine("Top 5 most common words are ");
            foreach (var item in sorted)
            {
                Console.WriteLine("\t" + item);
            }
        }

        public static void FiveLeastCommonWords(List<string> words)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            foreach (var item in words)
            {
                if (!result.ContainsKey(item))
                {
                    result.Add(item, 1);
                }
                else
                {
                    result[item] += 1; 
                }
            }

            List<string> sorted = result.OrderBy(x => x.Value).Take(5).Select(x => x.Key).ToList();

            Console.WriteLine("Top 5 least common words are ");
            foreach (var item in sorted)
            {
                Console.WriteLine("\t" + item);
            }

        }

        #endregion

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string filepath = @"D:\Georgi_Konovski_-_Vyv_vremeto_-_10385-b.txt";

            List<string> listOfWord = ReadTextFromFile(filepath);

            List<Thread> threads = new List<Thread>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Thread t1 = new Thread(() => FindWordsCount(listOfWord));
            t1.Start();
            threads.Add(t1);

            Thread t2 = new Thread(() => FindShortestWord(listOfWord));
            t2.Start();
            threads.Add(t2);

            Thread t3 = new Thread(() => FindLongestWord(listOfWord));
            t3.Start();
            threads.Add(t3);

            Thread t4 = new Thread(() => AvgWordLength(listOfWord));
            t4.Start();
            threads.Add(t4);

            Thread t5 = new Thread(() => FiveMostCommonWords(listOfWord));
            t5.Start();
            threads.Add(t5);

            Thread t6 = new Thread(() => FiveLeastCommonWords(listOfWord));
            t6.Start();
            threads.Add(t6);

            foreach (var item in threads)
            {
                item.Join();
            }

            stopWatch.Stop();
            var ts = stopWatch.Elapsed;

            Console.WriteLine("RunTime " + ts);


            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}