using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UI.Utils
{
    public class CsvReporter : ICsvReporter
    {
        private readonly string m_CsvFileName = @"..\..\..\Reports\ExportedData_{0}.csv"; 

        public string ToCsv<T>(string separator, IEnumerable<T> objectlist)
        {
            Type t = typeof(T);
            PropertyInfo[] properties = t.GetProperties();

            string header = string.Join(separator, properties.Select(f => f.Name).ToArray());

            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine(header);

            foreach (var o in objectlist)
                csvData.AppendLine(ToCsvFields(separator, properties, o));

            var filePath = WriteToFile(csvData);
            return File.Exists(filePath) ? $"CSV file generated in {filePath}" : 
                                           "Unable to generate report";
        }

        private string ToCsvFields(string separator, PropertyInfo[] properties, object o)
        {
            StringBuilder csvLine = new StringBuilder();

            foreach (var f in properties)
            {
                if (csvLine.Length > 0)
                    csvLine.Append(separator);

                var x = f.GetValue(o);

                if (x != null)
                    csvLine.Append(x.ToString());
            }

            return csvLine.ToString();
        }

        private string WriteToFile(StringBuilder stringBuilder)
        {
            var fileName = string.Format(m_CsvFileName, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            File.WriteAllText(fileName, stringBuilder.ToString());
            return Path.GetFullPath(fileName);
        }
    }
}
