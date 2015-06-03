using ArcGisConnection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

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

            Console.WriteLine("Datasets in use on {0}", DateTimeOffset.Now);
            Console.WriteLine("===========================================");
            Console.WriteLine();

            foreach (var dir in dirs)
            {

                Console.WriteLine(dir.FullName);
                Console.WriteLine("--------------------------------");
                Console.WriteLine();

                var dsGroups = from d in dir.GetDatasetInfos().SelectMany(d => d)
                                   where !string.IsNullOrWhiteSpace(d.ConnectionString)
                                   group d by d.ConnectionString;

                foreach (var g in dsGroups)
                {
                    Console.WriteLine("### {0} ###", g.Key);
                    Console.WriteLine();

                    var dict = g.First().GetConnectionStringParts();
                    foreach (var kvp in dict)
                    {
                        Console.WriteLine("* `{0}`:\t`{1}`", kvp.Key, kvp.Value);
                    }

                    Console.WriteLine("#### Tables ####\n");

                    foreach (var ds in g)
                    {
                        Console.WriteLine("* `{0}`", ds.DataSet);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
