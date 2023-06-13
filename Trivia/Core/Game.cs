using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core
{
    public class Game
    {
        private List<Question> Questions { get; set; }
        private int CurrentIndex { get; set; }
        private List<Question> WrongQuestions { get; set; }
 
        public Game(String filePath)
        {
            CurrentIndex = 0;
            this.Questions = new List<Question>(); // Initialize the Questions list
            InstantiateQuestions(filePath);
            this.WrongQuestions = new List<Question>();
        }

        public bool HasMoreQuestions()
        {
            return CurrentIndex < Questions.Count;
        }
        public bool HasMoreWrongs(int j)
        {
            return j < WrongQuestions.Count;
        }

        public string AskQuestion()
        {
            return Questions[CurrentIndex].QuestionText;
        }

        public string RetWrong(int i)
        {
            return WrongQuestions[i].QuestionText;
        }

        public IEnumerable<string> GetAnswersList()
        {
            List<string> answers = Questions[CurrentIndex].IncorrectAnswers;
            answers.Add(Questions[CurrentIndex].CorrectAnswer);
            return ExtensionMethods.GetShuffledAnswers(answers);
        }

        public bool CheckAnswer(string answer)
        {
                var isCorrect = Questions[CurrentIndex].CheckAnswer(answer);
                if(!isCorrect)
                {
                    WrongQuestions.Add(Questions[CurrentIndex]);
                }
                CurrentIndex++;
                return isCorrect;
        }

        private void AddQuestion(Question q)
        {
            Questions.Add(q);
        }


        public void PromptAddQuestion()
        {
            Console.WriteLine("Enter the question:");
            string questionText = Console.ReadLine();

            Console.WriteLine("Enter the correct answer:");
            string correctAnswer = Console.ReadLine();

            List<string> incorrectAnswers = new List<string>();
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"Enter incorrect answer {i}:");
                string incorrectAnswer = Console.ReadLine();
                incorrectAnswers.Add(incorrectAnswer);
            }
            Question newQuestion = new Question(questionText, correctAnswer, incorrectAnswers, false);
            AddQuestion(newQuestion);
            SaveQuestionsToJson(Questions,"C:\\repos\\Trivia\\dev-interns-dotnet-continued2022\\Core\\Files\\Questions.json");
            Console.WriteLine("Question added successfully.");
        }

        public void SaveWrongsToJson()
        {
            SaveQuestionsToJson(WrongQuestions, "C:\\repos\\Trivia\\dev-interns-dotnet-continued2022\\Core\\Files\\WrongQuestions.json");
        }
        private void SaveQuestionsToJson(List<Question> questionList, string filePath)
        {
            string json = JsonConvert.SerializeObject(questionList, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void InstantiateQuestions(String filePath)
        {
            List<Question> questionList = new List<Question>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string fileContent = sr.ReadToEnd();
                    Questions = JsonConvert.DeserializeObject<List<Question>>(fileContent);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading the file.");
            }
            if (Questions == null)
            {
                questionList = new List<Question>();
            }
            foreach (Question i in questionList)
            {
                AddQuestion(i);
            }
        }
    }
}
