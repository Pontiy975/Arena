namespace Arena.StatsSystem
{
    public enum StatModType
    {
        Flat,
        PercentAdd,
        PercentMult,
        BaseMult
    }

    public class StatModifier
    {
        public int Value { get; private set; }
        public StatModType Type { get; private set; }
        public int Order { get; private set; }
        public object Source { get; private set; }

        public StatModifier(int value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        public StatModifier(int value, StatModType type) : this(value, type, (int)type, null) { }
        public StatModifier(int value, StatModType type, int order) : this(value, type, order, null) { }
        public StatModifier(int value, StatModType type, object source) : this(value, type, (int)type, source) { }
    }
}
