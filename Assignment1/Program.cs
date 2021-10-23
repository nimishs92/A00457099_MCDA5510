using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Assignment1
{
    class Program
    {
        // private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            // Load configuration
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var logger = new FileInfo("log4net.config");
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug("Hello World");
            log.Error("Hello World");

            Stopwatch stopWatch = Stopwatch.StartNew();

            SimpleCSVParser CSVParser = new SimpleCSVParser();
            List<string> lstFiles = new DirWalker().Walk1(@"C:\Users\nimis\Downloads\Sample Data\Sample Data\2019");
            List<string[]> lstRecords = new List<string[]>();


            Console.WriteLine(lstFiles.Count);

            foreach (var file in lstFiles)
            {
                lstRecords.AddRange(CSVParser.Parse(file));
            }
            
            stopWatch.Stop();
            //TimeSpan timespan = stopWatch.Elapsed;

            Console.WriteLine("Elapsed time {0} ms", stopWatch.ElapsedMilliseconds);
            Console.WriteLine("Valid Rows : " +  CSVParser.ValidRows);
            Console.WriteLine("Invalid Rows : " + CSVParser.InvalidRows);

            string csv = String.Join(",",CSVParser.GetHeaderFields(lstFiles[0]).Select(x => x.ToString())) + "\n";
            foreach (var record in lstRecords)
            {
                csv += String.Join(",", record.Select(x => x.ToString())) + "\n";
            }
            File.WriteAllText("Test.csv", csv);
            
        }
    }
}
