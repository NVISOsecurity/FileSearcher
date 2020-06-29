# FileSearcher

Unmanaged assembly file searcher for when a fully interactive beacon session is not opsec safe enough. 
Has different capabilities as seen in the help function:

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
Disclaimer:
The -v flag (verbose)  displays all folders that have been searched - as you can imagine, for a recursive search this can create A LOT OF SPAM, only use if required!

Example code:

### Search for files containing the word "password" ###
```
Filesearch.exe -s password -f * -p <path you want to search> -r
```

### List all mounted drives
```
Filesearch.exe -D or Filesearch.exe --list-drives

```

