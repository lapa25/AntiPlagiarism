using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStoringService.UseCases.GetWorkFile
{
    public interface IGetWorkFileHandler
    {
        GetWorkFileResponse? Handle(Guid workId);
    }
}
