using DiskSpaceAnalyzerLib.Models;
using SQLite;
using System.Linq.Expressions;

namespace DiskSpaceAnalyzerLib.Databases;

public static class AnalyzedFilesDatabase
{
    private static SQLiteAsyncConnection Connection => Database.Connection;
    private static AsyncTableQuery<AnalyzedFile> Table => Connection.Table<AnalyzedFile>();

    public static async Task<List<AnalyzedFile>> GetFilesAsync() => await Table.ToListAsync();
    public static async Task<List<AnalyzedFile>> GetFilesAsync(Expression<Func<AnalyzedFile, bool>> validator) =>
        await Table.Where(validator).ToListAsync();
    public static async Task<int> GetFilesCountAsync() => await Table.CountAsync();
    public static async Task<int> GetFilesCountAsync(Expression<Func<AnalyzedFile, bool>> validator) =>
        await Table.Where(validator).CountAsync();
    public static async Task AddFileAsync(AnalyzedFile file) => await Connection.InsertAsync(file);
    public static async Task DeleteFilesAsync(Expression<Func<AnalyzedFile, bool>> validator) =>
        await Table.Where(validator).DeleteAsync();
}
