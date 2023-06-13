using System;
using System.Collections.Generic;

namespace Core
{
    public class Question
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> IncorrectAnswers { get; set; }

        public Question(string qText, string correctAnswer, List<string> incorrectAnswers, bool wronglyAnswered)
        {
            QuestionText = qText;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = incorrectAnswers;
        }

        public bool CheckAnswer(string answer)
        {
            return answer.Equals(CorrectAnswer);
        }
    }
}
