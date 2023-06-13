using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.PatientSatisfaction.Model
{
    public class Rating
    {
        public string Category { get; set; }
        public int NumberOf1s { get; set; }
        public int NumberOf2s { get; set; }
        public int NumberOf3s { get; set; }
        public int NumberOf4s { get; set; }
        public int NumberOf5s { get; set; }
        public double AverageScore { get; set; }

        public Rating(string category, List<int> numbersOfGrades)
        {
            Category = category;
            NumberOf1s = numbersOfGrades[0];
            NumberOf2s = numbersOfGrades[1];
            NumberOf3s = numbersOfGrades[2];
            NumberOf4s = numbersOfGrades[3];
            NumberOf5s = numbersOfGrades[4];
            AverageScore = CalculateAverageScore();        
        }

        private double CalculateAverageScore()
        {
            double sum = NumberOf1s + NumberOf2s + NumberOf3s + NumberOf4s + NumberOf5s;
            double totalScore = NumberOf1s + 2 * NumberOf2s + 3 * NumberOf3s + 4 * NumberOf4s + 5 * NumberOf5s;
            return Math.Round(totalScore / sum, 4);
        }
    }
}
