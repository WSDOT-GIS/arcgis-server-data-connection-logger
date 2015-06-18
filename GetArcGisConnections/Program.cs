using ArcGisConnection;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GetArcGisConnections
{
    class Program
    {
        static IEnumerable<DirectoryInfo> GetDirectoryInfos(IEnumerable<string> paths)
        {
            return from s in paths
                   where !string.IsNullOrWhiteSpace(s) && Directory.Exists(s)
                   select new DirectoryInfo(s);
        }

        static void Main(string[] args)
        {
            IEnumerable<DirectoryInfo> dirs;

            // Get the directories from the arguments.
            dirs = GetDirectoryInfos(args);

            // If no directories are provided at the command line, use configuration
            if (dirs.Count() < 1)
            {
                string directoryList = ConfigurationManager.AppSettings.Get("directories");
                dirs = GetDirectoryInfos(directoryList.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }

            if (dirs.Count() < 1)
            {

                const string fmt = "{0} [\"{1}\" \"{2}\"]";
                string[] sampleDirs = { 
                    Assembly.GetEntryAssembly().GetName().Name  , 
                    @"\\myserver1\share$\arcgisserver\directories\arcgissystem\arcgisinput", 
                    @"\\myserver2\share$\arcgisserver\directories\arcgissystem\arcgisinput" 
                };
                string usage = string.Format(fmt, sampleDirs);
                Console.Error.WriteLine("No valid directories were provided. Provide directories either from the command line or the config file.");

                Console.Error.WriteLine("Usage:\n\n{0}", usage);
                return;
            }

            var outputPath = string.Format("output {0:s}.csv", DateTimeOffset.Now).Replace(':', '-');
            using (var writer = new StreamWriter(outputPath))
            using (var csvWriter = new CsvWriter(writer))
            {
                var records = dirs.GetFlattenedOutput();
                csvWriter.WriteRecords(records);
            }
        }
    }
}
