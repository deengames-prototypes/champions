using Puffin.Infrastructure.MonoGame;
using DeenGames.Champions.Scenes;
using System.Collections.Generic;
using DeenGames.Champions.Models;

namespace DeenGames.Champions
{
    class ChampionsGame : PuffinGame
    {
        internal static PuffinGame LatestInstance { get; private set; }

        internal const int GAME_WIDTH = 960;
        internal const int GAME_HEIGHT = 540;
        
        public ChampionsGame() : base(GAME_WIDTH, GAME_HEIGHT)
        {
            ChampionsGame.LatestInstance = this;
        }

        override protected void Ready()
        {
            base.Ready();
            
            //this.ShowScene(new PickUnitsScene());
            this.ShowScene(new BattleScene(new List<Unit>
            {
                new Unit(Specialization.Faris, 2),
                new Unit(Specialization.Archer, 2),
                new Unit(Specialization.Lancer, 2),
                new Unit(Specialization.Archer, 3),
                new Unit(Specialization.Faris, 3),
            }));
        }
    }
}