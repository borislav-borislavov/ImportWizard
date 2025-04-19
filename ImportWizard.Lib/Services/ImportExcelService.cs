using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Dapper;
using OfficeOpenXml;
using System.IO;
using System.Globalization;

namespace Og.Gauntlet.Services
{
    //public class ImportExcelService
    //{
    //    public bool GenerateIdentityCol { get; internal set; }
    //    public string TableName { get; internal set; }
    //    public string ImportFilePath { get; internal set; }
    //    public bool ShouldReplaceDiacritics { get; set; }
    //    public int HeaderRowStart { get; set; }
    //    public bool SkipEmptyColumns { get; internal set; }
    //    public int BatchSize { get; internal set; }

    //    private SqlConnection _connection;

    //    public event EventHandler<string> OnMessage;

    //    public ImportExcelService(string connectionString)
    //    {
    //        _connection = new SqlConnection(connectionString);
    //    }

    //    public async Task Import(bool reduceMemoryUsage)
    //    {
    //        try
    //        {
    //            if (string.IsNullOrEmpty(TableName)) throw new Exception("Please specify new table name.");

    //            OnMessage?.Invoke(this, $"Initializing...");

    //            TableName = TableName.Trim();

    //            var dataTable = await Task.Run(() => GetDataTableFromExcel(ImportFilePath));

    //            CreateTable(dataTable);

    //            var sw = Stopwatch.StartNew();

    //            _connection.Open();
    //            var totalRows = dataTable.Rows.Count;

    //            await Task.Run(() =>
    //            {
    //                var options =
    //                    SqlBulkCopyOptions.TableLock //When TableLock is set to true, SQL Server minimizes logging for the bulk operation. This can lead to faster data loading because it reduces the amount of log data that needs to be written and managed.
    //                    | SqlBulkCopyOptions.KeepNulls; //Preserve null values in the destination table regardless of the settings for default values. When not specified, null values are replaced by default values where applicable.

    //                using (var bulkCopy = new SqlBulkCopy(_connection, options, null))
    //                {
    //                    //The main advantage of enabling streaming is reducing memory usage during bulk copy of max data types.
    //                    //bulkCopy.EnableStreaming

    //                    bulkCopy.BulkCopyTimeout = 0;
    //                    bulkCopy.DestinationTableName = TableName;
    //                    bulkCopy.NotifyAfter = 1000;

    //                    if (BatchSize > 0)
    //                    {
    //                        //By default, SqlBulkCopy will process the operation in a single batch.
    //                        //a good way is to try different valeus and see what works for you, i found my imports worked well with batch size of 10k
    //                        bulkCopy.BatchSize = BatchSize;
    //                        bulkCopy.NotifyAfter = BatchSize;
    //                    }
    //                    else
    //                    {
    //                        bulkCopy.NotifyAfter = 1000;
    //                    }

    //                    foreach (DataColumn dataCol in dataTable.Columns)
    //                    {
    //                        bulkCopy.ColumnMappings.Add(dataCol.ColumnName, dataCol.ColumnName);
    //                    }

    //                    var lastNotifyDate = DateTime.MinValue;
    //                    var notifyInterval = TimeSpan.FromSeconds(1);

    //                    bulkCopy.SqlRowsCopied += (s, ee) =>
    //                    {
    //                        if ((DateTime.Now - lastNotifyDate) > notifyInterval)
    //                        {
    //                            var progress = ((decimal)ee.RowsCopied / totalRows) * 100;
    //                            var msg = $"Importing data: {decimal.Round(progress)}%";
    //                            OnMessage?.Invoke(this, msg);
    //                            lastNotifyDate = DateTime.Now;
    //                        }
    //                    };

    //                    bulkCopy.WriteToServer(dataTable);

    //                    OnMessage?.Invoke(this, $"Importing data: 100%");
    //                }
    //            });

    //            sw.Stop();

    //            var total = sw.Elapsed;

