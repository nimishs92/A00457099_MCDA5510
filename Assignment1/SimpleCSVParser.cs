using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Assignment1
{
    public class SimpleCSVParser
    {
        public int ValidRows { get; set; }
        public int InvalidRows { get; set; }

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
                    if (!parser.EndOfData) parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        //Process row
                        List<string> fields = new List<string>(parser.ReadFields());
                        fields.Add(GetDateFromFileName(fileName));
                        if (!this.IsEmptyField(fields))
                        {
                            final.Add(fields);
                            this.ValidRows += 1;
                        }
                        else
                        {
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
            int year = int.Parse(sptFileName[sptFileName.Length - 4]);
            int month = int.Parse(sptFileName[sptFileName.Length - 3]);
            int day = int.Parse(sptFileName[sptFileName.Length - 2]);

            return new DateTime(year, month, day).ToString();
        }

        public List<string> GetHeaderFields(string fileName)
        {
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
        private bool IsEmptyField(List<string> fields)
        {
            foreach (string field in fields)
            {
                if (string.IsNullOrEmpty(field))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
