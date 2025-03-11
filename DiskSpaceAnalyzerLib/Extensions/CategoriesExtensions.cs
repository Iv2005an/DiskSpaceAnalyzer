using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Extensions;

public static class CategoriesExtensions
{
    public static string[] GetExtensions(this Categories category) =>
        category != Categories.Error
        && category != Categories.Other
        ? Models.Category.CategoriesExtensions[(int)category] : [];
}
