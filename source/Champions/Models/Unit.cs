using System;

namespace DeenGames.Champions.Models
{
    class Unit
    {
        // TODO: move into unit factory
        private const int HEALTH_PER_LEVEL = 75;
        private const int HIGH_STAT_MULTIPLIER = 30;
        private const int MEDIUM_STAT_MULTIPLIER = 20;
        private const int LOW_STAT_MULTIPLIER = 10;

        // Like your class/job eg. knight
        public Specialization Specialization { get; set; }
        public int Level { get; } = 1;
        public int Strength { get; private set; }
        public int Toughness { get; private set; }
        public int Speed { get; private set; }
        public int Intelligence { get; private set; }
        public int CurrentHealth { get; internal set; }
        public int TotalHealth { get; private set; }

        public Unit(Specialization specialization, int level)
        {
            this.Specialization = specialization;
            this.Level = level;
            this.SetStats();
        }

        // TODO: move into unit factory
        private void SetStats()
        {
            this.TotalHealth = this.CurrentHealth = HEALTH_PER_LEVEL * this.Level;

            switch (this.Specialization)
            {
                case Specialization.Archer:
                    // High attack, medium defense, medium speed
                    this.Strength = HIGH_STAT_MULTIPLIER * this.Level;
                    this.Toughness = MEDIUM_STAT_MULTIPLIER * this.Level;
                    this.Speed = MEDIUM_STAT_MULTIPLIER * this.Level;
                    this.Intelligence = MEDIUM_STAT_MULTIPLIER * this.Level;
                    break;
                case Specialization.Faris:
                    // Medium attack, high defense, medium speed
                    this.Strength = MEDIUM_STAT_MULTIPLIER * this.Level;
                    this.Toughness = HIGH_STAT_MULTIPLIER * this.Level;
                    this.Speed = MEDIUM_STAT_MULTIPLIER * this.Level;
                    this.Intelligence = HIGH_STAT_MULTIPLIER * this.Level;
                    break;
                case Specialization.Lancer:
                    // High attack, low defense, high speed
                    this.Strength = HIGH_STAT_MULTIPLIER * this.Level;
                    this.Toughness = LOW_STAT_MULTIPLIER * this.Level;
                    this.Speed = HIGH_STAT_MULTIPLIER * this.Level;
                    this.Intelligence = LOW_STAT_MULTIPLIER * this.Level;
                    break;
                    
                case Specialization.Slime:
                    // Medium attack, low defense, low speed
                    this.Strength = MEDIUM_STAT_MULTIPLIER * this.Level;
                    this.Toughness = LOW_STAT_MULTIPLIER * this.Level;
                    this.Speed = LOW_STAT_MULTIPLIER * this.Level;
                    this.Intelligence = LOW_STAT_MULTIPLIER * this.Level;
                    break;
                default:
                    throw new ArgumentException($"Not sure the stats curve for {this.Specialization}");
            }
        }
    }
}