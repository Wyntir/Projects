using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public static class ExtensionMethods
    {
        public static IEnumerable<string> GetShuffledAnswers(IEnumerable<string> questions)
        {
            Random rand = new Random();
            var shuffled = questions.OrderBy(_ => rand.Next()).ToList();
            return shuffled;
        }
    }
}
