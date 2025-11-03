using Arena.StatsSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Arena.Components
{
    public class StatsComponent : MonoBehaviour
    {
        private Dictionary<StatType, Stat> _stats = new();
        private Dictionary<StatType, List<StatModifier>> _permanentModifiers = new();

        public Stat this[StatType k] => _stats.TryGetValue(k, out var stat) ? stat : null;
        public Dictionary<StatType, Stat> Stats => _stats;

        public void SetBaseStats(IEnumerable<Stat> baseStats)
        {
            _stats.Clear();

            foreach (var baseStat in baseStats)
            {
                Stat stat = new(baseStat);
                //stat.AddModifier(new(modifier, StatModType.BaseMult));
                _stats.Add(baseStat.Type, stat);
            }
        }

        public void AddPermanentModifier(StatType type, StatModifier modifier)
        {
            //Debug.Log($"Permanent / {name} / {type}: {modifier.Type} {modifier.Value} {modifier.Source}");

            if (!_stats.ContainsKey(type))
                _stats[type] = new(type);

            if (_permanentModifiers.TryGetValue(type, out var list))
            {
                list.Add(modifier);
                return;
            }

            _permanentModifiers[type] = new() { modifier };
        }

        public void AddTemporaryModifier(StatType type, StatModifier modifier)
        {
            //Debug.Log($"Temporary / {name} / {type}: {modifier.Type} {modifier.Value} {modifier.Source}");

            if (!_stats.ContainsKey(type))
                _stats[type] = new(type);

            _stats[type].AddModifier(modifier);
        }

        public void RecalculateStats()
        {
            foreach (var stat in _stats.Values)
                stat.Reset();

            foreach (var modifiers in _permanentModifiers)
            {
                for (int i = 0; i < modifiers.Value.Count; i++)
                {
                    _stats[modifiers.Key].AddModifier(modifiers.Value[i]);
                }
            }
        }

        public void Clear()
        {
            _stats.Clear();
            _permanentModifiers.Clear();
        }

        public Dictionary<StatType, List<StatModifier>> GetModifiersSnapshot()
        {
            return new(_permanentModifiers);
        }

        public void RestoreModifiersFromSnapshot(Dictionary<StatType, List<StatModifier>> modifiers)
        {
            if (modifiers != null)
            {
                _permanentModifiers = new(modifiers);
                RecalculateStats();
            }
        }

        public void AddPermanentModifiers(Dictionary<StatType, List<StatModifier>> modifiers)
        {
            foreach (var stat in modifiers)
            {
                if (_permanentModifiers.TryGetValue(stat.Key, out List<StatModifier> value))
                    value.AddRange(stat.Value);
                else
                    _permanentModifiers[stat.Key] = stat.Value;
            }

            RecalculateStats();
        }

        public Dictionary<StatType, StatModifier> GetValuesAsModifiers()
        {
            Dictionary<StatType, StatModifier> modifiers = new();

            foreach (var kvp in _stats)
            {
                modifiers[kvp.Key] = new(kvp.Value.Value, StatModType.Flat, "Weapon");
            }

            return modifiers;
        }
    }
}
