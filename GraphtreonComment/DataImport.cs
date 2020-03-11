
using System.IO;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace MGBot
{
    public class DataImport
    {
        public static DataTable NewDataTable(string fileName, bool firstRowContainsFieldNames = true, int colcount =2, bool delimiter = true)
        {
            DataTable result = new DataTable();

            string ext = Path.GetExtension(fileName);
            string deli = ",";
            if (ext == ".txt") deli = ":";
            using (TextFieldParser tfp = new TextFieldParser(fileName))
            {
                if(delimiter)
                    tfp.SetDelimiters(deli);
                else
                    tfp.SetDelimiters("__");

                // Get Some Column Names
                if (!tfp.EndOfData)
                {
                    string[] fields = tfp.ReadFields();
                    if (fields.Count() != colcount)
                    {
                        for (int i = 0; i < colcount; i++)
                        {
                            result.Columns.Add("Col" + i);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < fields.Count(); i++)
                        {
                            if (firstRowContainsFieldNames)
                                result.Columns.Add(fields[i]);
                            else
                                result.Columns.Add("Col" + i);
                        }

                    }
                    // If first line is data then add it
                    if (!firstRowContainsFieldNames)
                        result.Rows.Add(fields);
                }

                // Get Remaining Rows
                while (!tfp.EndOfData)
                {
                    try {
                        result.Rows.Add(tfp.ReadFields());
                    }
                    catch (Exception ex) {
                        continue;
                    }
                }
            }

            return result;
        }
    }
}
