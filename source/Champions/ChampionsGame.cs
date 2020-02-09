using Puffin.Infrastructure.MonoGame;
using DeenGames.Champions.Scenes;

namespace DeenGames.Champions
{
    class ChampionsGame : PuffinGame
    {
        internal const int GAME_WIDTH = 960;
        internal const int GAME_HEIGHT = 540;
        
        public ChampionsGame() : base(GAME_WIDTH, GAME_HEIGHT)
        {
        }

        override protected void Ready()
        {
            this.ShowScene(new PickUnitsScene());
        }
    }
}