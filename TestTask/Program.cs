using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TestTask
{
    public class Program
    {

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        static void Main(string[] args)
        {            
            try
            {
                Console.WriteLine("Введите путь до первого файла: ");
                IReadOnlyStream inputStream1 = GetInputStream(InputPathFile());
                Console.WriteLine("Введите путь до второго файла: ");
                IReadOnlyStream inputStream2 = GetInputStream(InputPathFile());

                IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
                IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);

                Console.WriteLine("Удаление из одиночной статистки.");
                RemoveCharStatsByType(ref singleLetterStats);
                Console.WriteLine("Удаление из статистики парных букв.");
                RemoveCharStatsByType(ref doubleLetterStats);

                Console.WriteLine("Количество вхождений каждой буквы(регистрозависимо): ");
                PrintStatistic(singleLetterStats);   
                Console.WriteLine("Количество вхождений каждой пары букв(не регистрозависимо): ");
                PrintStatistic(doubleLetterStats);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadLine();
            }
            // TODO : Необжодимо дождаться нажатия клавиши, прежде чем завершать выполнение программы.            
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            return new ReadOnlyStream(fileFullPath);
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - регистрозависимый.
            List<LetterStats> singleCharStats = new List<LetterStats>();
            string letter;
            string simvols = "!@#$?|/.,()%^&*\\";
            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                letter = stream.ReadNextChar().ToString();
                if (!string.IsNullOrWhiteSpace(letter) && !simvols.Contains(letter))
                {
                    LetterStats letterStats = singleCharStats.FirstOrDefault(l => l.Letter == letter);
                    if (letterStats == null)
                        singleCharStats.Add(new LetterStats(letter, 1));
                    else
                        IncStatistic(letterStats);             
                }              
            }
            stream.Dispose();
            return singleCharStats;            
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - НЕ регистрозависимый.
            string firstChar, secondChar;
            string simvols = "\\!@#$?|/.,()%^&*";
            List<LetterStats> coupleCharStats = new List<LetterStats>();             
            stream.ResetPositionToStart();
            firstChar = stream.ReadNextChar().ToString();
            while (!stream.IsEof)
            {
                secondChar = stream.ReadNextChar().ToString();
                if (!string.IsNullOrWhiteSpace(firstChar) && !string.IsNullOrWhiteSpace(secondChar) && !simvols.Contains(firstChar))
                {
                    if (firstChar.ToLower() == secondChar.ToLower())
                    {
                        var letter = $"{firstChar}{secondChar}";
                        LetterStats letterStats = coupleCharStats.FirstOrDefault(l => l.Letter.ToLower() == letter.ToLower());

                        if (letterStats == null)
                            coupleCharStats.Add(new LetterStats(letter.ToUpper(), 1));
                        else
                            IncStatistic(letterStats);
                       
                        firstChar = stream.ReadNextChar().ToString();
                        continue;                                                  
                    }
                    firstChar = secondChar;
                }
                else
                    firstChar = secondChar;                
            }
            stream.Dispose();
            return coupleCharStats;            
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static void RemoveCharStatsByType(ref IList<LetterStats> letters)
        {
            // TODO : Удалить статистику по запрошенному типу букв.
            bool flag = true;            
            while(flag)
            {  
                Console.WriteLine($"1. Удалить согласные из списка статистики. \n2. Удалить гласные из спика статистики.\n3. Оставить статистику без изменений.\n");       
                int.TryParse(Console.ReadLine(), out int result);
                switch(result)
                {
                    case 1:
                       letters = letters?.Where(x => x.SimvolType != CharType.Consonants).ToList();
                       flag = false;
                       break;
                    case 2:
                       letters = letters?.Where(x => x.SimvolType != CharType.Vowel).ToList();
                       flag = false;
                       break;
                    case 3:
                       flag = false;
                       break;
                    default:
                       Console.WriteLine("Не корректный ввод \n");
                       break;
                }
               
            }
        }

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {            
            // TODO : Выводить на экран статистику. Выводить предварительно отсортировав по алфавиту!
            var letterStats = letters.OrderBy(x => x.Letter);
            foreach(var item in letterStats)
            {
                Console.WriteLine($"[{item.Letter}] = {item.CountChar} .");
            }                        
        }        

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(LetterStats letterStats) => letterStats.CountChar++;        

        private static string InputPathFile()
        {
            string pathFile =null;
            var flag = true;           
            while(flag)
            {
                try
                {
                    pathFile = Console.ReadLine();
                    var fileInfo = new FileInfo(pathFile);

                    if (fileInfo.Exists)                    
                        flag = false;                    
                    else                    
                        Console.WriteLine("Путь к файлу не найден.");                        
                    
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return pathFile;                       
        }
    }
}
    
