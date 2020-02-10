using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeenGames.Champions.Models;
using Puffin.Core;
using Puffin.Core.Ecs;
using Puffin.Core.Ecs.Components;

namespace DeenGames.Champions.Scenes
{
    class BattleScene : Scene
    {
        private readonly int PLAYER_X = ChampionsGame.GAME_WIDTH - MONSTERS_X - Constants.IMAGE_SIZE;
        private const int MONSTERS_X = 100;

        private readonly TimeSpan DELAY_BETWEEN_ACTIONS = TimeSpan.FromSeconds(1);
        private DateTime lastActionTime;
        private List<Unit> turns = new List<Unit>();

        private List<Unit> party;
        private List<Unit> monsters;
        private Random random = new Random();
        private Entity partyArrow;
        private Entity monsterArrow;

        // Poor man's MVVM: map of model => view-model
        private IDictionary<Unit, Entity> battleEntities = new Dictionary<Unit, Entity>();

        public BattleScene(List<Unit> party) : base()
        {
            // Grass?
            this.BackgroundColour = 0x3c5956;

            partyArrow = new Entity().Move(PLAYER_X - Constants.IMAGE_SIZE, 200)
                .Sprite(Path.Combine("Content", "Images", "Arrow-Right.png"));
            this.Add(partyArrow);

            monsterArrow = new Entity().Move(MONSTERS_X + (2 * Constants.IMAGE_SIZE), 200)
                .Sprite(Path.Combine("Content", "Images", "Arrow-Left.png"));
            this.Add(monsterArrow);

            var random = new Random();
            this.party = party;
            this.monsters = new List<Unit>()
            {
                new Unit(Specialization.Slime, random.Next(1, 4)),
                new Unit(Specialization.Slime, random.Next(1, 4)),
                new Unit(Specialization.Slime, random.Next(1, 4)),
                new Unit(Specialization.Slime, random.Next(1, 4)),
                new Unit(Specialization.Slime, random.Next(1, 4)),
            };

            for (var i = 0; i < this.party.Count; i++)
            {
                var unit = this.party[i];
                var entity = new Entity()
                    .Move(PLAYER_X, 200 + (int)(i * Constants.IMAGE_SIZE * 1.5))
                    .Spritesheet(Constants.SpecializationsImageFile, Constants.IMAGE_SIZE, Constants.IMAGE_SIZE, (int)unit.Specialization);
                this.Add(entity);
                this.battleEntities[unit] = entity;
            }

            for (var i = 0; i < this.monsters.Count; i++)
            {
                var unit = this.monsters[i];
                var entity = new Entity()
                    .Move(MONSTERS_X, 200 + (int)(i * Constants.IMAGE_SIZE * 1.5))
                    .Spritesheet(Constants.SpecializationsImageFile, Constants.IMAGE_SIZE, Constants.IMAGE_SIZE, (int)unit.Specialization);
                this.Add(entity);
                this.battleEntities[unit] = entity;
            }

            lastActionTime = DateTime.Now;
        }

        override public void Update(int elapsedMilliseconds)
        {
            var now = DateTime.Now;
            if ((now - lastActionTime) < DELAY_BETWEEN_ACTIONS)
            {
                // Not yet time to do something else
                return;
            }

            if (!turns.Any())
            {
                // Generate a new round of turns
                // TODO: extract to a class and more turns if more speed
                this.turns = GenerateRoundOfTurns();
            }

            var next = this.turns.First();
            this.turns.RemoveAt(0);

            this.ExecuteTurn(next);
            this.lastActionTime = now;
        }

        private void ExecuteTurn(Unit next)
        {
            // TODO: AI based on level, etc.

            // Random target. TODO: intelligently target ... weakest? strongest? etc.
            Unit target;
            if (this.party.Contains(next))
            {
                target = this.monsters[random.Next(this.monsters.Count)];
                this.partyArrow.Y = this.battleEntities[next].Y;
                this.partyArrow.Get<SpriteComponent>().IsVisible = true;
                this.monsterArrow.Get<SpriteComponent>().IsVisible = false;
            }
            else
            {
                target = this.party[random.Next(this.party.Count)];
                this.monsterArrow.Y = this.battleEntities[next].Y;
                this.partyArrow.Get<SpriteComponent>().IsVisible = false;
                this.monsterArrow.Get<SpriteComponent>().IsVisible = true;
            }
            
            // Basic attack. TODO: intelligently pick a move.
            target.CurrentHealth -= next.Strength;
            if (target.CurrentHealth <= 0)
            {
                this.turns.RemoveAll(u => u == target);
                this.Remove(this.battleEntities[target]);
                
                if (this.monsters.Contains(target))
                {
                    this.monsters.Remove(target);
                    if (!this.monsters.Any())
                    {
                        // VICTORY~!
                        //ChampionsGame.LatestInstance.ShowScene(...)
                    }
                 }
                 else
                 {
                    this.party.Remove(target);
                    if (!this.party.Any())
                    {
                        // Defeat! :(
                        //ChampionsGame.LatestInstance.ShowScene(...)
                    }
                 }
            }

            Console.WriteLine($"{next.Specialization} attacks {target.Specialization}! {(target.CurrentHealth <= 0 ? $"{target.Specialization} dies!" : "")}");
        }

        private List<Unit> GenerateRoundOfTurns()
        {
            // Simple: players first, speed-descending; then monsters first, speed-descending
            var turns = this.party.OrderByDescending(u => u.Speed).ToList();
            turns.AddRange(this.monsters.OrderByDescending(u => u.Speed));
            return turns;
        }
    }
}