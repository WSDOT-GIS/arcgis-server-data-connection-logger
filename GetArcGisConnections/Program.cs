using ArcGisConnection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GetArcGisConnections
{
    class Program
    {
        static void Main(string[] args)
        {

            // Get the directories from the arguments.
            var dirs = from a in args
                   where !string.IsNullOrWhiteSpace(a) && Directory.Exists(a)
                   select new DirectoryInfo(a);
            if (dirs.Count() < 1)
            {

                const string fmt = "{0} \"{1}\" \"{2}\" > output.md";
                string[] sampleDirs = { 
                    Assembly.GetEntryAssembly().GetName().Name  , 
                    @"\\myserver1\share$\arcgisserver\directories\arcgissystem\arcgisinput", 
                    @"\\myserver2\share$\arcgisserver\directories\arcgissystem\arcgisinput" 
                };
                string usage = string.Format(fmt, sampleDirs);
                Console.Error.WriteLine("No valid directories were provided");

                Console.Error.WriteLine("Usage:\n\n{0}", usage);
                return;
            }

            dirs.WriteMarkdown(Console.Out);
        }
    }
}
