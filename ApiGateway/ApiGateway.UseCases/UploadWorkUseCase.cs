using ApiGateway.UseCases.Abstractions;
using ApiGateway.UseCases.DTOS;
using Microsoft.Extensions.Logging;

namespace ApiGateway.UseCases
{

    public sealed record UploadWorkCommand(
        Stream FileStream,
        string FileName,
        string ContentType,
        string StudentId,
        string AssignmentId
    );

    public sealed class UploadWorkUseCase(
        IFileStoringGatewayClient fileStoring,
        IFileAnalysisGatewayClient fileAnalysis,
        ILogger<UploadWorkUseCase> logger)
    {
        private readonly IFileStoringGatewayClient _fileStoring = fileStoring;
        private readonly IFileAnalysisGatewayClient _fileAnalysis = fileAnalysis;
        private readonly ILogger<UploadWorkUseCase> _logger = logger;

        public async Task<UploadWorkResultDto> ExecuteAsync(
            UploadWorkCommand command,
            CancellationToken cancellationToken = default)
        {
            WorkCreatedDto stored = await _fileStoring.UploadWorkAsync(
                command.FileStream,
                command.FileName,
                command.ContentType,
                command.StudentId,
                command.AssignmentId,
                cancellationToken);

            string reportStatus = "AnalysisFailed";
            AnalysisResultDto? analysisResult = null;

            try
            {
                AnalysisDto analysisRequest = new(
                    stored.WorkId,
                    stored.StudentId,
                    stored.AssignmentId);

                analysisResult = await _fileAnalysis.AnalysisAsync(analysisRequest, cancellationToken);

                if (analysisResult is not null)
                {
                    reportStatus = "Completed";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while running analysis for work {WorkId} (student {StudentId}, assignment {AssignmentId})",
                    stored.WorkId,
                    stored.StudentId,
                    stored.AssignmentId);
            }

            return new UploadWorkResultDto(
                stored.WorkId,
                reportStatus,
                analysisResult);
        }
    }
}
