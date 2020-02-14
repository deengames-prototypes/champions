using System;
using System.Collections.Generic;
using System.Linq;
using DeenGames.Champions.Events;
using Puffin.Core.Events;

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
            var totalDamage = 0;
            for (var i = 0; i < RAIN_OF_ARROWS_NUM_HITS; i++)
            {
                var target = GetRandomTarget(monsters);
                int damage = Math.Max(0, user.Strength - target.Toughness);
                target.CurrentHealth -= damage;
                totalDamage += damage;
            }
            
            EventBus.LatestInstance.Broadcast(ChampionsEvent.OnAttackOrSkill,
                $"{user.Name} attacks {RAIN_OF_ARROWS_NUM_HITS} times for {totalDamage} total damage!");
        }

        private static void Ruqiyah(Unit user, List<Unit> party, List<Unit> monsters)
        {
            var target = GetRandomTarget(party);
            var healAmount = (int)Math.Ceiling(target.TotalHealth * RUQIYAH_HEAL_PERCENT);
            target.CurrentHealth += healAmount;
            target.CurrentHealth = Math.Min(target.CurrentHealth, target.TotalHealth);

            EventBus.LatestInstance.Broadcast(ChampionsEvent.OnAttackOrSkill,
                $"{user.Name} uses ruqiyah to heal {target.Name} for {healAmount} health!");
        }

        private static void Impale(Unit user, List<Unit> party, List<Unit> monsters)
        {
            var target = GetRandomTarget(monsters);
            var damage = IMPALE_DAMAGE_MULTIPLIER * user.Strength; // Ignores defense
            target.CurrentHealth -= damage;

            EventBus.LatestInstance.Broadcast(ChampionsEvent.OnAttackOrSkill,
                $"{user.Name} impales {target.Name} for {damage} damage!");
        }

        private static void Digest(Unit user, List<Unit> party, List<Unit> monsters)
        {
            var target = GetRandomTarget(party);
            target.CurrentBattleToughnessModifiers += DIGEST_TOUGHNESS_MODIFIER;
            EventBus.LatestInstance.Broadcast(ChampionsEvent.OnAttackOrSkill,
                $"{user.Name} spews acid on {target.Name}! Defense droppped!");
        }

        private static Unit GetRandomTarget(List<Unit> pool)
        {
            var candidates = pool.Where(u => u.CurrentHealth > 0);
            return candidates.ElementAt(random.Next(candidates.Count()));
        }
    }
}