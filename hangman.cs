using System;
using System.Linq; // necessary for String.Contains
using System.Collections.Generic; // Necessary to create List<char>
using System.Threading; // used for thread.sleep

namespace HangmanApplication {
    class Hangman2 {
        static void Greeting () {
            // Greeting printed at the beginning of the game
            Console.Clear ();
            Console.WriteLine ();
            Console.WriteLine ("===================================================");
            Console.WriteLine ("================Welcome to Hangman!================");
            Console.WriteLine ("===================================================");
            Console.WriteLine ("Press any key to begin.");
            // Wait for user input to begin the game
            Console.ReadKey ();
            Console.Clear ();
        }
        static bool Goodbye (bool game_won, string target) {
            // check if Player2 guessed the word
            if (game_won) {
                Console.WriteLine ("Player2 won!!!  The word was {0}", target);
            } else {
                Console.WriteLine ("Player1 won!!!  The word was {0}", target);
            }
            Console.WriteLine ();

            /* check if user wishes to play again
               yes = play again (main while loop)
               no  = exit and print goodbye message */
            Console.Write ("Play again? [Yes] or [No] ");
            string answer = Console.ReadLine ().Trim ().ToLower ();
            bool flag;
            if (answer.FirstOrDefault () == 'y') {
                flag = true;
                Console.Clear ();
            } else {
                flag = false;
                // ending message
                Console.WriteLine ();
                Console.WriteLine ("===================================================");
                Console.WriteLine ("================Thanks for playing!================");
                Console.WriteLine ("===================================================");
                Console.ReadLine ();
            }
            return flag;
        }

        static string ReadTarget (Alphabet alph) {
            // method for reading in Player1's secret word
            // returns string target
            string target = " ";
            bool valid_target = false;

            // TODO: implement a dictionary check

            // checks are described below, while loop is exited once a valid phrase is entered
            while (!valid_target) {
                Console.Write ("Player1, please enter a word or phrase: ");
                target = Console.ReadLine ();

                bool innerBool = true;

                // check if target is empty
                if (target.Trim ().Length == 0) {
                    innerBool = false;
                }
                // check if target contains invalid characters
                else {
                    foreach (var cvalue in target) {
                        if (!alph.alist.Contains (cvalue) && !Char.IsWhiteSpace (cvalue)) {
                            innerBool = false;
                        }
                    }
                }

                if (innerBool) {
                    target = target.Trim ().ToLower ();
                    valid_target = true;
                    Console.WriteLine ("Thanks! Your word has been entered.");
                }
            }
            // Brief delay before screen is cleared
            Thread.Sleep (1000);
            Console.Clear ();
            return target;
        }

        static List<char> InitializeGuess (string target) {
            // method for initializing 'guess.correct'
            // A char list of '0's and '1's
            // returns List<char> correct
            List<char> correct = new List<char> ();

            for (int i = 0; i < target.Length; i++) {
                if (target[i] == ' ') {
                    correct.Add ('1'); // add a '1' for spaces
                } else {
                    correct.Add ('0'); // add a '0' otherwise
                }
            }

            return correct;
        }

        static char ReadGuess (Alphabet alph) {
            // method for reading Player2's guessed character
            char char_guess = ' ';
            bool valid_guess = false;
            Console.Write ("Player1 guess a letter:");
            string guess = Console.ReadLine ();
            // To-Do; enter routine to trim spaces
            guess = guess.Trim ();
            if (guess == "exit") {
                System.Environment.Exit (1);
            } else {
                while (!valid_guess) {
                    guess = guess.ToLower ();
                    if ((guess.Length == 0) || !alph.alist.Contains (guess.FirstOrDefault ())) {
                        Console.Write ("Player1 guess a letter:");
                        guess = Console.ReadLine ();
                    } else {
                        char_guess = guess[0];
                        valid_guess = true;
                    }
                }
            }
            return char_guess;
        }

        static string ListToString (List<char> list_in) {
            // simple method for converting character list to string
            string string_out = "";
            foreach (char cvalue in list_in) {
                string_out += cvalue.ToString ();
            }
            return string_out;
        }

        public class Alphabet {
            // creates a character list of the entire alphabet
            public List<char> alist = new List<char> ();

            public void Build () {
                for (char c = 'a'; c <= 'z'; c++) {
                    alist.Add (c);
                }
            }
        }

        public class GuessClass {
            // class which collects necessary items for reading player2's input
            // int num = number of wrong guesses made
            // List<char> correct = list of 1's and 0's corresponding to characters which have and have not been guessed correctly
            // List<char> full = full list of all guesses which have been made
            // bool won = true if player has won (i.e. correct contains no 0's); and false if otherwise
            public int num;
            public List<char> correct;

