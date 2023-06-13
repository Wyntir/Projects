using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;
using System.Runtime.InteropServices;

namespace BlackjackUpdated
{
    class Program
    {
        static Random cardRandomizer = new Random();
        static readonly List<Card> deck = new List<Card>(); // Represents the deck of cards
        static readonly List<Card[]> playersHands = new List<Card[]>(); // List to store the hands of all players
        static readonly List<int> playersTotals = new List<int>(); // List to store the totals of all players
        static int dealtCards = 0;
        static readonly Card[] dealerCards = new Card[11];
        static int dealerTotal = 0;
        static int dealerCardCount = 0;
        static int numPlayers = 0;
        //users to store the player choice (hit or stay)

        static string playAgain = "Y";

        static void Main(string[] args)
        {
            ConsoleHelper.SetCurrentFont("Lucida Console", 18);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Create the deck of cards
            CreateDeck();
            while (playAgain.ToUpper() == "Y")
            {
                // StartGame
                Console.WriteLine("Welcome to Blackjack - are you ready to play? (Y)es (N)o");
                var decision = Console.ReadLine().ToUpper();

                if (decision == "Y")
                {
                    ShuffleDeck(); // Shuffle the deck
                    dealerCards[0] = DealCard(); // Dealer's first card (face up)
                    dealerCards[1] = DealCard(); // Dealer's second card (Face down)
                    dealerTotal += dealerCards[0].Value;
                    dealerTotal += dealerCards[1].Value;

                    playersHands.Clear();
                    playersTotals.Clear();

                    // Prompt for the number of players
                    Console.WriteLine("How many players? Enter a number:");
                    string input = Console.ReadLine();
                    while (!int.TryParse(input, out numPlayers) || numPlayers < 1)
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number of players:");
                        input = Console.ReadLine();
                    }

                    // Initialize hands and totals for each player
                    for (int i = 0; i < numPlayers; i++)
                    {
                        Card[] hand = new Card[11];
                        hand[0] = DealCard();
                        hand[1] = DealCard();
                        int total = hand[0].Value + hand[1].Value;
                        playersHands.Add(hand);
                        playersTotals.Add(total);
                    }

                    DisplayWelcomeMessage();

                }
                else
                {
                    Environment.Exit(0);
                }

                /* START GAME LOOP */
                for (int i = 0; i < numPlayers; i++)
                {

                      Console.WriteLine("Player {0}, would you like to hit or stay? H for hit S for stay", i + 1);
                        string playerChoice = Console.ReadLine().ToUpper();
                        if (playerChoice.Equals("H"))
                        {
                            Hit(i);
                        }
                        else if (playerChoice.Equals("S"))
                        {
                            Console.WriteLine("Player {0} stays.", i + 1);
                        }
                    
                }
                /* END GAME LOOP */

                DisplayFinalResults(numPlayers);

                Console.WriteLine("Would you like to play again? (Y)es or (N)o?");
                PlayAgain();
            }
        }

        private static void DisplayWelcomeMessage()
            {
                for (int i = 0; i < playersHands.Count; i++)
                {
                    Console.WriteLine("Player {0}, you were dealt the cards: {1} and {2}", i + 1, playersHands[i][0].Name, playersHands[i][1].Name);
                    printCard(playersHands[i][0].Rank, playersHands[i][0].Suit);
                    printCard(playersHands[i][1].Rank, playersHands[i][1].Suit);
                Console.WriteLine("Your total is {0}", playersTotals[i]);
                }
                Console.WriteLine("The dealer's visible card is {0}", dealerCards[0].Name);
                printCard(dealerCards[0].Rank, dealerCards[0].Suit);
        }
            private static void DisplayPlayerStats(int playerIndex)
            {
                Console.WriteLine("Player {0}, you were dealt the cards: {1} and {2}", playerIndex + 1, playersHands[playerIndex][0].Name, playersHands[playerIndex][1].Name);
                printCard(playersHands[playerIndex][0].Rank, playersHands[playerIndex][0].Suit);
                printCard(playersHands[playerIndex][1].Rank, playersHands[playerIndex][1].Suit);
                Console.WriteLine("Your total is {0}", playersTotals[playerIndex]);
                Console.WriteLine("The dealer's visible card is {0}", dealerCards[0].Name);
                printCard(dealerCards[0].Rank, dealerCards[0].Suit);
        }


            static void Hit(int playerIndex)
            {
                Card[] hand = playersHands[playerIndex];
                int total = playersTotals[playerIndex];
                int cardCount = Array.FindIndex(hand, card => card == null);
                hand[cardCount] = DealCard();
                total += hand[cardCount].Value;

                if (hand[cardCount].Value == 11 && total > 21)
                {
                    total -= 10;
                    hand[cardCount].Value = 1;
                }

                playersHands[playerIndex] = hand;
                playersTotals[playerIndex] = total;

                DisplayPlayerStats(playerIndex);

                Console.WriteLine("Player {0}, your card is a(n) {1} and your new Total is {2}", playerIndex + 1, hand[cardCount].Name, total);
                printCard(playersHands[playerIndex][cardCount].Rank, playersHands[playerIndex][cardCount].Suit);
                if (total == 21)
                {
                    Console.WriteLine("Player {0}, you got Blackjack!", playerIndex + 1);
                }
                else if (total > 21)
                {
                    Console.WriteLine("Player {0}, you busted! Sorry!", playerIndex + 1);
                }
            }

            private static Card DealCard()
            {
                Card card = deck[dealtCards];
                deck.RemoveAt(dealtCards);
                dealtCards++;
                return card;
            }

