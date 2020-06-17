using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearch
{
    public class Info
    {
       public static void ShowBanner()
        {
            Console.WriteLine(" ______ _ _                               _                ");
            Console.WriteLine("|  ____(_) |                             | |              ");
            Console.WriteLine("| |__   _| | ___  ___  ___  __ _ _ __ ___| |__   ___ _ __ ");
            Console.WriteLine("|  __| | | |/ _ |/ __|/ _ || _` | '__/ __| '_ | / _ | '__|");
            Console.WriteLine("| |    | | |  __/|__ |  __/ (_| | | | (__| | | |  __/ |   ");
            Console.WriteLine("|_|    |_|_||___||___/|___||__,_|_|  |___|_| |_||___|_|   ");
            Console.WriteLine("\r Version 0.0.1 by WingsOfDoom \n https://twitter.com/Jean_Maes_1994");
        }

        public static void PrintUseage()
        {
            string usage = @"
              .NET assembly to search files. Can be executed standalone or using your favorite unmanaged CLR loader. 
              Optional Arguments:
                -d not looking for a file, looking for a directory instead.
                -p path to search
                -r recursive search
                - e, --ext file extension to search for
                -h, prints this help.
                FileName, accepts wildcard characters.
               
             Default behaviour:
             If no arguments are provided, the usage is printed. 
             If only a filename is provided, will search c:\ recursively for said file.
            ";
            Console.WriteLine(usage);

        }
    }
}
