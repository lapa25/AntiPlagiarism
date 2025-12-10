namespace FileAnalysisService.Entities.ValueObjects
{
    public readonly record struct Similarity
    {
        public double Value { get; }

        public Similarity(double value)
        {
            if (value is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Similarity must be between 0 and 1");
            }

            Value = value;
        }

        public static Similarity Zero => new(0);
        public static Similarity One => new(1);

        public override string ToString()
        {
            return Value.ToString("0.###");
        }
    }
}
