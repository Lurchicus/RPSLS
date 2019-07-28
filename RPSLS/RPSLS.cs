using System;

namespace RPSLS
{
    /// <summary>
    /// A console version of the game Rock, Paper, Scissors, Lizard, Spock as invented by Sam Kass and Karen Bryla
    /// </summary>
    class RPSLS
    {
        // Player and Computer alias
        public static string[] Alias = { "Rock", "Paper", "Scissors", "Lizard", "Spock" };

        // Extended rules
        // Spock smashes scissors and vaporizes rock; 
        // Spock is poisoned by lizard and disproven by paper. 
        // Lizard poisons Spock and eats paper; 
        // Lizard is crushed by rock and decapitated by scissors.

        public static int[] VerbMap = {
        //  Rock Paper Scissors Lizard Spock 
            0,  -1,    1,       1,    -1, // Rock
            1,   0,   -1,      -1,     1, // Paper
           -1,   1,    0,       1,    -1, // Scissors
           -1,   1,   -1,       0,     1, // Lizard
            1,  -1,    1,      -1,     0  // Spock
        }; // Win(1), lose(-1) or draw(0) map

        public static string[] VerbList = {
            // Rock             Paper              Scissors             Lizard            Spock 
              "matches",       "is covered by",   "smashes",           "crushes",        "is vaporized by", // Rock 
              "covers",        "matches",         "is cut by",         "is eaten by",    "disproves",       // Paper
              "are broken by", "cuts",            "matches",           "decapatate",     "is smashed by",   // Scissors
              "is crushed by", "eats",            "is decapitated by", "matches",        "poisons",         // Lizard
              "vaporizes",     "is disproved by", "smashes",           "is poisoned by", "matches"          // Spock
        };

        static void Main(string[] args)
        {
            bool Debug = false; // Turn inline debugging on or off
            int PlayerScore = 0; // Player score
            int ComputerScore = 0; // Computer score
            string Result = ""; // Result string

            Random Rando = new Random(); // RNG for the computer

            Console.WriteLine("RPSLS (input Rock, Paper, Scissors, Lizard or Spock) v1.0!");
            Console.Write("RPSLS>");

            string Input = Console.ReadLine();
            while (Input != null)
            {
                int Computer = Rando.Next(4);
                int Player = ParseInput(Input);
                if (Debug) {
                    Console.WriteLine("Player:{0} Computer:{1}", Player, Computer);
                }

                if (Player == -2) { Input = null; break; }
                if (Player == -1)
                {
                    Console.WriteLine("I don't understand that, enter "+
                                      "Rock, Paper, Scissors, Lizard or Spock " +
                                      "or enter nothing to exit.\n");
                }
                else
                {
                    // Determine the index into the verb list
                    int Verbs = ((Player) * 5) + Computer;
                    if (Debug) { Console.WriteLine("Verbs:{0}", Verbs); }

                    switch (VerbMap[Verbs])
                    {
                        case -1:
                            Result = "Player loses to Computer!";
                            ComputerScore++;
                            break;
                        case 1:
                            Result = "Player wins over Computer!";
                            PlayerScore++;
                            break;
                        case 0:
                        default:
                            Result = "Player ties with Computer!";
                            break;
                    }

                    // Show what happened and the results
                    Console.WriteLine("{0} {1} {2}! {3} Player: {4} Computer: {5}\n", 
                                      Alias[Player], VerbList[Verbs], Alias[Computer], 
                                      Result, PlayerScore, ComputerScore);
                }
                Console.Write("RPSLS>");
                Input = Console.ReadLine();
            }
        }

        static int ParseInput(string Input)
        {
            int Ret = -1;
            for (int Ix = 0; Ix < 5; Ix++)
            {
                string Check = Alias[Ix];
                if (Input.ToLower() == Check.ToLower())
                {
                    Ret = Ix;
                    break;
                }
            }
            if (Ret == -1)
            {
                if (Input.Length > 0)
                {
                    // Huh? Invalid input
                    Ret = -1;
                }
                else
                {
                    // Nothing entered, lets exit
                    Ret = -2;
                }
            }
            return Ret;
        }
    }
}
