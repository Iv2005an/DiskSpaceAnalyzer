using DiskSpaceAnalyzerLib.Models;
using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Extensions;

public static class CategoriesExtensions
{
    public static string[] GetExtensions(this Categories category)
    {
        return category != Categories.Error
               && category != Categories.Other
            ? Category.CategoriesExtensions[(int)category]
            : [];
    }
}