using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunter.Abstractions.Files;
using WorkHunter.Models.Entities.Interviews;

namespace WorkHunter.Abstractions.Interviews
{
    public interface IVideoInterviewFileService : IBaseFileService<VideoInterviewFile>
    {
    }
}
