namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public class LetterStats
    {
        /// <summary>
        /// Буква/Пара букв для учёта статистики.
        /// </summary>
        public string Letter { get; set; }

        /// <summary>
        /// Кол-во вхождений буквы/пары.
        /// </summary>
        public int CountChar { get; set; }

        public CharType SimvolType;

        public LetterStats(string letter, int countChar)
        {
            Letter = letter;
            CountChar = countChar;
            DifineSimvolType(letter);
        }

        private void DifineSimvolType(string simvol)
        {
            char[] first;
            string vowels = "аеёиоуыэюяeyioa";
            first = simvol.ToCharArray();
            if (vowels.Contains(first[0].ToString().ToLowerInvariant()))
                SimvolType = CharType.Vowel;
            else
                SimvolType = CharType.Consonants;
        }
    }
}