            public List<char> full = new List<char> ();
            public bool won;
            public void getValues (int n, List<char> c, bool w) {
                // method for initializing a guess class; full not initialized with the rest as it is build over time.
                num = n;
                correct = c;
                won = w;
            }
            public bool GuessDone () {
                // method for seeing if there are any zeros in guess.correct
                // returns bool no_zero
                bool no_zero = true;
                foreach (char cvalue in correct) {
                    if (cvalue == '0') {
                        no_zero = false;
                        break;
                    }
                }
                return no_zero;
            }
        }
        static void Display (GuessClass guess, string target, int max_num) {
            // method for displaying letter's guessed (guess.full); number of wrong guesses (guess.num);
            // adds either underscores or letter (if the letter has been guessed)

            Console.WriteLine ("Letters already guessed: {0}   {1}/{2}", ListToString (guess.full), guess.num, max_num);
            Console.WriteLine ();

            for (int i = 0; i < target.Length; i++) {
                // add space between underscores for clarity
                Console.Write ("  ");
                if (guess.correct[i] == '0') {
                    Console.Write ('_');
                } else //  (guess.correct[i] == '1')
                {
                    Console.Write (target[i]);
                }
            }
            Console.WriteLine ();
            bool print_num = true;
            if (print_num) {
                int i = 0;
                foreach (char cvalue in target) {
                    if (cvalue != ' ') {
                        // more space for single digit numbering
                        if (i < 9) {
                            Console.Write ("  ");
                        } else {
                            Console.Write (" ");
                        }
                        Console.Write (i + 1);
                        i++;
                    }
                    // space for case where there is a space in phrase
                    else {
                        Console.Write ("   ");
                    }
                }

            }
            Console.WriteLine ();
        }
        static void CheckGuess (char char_guess, GuessClass guess, string target) {
            // 1. check if guessed character (char_guess) has already been guessed (is in guess.full)
            // 2. add guessed character to list of guessed characters (add char_guess to guess.full)
            // 3. check if guessed character is in secret word (target)
            // 4.     true: for loop; where char_guess = target[i] ; set full_guess[i] = '1'
            // 5.     false: increment number of wrong guesses (guess.num++)

            if (guess.full.Contains (char_guess)) {
                Console.WriteLine ("'{0}' has already been guessed", char_guess);
            } else {
                guess.full.Add (char_guess);
                if (target.Contains (char_guess)) {
                    Console.WriteLine ("Good Guess!");
                    for (int i = 0; i < target.Length; i++) {
                        if (target[i] == char_guess) // inser '1's in the appropriate places
                        {
                            guess.correct[i] = '1';
                        }
                    }
                } else // guess is wrong
                {
                    guess.num++;
                }
            }
        }

        static void DrawMan (int num_guesses) {
            // method for drawing the hang man
            // To-Do; define head, body and lengths as string arrays
            // simplify printing over multiple lines?
            string[] head = { "|", "|     O" };
            string[] tors = { "|", "|     |", "|    -|", "|    -|-", "|   >-|-", "|   >-|-<" };
            string[] legs = { "|", "|    /", "|    / \\", "|   _/ \\", "|   _/ \\_" };
            string drawFormat = "{0}\n{1}\n{2}";
            Console.WriteLine ("_______");
            switch (num_guesses) {
                case 0:
                    Console.WriteLine (drawFormat, head[0], tors[0], legs[0]);
                    break;
                case 1:
                    Console.WriteLine (drawFormat, head[1], tors[0], legs[0]);
                    break;
                case 2:
                    Console.WriteLine (drawFormat, head[1], tors[1], legs[0]);
                    break;
                case 3:
                    Console.WriteLine (drawFormat, head[1], tors[1], legs[1]);
                    break;
                case 4:
                    Console.WriteLine (drawFormat, head[1], tors[1], legs[2]);
                    break;
                case 5:
                    Console.WriteLine (drawFormat, head[1], tors[2], legs[2]);
                    break;
                case 6:
                    Console.WriteLine (drawFormat, head[1], tors[3], legs[2]);
                    break;
                case 7:
                    Console.WriteLine (drawFormat, head[1], tors[3], legs[3]);
                    break;
                case 8:
                    Console.WriteLine (drawFormat, head[1], tors[3], legs[4]);
                    break;
                case 9:
                    Console.WriteLine (drawFormat, head[1], tors[4], legs[4]);
                    break;
                case 10:
                    Console.WriteLine (drawFormat, head[1], tors[5], legs[4]);
                    break;
            }
            Console.WriteLine ("|");
        }

        static int Difficulty () {
            // method for setting the difficulty of the game
            Console.WriteLine ("Easy(10), Medium(8), or Hard(6)?");
            Console.Write ("Please enter a difficulty for the game:");
            string read_diff = Console.ReadLine ();
            read_diff = read_diff.ToLower ().Trim ();
            int max_num;
            string diff_read;

            // switch case for text entry (number entry handled separately below)
            // check the first letter only for relaxed input 
            switch (read_diff.FirstOrDefault ()) {
                case 'e':
                    max_num = 10;
                    diff_read = "Easy";
                    break;
                case 'm':
                    max_num = 8;
                    diff_read = "Medium";
                    break;
                case 'h':
                    max_num = 6;
                    diff_read = "Hard";
                    break;
                default: //hard is default
                    max_num = 6;
                    diff_read = "Hard";
                    break;
            }

            // switch case for number entry
            switch (read_diff) {
                case "10":
                    max_num = 10;
                    diff_read = "Easy";
                    break;
                case "8":
                    max_num = 8;
                    diff_read = "Medium";
                    break;
            }

            Console.WriteLine ("Difficulty set to {0}, Player2 will have {1} tries", diff_read, max_num);
            Thread.Sleep (1000);
            Console.Clear ();
            return max_num;
        }

        static void Main () {
            Alphabet alph = new Alphabet ();
            alph.Build ();

            Greeting ();

            bool playAgain = true;
            while (playAgain) {
                string target = ReadTarget (alph);

                GuessClass guess = new GuessClass ();
                guess.getValues (0, InitializeGuess (target), false);

                bool game_won = false;
                char char_guess;
                int max_num = Difficulty ();

                while (!game_won && guess.num < max_num) {
                    Display (guess, target, max_num); //need to add number of guesses
                    DrawMan (guess.num);
                    char_guess = ReadGuess (alph); // read user input
                    CheckGuess (char_guess, guess, target);
                    game_won = guess.GuessDone ();
                    Console.Clear ();

                }

                Display (guess, target, max_num);
                DrawMan (guess.num);
                playAgain = Goodbye (game_won, target);
            }

        }
    }
}