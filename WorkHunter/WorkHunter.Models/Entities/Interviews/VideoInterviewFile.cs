using Common.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Entities.WorkHunters;

namespace WorkHunter.Models.Entities.Interviews
{
    public sealed class VideoInterviewFile : BaseFile
    {
        public Guid WResponseId { get; set; }

        public WResponse? WResponse { get; set; }

        public required string CreatedById { get; set; }

        public User? CreatedBy { get; set; }
    }
}
