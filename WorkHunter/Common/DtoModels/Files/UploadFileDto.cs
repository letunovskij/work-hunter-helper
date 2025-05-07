using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DtoModels.Files
{
    public sealed record UploadFileDto
    {
        public string? FileName { get; set; }

        public long Length { get; set; }

        public required Lazy<Stream> Content { get; set; }
    }
}
