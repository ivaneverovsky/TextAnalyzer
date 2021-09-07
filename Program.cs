using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextAnalyzer
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            string path;
            string testText;

            bool bull = false;

            List<string> text = new List<string>(); //store user's or test text
            List<string> words = new List<string>(); //store words from text
            List<string> triplets = new List<string>(); //store triplets
            Dictionary<string, int> results = new Dictionary<string, int>(); //store results
            Dictionary<string, int> best10 = new Dictionary<string, int>(); // store best 10

            Stopwatch timer = new Stopwatch();

            do
            {
                Console.Write("Welcome to TxT-Analyzer v 1.0\n\n\nAre you going to analyze your txt file (Y / N) ?\n");
                Console.Write("Your answer: ");
                string answer = Console.ReadLine();
                if (answer.ToUpper() == "Y")
                {
                    Console.Write("\nEnter path to your file (in format {diskname}:\\...\\{filename}.txt)\nAs an example - C:\\Users\\{username}\\Desktop\\{filename}.txt: ");
                    //path = @"C:\Users\SS\Desktop\test.txt";
                    path = Console.ReadLine();
                    Console.Beep();
                    timer.Start();

                    try
                    {
                        using (StreamReader sr = new StreamReader(path, Encoding.Default))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                                text.Add(line);
                        }
                        bull = true;
                    }
                    catch
                    {
                        Console.Write("Could not find file :(\n\nPress any key...");
                        timer.Reset();
                        Console.ReadLine();
                        Console.Beep();
                        Console.Clear();
                    }
                }
                else if (answer.ToUpper() == "N")
                {
                    Console.Beep();
                    //try this text to see results in more "friendly" format
                    testText = "one kek kok kok kak tuk tuk tuk fuk fuk fuk fuk cup cup cup cup cup gag gag gag gag gag gag sev sev sev sev sev sev sev egh egh egh egh egh egh egh egh nin nin nin nin nin nin nin nin nin ten ten ten ten ten ten ten ten ten ten elv elv elv elv elv elv elv elv elv elv elv";
                    //testText = "Необходимо написать консольное приложение на C#, выполняющее частотный анализ текста. Входные параметры: путь к текстовому файлу. Выходные результаты: вывести на экран через запятую 10 самых часто встречающихся в тексте триплетов(3 идущих подряд буквы слова), и на следующей строке время работы программы в миллисекундах. Требования: программа должна обрабатывать текст в многопоточном режиме. Оцениваться будет правильность решения, качество кода и быстродействие программы.";
                    timer.Start();
                    text.Add(testText);
                    bull = true;
                }
                else
                {
                    Console.Write("Wrong input! Try again!\n\nPress any key...");
                    Console.ReadLine();
                    Console.Beep();
                    Console.Clear();
                }
            } while (!bull);

            words = WordCreator(text); //call method to get words from text
            triplets = WordAnalyzer(words); //call method to analyze triplets in words
            results = TripletsCounter(triplets); //call method to count triplets
            best10 = ShowBest10(results); //call method to show best 10

            timer.Stop();

            Console.WriteLine("\n");
            int i = 1;
            foreach (var best in best10)
                Console.WriteLine("{0}. Triplet {1} repeats {2} times", i++, best.Key, best.Value);

            Console.WriteLine("\n{0} ms", timer.ElapsedMilliseconds);
            Console.WriteLine("\n\nP.S. In some cases program will show more then 10 results. \nIt happens when we got different triplets repeated equal times.");
            Console.ReadLine();
        }

        //remove punctuation, find words, write them in Words list
        static  List<string> WordCreator(List<string> text)
        {
            List<string> words = new List<string>(); //store words from text

            string empty = "";
            string newLine;

            foreach (var line in text)
            {
                //remove punctuation
                newLine = line;
                for (int i = 0; i < newLine.Length; i++)
                    if (char.IsPunctuation(newLine[i]) || newLine[i].ToString() == "\t" || char.IsDigit(newLine[i]))
                        newLine = newLine.Replace(newLine[i].ToString(), empty);

                //remove double+ white spaces
                newLine = new Regex(@"\s+").Replace(newLine, " ");

                //create words, add them to list
                string[] word = newLine.Split(new char[] { ' ' });
                for (int i = 0; i < word.Length; i++)
                    if (word[i] != "")
                        words.Add(word[i].ToLower());
            }
            return words;
        }

        //create tripplets, write them down
        static List<string> WordAnalyzer(List<string> words)
        {
            List<string> triplets = new List<string>(); //store triplets

            foreach (var word in words)
                if (word.Length >= 3)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (word.Length == i + 2)
                            break;
                        string trip = word[i].ToString() + word[i + 1].ToString() + word[i + 2].ToString();
                        triplets.Add(trip);
                    }
            return triplets;
        }

        //count triplets
        static Dictionary<string, int> TripletsCounter(List<string> triplets)
        {
            Dictionary<string, int> results = new Dictionary<string, int>();
            int counter = 0;
            foreach (var triplet in triplets)
                if (!results.ContainsKey(triplet))
                {
                    for (int i = 0; i < triplets.Count; i++)
                        if (triplet == triplets[i])
                            counter++;
                    results.Add(triplet, counter);
                    counter = 0;
                }
            return results;
        }

        //find best 10 and show them
        static Dictionary<string, int> ShowBest10(Dictionary<string, int> results)
        {
            Dictionary<string, int> best10 = new Dictionary<string, int>();

            do
            {
                int max = 0;
                string triplet;

                foreach (var result in results)
                    if (result.Value > max)
                        max = result.Value;

                foreach (var result in results)
                    if (max == result.Value)
                    {
                        best10.Add(result.Key, result.Value);
                        triplet = result.Key;
                        results.Remove(triplet); //pay attention while debugging this program, it removes results from "result" list!
                    }
            } while (best10.Count <= 9);
           
            return best10;
        }
    }
}

