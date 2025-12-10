using FileAnalysisService.UseCases.Abstractions;
using System.Text.RegularExpressions;

namespace FileAnalysisService.Infrastructure
{
    public sealed class WordCloudGenerator : IWordCloudGenerator
    {
        private static readonly Regex NonLetterRegex = new("[^\\p{L}\\p{Nd}\\s]+", RegexOptions.Compiled);

        public string? GenerateUrl(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            string normalized = text.ToLowerInvariant();
            normalized = NonLetterRegex.Replace(normalized, " ");

            string[] tokens = [.. normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries)];

            if (tokens.Length == 0)
            {
                return null;
            }

            IEnumerable<string> limitedTokens = tokens.Take(300);

            string combined = string.Join(" ", limitedTokens);
            string encoded = Uri.EscapeDataString(combined);

            return $"https://quickchart.io/wordcloud?text={encoded}";
        }
    }
}
