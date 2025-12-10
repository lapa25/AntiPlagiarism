using FileStoringService.UseCases.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStoringService.UseCases.GetWork
{
    public sealed class GetWorkHandler(IWorkRepository works) : IGetWorkHandler
    {
        private readonly IWorkRepository _works = works;

        public GetWorkResponse? Handle(Guid workId)
        {
            Entities.Models.Work? work = _works.Get(workId);
            return work is null
                ? null
                : new GetWorkResponse(
                work.Id,
                work.StudentId.Value,
                work.AssignmentId.Value,
                work.UploadedAt,
                work.OriginalFileName);
        }
    }
}

