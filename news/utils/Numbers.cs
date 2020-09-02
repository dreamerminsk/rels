using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news.utils
{
    public static class Numbers
    {
        public static List<int> ExtractNumbers(string text)
        {
            List<int> numbers = new List<int>();
            string number = "0";
            foreach (char letter in text)
            {
                if (Char.IsDigit(letter))
                {
                    number += letter;
                }
                else
                {
                    numbers.Add(int.Parse(number));
                    number = "0";
                }
            }
            numbers.Add(int.Parse(number));
            return numbers;
        }
    }
}
