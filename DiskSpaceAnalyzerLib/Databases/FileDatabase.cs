using System.Linq.Expressions;
using DiskSpaceAnalyzerLib.Models;
using SQLite;

namespace DiskSpaceAnalyzerLib.Databases;

public static class FileDatabase
{
    private static SQLiteAsyncConnection Connection => Database.Connection;
    private static AsyncTableQuery<AnalyzedFile> Table => Connection.Table<AnalyzedFile>();

    public static async Task<List<AnalyzedFile>> GetFilesAsync()
    {
        return await Table.ToListAsync();
    }

    public static async Task<List<AnalyzedFile>> GetFilesAsync(Expression<Func<AnalyzedFile, bool>> validator)
    {
        return await Table.Where(validator).ToListAsync();
    }

    public static async Task<int> GetFilesCountAsync()
    {
        return await Table.CountAsync();
    }

    public static async Task<int> GetFilesCountAsync(Expression<Func<AnalyzedFile, bool>> validator)
    {
        return await Table.Where(validator).CountAsync();
    }

    public static async Task AddFileAsync(AnalyzedFile file)
    {
        await Connection.InsertAsync(file);
    }

    public static async Task DeleteFilesAsync(Expression<Func<AnalyzedFile, bool>> validator)
    {
        await Table.Where(validator).DeleteAsync();
    }
}