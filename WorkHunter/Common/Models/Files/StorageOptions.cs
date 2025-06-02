namespace Common.Models.Files;

public class StorageOptions
{
    public required string BasePath { get; set; }

    public required string VideoStorageFolder { get; set; }

    public required IReadOnlyList<string> SupportedFormats { get; set; }

    public required long MaxFileSize { get; set; }

    public int MaxFileCount { get; set; }
}
