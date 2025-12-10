using FileAnalysisService.Entities.ValueObjects;
using System;
namespace FileAnalysisService.Entities.Models
{
    public sealed class FileHash
    {
        public Guid Id { get; init; }

        public Guid WorkId { get; init; }

        public StudentId StudentId { get; init; } = default!;

        public AssignmentId AssignmentId { get; init; } = default!;

        public DateTime UploadedAt { get; init; }

        public FileContentHash Hash { get; init; } = default!;
    }
}
