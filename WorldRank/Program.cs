using WorldRank;

IPlayerRepository playerRepo = new InMemoryPlayerRepository();
IWalletRepository walletRepo = new InMemoryWalletRepository();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Choose an action:");
    Console.WriteLine("1  - Add a player");
    Console.WriteLine("2  - List all players");
    Console.WriteLine("3  - Find a player by ID");
    Console.WriteLine("4  - Delete a player");
    Console.WriteLine("5  - Add a wallet to a player");
    Console.WriteLine("6  - Deposit into a wallet");
    Console.WriteLine("7  - List a player's wallets");
    Console.WriteLine("8 - Exit");
    Console.Write("Pick an action: ");

    var action = Console.ReadLine();

    if (action == "8")
    {
        Console.WriteLine("Goodbye");
        break;
    }

    switch (action)
    {
        case "1":
        {
            var name = AskNonEmpty("Enter name: ");

            Console.Write("Score: ");
            if (!int.TryParse(Console.ReadLine(), out var score) || score < 0)
            {
                Console.WriteLine("Score must be a whole number that is 0 or greater.");
                break;
            }

            var toAdd = new Player(0, name);
            toAdd.UpdateScore(score);
            var created = playerRepo.AddPlayer(toAdd);

            Console.WriteLine($"Player '{created.Name}' added with ID {created.Id}.");
            break;
        }

        case "2":
        {
            var all = playerRepo.GetAll();
            if (all.Count == 0)
            {
                Console.WriteLine("No players yet.");
                break;
            }
            foreach (var p in all)
                Console.WriteLine(p);
            break;
        }

        case "3":
        {
            var id = AskInt("Enter player ID: ");
            var found = playerRepo.FindPlayer(id);
            Console.WriteLine(found is null ? "Player doesn't exist." : $"Found: {found}");
            break;
        }

        case "4":
        {
            var id = AskInt("Enter player ID to delete: ");
            playerRepo.DeletePlayer(id);
            Console.WriteLine("Player deleted (if they existed).");
            break;
        }

        case "5":
        {
            var id = AskInt("Enter player ID: ");
            var player = playerRepo.FindPlayer(id);
            if (player is null)
            {
                Console.WriteLine("Player doesn't exist.");
                break;
            }

            var currency = AskCurrency();
            if (currency is null)
                break;

            try
            {
                var wallet = new Wallet(currency.Value);
                player.AddWallet(wallet);        // enforces one-per-currency
                walletRepo.Add(wallet, player.Id);
                Console.WriteLine($"{currency} wallet added to player {player.Id}.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        }

        case "6":
        {
            var wallet = SelectWallet();
            if (wallet is null)
                break;

            var amount = AskDecimal("Amount to deposit: ");
            try
            {
                wallet.Deposit(amount);
                Console.WriteLine($"Deposited. New balance: {wallet.Balance} {wallet.Currency}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        }

        case "7":
        {
            var id = AskInt("Enter player ID: ");
            var player = playerRepo.FindPlayer(id);
            if (player is null)
            {
                Console.WriteLine("Player doesn't exist.");
                break;
            }

            var wallets = walletRepo.GetByPlayer(player.Id);
            if (wallets.Count == 0)
            {
                Console.WriteLine("This player has no wallets.");
                break;
            }
            foreach (var w in wallets)
                Console.WriteLine(w);
            break;
        }

        default:
            Console.WriteLine("Invalid option. Try again.");
            break;
    }

    // ---- helper functions ----

    string AskNonEmpty(string prompt)
    {
        string? input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(input));
        return input;
    }

    int AskInt(string prompt)
    {
        int value;
        Console.Write(prompt);
        while (!int.TryParse(Console.ReadLine(), out value))
        {
            Console.Write("Please enter a whole number: ");
        }
        return value;
    }

    decimal AskDecimal(string prompt)
    {
        decimal value;
        Console.Write(prompt);
        while (!decimal.TryParse(Console.ReadLine(), out value))
        {
            Console.Write("Please enter a valid amount: ");
        }
        return value;
    }

    Currency? AskCurrency()
    {
        Console.Write($"Currency ({string.Join(", ", Enum.GetNames<Currency>())}): ");
        var input = Console.ReadLine();
        if (Enum.TryParse<Currency>(input, ignoreCase: true, out var currency))
            return currency;

        Console.WriteLine("Invalid currency.");
        return null;
    }

    Wallet? SelectWallet()
    {
        var id = AskInt("Enter player ID: ");
        var player = playerRepo.FindPlayer(id);
        if (player is null)
        {
            Console.WriteLine("Player doesn't exist.");
            return null;
        }

        var wallets = walletRepo.GetByPlayer(player.Id);
        if (wallets.Count == 0)
        {
            Console.WriteLine("This player has no wallets.");
            return null;
        }

        var currency = AskCurrency();
        if (currency is null)
            return null;

        var wallet = wallets.FirstOrDefault(w => w.Currency == currency.Value);
        if (wallet is null)
            Console.WriteLine($"Player has no {currency} wallet.");
        return wallet;
    }
}