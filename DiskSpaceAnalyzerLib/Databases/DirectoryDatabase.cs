using System.Linq.Expressions;
using DiskSpaceAnalyzerLib.Models;
using SQLite;

namespace DiskSpaceAnalyzerLib.Databases;

public static class DirectoryDatabase
{
    private static SQLiteAsyncConnection Connection => Database.Connection;
    private static AsyncTableQuery<AnalyzedDirectory> Table => Connection.Table<AnalyzedDirectory>();

    public static async Task<List<AnalyzedDirectory>> GetDirectoriesAsync()
    {
        return await Table.ToListAsync();
    }

    public static async Task<List<AnalyzedDirectory>> GetDirectoriesAsync(
        Expression<Func<AnalyzedDirectory, bool>> validator)
    {
        return await Table.Where(validator).ToListAsync();
    }

    public static async Task<int> GetDirectoriesCountAsync()
    {
        return await Table.CountAsync();
    }

    public static async Task<int> GetDirectoriesCountAsync(
        Expression<Func<AnalyzedDirectory, bool>> validator)
    {
        return await Table.Where(validator).CountAsync();
    }

    public static async Task AddDirectoryAsync(AnalyzedDirectory directory)
    {
        await Connection.InsertAsync(directory);
    }

    public static async Task DeleteDirectoriesAsync(
        Expression<Func<AnalyzedDirectory, bool>> validator)
    {
        await Table.Where(validator).DeleteAsync();
    }
}