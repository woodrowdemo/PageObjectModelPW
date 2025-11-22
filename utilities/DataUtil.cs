using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace PageObjectModelPW.utilities
{
    class DataUtil
    {
        // Ensure EPPlus is running under a non-commercial personal license
        static DataUtil()
        {
            // Set the non-commercial personal license and author name
            ExcelPackage.License.SetNonCommercialPersonal("My Name");
        }


        public static IEnumerable<TestCaseData> GetTestDataFromExcel(string filePath, string sheetName, List<string> columnNames)
        {
            var testData = new List<TestCaseData>();

            // Load Excel file
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null)
                {
                    throw new ArgumentException($"Sheet '{sheetName}' not found in the Excel file.");
                }

                // Find column indices for specified column names (trim header values)
                var columnIndices = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    var header = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(header) && columnNames.Contains(header))
                    {
                        columnIndices[header] = col;
                    }
                }

                // Check if all column names are found
                foreach (var columnName in columnNames)
                {
                    if (!columnIndices.ContainsKey(columnName))
                    {
                        throw new ArgumentException($"Column name '{columnName}' not found in the Excel file.");
                    }
                }

                // Read test data
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var data = new List<string>();
                    foreach (var columnName in columnNames)
                    {
                        var raw = worksheet.Cells[row, columnIndices[columnName]].Value?.ToString();
                        var value = raw?.Trim();

                        // Normalize runmode to uppercase single character (helps comparisons)
                        if (string.Equals(columnName, "runmode", StringComparison.OrdinalIgnoreCase))
                        {
                            value = value?.ToUpperInvariant();
                        }

                        data.Add(value);
                    }

                    // If runmode is explicitly 'N', skip adding this testcase so it won't be reported as Ignored
                    var runmodeValue = data.ElementAtOrDefault(columnNames.FindIndex(n => string.Equals(n, "runmode", StringComparison.OrdinalIgnoreCase)));
                    if (string.Equals(runmodeValue, "N", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[DataUtil] Skipping row {row} because runmode='{runmodeValue}'");
                        continue; // skip this row
                    }

                    // Diagnostic output: shows exact values to reveal hidden chars if needed
                    Console.WriteLine($"[DataUtil] Adding Row {row}: {string.Join(" | ", data.Select((v, i) => $"{columnNames[i]}:'{v}'(len:{v?.Length ?? 0})"))}");

                    testData.Add(new TestCaseData(data.ToArray()));
                }
            }

            return testData;
        }
    }
}
