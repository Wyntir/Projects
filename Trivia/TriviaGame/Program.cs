using Core;
using System;
using System.Collections.Generic;
using System.IO;
using Json.Net;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace TriviaGame
{
    class Question
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> IncorrectAnswers { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game("C:\\repos\\Trivia\\dev-interns-dotnet-continued2022\\Core\\Files\\Questions.json");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
 ______  ______   __   __   __ __   ______   __   __   __   ______   __  __   ______  
/\__  _\/\  == \ /\ \ /\ \ / //\ \ /\  __ \ /\  -.\ \ /\ \ /\  ___\ /\ \_\ \ /\__  _\ 
\/_/\ \/\ \  __< \ \ \\ \ \'/ \ \ \\ \  __ \\ \ \-.  \\ \ \\ \ \__ \\ \  __ \\/_/\ \/ 
   \ \_\ \ \_\ \_\\ \_\\ \__|  \ \_\\ \_\ \_\\ \_\\ \_\\ \_\\ \_____\\ \_\ \_\  \ \_\ 
    \/_/  \/_/ /_/ \/_/ \/_/    \/_/ \/_/\/_/ \/_/ \/_/ \/_/ \/_____/ \/_/\/_/   \/_/ 
                       ");
            bool quit = false;
            while (!quit)
            {
                Console.WriteLine("┌───────────────────────────────────┐");
                Console.WriteLine("│     Welcome to Trivia Night       │");
                Console.WriteLine("│        Select an option:          │");
                Console.WriteLine("└───────────────────────────────────┘");
                Console.WriteLine("┌───────────────────────────────────┐");
                Console.WriteLine("│        1. Play Trivia             │");
                Console.WriteLine("│        2. Add a Question          │");
                Console.WriteLine("│        3. View JSON Data          │");
                Console.WriteLine("│        4. Quit                    │");
                Console.WriteLine("└───────────────────────────────────┘");
                Console.ForegroundColor = ConsoleColor.White;


                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PlayTrivia(game);
                        break;
                    case "2":
                        AddQuestion(game);
                        break;
                    case "3":
                        ViewQuestions(game);
                        break;
                    case "4":
                        Console.WriteLine("Thank you for playing Trivia. Goodbye!");
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        static void ViewQuestions(Game game)
        {
            string filePath = "C:\\repos\\Trivia\\dev-interns-dotnet-continued2022\\Core\\Files\\Questions.json";
            try
            {
                string fileContent = File.ReadAllText(filePath);
                Console.WriteLine(fileContent);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading the file.");
            }
        }
        static void PlayTrivia(Game game)
        {
            int score = 0;
            int size = 0;
            while (game.HasMoreQuestions())
            {
                Console.WriteLine(game.AskQuestion());
                var answers = game.GetAnswersList();
                int count = 1;
                foreach (string answer in answers)
                {
                    Console.WriteLine("({0}){1}",count,answer);
                    count++;
                }
                var userAnswer = Console.ReadLine();
                var isCorrect = game.CheckAnswer(userAnswer);

                if (isCorrect)
                {
                    score++;
                    size++;
                    Console.WriteLine("You Got It!!!");
                }
                else
                {
                    size++;
                    Console.WriteLine("NOPE.");
                }
            }
            double percentage = (double)score / size * 100;
            Console.WriteLine("Game Has Ended! You Scored {0}/{1} which is {2}%", score, size, percentage);
            Console.WriteLine("The Questions You Got Wrong Are Below:");
            int j = 0;
            game.SaveWrongsToJson();
            while (game.HasMoreWrongs(j))
            {
                Console.WriteLine(game.RetWrong(j));
                j++;
            }
        }
        static void AddQuestion(Game game)
        {
            game.PromptAddQuestion();
        }
    }
}