    //            MessageBox.Show($"Success! Foud {totalRows} in file.", $"{total}");
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message);
    //        }
    //    }

    //    private bool CreateTable(DataTable dt)
    //    {
    //        //TODO check properly if the table has schema

    //        var tableExists = _connection.QueryFirst<int>($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{TableName}'");

    //        if (tableExists != 0)
    //        {
    //            if (MessageBox.Show($"Table '{TableName}' already exists, do you wish to drop it (if no data will be appended)?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
    //            {
    //                _connection.Execute($"DROP TABLE [{TableName}]");
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }

    //        var dataColumns = dt.Columns.Cast<DataColumn>();

    //        var strCols = dataColumns.Select(c => $"[{c}] NVARCHAR({(c.MaxLength > 0 ? c.MaxLength : 100)})");
    //        var identityColSql = GenerateIdentityCol ? "ID INT PRIMARY KEY IDENTITY(1,1), " : "";
    //        var createTableSql = $"CREATE TABLE {TableName} ({identityColSql}{string.Join(", ", strCols)})";

    //        _connection.Execute(createTableSql);

    //        return true;
    //    }

    //    /// <summary>
    //    /// If this throws an exception while reading copy all the data from the excel file in to a fresh one and try with it
    //    /// </summary>
    //    /// <param name="path"></param>
    //    /// <param name="hasHeader"></param>
    //    /// <returns></returns>
    //    public DataTable GetDataTableFromExcel(string path)
    //    {
    //        var dt = new DataTable();
    //        var hasHeader = HeaderRowStart > 0;

    //        OnMessage?.Invoke(this, $"Parsing Excel File");
    //        using (var pck = GetPackage())
    //        {
    //            var ws = pck.Workbook.Worksheets[0];

    //            foreach (ExcelRangeColumn xCol in ws.Columns)
    //            {
    //                if (xCol.Hidden)
    //                {
    //                    xCol.Hidden = false;
    //                }
    //            }

    //            var dimension = ws.Dimension;
    //            int startColumn = dimension.Start.Column;
    //            int endColumn = dimension.End.Column;

    //            for (int columnIndex = startColumn; columnIndex <= endColumn; columnIndex++)
    //            {
    //                if (HeaderRowStart > 0)
    //                {
    //                    var cellText = ws.Cells[HeaderRowStart, columnIndex].Text;

    //                    if (cellText.IndexOf(' ') > 0)
    //                    {
    //                        var words = cellText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    //                        var capitalizedWords = words.Select(w => w[0].ToString().ToUpper() + w.Substring(1).ToLower());

    //                        cellText = string.Concat(capitalizedWords);
    //                    }
    //                    else if (cellText.Length > 0)
    //                    {
    //                        cellText = cellText.ToUpper()[0] + cellText.Substring(1).ToLower();
    //                    }
    //                    else
    //                    {
    //                        cellText = $"Column{columnIndex}";
    //                    }

    //                    dt.Columns.Add(hasHeader ? cellText : $"Column{columnIndex}");
    //                }
    //                else
    //                {
    //                    dt.Columns.Add($"Column{columnIndex}");
    //                }
    //            }

    //            var colsCount = dt.Columns.Count;

    //            var startRow = hasHeader ? HeaderRowStart + 1 : 1;

    //            for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
    //            {
    //                DataRow row = dt.Rows.Add();

    //                for (int columnIndex = startColumn; columnIndex <= endColumn; columnIndex++)
    //                {
    //                    if (columnIndex > colsCount)
    //                        break;

    //                    var cellText = ws.Cells[rowNum, columnIndex].Text;

    //                    if (cellText.Length > dt.Columns[columnIndex - 1].MaxLength)
    //                    {
    //                        dt.Columns[columnIndex - 1].MaxLength = cellText.Length;
    //                    }

    //                    if (ShouldReplaceDiacritics)
    //                    {
    //                        cellText = ReplaceDiacritics(cellText);
    //                    }

    //                    row[columnIndex - 1] = cellText;
    //                }
    //            }
    //        }

    //        if (SkipEmptyColumns)
    //        {
    //            var emptyCols = dt.Columns
    //                .Cast<DataColumn>()
    //                .Where(c => c.MaxLength == 0)
    //                .ToList();

    //            foreach (var col in emptyCols)
    //            {
    //                dt.Columns.Remove(col);
    //            }
    //        }

    //        return dt;
    //    }

    //    private ExcelPackage GetPackage()
    //    {
    //        var ext = Path.GetExtension(ImportFilePath).ToLower();

    //        if (ext == ".csv")
    //        {
    //            try
    //            {
    //                var pck = new ExcelPackage();
    //                var format = new ExcelTextFormat();
    //                //Default Delimiter is , and will work for most cases
    //                //format.Delimiter = ';';
    //                format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
    //                format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
    //                format.Encoding = new UTF8Encoding();

    //                //create a WorkSheet
    //                var worksheet = pck.Workbook.Worksheets.Add("Sheet 1");

    //                //load the CSV data into cell A1
    //                worksheet.Cells["A1"].LoadFromText(new FileInfo(ImportFilePath), format);
    //                return pck;
    //            }
    //            catch (ArgumentException ae)
    //            {
    //                if (ae.Message == "Row out of range")
    //                    throw new Exception("Number of rows exceeds maximum of 1.048.576");

    //                throw;
    //            }
    //        }
    //        else
    //        {
    //            return new ExcelPackage(new FileInfo(ImportFilePath));
    //        }

    //    }

    //    private string ReplaceDiacritics(string value)
    //    {
    //        //Ă ă
    //        value = value.Replace('Ă', 'A');
    //        value = value.Replace('ă', 'a');

    //        //Â â Î î
    //        value = value.Replace('Â', 'A');
    //        value = value.Replace('â', 'a');
    //        value = value.Replace('Î', 'I');
    //        value = value.Replace('î', 'i');

    //        //Ș ș Ț ț
    //        value = value.Replace('Ș', 'S');
    //        value = value.Replace('ș', 's');
    //        value = value.Replace('Ț', 'T');
    //        value = value.Replace('ț', 't');

    //        return value;
    //    }
    //}
}
