namespace DeenGames.Champions.Models
{
    class Unit
    {
        // Like your class/job eg. knight
        public Specialization Specialization { get; set; }
        public int Level { get; } = 1;

        public Unit(Specialization specialization, int level)
        {
            this.Specialization = specialization;
            this.Level = level;
        }
    }
}