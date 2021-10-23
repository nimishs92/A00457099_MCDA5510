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
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            // Load configuration
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            Stopwatch stopWatch = Stopwatch.StartNew();

            SimpleCSVParser CSVParser = new SimpleCSVParser();
            List<string> lstFiles = new DirWalker().Walk1(@"C:\Users\nimis\Downloads\Sample Data\Sample Data");
            List<List<string>> lstRecords = new List<List<string>>();


            Console.WriteLine(lstFiles.Count);

            foreach (var file in lstFiles)
            {
                lstRecords.AddRange(CSVParser.Parse(file));
            }
                     
            Console.WriteLine("Valid Rows : " +  CSVParser.ValidRows);
            Console.WriteLine("Invalid Rows : " + CSVParser.InvalidRows);

            List<string> HeaderFields = CSVParser.GetHeaderFields(lstFiles[0]);
            HeaderFields.Add("Date");

            using (StreamWriter file = new("WriteLines2.csv", append: true))
            {
                string csv = String.Join(",", HeaderFields.Select(x => x.ToString()));
                ExampleAsync(csv, file);
                foreach (var record in lstRecords)
                {
                    //csv += String.Join(",", record.Select(x => x.ToString())) + "\n";
                    ExampleAsync(String.Join(",", record), file);
                }
                //File.WriteAllText("Test.csv", csv);
            }
            stopWatch.Stop();
            Console.WriteLine("Elapsed time {0} ms", stopWatch.ElapsedMilliseconds);
            log.Debug(String.Format("Elapsed time {0} ms", stopWatch.ElapsedMilliseconds));
        }

        public static void ExampleAsync(string data, StreamWriter file)
        {
            file.WriteLine(data);
        }
    }
}
