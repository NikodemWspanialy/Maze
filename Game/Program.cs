using Game;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("LOGS:");
        GameMenager game = new GameMenager();
        game.Run();
    }
}