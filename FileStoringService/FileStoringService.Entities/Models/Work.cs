using FileStoringService.Entities.ValueObjects;

namespace FileStoringService.Entities.Models
{
    public sealed class Work
    {
    	public Guid Id { get; init; }

    	public StudentId StudentId { get; init; } = default!;

    	public AssignmentId AssignmentId { get; init; } = default!;

    	public DateTime UploadedAt { get; init; }

    	public string StoragePath { get; init; } = default!;

    	public string OriginalFileName { get; init; } = default!;
    }
}
