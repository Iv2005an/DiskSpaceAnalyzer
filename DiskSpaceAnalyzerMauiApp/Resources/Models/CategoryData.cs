using DiskSpaceAnalyzerLib.Models;

namespace DiskSpaceAnalyzerMauiApp.Resources.Models;

public static class CategoryData
{
    private static List<Color> CategoryColors =>
    [
        Colors.Orange,
        Colors.Purple,
        Colors.DodgerBlue,
        Colors.DeepPink,
        Colors.Olive,
        Colors.Cyan,
        Colors.DarkSlateBlue,
        Colors.OrangeRed,
        Colors.Green,
        Colors.Grey,
        Colors.MediumPurple,
        Colors.RoyalBlue,
        Colors.PaleVioletRed,
        Colors.DarkGoldenrod,
        Colors.LightGreen,
        Colors.RosyBrown,
        Colors.Red,
    ];

    public static Color GetColor(Category.Categories category) => CategoryColors[(int)category];

    private static List<string> CategoryNames =>
    [
        "Изображения",
        "Вектор",
        "Текст",
        "Аудио",
        "Видео",
        "Электронные книги",
        "Сапр",
        "Презентации",
        "Электронные таблицы",
        "Базы данных",
        "Архивы",
        "Веб",
        "Разработка",
        "Системные",
        "Исполняемые файлы",
        "Настройки",
        "Другое",
        "Ошибки",
    ];

    public static string GetName(Category.Categories category) => CategoryNames[(int)category];

    public static string GetReadableWeight(long weight)
    {
        string suffix;
        double readable;
        switch (weight)
        {
            // Exabyte
            case >= 0x1000000000000000:
                suffix = "EB";
                readable = (weight >> 50);
                break;
            // Petabyte
            case >= 0x4000000000000:
                suffix = "PB";
                readable = (weight >> 40);
                break;
            // Terabyte
            case >= 0x10000000000:
                suffix = "TB";
                readable = (weight >> 30);
                break;
            // Gigabyte
            case >= 0x40000000:
                suffix = "GB";
                readable = (weight >> 20);
                break;
            // Megabyte
            case >= 0x100000:
                suffix = "MB";
                readable = (weight >> 10);
                break;
            // Kilobyte
            case >= 0x400:
                suffix = "KB";
                readable = weight;
                break;
            default:
                return weight.ToString("0 B"); // Byte
        }

        readable /= 1024;
        return readable.ToString("0.### ") + suffix;
    }
}