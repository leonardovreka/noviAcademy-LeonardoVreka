var players = new List<Player>();

while (true)
{
    Console.WriteLine("Choose an action:");
    Console.WriteLine("Type 1 if you want to add a player.");
    Console.WriteLine("Type 2 if you want to list all players.");
    Console.WriteLine("Type 3 if you want to find a specific player.");
    Console.WriteLine("Type 4 if you want to exit.");
    Console.Write("Pick an action: ");

    var action = Console.ReadLine();

    if (action == "4")
    {
        Console.WriteLine("Goodbye");
        break;
    }

    switch (action)
    {
        case "1":
            // checkarw edw gia keno h null kai oxi ston constractor gia na min exw 2 elenxous
            string? name;
            do
            {
                 Console.Write("Enter name: ");
                 name = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(name));

            players.Add(new Player(name));
            Console.WriteLine($"Player {name} added in the list.");
            break;

        case "2":
            if (players.Count == 0)
            {
                Console.WriteLine("No players inside the list yet.");
                break;
            }
            foreach (var player in players)
            {
                Console.WriteLine(player.Name);
            }
            break;

        case "3":
            string? search;
            do
            {
                Console.Write("Give me a player you want to find: ");
                search = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(search));

            var find = players.FirstOrDefault(p => p.Name == search);

            if (find is null)
            {
                Console.WriteLine("Player doesnt exist.");
                break;
            }

            Console.WriteLine($"Found: {find.Name}");
            break;

        default:
            Console.WriteLine("Invalid option. Try again.");
            break;
    }
}