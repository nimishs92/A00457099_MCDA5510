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

        public IList<string[]> Parse(string fileName)
        {
            List<string[]> final = new List<string[]>();
            try
            {
                using (TextFieldParser parser = new TextFieldParser(fileName))
                {

                    // Console.WriteLine(fileName);
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    // Ignore headers.
                    if (!parser.EndOfData) parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        //Process row
                        string[] fields = parser.ReadFields();
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

        public IList<string> GetHeaderFields(string fileName)
        {
            try
            {
                using (TextFieldParser parser = new TextFieldParser(fileName))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    // Ignore headers.
                    if (!parser.EndOfData) return parser.ReadFields();
                    else throw new Exception("No Header Found");
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        private bool IsEmptyField(string[] fields)
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
