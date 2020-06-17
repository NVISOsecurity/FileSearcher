# FileSearcher

Enumeration tool written in C#, useful for passive reconnaisance when you are not able to be fully interactive. 
Written for Cobalt-Strike's Execute-Assembly, but can be used in any C2 supporting .NET assembly executions or any unmanaged CLR loader.

I do not recommend using this to enumerate an entire file system, although it's possible...
The best approach would be to enumerate directories you think are interesting, instead of just blasting the entire file system.

The verbose flag will show all directories searched in the output, useful for when you want to find additional interesting folders.



```
 ______ _ _                               _
|  ____(_) |                             | |
| |__   _| | ___  ___  ___  __ _ _ __ ___| |__   ___ _ __
|  __| | | |/ _ |/ __|/ _ || _` | '__/ __| '_ | / _ | '__|
| |    | | |  __/|__ |  __/ (_| | | | (__| | | |  __/ |
|_|    |_|_||___||___/|___||__,_|_|  |___|_| |_||___|_|
 Version 1.0.1 - NVISO
 Developed by: https://twitter.com/Jean_Maes_1994

Usage:
  -p, --path=VALUE           path to start the search (defaults to c:\)
  -f, --file=VALUE           file to search for (accepts wildcard characters)
  -d, --dir=VALUE            directoryname to search for instead of searching
                               for file
  -s, --string=VALUE
  -r, --recurse              search recursively
  -v, --verbose              show live search results
  -D, --list-drives          lists all drives on the target
  -h, --help                 Display this help
```

## Usage
Example usage to search for files containing the keyword "password" on a users desktop

```
FileSearch.exe -r -s password -f * -p C:\Users\johndoe\Desktop
```
