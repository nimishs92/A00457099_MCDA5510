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
        public int ValidRows { get; set; }
        public int InvalidRows { get; set; }
        private string[] HeaderFields { get; set; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SimpleCSVParser()
        {
            // Load configuration
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

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

        private string GetDateFromFileName(string fileName)
        {
            string[] sptFileName = fileName.Split(@"\");
            int year    = int.Parse(sptFileName[sptFileName.Length - 4]);
            int month   = int.Parse(sptFileName[sptFileName.Length - 3]);
            int day     = int.Parse(sptFileName[sptFileName.Length - 2]);

            return new DateTime(year, month, day).ToString();
        }

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
