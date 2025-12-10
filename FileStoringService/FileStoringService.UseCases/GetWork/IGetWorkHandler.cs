using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStoringService.UseCases.GetWork
{
    public interface IGetWorkHandler
    {
        GetWorkResponse? Handle(Guid workId);
    }
}
