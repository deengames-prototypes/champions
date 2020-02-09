using System;
using Puffin.Infrastructure.MonoGame;

namespace DeenGames.Champions
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new ChampionsGame())
            {
                game.Run();
            }
        }
    }
}
