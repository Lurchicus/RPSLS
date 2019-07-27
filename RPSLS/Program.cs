using System;

namespace RPSLS
{
    class RPSLS
    {
        public static string[] PickList = { "Rock", "Paper", "Scissors", "Lizard", "Spock" };

        // Spock smashes scissors and vaporizes rock; 
        // Spock is poisoned by lizard and disproven by paper. 
        // Lizard poisons Spock and eats paper; 
        // Lizard is crushed by rock and decapitated by scissors.

        //                              Rock Paper Scissors Lizard Spock 
        public static int[] VerbMap = { 0,  -1,    1,       1,    -1, // Rock
                                        1,   0,   -1,      -1,     1, // Paper
                                       -1,   1,    0,       1,    -1, // Scissors
                                       -1,   1,   -1,       0,     1, // Lizard
                                        1,  -1,    1,      -1,     0  // Spock
        }; //                          Win(1), lose(-1) or draw(0) map

        //                                   Rock             Paper              Scissors             Lizard            Spock 
        public static string[] VerbList = { "matches",       "is covered by",   "smashes",           "crushes",        "is vaporized by", // Rock 
                                            "covers",        "matches",         "is cut by",         "is eaten by",    "disproves",       // Paper
                                            "are broken by", "cuts",            "matches",           "decapatate",     "is smashed by",   // Scissors
                                            "is crushed by", "eats",            "is decapitated by", "matches",        "poisons",         // Lizard
                                            "vaporizes",     "is disproved by", "smashes",           "is poisoned by", "matches"          // Spock
        };

        static void Main(string[] args)
        {
            int PWin = 0; // Player score
            int CWin = 0; // Computer score
            string SWin = ""; // Result string

            Random Rando = new Random(); // RNG for the computer

            Console.WriteLine("RPSLS (input Rock, Paper, Scissors, Lizard or Spock) v1.0!");
            Console.Write("RPSLS>");

            string Inp = Console.ReadLine();
            while (Inp != null)
            {
                int Computer = Rando.Next(4) + 1;
                int Player = ParseInput(Inp);

                if (Player == 0) { Inp = null; break; }
                if (Player == -1)
                {
                    Console.WriteLine("I don't understand that, enter Rock, Paper, Scissors, Lizard or Spock " +
                                      "or enter nothing to exit.\n");
                }
                else
                {
                    int VerbIndex = ((Player - 1) * 5) + Computer - 1;

                    int Result = VerbMap[VerbIndex];
                    switch (Result)
                    {
                        case -1:
                            SWin = "Player loses!";
                            CWin++;
                            break;
                        case 1:
                            SWin = "Player wins!";
                            PWin++;
                            break;
                        case 0:
                        default:
                            SWin = "Player ties!";
                            break;
                    }

                    Console.WriteLine("{0} {1} {2}! {3} Player: {4} Computer: {5}\n", 
                                      PickList[Player - 1], VerbList[VerbIndex], PickList[Computer - 1], SWin, PWin, CWin);
                }
                Console.Write("RPSLS>");
                Inp = Console.ReadLine();
            }
        }

        static int ParseInput(string Inp)
        {
            int Ret = 0;
            for (int Ix=0; Ix < 5; Ix++)
            {
                string Check = PickList[Ix];
                if (Inp.ToLower() == Check.ToLower()) 
                {
                    Ret = Ix + 1;
                    break;
                }
                if (Inp.Length > 0)
                {
                    // Huh? Invalid input
                    Ret = -1;
                }
                else
                {
                    // Nothing entered, lets exit
                    Ret = 0;
                }
            }
            return Ret;
        }
    }
}
