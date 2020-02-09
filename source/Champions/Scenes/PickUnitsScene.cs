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
    class PickUnitsScene : Scene
    {
        private const string DEFAULT_LABEL_TEXT = "Mouse over a unit to see it's stats";
        private const int NUM_CHOICES = 20;
        private const int TEAM_SIZE = 5;

        private List<Unit> pickedUnits = new List<Unit>(TEAM_SIZE);
        private Entity label;

        public PickUnitsScene()
        {
            var units = this.GenerateUnits();

            this.label = new Entity().Label(DEFAULT_LABEL_TEXT).Move(32, ChampionsGame.GAME_HEIGHT - 32);
            this.Add(label);

            for (var i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                var relativeX = (int)(1.5 * ((i % 5) * Constants.IMAGE_SIZE));
                var relativeY = (int)(1.5 * (i / 5) * Constants.IMAGE_SIZE);

                this.Add(new Entity()
                    .Move(300 + relativeX, 100 + relativeY)
                    .Spritesheet(Constants.SpecializationsImageFile, Constants.IMAGE_SIZE, Constants.IMAGE_SIZE, (int)unit.Specialization)
                    .Overlap(Constants.IMAGE_SIZE, Constants.IMAGE_SIZE, 0, 0,
                        () => label.Get<TextLabelComponent>().Text = $"Level {unit.Level} {unit.Specialization.ToString()}",
                        () => label.Get<TextLabelComponent>().Text = DEFAULT_LABEL_TEXT)
                    .Mouse(() => this.OnAddedUnit(unit), Constants.IMAGE_SIZE, Constants.IMAGE_SIZE)
                );
            }
        }

        private void OnAddedUnit(Unit unit)
        {
            this.pickedUnits.Add(unit);
            this.Add(new Entity()
                .Move(300 + (int)(this.pickedUnits.Count * 1.5 * Constants.IMAGE_SIZE), ChampionsGame.GAME_HEIGHT - 2 * Constants.IMAGE_SIZE)
                .Spritesheet(Path.Combine("Content", "Images", "Specializations.png"), Constants.IMAGE_SIZE, Constants.IMAGE_SIZE, (int)unit.Specialization)
                .Overlap(Constants.IMAGE_SIZE, Constants.IMAGE_SIZE, 0, 0,
                    () => label.Get<TextLabelComponent>().Text = $"Level {unit.Level} {unit.Specialization.ToString()}",
                    () => label.Get<TextLabelComponent>().Text = DEFAULT_LABEL_TEXT)
            );

            if (this.pickedUnits.Count == TEAM_SIZE)
            {
                ChampionsGame.LatestInstance.ShowScene(new BattleScene(this.pickedUnits));
            }
        }

        private IList<Unit> GenerateUnits()
        {
            var random = new Random();
            var toReturn = new List<Unit>();

            var specializations = new Specialization[] {
                Specialization.Archer, Specialization.Faris, Specialization.Lancer
            };

            while (toReturn.Count < NUM_CHOICES)
            {
                var specialization = (Specialization)specializations.GetValue(random.Next(specializations.Length));
                var level = random.Next(1, 4);
                toReturn.Add(new Unit(specialization, level));
            }

            return toReturn;
        }
    }
}