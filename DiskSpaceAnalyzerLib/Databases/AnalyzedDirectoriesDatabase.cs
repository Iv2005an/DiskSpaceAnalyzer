using DiskSpaceAnalyzerLib.Models;
using SQLite;
using System.Linq.Expressions;

namespace DiskSpaceAnalyzerLib.Databases;

public static class AnalyzedDirectoriesDatabase
{
    private static SQLiteAsyncConnection Connection => Database.Connection;
    private static AsyncTableQuery<AnalyzedDirectory> Table => Connection.Table<AnalyzedDirectory>();

    public static async Task<List<AnalyzedDirectory>> GetDirectoriesAsync() => await Table.ToListAsync();
    public static async Task<List<AnalyzedDirectory>> GetDirectoriesAsync(
        Expression<Func<AnalyzedDirectory, bool>> validator) =>
        await Table.Where(validator).ToListAsync();
    public static async Task<int> GetDirectoriesCount() => await Table.CountAsync();
    public static async Task AddDirectoryAsync(AnalyzedDirectory directory) =>
        await Connection.InsertAsync(directory);
    public static async Task DeleteDirectoryAsync(
        Expression<Func<AnalyzedDirectory, bool>> validator) =>
        await Table.Where(validator).DeleteAsync();
}
