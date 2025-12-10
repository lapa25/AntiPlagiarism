using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalysisService.UseCases.Analysis
{
    public sealed record AnalysisResponse(
        Guid ReportId,
        Guid WorkId,
        bool IsPlagiarism,
        Guid? SourceWorkId,
        double Similarity
    );
}
