using FileAnalysisService.Entities.ValueObjects;
using System;
namespace FileAnalysisService.Entities.Models
{
    public sealed class Report
    {
        public Guid Id { get; init; }

        public Guid WorkId { get; init; }

        public DateTime CreatedAt { get; init; }

        public bool IsPlagiarism { get; init; }

        public Guid? SourceWorkId { get; init; }

        public Similarity Similarity { get; init; }

        public string? WordCloudUrl { get; init; }
    }
}
