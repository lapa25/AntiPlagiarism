using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStoringService.UseCases.GetWork
{
    public sealed record GetWorkResponse(
        Guid Id,
        string StudentId,
        string AssignmentId,
        DateTime UploadedAt,
        string OriginalFileName
    );
}
