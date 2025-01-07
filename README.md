# DiskSpaceAnalyzer

## Command Line
### Commands:
- `help[h]` - show a help message(DEFAULT)
- `analyzed_dirs[ad]` - show a list of the analyzed directories
- `categories[c]` - show available file categories and their extensions
- `analyze[a]` - run a directory analysis
  - `--all[-a]` - use analyzed directories(DEFAULT)
  - `--repeat[-r]` - re-analysis of the analyzed directories
  - `<path_0> <path_1> ... <path_n>` - paths to source directories, herewith --all[-a] is not DEFAULT
- `info[i]` - show information about the analyzed directories
  - `--all[-a]` - use analyzed directories(DEFAULT)
  - `--repeat[-r]` - re-analysis of the analyzed directories
  - `<path_0> <path_1> ... <path_n>` - paths to source directories, herewith --all[-a] is not DEFAULT
- `sort[s]` - sort the data and save it
  - `--all[-a]` - use analyzed directories(DEFAULT)
  - `--repeat[-r]` - re-analysis of the analyzed directories
  - `--all_categories[-ac]` - use all categories(DEFAULT)
  - `<path_0> <path_1> ... <path_n>` - paths to source directories, herewith --all[-a] is not DEFAULT
  - `<category_1 category_2 ... category_n>` - select categories for sorting.
    Available categories:
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
## Save scheme:
```
<path_to_save>/DataSpaceAnalyzerSortedData/<category_name>/<files>
```
