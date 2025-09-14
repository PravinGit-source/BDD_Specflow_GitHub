using System;
using System.Data;
using System.IO;
using ExcelDataReader;

public static class ExcelReader
{
    private static DataTable _dataTable;

    static ExcelReader()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        LoadExcelData("TestData/LoginData.xlsx"); // Update with actual path
    }

    private static void LoadExcelData(string filePath)
    {
        try
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory; // bin/debug/net6.0/
            string fullPath = Path.Combine(basePath, filePath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Excel file not found at: {fullPath}");
            }

            using (var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read))
            {
                var readerConfig = new ExcelReaderConfiguration()
                {
                    FallbackEncoding = System.Text.Encoding.UTF8 // Support special characters
                };

                using (var reader = ExcelReaderFactory.CreateReader(stream, readerConfig))
                {
                    var dataSetConfig = new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true // ✅ Treat first row as column headers
                        }
                    };

                    var result = reader.AsDataSet(dataSetConfig);

                    if (result.Tables.Count == 0)
                    {
                        throw new Exception("No sheets found in the Excel file.");
                    }

                    _dataTable = result.Tables[0]; // Read first sheet
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Excel file: {ex.Message}");
            throw;
        }
    }

    public static string GetCellValue(int row, string columnName)
    {
        try
        {
            if (_dataTable == null)
            {
                throw new Exception("Excel data is not loaded.");
            }

            if (!_dataTable.Columns.Contains(columnName))
            {
                throw new Exception($"Column '{columnName}' not found in the Excel sheet.");
            }

            return _dataTable.Rows[row][columnName]?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading cell value: {ex.Message}");
            throw;
        }
    }
}