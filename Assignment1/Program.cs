using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Assignment1
{
    class Program
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            // Load log4net configuration.
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            Stopwatch stopWatch = Stopwatch.StartNew();

            SimpleCSVParser CSVParser = new SimpleCSVParser();
            List<string> lstFiles = new DirWalker().WalkRecursive(@"C:\Users\nimis\Downloads\Sample Data\Sample Data", "*.csv");
            List<List<string>> lstRecords = new List<List<string>>();

            // Parse CSV Files

            foreach (var file in lstFiles)
            {
                lstRecords.AddRange(CSVParser.Parse(file));
            }
                     
            Console.WriteLine("Valid Rows : " +  CSVParser.ValidRows);
            log.Debug("Valid Rows: " +  CSVParser.ValidRows);
            Console.WriteLine("Skipped Rows : " + CSVParser.InvalidRows);
            log.Debug("Skipped Rows : " + CSVParser.InvalidRows);

            // Add date to the headers
            List<string> HeaderFields = CSVParser.GetHeaderFields(lstFiles[0]);
            HeaderFields.Add("Date");

            // Write to the output folder
            using (StreamWriter file = new(@"..\..\..\Output\WriteLines2.csv", append: true))
            {
                string csv = String.Join(",", HeaderFields.Select(x => x.ToString()));
                WriteToFile(csv, file);
                foreach (var record in lstRecords)
                {
                    WriteToFile(String.Join(",", record), file);
                }
            }
            stopWatch.Stop();
            Console.WriteLine("Elapsed time {0} ms", stopWatch.ElapsedMilliseconds);
            log.Debug(String.Format("Elapsed time {0} ms", stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Write data to file.
        /// </summary>
        /// <param name="data">Data to write </param>
        /// <param name="file">Stream Writer represeting the file.</param>
        public static void WriteToFile(string data, StreamWriter file)
        {
            file.WriteLine(data);
        }
    }
}
