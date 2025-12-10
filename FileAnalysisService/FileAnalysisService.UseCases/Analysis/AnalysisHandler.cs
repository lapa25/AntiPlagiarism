using FileAnalysisService.Entities.Models;
using FileAnalysisService.Entities.ValueObjects;
using FileAnalysisService.UseCases.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace FileAnalysisService.UseCases.Analysis
{
    public sealed class AnalysisHandler(
        IFileAnalysisRepository repository,
        IFileStoringClient fileStoringClient,
        IWordCloudGenerator wordCloudGenerator) : IAnalysisHandler
    {
        private readonly IFileAnalysisRepository _repository = repository;
        private readonly IFileStoringClient _fileStoringClient = fileStoringClient;
        private readonly IWordCloudGenerator _wordCloudGenerator = wordCloudGenerator;

        public AnalysisResponse Handle(AnalysisRequest request)
        {
            using Stream fileStream = _fileStoringClient.DownloadFile(request.WorkId);

            using MemoryStream ms = new();
            fileStream.CopyTo(ms);
            byte[] bytes = ms.ToArray();

            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(bytes);
            string hashString = Convert.ToHexString(hashBytes);
            FileContentHash hashVo = FileContentHash.Create(hashString);

            FileHash? existing = _repository.FindPlagiarism(hashString, request.StudentId);

            bool isPlagiarism = existing is not null;
            Guid? sourceWorkId = existing?.WorkId;
            Similarity similarity = isPlagiarism ? Similarity.One : Similarity.Zero;

            FileHash hashEntity = new()
            {
                Id = Guid.NewGuid(),
                WorkId = request.WorkId,
                StudentId = StudentId.Create(request.StudentId),
                AssignmentId = AssignmentId.Create(request.AssignmentId),
                UploadedAt = DateTime.UtcNow,
                Hash = hashVo
            };

            _repository.AddHash(hashEntity);

            string text = ExtractText(bytes);
            string? wordCloudUrl = _wordCloudGenerator.GenerateUrl(text);

            Report report = new()
            {
                Id = Guid.NewGuid(),
                WorkId = request.WorkId,
                CreatedAt = DateTime.UtcNow,
                IsPlagiarism = isPlagiarism,
                SourceWorkId = sourceWorkId,
                Similarity = similarity,
                WordCloudUrl = wordCloudUrl
            };

            report = _repository.AddReport(report);
            return new AnalysisResponse(
                report.Id,
                report.WorkId,
                report.IsPlagiarism,
                report.SourceWorkId,
                report.Similarity.Value);
        }

        private static string ExtractText(byte[] bytes)
        {
            try
            {
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
