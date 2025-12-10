namespace FileAnalysisService.Entities.ValueObjects
{
    public sealed record StudentId
    {
        public string Value { get; }

        private StudentId(string value)
        {
            Value = value;
        }

        public static StudentId Create(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("StudentId can`t be empty", nameof(value))
                : new StudentId(value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
