using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeenGames.Champions.Models;
using Puffin.Core;
using Puffin.Core.Ecs;

namespace DeenGames.Champions.Scenes
{
    class BattleScene : Scene
    {
        private readonly int PLAYER_X = ChampionsGame.GAME_WIDTH - MONSTERS_X - Constants.IMAGE_SIZE;
        private const int MONSTERS_X = 100;

        private Unit[] party;
        private Unit[] monsters;

        // Poor man's MVVM: map of model => view-model
        private IDictionary<Unit, Entity> battleEntities = new Dictionary<Unit, Entity>();

        public BattleScene(IList<Unit> party) : base()
        {
            // Grass?
            this.BackgroundColour = 0x3c5956;
            
            var random = new Random();
            this.party = party.ToArray();
            this.monsters = new Unit[]
            {
                new Unit(Specialization.Slime, random.Next(1, 4)),
                new Unit(Specialization.Slime, random.Next(1, 4)),
                new Unit(Specialization.Slime, random.Next(1, 4)),
                new Unit(Specialization.Slime, random.Next(1, 4)),
                new Unit(Specialization.Slime, random.Next(1, 4)),
            };

            for (var i = 0; i < this.party.Length; i++)
            {
                var unit = this.party[i];
                var entity = new Entity()
                    .Move(PLAYER_X, 200 + (int)(i * Constants.IMAGE_SIZE * 1.5))
                    .Spritesheet(Constants.SpecializationsImageFile, Constants.IMAGE_SIZE, Constants.IMAGE_SIZE, (int)unit.Specialization);
                this.Add(entity);
                this.battleEntities[unit] = entity;
            }

            for (var i = 0; i < this.monsters.Length; i++)
            {
                var unit = this.monsters[i];
                var entity = new Entity()
                    .Move(MONSTERS_X, 200 + (int)(i * Constants.IMAGE_SIZE * 1.5))
                    .Spritesheet(Constants.SpecializationsImageFile, Constants.IMAGE_SIZE, Constants.IMAGE_SIZE, (int)unit.Specialization);
                this.Add(entity);
                this.battleEntities[unit] = entity;
            }
        }
    }
}