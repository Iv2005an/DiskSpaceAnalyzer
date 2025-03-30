using DiskSpaceAnalyzerLib.Models;
using SQLite;

namespace DiskSpaceAnalyzerLib.Databases;

public static class Database
{
    private const SQLiteOpenFlags Flags =
        SQLiteOpenFlags.ReadWrite |
        SQLiteOpenFlags.Create |
        SQLiteOpenFlags.SharedCache;

    private static string _databaseFilename = "DiskSpaceAnalyzerDB.db3";
    private static string? _databasePath;
    private static SQLiteAsyncConnection? _connection;

    public static string DatabaseFilename
    {
        get => _databaseFilename;
        set
        {
            _databaseFilename = value;
            _connection = null;
        }
    }

    public static string DatabasePath
    {
        get => Path.Combine(_databasePath ?? Environment.CurrentDirectory, DatabaseFilename);
        set
        {
            _databasePath = value;
            _connection = null;
        }
    }

    public static SQLiteAsyncConnection Connection
    {
        get
        {
            if (_connection is not null) return _connection;
            _connection = new(DatabasePath, Flags);
            _connection.CreateTableAsync<AnalyzedFile>().Wait();
            _connection.CreateTableAsync<AnalyzedDirectory>().Wait();
            return _connection;
        }
    }
}