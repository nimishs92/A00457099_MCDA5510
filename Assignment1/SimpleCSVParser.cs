using log4net;
using log4net.Config;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Assignment1
{
    public class SimpleCSVParser
    {
        /// <summary>
        /// Total number of valid rows.
        /// </summary>
        public int ValidRows { get; set; }
        /// <summary>
        /// Total number of invalid rows.
        /// </summary>
        public int InvalidRows { get; set; }
        /// <summary>
        /// Header Fields of the CSV.
        /// </summary>
        private string[] HeaderFields { get; set; }
        /// <summary>
        /// Logger.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleCSVParser()
        {
            // Load configuration
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// Parse the CSV file and return valid rows. 
        /// </summary>
        /// <param name="fileName">CSV file name with full path</param>
        /// <returns>List of valid rows</returns>
        public IList<List<String>> Parse(string fileName)
        {
            List<List<string>> final = new List<List<string>>();
            try
            {
                using (TextFieldParser parser = new TextFieldParser(fileName))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    // Ignore headers.
                    if (!parser.EndOfData) this.HeaderFields = parser.ReadFields();

                    int lineCount = 1;
                    while (!parser.EndOfData)
                    {
                        //Process row
                        List<string> fields = new List<string>(parser.ReadFields());
                        fields.Add(GetDateFromFileName(fileName));

                        string missingFieldname = string.Empty;
                        lineCount++;
                        if (!this.IsEmptyField(fields, out missingFieldname))
                        {
                            final.Add(fields);
                            this.ValidRows += 1;
                        }
                        else
                        {
                            log.Debug(String.Format("Field {0} on line {1} is missing in file {2}",missingFieldname,lineCount,fileName));
                            InvalidRows += 1;
                        }
                    }
                }
                return final;
            }
            catch (IOException ioe)
            {
                throw;
            }
        }
        /// <summary>
        /// Get date from the filepath.
        /// </summary>
        /// <param name="fileName">CSV file name with full path</param>
        /// <returns>Date time seralized as string.</returns>
        private string GetDateFromFileName(string fileName)
        {
            string[] sptFileName = fileName.Split(@"\");
            int year    = int.Parse(sptFileName[sptFileName.Length - 4]);
            int month   = int.Parse(sptFileName[sptFileName.Length - 3]);
            int day     = int.Parse(sptFileName[sptFileName.Length - 2]);

            return new DateTime(year, month, day).ToString();
        }
        /// <summary>
        /// Get header fields.
        /// </summary>
        /// <param name="fileName">CSV file name with full path</param>
        /// <returns>List of header fields</returns>
        public List<string> GetHeaderFields(string fileName)
        {
            if (this.HeaderFields != null)
            {
                return new List<string>(this.HeaderFields);
            }
            try
            {
                using (TextFieldParser parser = new TextFieldParser(fileName))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    // Ignore headers.
                    if (!parser.EndOfData) return new List<string>(parser.ReadFields());
                    else throw new Exception("No Header Found");
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if there is an empty field in the provided list. 
        /// </summary>
        /// <param name="fields">List of fields.</param>
        /// <param name="fieldName">Out Param, shows which field is empty.</param>
        /// <returns>True if there is an empty field else false.</returns>
        private bool IsEmptyField(List<string> fields, out string fieldName)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (string.IsNullOrEmpty(field))
                {
                    fieldName = this.HeaderFields[i];
                    return true;
                }
            }
            fieldName = string.Empty;
            return false;
        }
    }
}
