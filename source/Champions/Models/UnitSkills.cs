using System;
using System.Collections.Generic;
using System.Linq;

namespace DeenGames.Champions.Models
{
    public static class UnitSkills
    {
        private const int RAIN_OF_ARROWS_NUM_HITS = 3;
        private const float RUQIYAH_HEAL_PERCENT = 0.3f;
        private const int IMPALE_DAMAGE_MULTIPLIER = 3;
        private const int DIGEST_TOUGHNESS_MODIFIER = -10; // maybe too high
        
        private static Random random = new Random();

        // TODO: based on level, maybe stats?
        // Callback: caster, party, monsters
        public static Action<Unit, List<Unit>, List<Unit>> GetSkill(Specialization specialization)
        {
            switch (specialization)
            {
                case Specialization.Archer:
                    return UnitSkills.RainOfArrows;
                case Specialization.Faris:
                    return UnitSkills.Ruqiyah;
                case Specialization.Lancer:
                    return UnitSkills.Impale;
                case Specialization.Slime:
                    return UnitSkills.Digest;
                default:
                    throw new ArgumentException($"Not sure what skill to give a(n) {specialization}");
            }
        }

        private static void RainOfArrows(Unit user, List<Unit> party, List<Unit> monsters)
        {
            for (var i = 0; i < RAIN_OF_ARROWS_NUM_HITS; i++)
            {
                var target = GetRandomTarget(monsters);
                target.CurrentHealth -= Math.Max(0, user.Strength - target.Toughness);
            }
        }

        private static void Ruqiyah(Unit user, List<Unit> party, List<Unit> monsters)
        {
            var target = GetRandomTarget(party);
            target.CurrentHealth += (int)Math.Ceiling(target.TotalHealth * RUQIYAH_HEAL_PERCENT);
        }

        private static void Impale(Unit user, List<Unit> party, List<Unit> monsters)
        {
            var target = GetRandomTarget(monsters);
            target.CurrentHealth -= IMPALE_DAMAGE_MULTIPLIER * user.Strength;
        }

        private static void Digest(Unit user, List<Unit> party, List<Unit> monsters)
        {
            var target = GetRandomTarget(party);
            target.CurrentBattleToughnessModifiers += DIGEST_TOUGHNESS_MODIFIER;
        }

        private static Unit GetRandomTarget(List<Unit> pool)
        {
            var candidates = pool.Where(u => u.CurrentHealth > 0);
            return candidates.ElementAt(random.Next(candidates.Count()));
        }
    }
}