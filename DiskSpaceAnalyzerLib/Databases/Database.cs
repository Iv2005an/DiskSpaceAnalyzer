using DiskSpaceAnalyzerLib.Models;
using SQLite;

namespace DiskSpaceAnalyzerLib.Databases;

public static class Database
{
    private static string _databaseFilename = "DiskSpaceAnalyzerDB.db3";
    public static string DatabaseFilename
    {
        get => _databaseFilename;
        set
        {
            _databaseFilename = value;
            _connection = null;
        }
    }
    private static string? _databasePath;
    public static string DatabasePath
    {
        get => Path.Combine(_databasePath ?? Environment.CurrentDirectory, DatabaseFilename);
        set
        {
            _databasePath = value;
            _connection = null;
        }
    }
    public const SQLiteOpenFlags Flags =
    SQLiteOpenFlags.ReadWrite |
    SQLiteOpenFlags.Create |
    SQLiteOpenFlags.SharedCache;
    private static SQLiteAsyncConnection? _connection;
    public static SQLiteAsyncConnection Connection
    {
        get
        {
            if (_connection is null)
            {
                _connection = new SQLiteAsyncConnection(DatabasePath, Flags);
                _connection.CreateTableAsync<AnalyzedFile>().Wait();
                _connection.CreateTableAsync<AnalyzedDirectory>().Wait();
            }
            return _connection;
        }
    }

}