            private static void CreateDeck()
            {
                string[] suits = { "Hearts", "Diamonds", "Spades", "Clubs" };
                Dictionary<string, int> rankValues = new Dictionary<string, int>()
        {
            { "Ace", 11 },
            { "Two", 2 },
            { "Three", 3 },
            { "Four", 4 },
            { "Five", 5 },
            { "Six", 6 },
            { "Seven", 7 },
            { "Eight", 8 },
            { "Nine", 9 },
            { "Ten", 10 },
            { "Jack", 10 },
            { "Queen", 10 },
            { "King", 10 }
        };
                foreach (string suit in suits)
                {
                    foreach (string rank in rankValues.Keys)
                    {
                        int value = rankValues[rank];
                        Card card = new Card { Name = $"{rank} of {suit}", Value = value, Rank = rank, Suit = suit };
                        deck.Add(card);
                    }
                }
            }

            private static void DisplayFinalResults(int numPlayers)
            {
                for (int i = 0; i < numPlayers; i++)
                {
                    Console.WriteLine("Player {0}, your final total is {1}", i + 1, playersTotals[i]);
                }

                Console.WriteLine("Dealer's hand:");
                for (int i = 0; i <= dealerCardCount; i++)
                {
                    Console.WriteLine(dealerCards[i].Name);
                }
                Console.WriteLine("Dealer's total is {0}", dealerTotal);
                printCard(dealerCards[0].Rank, dealerCards[0].Suit);

                for (int i = 0; i < numPlayers; i++)
                {
                    if (playersTotals[i] > 21)
                    {
                        Console.WriteLine("Player {0}, you busted! Sorry!", i + 1);
                    }
                    else if (playersTotals[i] == dealerTotal)
                    {
                        Console.WriteLine("Player {0}, it's a push! Your total is the same as the dealer's.", i + 1);
                    }
                    else if (playersTotals[i] > dealerTotal && playersTotals[i] <= 21)
                    {
                        Console.WriteLine("Player {0}, congrats! You won the game! The dealer's total is {1}", i + 1, dealerTotal);
                    }
                    else
                    {
                        Console.WriteLine("Player {0}, sorry, you lost! The dealer's total was {1}", i + 1, dealerTotal);
                    }
                }
            }

        private static void printCard(string rank, string suit)
        {
            Card card = new Card { Name = $"{rank} of {suit}", Value = 0, Rank = rank, Suit = suit };

            Dictionary<string, string> rankSymbol = new Dictionary<string, string>()
    {
        { "Ace", "A" },
        { "Two", "2" },
        { "Three", "3" },
        { "Four", "4" },
        { "Five", "5" },
        { "Six", "6" },
        { "Seven", "7" },
        { "Eight", "8" },
        { "Nine", "9" },
        { "Ten", "10" },
        { "Jack", "J" },
        { "Queen", "Q" },
        { "King", "K" }
    };

            string topLine = "┌─────────┐";
            string rankLine = "│         │";
            string rankLine2 = "│         │";
            string suitLine = "│         │";
            string midLine = "│         │";
            string botLine = "└─────────┘";
            string rankS = rankSymbol[card.Rank];

            Console.WriteLine(topLine);
            Console.WriteLine(rankLine = $"│{rankS}        │");
            Console.WriteLine(midLine);
            Console.WriteLine(midLine);

            if (card.Suit == "Hearts")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                suitLine = "│    ♥    │";
            }
            else if (card.Suit == "Spades")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                suitLine = "│    ♠    │";
            }
            else if (card.Suit == "Diamonds")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                suitLine = "│    ♦    │";
            }
            else if (card.Suit == "Clubs")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                suitLine = "│    ♣    │";
            }

            Console.WriteLine(suitLine);
            Console.WriteLine(midLine);
            Console.WriteLine(midLine);
            Console.WriteLine(rankLine2 = $"│        {rankS}│");
            Console.WriteLine(botLine);

            Console.ForegroundColor = ConsoleColor.White;
        }


        class Card
        {
            public int Value { get; set; }
            public string Name { get; set; }
            public string Rank { get; set; }
            public string Suit { get; set; }
        }

        private static void ShuffleDeck()
        {
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = cardRandomizer.Next(n + 1);
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        static void PlayAgain()
        {
            // Loop until they make a valid choice
            do
            {
                playAgain = Console.ReadLine().ToUpper();
            } while (!playAgain.Equals("Y") && !playAgain.Equals("N"));

            if (playAgain.Equals("Y"))
            {
                Console.WriteLine("Press enter to restart the game!");
                Console.ReadLine();
                Console.Clear();
                dealerTotal = 0;
                dealerCardCount = 0;
                deck.Clear();
            }
            else if (playAgain.Equals("N"))
            {
                Console.WriteLine("Thank you for playing! Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
    public static class ConsoleHelper
    {
        private const int FixedWidthTrueType = 54;
        private const int StandardOutputHandle = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);


        private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FontInfo
        {
            internal int cbSize;
            internal int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
            public string FontName;
        }

        public static FontInfo[] SetCurrentFont(string font, short fontSize = 0)
        {
            Console.WriteLine("Set Current Font: " + font);

            FontInfo before = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };

            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {

                FontInfo set = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType,
                    FontName = font,
                    FontWeight = 400,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };

                // Get some settings from current font.
                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    Console.WriteLine("Set error " + ex);
                    throw new System.ComponentModel.Win32Exception(ex);
                }

                FontInfo after = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, set, after };
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                Console.WriteLine("Get error " + er);
                throw new System.ComponentModel.Win32Exception(er);
            }
        }
    }
}
