using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static System.Console;


namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            int wordMaxLength = 10, wordMinLength = 4, numberOfGuesses = 10;
            int langSelect = -1;
            Dictionary<string, string[]> Local = new Dictionary<string, string[]>(LocalizationReadFile(System.Environment.CurrentDirectory + "\\local.txt"));
            do
            {
                Clear();
                WriteLine("Choose languge\n1.English\n2.Svenska");
                Int32.TryParse(ReadKey().KeyChar.ToString(), out langSelect);
                switch (langSelect)
                {
                    case 1:
                        langSelect--;
                        break;
                    case 2:
                        langSelect--;
                        break;
                    default:
                        langSelect = -1;
                        break;

                }

            } while (langSelect < 0);
            string filePath = System.Environment.CurrentDirectory + Local["FilePath"][langSelect];
            char guessedLetter;
            List<string> wordList = new List<string>();
            wordList = ReturnAsList(filePath, wordMinLength, wordMaxLength);
            char[] secretWord = ChooseSecretWord(wordList).ToCharArray();
            StringBuilder guessedLetters = new StringBuilder();
            char[] correctLetters = new char[secretWord.Length];
            int guesses = 0;
            Array.Fill(correctLetters, '_');

            while (guesses < numberOfGuesses && correctLetters.Contains('_'))
            {
                ShowUI(correctLetters, guessedLetters, secretWord, Local, langSelect);
                WriteLine("\n{0} : ", Local["Guess"][langSelect]);
                while (guessedLetters.ToString().Contains(guessedLetter = ReadKey().KeyChar) || !Char.IsLetter(guessedLetter))
                {
                    if (Char.IsLetter(guessedLetter))
                    {
                        WriteLine("\n{0} \n{1}", Local["Err1"][langSelect], Local["AnyKey"][langSelect]);
                    }
                    else
                    {
                        WriteLine(Local["Err2"][langSelect]);
                        WriteLine(Local["AnyKey"][langSelect]);
                    }
                    ReadKey();
                    ShowUI(correctLetters, guessedLetters, secretWord, Local, langSelect);
                    WriteLine("\n{0} : ", Local["Guess"][langSelect]);
                }

                guessedLetters.Append((guesses == 0 ? null : ",") + guessedLetter);
                for (int i = 0; i < secretWord.Length; i++)
                {
                    if (secretWord[i].Equals(guessedLetter)) correctLetters[i] = guessedLetter;
                }
                if (!correctLetters.Contains('_'))
                {
                    ShowUI(correctLetters, guessedLetters, secretWord, Local, langSelect);
                    Win();

                }
                guesses++;
            }



        }

        static void ShowUI(char[] correctGuesses, StringBuilder guessedLetters, char[] secretWord, Dictionary<string, string[]> Local, int langSelect)
        {
            Clear();
            WriteLine("\t\t{0}\n", Local["Title"][langSelect]);
            Write("{0} : ", Local["Secret"][langSelect]);//"Secret word : ");
            foreach (char c in correctGuesses)
            {
                Write(c + " ");
            }
            Write("\n{0} : ", Local["Guessed"][langSelect]);

            Write(guessedLetters);

        }

        static string ChooseSecretWord(List<string> listOfWords)
        {
            var rand = new Random(Environment.TickCount);
            return listOfWords[rand.Next(0, listOfWords.Count - 1)];
        }

        public static List<string> ReturnAsList(string filePath, int minLength, int maxLength)
        {
            List<string> returnList = new List<string>();
            using (var sr = new StreamReader(filePath))
            {
                //Create a list where all lines are of a specified length,contain only letters and are not all caps
                foreach (string line in File.ReadLines(filePath).Where(x => x.Length >= minLength && x.Length <= maxLength && x.All(char.IsLetter) && !x.All(char.IsUpper)))
                {
                    returnList.Add(line);
                }
            }
            return returnList;
        }

        public static void Win()
        {

            WriteLine("\nGood job! You guess correctly\nPress any button to continue");
            ReadKey();

        }

        public static Dictionary<string, string[]> LocalizationReadFile(string filePath)
        {
            Dictionary<string, string[]> returnDict = new Dictionary<string, string[]>();
            string[] theReadLine = new string[2];
            using (var sr = new StreamReader(filePath))
            {
                foreach (string line in File.ReadLines(filePath))
                {
                    theReadLine = line.Split(':');
                    returnDict.Add(theReadLine[0], theReadLine[1].Split(','));
                }
            }
            return returnDict;
        }
    }
}
