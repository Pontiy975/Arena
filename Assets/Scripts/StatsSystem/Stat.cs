using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arena.StatsSystem
{
    public enum StatType
    {
        Speed,
        Health,
    }

    [Serializable]
    public class Stat
    {
        [field: SerializeField] public StatType Type { get; private set; }
        [field: SerializeField] public int BaseValue { get; private set; }

        public int Value
        {
            get
            {
                if (_isDirty)
                {
                    _value = GetValue();
                    _isDirty = false;
                }

                return _value;
            }
        }

        private List<StatModifier> _modifiers;

        private bool _isDirty = true;
        private int _value;


        public Stat()
        {
            _modifiers = new();
        }

        public Stat(Stat origin)
        {
            Type = origin.Type;
            BaseValue = origin.BaseValue;
            _modifiers = new(origin._modifiers);
            _isDirty = true;
            _value = origin._value;
        }

        public Stat(StatType type) : this()
        {
            Type = type;
            BaseValue = 0;
        }

        public Stat(StatType type, int baseValue) : this()
        {
            Type = type;
            BaseValue = baseValue;
        }

        public void AddModifier(StatModifier modifier)
        {
            _isDirty = true;
            _modifiers.Add(modifier);
            _modifiers.Sort(CompareModifierOrder);
        }

        public void RemoveModifier(StatModifier modifier)
        {
            _isDirty = true;
            _modifiers.Remove(modifier);
        }

        public void Clear()
        {
            _modifiers.Clear();
            _isDirty = true;
        }

        public void Reset()
        {
            _value = BaseValue;
            Clear();
        }

        private int GetValue()
        {
            int baseMult = 1;
            int sumPercentAdd = 0;

            foreach (StatModifier mod in _modifiers)
            {
                if (mod.Type == StatModType.BaseMult)
                    baseMult *= mod.Value;
            }

            int value = BaseValue * baseMult;

            for (int i = 0; i < _modifiers.Count; i++)
            {
                StatModifier mod = _modifiers[i];

                switch (mod.Type)
                {
                    case StatModType.Flat:
                        value += mod.Value;
                        break;

                    case StatModType.PercentAdd:
                        sumPercentAdd += mod.Value;
                        if (i + 1 >= _modifiers.Count || _modifiers[i + 1].Type != StatModType.PercentAdd)
                        {
                            value *= 1 + sumPercentAdd;
                            sumPercentAdd = 0;
                        }
                        break;

                    case StatModType.PercentMult:
                        value *= 1 + mod.Value;
                        break;
                }
            }

            return value;
        }

        private int CompareModifierOrder(StatModifier aMod, StatModifier bMod)
        {
            if (aMod.Order < bMod.Order) return -1;
            else if (aMod.Order > bMod.Order) return 1;
            return 0;
        }
    }
}