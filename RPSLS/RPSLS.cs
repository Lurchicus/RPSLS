using System;
using System.Reflection;
using System.IO;

namespace RPSLS
{
    /// <summary>
    /// A console version of the game Rock, Paper, Scissors, Lizard, Spock as 
    /// invented by Sam Kass and Karen Bryla
    /// </summary>
    public class RPSLS
    {
        // Player and Computer alias for each round
        public static readonly string[] Alias = ["Rock", "Paper", "Scissors", "Lizard", "Spock"];

        // Extended rules
        // Spock smashes scissors and vaporizes rock; 
        // Spock is poisoned by lizard and disproven by paper. 
        // Lizard poisons Spock and eats paper; 
        // Lizard is crushed by rock and decapitated by scissors.

        public static readonly int[] VerbMap = [
        //  Rock Paper Scissors Lizard Spock 
            0,  -1,    1,       1,    -1, // Rock
            1,   0,   -1,      -1,     1, // Paper
           -1,   1,    0,       1,    -1, // Scissors
           -1,   1,   -1,       0,     1, // Lizard
            1,  -1,    1,      -1,     0  // Spock
        ]; // Win(1), lose(-1) or draw(0) map

        public static readonly string[] VerbList = [
            // Rock             Paper              Scissors             Lizard            Spock 
              "matches",       "is covered by",   "smashes",           "crushes",        "is vaporized by", // Rock 
              "covers",        "matches",         "is cut by",         "is eaten by",    "disproves",       // Paper
              "are broken by", "cuts",            "matches",           "decapatates",    "are smashed by",  // Scissors
              "is crushed by", "eats",            "is decapitated by", "matches",        "poisons",         // Lizard
              "vaporizes",     "is disproved by", "smashes",           "is poisoned by", "matches"          // Spock
        ]; // Horizontal player alias vs vertical computer alias

        enum ErrorState : int
        {
            Invalid = -1,
            Exit = -2,
            License = -3,
            Nothing = -4,
        }

        static void Main()
        {
            bool Debug = false;     // Turn inline debugging on or off
            int PlayerScore = 0;    // Player score
            int ComputerScore = 0;  // Computer score
            string Result;          // Result string

            // Now with colorful text! :) - Save the initial colors, load all 16 colors into an
            // array, then pull out the colors we want (Red, Yellow, Green and Blue)
            ConsoleColor Back = Console.BackgroundColor;
            ConsoleColor Fore = Console.ForegroundColor;
            ConsoleColor[] CMap = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            ConsoleColor CRed = CMap[12];
            ConsoleColor CYellow = CMap[14];
            ConsoleColor CBlue = CMap[9];
            ConsoleColor CGreen = CMap[10];
            Random Rando = new(); // Some RNG for the computer

            CFore(CBlue);
            Wl("RPSLS "+GetVersion()+" a Rock Paper Scissors Lizard Spock game\n" +
               "as invented by  Sam Kass and Karen Bryla\n"+
               "written by Dan Rhea © 2019, 2022\n"+
               "under ther GPL3 license (enter \"License\" to view)\n");
            CFore(CBlue);
            W("RPSLS>");
            CFore(CYellow);

            string Input = R();
            while (Input != null)
            {
                int Computer = Rando.Next(4); // RNG computer alias
                int Player = ParseInput(Input); // Figure out the player alias
                if (Debug) {
                    Console.WriteLine("Player:{0} Computer:{1}", Player, Computer);
                }

                // Process player input
                if (Player == (int)ErrorState.Invalid)
                {
                    CFore(CBlue);
                    Wl("I don't understand \"" + Input + "\", enter \"Rock\", \"Paper\", "+
                        "\"Scissors\", \"Lizard\" or \"Spock\", or\nenter \"License\" to view "+
                        "the license or enter nothing to exit.\n");
                    CFore(CYellow);
                }
                else if (Player == (int)ErrorState.Exit)
                {
                    CFore(Fore);
                    CBack(Back);
                    break; // Exit 
                }
                else if (Player == (int)ErrorState.License)
                {
                    CFore(CYellow);
                    CBack(CBlue);
                    Console.Clear(); 
                    ShowFile("\\gnu_gpl3.txt"); // Show GNU GPL3 license
                    Wl(" ");
                    CFore(CYellow);
                    CBack(Back);
                    Console.Clear();
                }
                else if (Player == (int)ErrorState.Nothing)
                {
                    CFore(CRed);
                    Wl("Very funny...\n"); // They got cute and entered "nothing" :)
                    CFore(CYellow);
                }
                else
                {
                    // Determine the offset into the verb list
                    int Verbs = ((Player) * 5) + Computer;
                    if (Debug) { Console.WriteLine("Verbs:{0}", Verbs); }

                    // Use the offset to extract the result from the verb map
                    switch (VerbMap[Verbs])
                    {
                        case -1:
                            CFore(CRed);
                            Result = "Player loses to Computer!";
                            ComputerScore++;
                            break;
                        case 1:
                            CFore(CGreen);
                            Result = "Player wins over Computer!";
                            PlayerScore++;
                            break;
                        case 0:
                        default:
                            CFore(CYellow);
                            Result = "Player ties with Computer!";
                            break;
                    }

                    // Show what happened and the results
                    Console.WriteLine("{0} {1} {2}! {3} Player: {4} Computer: {5}\n",
                                      Alias[Player], VerbList[Verbs], Alias[Computer],
                                      Result, PlayerScore, ComputerScore);
                }

                // prompt the player for the next round
                CFore(CBlue);
                W("RPSLS>");
                CFore(CYellow);
                Input = R();
            }
        }

