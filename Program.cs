﻿using System;
using System.IO;

namespace studentlist1
{
    class Program
    {
        // The Main method 
        static void Main(string[] args)
        {
            /* Check arguments */
            if (args == null || args.Length != 1)
            {
                ShowUsage();
                return; //Exit Early.
            }

            var fileContents = LoadData(Constants.StudentList);

            if (args[0] == Constants.ShowAll) 
            {
                var words = fileContents.Split(',');
                foreach(var word in words) 
                {
                    Console.WriteLine(word);
                }
            }
            else if (args[0]== Constants.ShowRandom)
            {

                // We are loading data
                var words = fileContents.Split(Constants.StudentEntryDelimiter);
                var rand = new Random();
                var randomIndex = rand.Next(0,words.Length);
                Console.WriteLine(words[randomIndex]);
            }
            else if (args[0].Contains(Constants.AddEntry))
            {
                // read
                var argValue = args[0].Substring(1);

                // Write
                // But we're in trouble if there are ever duplicates entered
                UpdateContent(fileContents + Constants.StudentEntryDelimiter + argValue, "students.txt");
            }
            else if (args[0].Contains(Constants.FindEntry))
            {
                var words = fileContents.Split(Constants.StudentEntryDelimiter);
                var argValue = args[0].Substring(1);
                var indexLocation = -1;
                for (int idx = 0; idx < words.Length; idx++)
                {
                    if (words[idx] == argValue)
                    {
                        indexLocation = idx;
                        break;
                    }
                }
                if (indexLocation>= 0)
                {
                    Console.WriteLine($"Entry '{argValue}' found at index {indexLocation}");
                }
                else
                {
                    Console.WriteLine($"Entry '{argValue}' does not exist.");
                }
            }
            else if (args[0].Contains(Constants.ShowCount))
            {
                var words = fileContents.Split(Constants.StudentEntryDelimiter);
                Console.WriteLine(String.Format(" {0} words found", words.Length));
            }
            else
            {
                ShowUsage();
            }
            
        }

        //Read data from the given file
        static string LoadData(string fileName)
        {

            //The 'using' construct does the heavy lifting of flushing a stream
            //amd releasing system resources the stream was using.
            using (var fileStream = new FileStream(fileName,FileMode.Open))
            using (var reader = new StreamReader(fileStream))
            {
                //The format of our student list is that it is two lines
                //The first line is a coma-separated list of students
                //The second line is a timestamp.
                //Let us just retrieve the first line, which is a student name
                return reader.ReadLine();
            }   
        }
        //Writes the given string of data to the file with the given filename.
        //This method also adds a timestamp to the end of the file.
        static void UpdateContent(string content, string fileName)
        {
            var timestamp = String.Format("List last updated on {0}",DateTime.Now);

            //The 'using' construct does the heavy lifting of flushing a stream
            //amd releasing system resources the stream was using.
            using (var fileStream = new FileStream(fileName,FileMode.Open))
            using (var writer = new StreamWriter(fileStream))
            {
                writer.WriteLine(content);
                writer.WriteLine(timestamp);
            }
        }
        static void ShowUsage()
        {
             Console.WriteLine("Usage: dotnet dev275x.rollcall.dll (a | r | c | +WORD | WORD?)");
        }
    }
}
