using Lab2;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Riddle();
        List<VideoGame> games = LoadGamesFromCsv();
        Stack<VideoGame> gameStack = CreateGameStack(games);
        Dictionary<string, VideoGame> gameDictionary = CreateGameDictionary(games);
        Queue<VideoGame> gameQueue = CreateGameQueue(games);
        Stack<string> titleStack = CreateTitleStack(games);
        IgnorantUser(gameDictionary, gameQueue, gameStack, games, titleStack);
    }
    static void IgnorantUser(Dictionary<string, VideoGame> gameDictionary, Queue<VideoGame> gameQueue, Stack<VideoGame> gameStack, List<VideoGame> games, Stack<string> titleStack)
    {
        bool exit = false;
        List<VideoGame> playlist = new List<VideoGame>();

        while (!exit)
        {
            Console.WriteLine("Options:");
            Console.WriteLine("1. If you wanna know what the uniqueness of each data structure is click me ;) xoxo");
            Console.WriteLine("2. I'm so bad click me, I'll show you all my single values from my data structure.");
            Console.WriteLine("3. Come create a playlist with me. We'll have so much fun.");
            Console.WriteLine("4. Wanna take a peak of what we made together. AKA/View Playlist");
            Console.WriteLine("5. Fine leave or whatever I didn't want your company anyhow. AKA/Exit");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Uniqueness(gameStack, titleStack);
            }
            else if (choice == "2")
            {
                SingleValue(gameDictionary, gameQueue, gameStack);
            }
            else if (choice == "3")
            {
                Console.WriteLine("Pick one of those silly lttle numbers and I'll add it to our playlist. :");
                int gameNumber = 1;

            foreach (var game in games)
            {
                Console.WriteLine($"{gameNumber}. {game.Name}");
                gameNumber++;
            }

                Console.Write("Enter the numbers of games to add sugar (comma-separated) \nExample: 1,2,3,4 \n: \n");
                string gameNumbersToAdd = Console.ReadLine();
                string[] gameNumbers = gameNumbersToAdd.Split(',');

                foreach (string number in gameNumbers)
                {
                    if (int.TryParse(number, out int index) && index >= 1 && index <= games.Count)
                    {
                        playlist.Add(games[index - 1]);
                        Console.WriteLine($"Added {games[index - 1].Name} to OUR playlist Lovely <3.");
                    }
                    else
                    {
                        Console.WriteLine($"Not One of the numbers Pookie Bear: {number}");
                    }
                }
            }
            else if (choice == "4")
            {
                ViewPlaylist(playlist);
            }
            else if (choice == "5")
            {
                Console.WriteLine("I relly enjoyed making this with you! Have a good day! \nI hope you enjoyed how sexually promiscuous my code was.");
                exit = true;
            }
            else
            {
                Console.WriteLine("Damn how hard is it ta answer a Question. Please try again I beg you.");
            }
        }
    }
    static void Uniqueness(Stack<VideoGame> gameStack, Stack<string> titleStack)
    {
        Console.WriteLine("mmh look how my stack of unique game titles is sorted by year \n doesn't that just look so nice:");
        foreach (string title in titleStack)
        {
            Console.WriteLine(title);
        }

        if (gameStack.Count > 0)
        {
            VideoGame topGame = gameStack.Peek();
            Console.WriteLine($"This is our top game in the stack: {topGame.Name}");
        }
        else
        {
            Console.WriteLine("Oh no the game stack is empty:  :<");
        }
    }

    static void SingleValue(Dictionary<string, VideoGame> gameDictionary, Queue<VideoGame> gameQueue, Stack<VideoGame> gameStack)
    {
        Console.WriteLine("Displaying a single value from each data structure, As asked:");
        if (gameDictionary.Count > 0)
        {
            var randomGame = gameDictionary.Values.First();
            Console.WriteLine($"Is this your card: {randomGame.Name}");
        }
        else
        {
            Console.WriteLine("The game dictionary is empty.");
        }
        if (gameQueue.Count > 0)
        {
            VideoGame nextGame = gameQueue.Peek();
            Console.WriteLine($"Up next in the Queue is: {nextGame.Name}");
        }
        else
        {
            Console.WriteLine("The game queue is empty.");
        }
        if (gameStack.Count > 0)
        {
            VideoGame topGame = gameStack.Peek();
            Console.WriteLine($"Hooray it's: {topGame.Name}");
        }
        else
        {
            Console.WriteLine("The game stack is empty.");
        }
    }

    static void ViewPlaylist(List<VideoGame> playlist)
    {
        Console.WriteLine("Your Playlist Sugar:");
        foreach (var game in playlist)
        {
            Console.WriteLine(game.Name);
        }
    }

    static Stack<VideoGame> CreateGameStack(List<VideoGame> games)
    {
        return new Stack<VideoGame>(games);
    }

    static Dictionary<string, VideoGame> CreateGameDictionary(List<VideoGame> games)
    {
        Dictionary<string, VideoGame> gameDictionary = new Dictionary<string, VideoGame>();

        foreach (var game in games)
        {
            string key = game.Name + " - " + game.Platform;
            if (!gameDictionary.ContainsKey(key))
            {
                gameDictionary.Add(key, game);
            }
            else
            {
                int count = 2;
                string newKey;
                do
                {
                    newKey = $"{key} ({count})";
                    count++;
                } while (gameDictionary.ContainsKey(newKey));

                gameDictionary.Add(newKey, game);
            }
        }

        return gameDictionary;
    }

    static Queue<VideoGame> CreateGameQueue(List<VideoGame> games)
    {
        return new Queue<VideoGame>(games);
    }

    static Stack<string> CreateTitleStack(List<VideoGame> games)
    {
        var sortedTitles = games.OrderBy(game => game.Year).Select(game => game.Name);
        return new Stack<string>(sortedTitles);
    }
    static void Riddle()
    {
        List<VideoGame> expectedAnswer = RiddleInfo();
        Console.WriteLine("For you to grade my code you must first answer my riddles 1.\nWhat walks on four legs in the morning, two legs at noon, and three legs in the evening?");

        int attempts = 3;
        bool isCorrect = false;

        while (attempts > 0 && !isCorrect)
        {
            string answer = Console.ReadLine().ToLower(); 
            if (IsAnswerCorrect(answer, expectedAnswer))
            {
                Console.WriteLine("You may enter.");
                isCorrect = true;
            }
            else
            {
                Console.WriteLine($"Sorry, that's not correct. {attempts - 1} attempt(s) remaining. Try Again.");
                attempts--;
            }
        }

        if (!isCorrect)
        {
            Console.WriteLine("You have run out of attempts. Access denied.");
        }
    }

    static bool IsAnswerCorrect(string userAnswer, List<VideoGame> expectedAnswer)
    {
        string expectedAnswerAsString = string.Join(",", expectedAnswer.Select(game => game.Name.ToLower())); 
        return string.Equals(userAnswer, expectedAnswerAsString, StringComparison.OrdinalIgnoreCase);
    }

    static List<VideoGame> RiddleInfo()
    {
        string filepath = @"C:\Users\maddu\source\repos\Lab2\Lab2\videogames (1).csv";
        List<VideoGame> games = File.ReadLines(filepath)
            .Skip(16328)
            .Select(line =>
            {
                var values = line.Split(',');
                return new VideoGame
                {
                    Name = values[0],
                };
            })
            .ToList();

        return games;
    }
    static List<VideoGame> LoadGamesFromCsv()
    {
        string filepath = @"C:\Users\maddu\source\repos\Lab2\Lab2\videogames (1).csv";

        List<VideoGame> games = File.ReadLines(filepath)
            .Skip(1)
            .Select(line =>
            {
                var values = line.Split(',');
                return new VideoGame
                {
                    Name = values[0],
                    Platform = values[1],
                    Year = int.Parse(values[2]),
                    Genre = values[3],
                    Publisher = values[4],
                    NA_Sales = double.Parse(values[5]),
                    EU_Sales = double.Parse(values[6]),
                    JP_Sales = double.Parse(values[7]),
                    Other_Sales = double.Parse(values[8]),
                    Global_Sales = double.Parse(values[9])
                };
            })
            .ToList();

        return games;
    }
}

