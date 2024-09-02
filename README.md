# DiskSpaceAnalyzer

## Command Line
### Commands:
- `help[h]` - show a help message(DEFAULT)
- `analyzed_dirs[ad]` - show a list of the analyzed directories
- `categories[c]` - show available file categories and their extensions
- `analyze[a]` - run a directory analysis
  - `--all[-a]` - use analyzed directories(DEFAULT)
  - `--repeat[-r]` - re-analysis of the analyzed directories
  - `<path_0> <path_1> ... <path_n>` - paths to directories for analysis, herewith --all[-a] is not DEFAULT
- `percentages[p]` - show a percentage of the categories in the analyzed directories
  - `--all[-a]` - use analyzed directories(DEFAULT)
  - `--repeat[-r]` - re-analysis of the analyzed directories
  - `<path_0> <path_1> ... <path_n>` - paths to directories for analysis, herewith --all[-a] is not DEFAULT
- `sort[s]` - sort the data and save it
  - `--all[-a]` - use analyzed directories(DEFAULT)
  - `--repeat[-r]` - re-analysis of the analyzed directories
  - `--all_categories[-ac]` - use all categories(DEFAULT)
  - `<path_0> <path_1> ... <path_n>` - paths to directories for analysis, herewith --all[-a] is not DEFAULT
  - `<category_1 category_2 ... category_n>` - select categories for sorting. Available categories:
      - Raster
      - Vector
      - Text
      - Audio
      - Video
      - EBook
      - CAD
      - Presentation
      - Spreadsheet
      - Database
      - Archive
      - Web
      - Developer
      - System
      - Executables
      - Settings
      - Other
  - `<path_to_save>` - path to directory for saving sorted data(REQUIRED)
### Examples:
```
./dsa.exe
./dsa.exe help
./dsa.exe h
./dsa.exe a -a E:\ D:\
./dsa.exe s D:\SortedData
./dsa.exe s -a E:\additional_photos D:\SortedData
./dsa.exe s Presentation Spreadsheet Text E:\CollegeFiles D:\SortedCollegeFiles
```
## Save schemes:
```
<path_to_save>/
  /Raster/<Year>/<Month>/<Day>/<files>
  /Vector/<files>
  /Text/<files>
  /Audio/<Album>/<files>
  /Video/<Year>/<Month>/<Day>/<files>
  /EBook/<files>
  /CAD/<files>
  /Presentation/<files>
  /Spreadsheet/<files>
  /Database/<files>
  /Archive/<files>
  /Web/<files>
  /Developer/<files>
  /System/<files>
  /Executables/<files>
  /Settings/<files>
  /Other/<files>
  duplicates.txt
```
