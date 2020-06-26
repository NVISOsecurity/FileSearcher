using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;
using System.IO;

namespace FileSearch
{
    class Program
    {
        public static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage:");
            p.WriteOptionDescriptions(Console.Out);
        }
        private static void ListDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  Drive type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                    Console.WriteLine("  File system: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Available space to current user:{0, 15} ",
                        (d.AvailableFreeSpace / 1000000) + "MB" + "/" + (d.AvailableFreeSpace / 1000000000) + "GB");
                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} ",
                        (d.TotalSize / 1000000) + "MB" + "/" + (d.TotalSize / 1000000000) + "GB");
                    Console.WriteLine("\n");
                }
            }
        }

        public static IEnumerable<string> EnumerateDirectories(string path,string searchPattern,Boolean isrecurse,Boolean isverbose)
        {
            SearchOption searchOpt;
            if (isrecurse)
            {
                searchOpt = SearchOption.AllDirectories;

            }
            else
                searchOpt = SearchOption.TopDirectoryOnly;

            if (isverbose)
            { Console.WriteLine("searching for directory: {0} in {1}", searchPattern, path); }

            try
            {
                var directories = Enumerable.Empty<string>();
                if (searchOpt == SearchOption.AllDirectories)
                {
                    directories = Directory.EnumerateDirectories(path)
                                        .SelectMany(x => EnumerateDirectories(x, searchPattern, isrecurse, isverbose));
                }

                return directories.Concat(Directory.EnumerateDirectories(path, searchPattern));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Enumerable.Empty<string>();
            }
            catch (PathTooLongException ex)
            {
                return Enumerable.Empty<string>();
            }

        }
        public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, Boolean isrecurse,Boolean isverbose)
        {
            SearchOption searchOpt;
            if (isrecurse)
            {
                searchOpt = SearchOption.AllDirectories;
           
            }
            else
                searchOpt = SearchOption.TopDirectoryOnly;

            if (isverbose)
            { Console.WriteLine("searching for file {0} in {1}", searchPattern, path); }

            try
            {
                var dirFiles = Enumerable.Empty<string>();
                if (searchOpt == SearchOption.AllDirectories)
                {
                    dirFiles = Directory.EnumerateDirectories(path)
                                        .SelectMany(x => EnumerateFiles(x, searchPattern, isrecurse,isverbose));
                }
                return dirFiles.Concat(Directory.EnumerateFiles(path, searchPattern));

            }
            catch (UnauthorizedAccessException ex)
            {
                return Enumerable.Empty<string>();
            }
            catch(PathTooLongException ex)
            {
                return Enumerable.Empty<string>();
            }
        }

        public static void ReadFiles(IEnumerable<string> dirFiles,string filecontents)
        {
            //throw all garbage out that is not interesting for us (.exe,.zip,.gzip,.tar,.msi,.sys.
            
            HashSet<string> garbageExtensions = new HashSet<string> {"zip","exe","gzip","tar","msi","sys","png","jpeg",
                "gif","svg","7z","JPG","der","mp4","mp3","mov","avi","iso","mid","midi","dll","sys","sav"};

            var fileList = dirFiles.ToList()
            .FindAll(file => !garbageExtensions.Contains(file.Substring(file.LastIndexOf('.') + 1)));
            var files = from file in fileList
                        from line in File.ReadLines(file)
                        where line.Contains(filecontents)
                        select new
                        {
                            File = file,
                            Line = line
                        };
            foreach (var f in files)
            {
                Console.WriteLine("FILE FOUND containing '{0}' : {1}", filecontents, f.File);
                Console.WriteLine("The interesting content of the file is: \n {0}", f.Line + "\n");
                
            }
            Console.WriteLine("search completed.");
            return;
        }

        static void Main(string[] args)
        {
            Boolean help = false;
            Info.ShowBanner();
            string path = @"c:\";
            string filesearchPattern = string.Empty;
            string directorysearchPattern = string.Empty;
            string filecontains = string.Empty;
            Boolean isrecurse = false;
            Boolean isverbose = false;
            Boolean listdrives = false;
            Boolean seachcontents = false;

            IEnumerable<string> files,dirs = null;
            

            //argparser (ndesk options for the win)

            var options = new OptionSet()
            {
                { "p|path=", "path to start the search (defaults to c:\\)", v => path = v },
                { "f|file=","file to search for (accepts wildcard characters)",v=>filesearchPattern= v},
                {"d|dir=","directoryname to search for instead of searching for file", v=>directorysearchPattern = v},
                {"s|string=","",v=>filecontains = v },
                {"r|recurse","search recursively",v=>isrecurse=true },
                {"v|verbose","show live search results",v=>isverbose=true },
                {"D|list-drives","lists all drives on the target", v => listdrives = true },
                {"h|help", "Display this help", v => help = v != null}
            };
         
            try
            {
                options.Parse(args);

                if (help)
                {
                    ShowHelp(options);
                    return;
                }

                //can't search for a dir and a file at the same time, technically it should be possible, but I think the filesystem will explode.
                if(!string.IsNullOrEmpty(filesearchPattern) && !string.IsNullOrEmpty(directorysearchPattern))
                {
                    Console.Error.Write("\n\nEither search for a directory or a file, not both!! \n\n");
                    return;
                }

                //if no filename and no list-drives -> display help //TODO: directorysearch
                if((string.IsNullOrEmpty(filesearchPattern) & string.IsNullOrEmpty(directorysearchPattern)) && !listdrives)
                {
                    ShowHelp(options);
                    return;
                }

                //list drive if listdrive is passed
                if (listdrives)
                { ListDrives(); }
                
                // logic for filesearch 
                if(!string.IsNullOrEmpty(filesearchPattern))
                {
                    if (!string.IsNullOrEmpty(filecontains))
                        seachcontents = true;
                    Console.WriteLine("file search started...");
                    files = EnumerateFiles(path, filesearchPattern, isrecurse, isverbose);
                    if(!string.IsNullOrEmpty(filecontains))
                    {
                        ReadFiles(files, filecontains);
                        return;
                    }
                    foreach(string file in files)
                    {
                        Console.WriteLine("FILE FOUND:" + file);
                    }
                    Console.WriteLine("filesearch completed.");
                    return;
                }


                //logic for dirsearch
                if(!string.IsNullOrEmpty(directorysearchPattern))
                {
                    Console.WriteLine("dir search started...");
                    dirs = EnumerateDirectories(path, directorysearchPattern, isrecurse, isverbose);
                    foreach(string dir in dirs)
                    {
                        Console.WriteLine("DIRECTORY FOUND:" + dir);
                    }
                    Console.WriteLine("dir search completed.");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                ShowHelp(options);
                return;
            }
            

        }
    }
}