        /// <summary>
        /// Get and return the program version
        /// </summary>
        /// <returns>version string</returns>
        public static string GetVersion()
        {
            Assembly thisAsbly = typeof(RPSLS).Assembly;
            AssemblyName thisAsblyName = thisAsbly.GetName();
            Version ver = thisAsblyName.Version;
            return ver.ToString();
        }

        /// <summary>
        /// See if player entered a valid alias
        /// </summary>
        /// <param name="Input">Alias string</param>
        /// <returns>Alias index or -1 invalid or -2 exit</returns>
        public static int ParseInput(string Input)
        {
            int Ret = -1;

            if (Input.Length == 0) { return -2; }
            for (int Ix = 0; Ix < Alias.Length; Ix++)
            {
                // Scan for a matching alias and use it's offset value
                if (Input.Equals(Alias[Ix], StringComparison.CurrentCultureIgnoreCase))
                {
                    Ret = Ix;
                    break;
                }
            }
            if (Input.Equals("nothing", StringComparison.CurrentCultureIgnoreCase)) { return -4; }
            if (Input.Equals("license", StringComparison.CurrentCultureIgnoreCase)) { return -3; }
            if (Ret == -1) { return -1; }

            return Ret; // Alias index
        }

        /// <summary>
        /// Will display a paginated file to the Console. If a path
        /// is not supplied, the same path where the program resides
        /// is used
        /// </summary>
        /// <param name="FileName">File to display</param>
        /// <param name="PathName">Path the file resides in (optional)</param>
        public static void ShowFile(String FileName, String PathName = null)
        {
            Int32 ScreenHeight = Console.WindowHeight;
            Int32 ScreenWidth = Console.WindowWidth;
            Int32 Row = 0;
            String Inp = "";
            string AppPath = SetPath(FileName, PathName);
            try
            {
                using StreamReader sr = new(AppPath);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Wl(line);
                    if (line.Length > ScreenWidth - 1)
                    {
                        Row += (Int32)(line.Length / ScreenWidth - 1);
                    }
                    else
                    {
                        Row++;
                    }
                    if (Row >= ScreenHeight - 1)
                    {
                        Row = 0;
                        W("Press Enter to continue (enter q to quit):");
                        Inp = R();
                        if (Inp == "q" || Inp == "Q")
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Wl("Could not read the text in file " + FileName + ".");
                Wl(e.Message);
            }
        }

        /// <summary>
        /// Make sure we have a properly formed path. If initial path is empty,
        /// set it to the application location.
        /// </summary>
        /// <param name="FileName">Name of file to show</param>
        /// <param name="PathName">Path where file is located</param>
        /// <returns>Fully formed path with filename</returns>
        public static string SetPath(String FileName, String PathName = null)
        {
            String AppPath = "";

            if (PathName == null)
            {
                AppPath = Assembly.GetExecutingAssembly().Location;
                string Nam = Path.GetFileName(AppPath);
                AppPath = AppPath[..^Nam.Length];
                if (FileName[..2] != "\\")
                {
                    AppPath += "\\" + FileName;
                }
                else
                {
                    AppPath += FileName;
                }
            }
            else
            {
                if (FileName[..2] != "\\")
                {
                    AppPath += PathName + "\\" + FileName;
                }
                else
                {
                    AppPath = PathName + FileName;
                }
            }
            return AppPath;
        }

        /// <summary>
        /// wrapper function for Console.WriteLine()
        /// </summary>
        /// <param name="Msg">string to output</param>
        public static void Wl(string Msg)
        {
            if (Msg != null)
            {
                Console.WriteLine(Msg);
            }
            else
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Wrapper function for Console.Write()
        /// </summary>
        /// <param name="Msg">string to output</param>
        public static void W(string Msg)
        {
            if (Msg != null)
            {
                Console.Write(Msg);
            }
        }

        /// <summary>
        /// Wrapper function for Console.ReadLine
        /// </summary>
        /// <returns>string input</returns>
        public static string R()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Set the foreground color
        /// </summary>
        /// <param name="ForeHue">ConsoleColor value</param>
        public static void CFore(ConsoleColor ForeHue)
        {
            Console.ForegroundColor = ForeHue;
        }

        /// <summary>
        /// Set the background color
        /// </summary>
        /// <param name="BackHue">ConsoleColor value</param>
        public static void CBack(ConsoleColor BackHue)
        {
            Console.BackgroundColor = BackHue;
        }
    }
}