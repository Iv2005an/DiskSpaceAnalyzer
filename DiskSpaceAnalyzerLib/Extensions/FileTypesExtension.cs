using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerLib.Extensions;

public static class FileTypesExtension
{
    public static string[] GetExtensions(this FileTypes fileType) =>
        fileType != FileTypes.Error
        && fileType != FileTypes.Other
        ? FileTypesExtensions[(int)fileType] : [];
}
