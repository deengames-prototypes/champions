using System;
using System.Collections.Generic;
using DeenGames.Champions.Events;
using Puffin.Core.Events;

namespace DeenGames.Champions.Models
{
    public class Unit
    {
        // TODO: move into unit factory
        private const int HEALTH_PER_LEVEL = 75;
        private const int HIGH_STAT_MULTIPLIER = 30;
        private const int MEDIUM_STAT_MULTIPLIER = 20;
        private const int LOW_STAT_MULTIPLIER = 10;

        public readonly int Id;
        // Like your class/job eg. knight
        public Specialization Specialization { get; set; }
        public int Level { get; } = 1;
        public int Strength { get; private set; }
        
        public int Toughness { get {
            return this.toughness + this.CurrentBattleToughnessModifiers;
        } }

        public int Speed { get; private set; }
        public int Intelligence { get; private set; }
        public int CurrentHealth { get; internal set; }
        public int TotalHealth { get; private set; }

        public float SkillProbability { get {
            return this.Intelligence / 100f;
        } }

        // Temporary effects that only last this battle.
        public int CurrentBattleToughnessModifiers { get; set; }
        private Action<Unit, List<Unit>, List<Unit>> skill;

        private int toughness;
        public string Name { get { return $"{this.Specialization} {this.Id}"; } }

        public Unit(int id, Specialization specialization, int level)
        {
            this.Id = id;
            this.Specialization = specialization;
            this.Level = level;
            this.SetStats();
        }

        public void Attack(Unit target)
        {
            var damage = Math.Max(0, this.Strength - target.Toughness);
            target.CurrentHealth -= damage;
        }

        public void UseSkill(List<Unit> party, List<Unit> monsters)
        {
            this.skill.Invoke(this, party, monsters);
        }

        // TODO: move into unit factory
        private void SetStats()
        {
            this.TotalHealth = this.CurrentHealth = HEALTH_PER_LEVEL * this.Level;
            this.skill = UnitSkills.GetSkill(this.Specialization);

            switch (this.Specialization)
            {
                case Specialization.Archer:
                    // High attack, medium defense, medium speed
                    this.Strength = HIGH_STAT_MULTIPLIER * this.Level;
                    this.toughness = MEDIUM_STAT_MULTIPLIER * this.Level;
                    this.Speed = MEDIUM_STAT_MULTIPLIER * this.Level;
                    this.Intelligence = MEDIUM_STAT_MULTIPLIER * this.Level;
                    break;
                case Specialization.Faris:
                    // Medium attack, high defense, medium speed
                    this.Strength = MEDIUM_STAT_MULTIPLIER * this.Level;
                    this.toughness = HIGH_STAT_MULTIPLIER * this.Level;
                    this.Speed = MEDIUM_STAT_MULTIPLIER * this.Level;
                    this.Intelligence = HIGH_STAT_MULTIPLIER * this.Level;
                    break;
                case Specialization.Lancer:
                    // High attack, low defense, high speed
                    this.Strength = HIGH_STAT_MULTIPLIER * this.Level;
                    this.toughness = LOW_STAT_MULTIPLIER * this.Level;
                    this.Speed = HIGH_STAT_MULTIPLIER * this.Level;
                    this.Intelligence = LOW_STAT_MULTIPLIER * this.Level;
                    break;
                    
                case Specialization.Slime:
                    // Medium attack, low defense, low speed
                    this.Strength = MEDIUM_STAT_MULTIPLIER * this.Level;
                    this.toughness = LOW_STAT_MULTIPLIER * this.Level;
                    this.Speed = LOW_STAT_MULTIPLIER * this.Level;
                    this.Intelligence = LOW_STAT_MULTIPLIER * this.Level;
                    break;
                default:
                    throw new ArgumentException($"Not sure the stats curve for {this.Specialization}");
            }
        }
    }
}