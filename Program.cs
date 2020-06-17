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
            { Console.WriteLine("searching for {0} in {1}", searchPattern, path); }

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

        static void Main(string[] args)
        {
            
            Info.ShowBanner();
            Boolean help = false;
            string path = @"c:\";
            string filesearchPattern = string.Empty;
            string directorysearchPattern = string.Empty;
            Boolean isrecurse = false;
            Boolean isverbose = false;
            Boolean listdrives = false;

            IEnumerable<string> files = null;

            //argparser

            var options = new OptionSet()
            {
                { "p|path=", "path to start the search (defaults to c:\\)", v => path = v },
                { "f|file=","file to search for (accepts wildcard characters)",v=>filesearchPattern= v},
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

                //if no filename and no list-drives -> display help //TODO: directorysearch
                if(string.IsNullOrEmpty(filesearchPattern) && !listdrives)
                {
                    ShowHelp(options);
                }

                if (listdrives)
                { ListDrives(); }
                


                if(!string.IsNullOrEmpty(filesearchPattern))
                {
                    files =EnumerateFiles(path, filesearchPattern, isrecurse, isverbose); 
                    foreach(string file in files)
                    {
                        Console.WriteLine("FILE FOUND:" + file);
                    }
                    Console.WriteLine("search completed.");
                }
             


            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                ShowHelp(options);
                return;
            }
            

            //Info.PrintUseage();
            //IEnumerable<String> files = EnumerateFiles(@"c:\Users\Jean\Desktop", "topsecret*", false,true);
            //foreach ( string file in files)
            //{
            //    Console.WriteLine("FILE FOUND:" + file);
            //}
           

        }
    }
}
