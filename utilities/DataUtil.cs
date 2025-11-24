using OfficeOpenXml;

namespace PageObjectModelPW.utilities
{
    class DataUtil
    {
        // Ensure EPPlus is running under a non-commercial personal license
        static DataUtil( )
        {
            // Set the non-commercial personal license and author name
            ExcelPackage.License.SetNonCommercialPersonal("My Name");
        }


        public static IEnumerable<TestCaseData> GetTestDataFromExcel(string filePath, string sheetName, List<string> columnNames)
        {
            Console.WriteLine($"[DataUtil] Reading test data from: {filePath}");
            var testData = new List<TestCaseData>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null) throw new ArgumentException($"Sheet '{sheetName}' not found.");

                var columnIndices = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    var header = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                    if (string.IsNullOrEmpty(header)) continue;
                    var match = columnNames.FirstOrDefault(c => string.Equals(c, header, StringComparison.OrdinalIgnoreCase));
                    if (match != null) columnIndices[match] = col;
                }

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var values = new List<string>();
                    foreach (var colName in columnNames)
                    {
                        var raw = worksheet.Cells[row, columnIndices[colName]].Value?.ToString();
                        values.Add(raw?.Trim());
                    }

                    // Build a readable test name including runmode so Test Explorer shows it
                    var testName = $"{values.ElementAtOrDefault(0) ?? "case"} [{values.ElementAtOrDefault(2) ?? ""}]";
                    var tc = new TestCaseData(values.ToArray()).SetName(testName);

                    // If runmode is 'N' mark the testcase as ignored (Test Explorer will show Skipped)
                    var runmode = values.ElementAtOrDefault(2)?.Trim();
                    if (string.Equals(runmode, "N", StringComparison.OrdinalIgnoreCase))
                    {
                        tc = tc.Ignore("Runmode set to N");
                        Console.WriteLine($"[DataUtil] Marking ignored: Row {row} -> runmode='{runmode}'");
                    }
                    else
                    {
                        Console.WriteLine($"[DataUtil] Adding test: Row {row} -> runmode='{runmode}'");
                    }

                    testData.Add(tc);
                }
            }
            return testData;
        }
    }
}
