namespace FileAnalysisService.Entities.ValueObjects
{
    public sealed record AssignmentId
    {
        public string Value { get; }

        private AssignmentId(string value)
        {
            Value = value;
        }

        public static AssignmentId Create(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("AssignmentId cannot be empty", nameof(value))
                : new AssignmentId(value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
