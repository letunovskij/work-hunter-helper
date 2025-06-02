using Common.DtoModels.Files;

namespace WorkHunter.Api.Utils
{
    internal static class FileUtils
    {
        internal static IEnumerable<UploadFileDto> GetFilesContent(IFormFileCollection files)
        {
            foreach (var file in files)
            {
                var lazyStream = new Lazy<Stream>(() =>
                {
                    var ms = new MemoryStream();
                    file.CopyTo(ms);
                    ms.Position = 0;
                    return ms;
                });

                yield return new()
                {
                    FileName = file.FileName,
                    Length = file.Length,
                    Content = lazyStream
                };
            }
        }
    }
}
