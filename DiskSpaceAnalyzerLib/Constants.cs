using SQLite;

namespace DiskSpaceAnalyzerLib;

public static class Constants
{
    public const string DatabaseFilename = "DiskSpaceAnalyzerDB.db3";
    public const SQLiteOpenFlags Flags =
        SQLiteOpenFlags.ReadWrite |
        SQLiteOpenFlags.Create |
        SQLiteOpenFlags.SharedCache;
    private static string? _databasePath;
    public static string DatabasePath
    {
        get => Path.Combine(_databasePath ?? Environment.CurrentDirectory, DatabaseFilename);
        set { _databasePath = value; }
    }

    public enum FileTypes
    {
        Raster,
        Vector,
        Text,
        Audio,
        Video,
        EBook,
        CAD,
        Presentation,
        Spreadsheet,
        Database,
        Archive,
        Web,
        Developer,
        System,
        Executables,
        Settings,
        Other,
        Error,
    }
    public static readonly string[][] FileTypesExtensions =
    [
        [
            "ART", "ARW", "BMP", "CR", "CRW", "DCM", "DDS", "DJVU", "DNG", "EXR", "FPX", "GIF", "ICO", "JPG", "JP",
            "JPEG", "NEF", "ORF", "PCD", "PCX", "PEF", "PGM", "PICT", "PNG", "PSD", "RAF", "SFW", "TGA", "TIFF", "WBMP",
            "XCF", "YUV", "KDC", "PCT", "SR", "TIF", "HDR", "WEBP", "NRW", "PLIST", "ITHMB", "THM", "PSPIMAGE", "MAC",
            "HEIC", "RWL", "FLIF", "AVIF", "RAW", "PICTCLIPPING", "JXR", "CR2"
        ],
        [
            "EMF", "EPS", "SVG", "WPG", "AI", "SVGZ", "WMF", "ODG", "CDR", "VSD", "STD", "PD", "EMZ", "MIX", "OTG",
            "CVS", "GVDESIGN"
        ],
        [
            "PDF", "TXT", "DOC", "ODT", "XPS", "CHM", "RTF", "SXW", "DOCX", "WPD", "WPS", "DOCM", "HWP", "PUB", "XML",
            "LOG", "OXPS", "VNT", "DOT", "PAGES", "M3U", "DOTX", "SHS", "MSG", "ODM", "PMD", "VMG", "EML", "TEX", "WP5",
            "CSK", "FDXT", "ADOC", "AFPUB"
        ],
        [
            "MPC", "AAC", "FLAC", "M4A", "MMF", "MP3", "OGG", "WAV", "WMA", "MID", "AMR", "APE", "AU", "CAF", "GSM",
            "OMA", "QCP", "VQF", "RA", "AIF", "MP2", "M4P", "AWB", "M4R", "RAM", "ASX", "MPGA", "AIFF", "KOZ", "M4B",
            "KAR", "IFF", "MIDI", "3GA", "OPUS", "AUP", "XSPF", "AIFC", "RTA", "CDA", "M3U8", "MPA", "AA", "AAX", "OGA",
            "NFA", "ADPCM", "CDO", "FLP", "AIMPPL", "4MP", "MUI"
        ],
        [
            "AVI", "MPEG", "M4V", "MOV", "MP4", "WMV", "MPG", "SWF", "3GP", "3G2", "MKV", "OGV", "WEBM", "ASF", "TS",
            "MXF", "RM", "THP", "MTS", "RMVB", "F4V", "MOD", "VOB", "H264", "FLV", "3GPP", "DIVX", "QT", "AMV", "DVSD",
            "M2TS", "IFO", "MSWMM", "SRT", "CPI", "WLMP", "VPJ", "CED", "VEP", "VEG", "264", "DAV", "PDS", "DIR", "ARF",
            "MEPX", "XESC", "BIK", "NFV", "TVS", "IMOVIEMOBILE", "RCPROJECT", "ESP3", "VPROJ", "AEP", "CAMPROJ",
            "CAMREC", "CMPROJ", "CMREC", "MODD", "MPROJ", "OSP", "TREC", "G64", "VRO", "BRAW", "MSE", "PZ"
        ],
        [
            "CBR", "EPUB", "FB2", "LIT", "LRF", "MOBI", "TCR", "PRC", "AZW3", "AZW", "ACSM", "OPF", "MBP", "CBZ", "APNX",
            "CBT", "VBK", "IBOOKS", "KFX"
        ],
        ["DXF", "DWG", "3DM", "3DS", "MAX", "OBJ", "STP"],
        ["ODP", "PPT", "PPTX", "PPS", "PPSX", "PPTM", "KEY", "FLIPCHART"],
        ["ODS", "XLS", "XLSX", "CSV", "WKS", "XLSM", "XLSB", "XLR", "WK3", "NUMBERS"],
        ["PDB", "DBF", "BUP", "DB-JOURNAL", "DB", "DB3", "SQLITE", "SQLITE3", "CRYPT", "ACCDB", "MDB", "SQL"],
        [
            "ZIP", "RAR", "7Z", "BZ2", "GZ", "TAR", "SNB", "JAR", "APK", "MHT", "TGZ", "GZIP", "CRX", "DEB", "RPM",
            "SITX", "TAR.GZ", "ZIPX", "SIT", "ACE", "DD", "R01", "MPKG", "PUP", "TBZ"
        ],
        [
            "HTML", "HTM", "XHTML", "ASPX", "PHP", "JS", "NZB", "JSON", "DO", "CSS", "WEBLOC", "XFDL", "ASP", "CER",
            "CFM", "CSR", "JSP", "RSS", "CFML", "MHTML", "WEBARCHIVE"
        ],
        [
            "RC", "P", "D", "C", "CLASS", "CPP", "CS", "DTD", "FLA", "H", "JAVA", "LUA", "M", "PL", "PY", "SH", "SLN",
            "SWIFT", "VCXPROJ", "XCODEPROJ", "ASC", "BAS", "ASM", "CBL", "VBP", "IWB", "PB", "YML", "PIKA", "S19", "XT",
            "SUO", "FSPROJ", "PBJ", "PBXUSER", "PYW", "XQ", "CD", "SB", "SB2", "ISE", "KV", "COD", "NIB", "PWN", "B",
            "HPP", "APA", "BET", "BLUEJ", "ERB", "FXC", "M4", "OWL", "SMA", "TRX", "VC", "DEF", "XAP", "O", "PAS", "QPR",
            "RESOURCES", "VBPROJ", "VBX", "XIB", "MD", "CCC", "WWP", "SS"
        ],
        [
            "CUR", "ANI", "DVD", "DAT", "LNK", "DLL", "NFO", "PROP", "BIN", "CAB", "CPL", "DESKTHEMEPACK", "DMP", "DRV",
            "ICNS", "SYS", "CAT", "HLP", "ICL", "BUD", "DEV", "FFL", "FFO", "POL", "ION", "PNF", "DIT", "HTT", "NB0",
            "MLC", "BASH_HISTORY", "EBD", "FOTA", "REG", "BASHRC", "HIV", "BASH_PROFILE", "NT", "QVM", "ICONPACKAGE",
            "IPTHEME", "PCK", "HDMP", "MDMP", "SDT", "TTF", "OTF", "DMG", "TMP", "URL", "IMG", "BAK", "TORRENT", "MSI",
            "CRDOWNLOAD", "ISO", "HEX"
        ],
        ["SCR", "EXE", "IPA", "APP", "BAT", "PS1", "CGI", "COM", "GADGET", "PIF", "VB", "WSF", "CMD", "DS", "AIR"],
        [
            "ACT", "INF", "INI", "CFG", "PKG", "CDT", "ICM", "GID", "API", "DUN", "HT", "PRF", "FM3", "RDF", "ASW",
            "MIND", "TSCPROJ"
        ]
    ];
}
