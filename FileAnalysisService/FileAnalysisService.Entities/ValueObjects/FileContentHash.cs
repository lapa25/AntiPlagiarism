using System.Text.RegularExpressions;

namespace FileAnalysisService.Entities.ValueObjects
{
    public sealed record FileContentHash
    {
        private static readonly Regex Hex64Regex = new("^[0-9A-Fa-f]{64}$", RegexOptions.Compiled);

        public string Value { get; }

        private FileContentHash(string value)
        {
            Value = value;
        }

        public static FileContentHash Create(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Hash cannot be empty", nameof(value))
                : !Hex64Regex.IsMatch(value)
                ? throw new ArgumentException("Hash must be 64 hex characters (SHA256)", nameof(value))
                : new FileContentHash(value.ToUpperInvariant());
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
