﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CottonHarvestDataTransferApp.Classes
{
    /// <summary>
    /// This class represents a single line from a CSV file.
    /// Internally it splits the line into a list of string data values
    /// </summary>
    public class CSVRow
    {
        private string _lineText;

        protected List<string> dataValues;

        public string GetColumnValue(int index)
        {
            if (index < dataValues.Count())
            {
                return dataValues[index];
            }
            else
            {
                return string.Empty;
            }
        }

        public string LineText
        {
            get
            {
                return _lineText;
            }
        }

        public void SetColumnValue(int index, string value)
        {
            dataValues[index] = value;

            //rebuild _lineText to reflect change
            string line = "";
            foreach(var val in dataValues)
            {
                string escapedVal = "";
                if (val.Contains("\""))
                {
                    escapedVal = val.Replace("\"", "\"\"");
                }

                if (escapedVal.Contains(","))
                {
                    escapedVal = "\"" + escapedVal + "\"";
                }

                line += escapedVal + ",";
            }

            _lineText = line.TrimEnd(',');
        }

        public CSVRow(string lineText)
        {
            _lineText = lineText;
            bool insideQuote = false;
            string dataValue = "";
            dataValues = new List<string>();

            char[] chars = lineText.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                char ch = chars[i];

                char? nextChar = null;

                if (i + 1 < chars.Length)
                {
                    nextChar = chars[i + 1];
                }

                switch (ch)
                {
                    case '"':
                        if (insideQuote && nextChar == '"') //process escaped quote
                        {
                            dataValue += ch.ToString();
                            i++; //skip next quote
                        }
                        else
                        {
                            insideQuote = !insideQuote;
                        }
                        break;
                    case ',':
                        if (!insideQuote)
                        {
                            dataValues.Add(dataValue);
                            dataValue = "";
                        }
                        else
                        {
                            dataValue += ch.ToString();
                        }
                        break;
                    case '\r':
                    case '\n':
                        dataValues.Add(dataValue);
                        dataValue = "";
                        break;
                    default:
                        dataValue += ch.ToString();
                        break;
                }

                if (i == chars.Length - 1 && !string.IsNullOrEmpty(dataValue))
                {
                    dataValues.Add(dataValue);
                }
            }
        }
    }

    /// <summary>
    /// This class represent the header row from a CSV.
    /// It is used to locate the column index of a column with a specific title   
    /// </summary>
    public class CSVHeader : CSVRow
    {
        public CSVHeader(string headerText) : base(headerText)
        {

        }

        public int GetColumnIndex(string title)
        {
            int i = 0;
            bool found = false; 
            for(i=0; i < dataValues.Count; i++)
            {
                if (dataValues[i].ToUpper().Trim() == title.ToUpper().Trim())
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                i = -1;

            return i;
        }
    }

    /// <summary>
    /// This class is used to extract line count and machine ids and write a file back out
    /// </summary>
    public class CSVFileParser
    {
        private string _filename;

        List<CSVRow> rows = new List<CSVRow>();

        CSVHeader header = null;  
        
        public CSVFileParser(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string line = reader.ReadLine();

                if (line != null)
                {
                    header = new CSVHeader(line);
                }
                while((line = reader.ReadLine()) != null)
                {
                    rows.Add(new CSVRow(line));
                }
            }
            
        }       

        public int LineCount
        {
            get
            {
                int count = rows.Count();

                if (header != null)
                {
                    count++;
                }

                return count;
            }
        }
        
        public string MachineID
        {
            get
            {
                int index = header.GetColumnIndex("Machine VIN");

                if (index < 0)
                {
                    index = header.GetColumnIndex("Machine SN");
                }

                if (index < 0)
                {
                    index = header.GetColumnIndex("Machine PIN");
                }

                if (index >= 0 && rows.Count() >= 1)
                {
                    return rows[0].GetColumnValue(index);
                }
                else
                {
                    return string.Empty;
                }
            }
        }                

        public string FirstLineText
        {
            get
            {
                if (rows.Count > 0)
                {
                    return rows[0].LineText.ToLower().Trim();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public void WriteFile(string filename, int skip)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                if (header != null)
                {
                    sw.WriteLine(header.LineText);
                }

                skip = skip - 1;
                if (skip < 0)
                {
                    skip = 0;
                }

                for(int i=skip; i < rows.Count(); i++)
                {
                    sw.WriteLine(rows[i].LineText);
                }
                sw.Flush();                
            }
        }
                
    }
}
