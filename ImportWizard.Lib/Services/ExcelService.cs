using Dapper;
using ImportWizard.Models;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportWizard.Services
{
    public class ExcelService
    {
        /// <summary>
        /// The excel file path
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// The size of the batch when transfering data to a SqlServer database
        /// </summary>
        public int BatchSize { get; set; }

        private ExcelPackage package = null;
        private ExcelWorkbook workbook = null;

        public List<ColumnPreference> ColumnPreferences { get; set; } = new List<ColumnPreference>();

        public List<ExcelWorksheet> Worksheets { get; } = new List<ExcelWorksheet>();

        private ExcelWorksheet? _selectedWorksheet;
        public ExcelWorksheet? SelectedWorksheet 
        {
            get => _selectedWorksheet;
            set
            {
                _selectedWorksheet = value;

                //Column preferences get reset when new sheet gets selected
                ColumnPreferences.Clear();
            }
        }

        public int HeaderRowNumber { get; set; }
        public string TableName { get; set; }
        public string ConnectionString { get; set; }

        public ExcelService(string filePath)
        {
            FilePath = filePath;

            new EPPlusLicense().SetNonCommercialPersonal("ImportWizard");
        }

        public void Initialize()
        {
            package = new ExcelPackage(FilePath);
            workbook = package.Workbook;

            foreach (var worksheet in workbook.Worksheets)
            {
                Worksheets.Add(worksheet);
            }
        }

        public void LoadColumnPreferences()
        {
            if (ColumnPreferences.Count > 0)
            {
                throw new Exception("Investigate if this shoud be happening");
            }

            var dimension = SelectedWorksheet.Dimension;
            int startColumn = dimension.Start.Column;
            int endColumn = dimension.End.Column;

            for (int i = 1; i <= endColumn; i++)
            {
                var cell = SelectedWorksheet.Cells[HeaderRowNumber, i];

                var colPref = new ColumnPreference
                {
                    Index = i - 1,
                    Selected = true,
                    Source = cell.Text,
                    Destination = cell.Text,
                    Type = SqlServerDataType.NVARCHAR,
                    MaxLength = -1,
                    IsNullable = true,
                    NumericPrecision = 1,
                    NumericScale = 0
                };

                ColumnPreferences.Add(colPref);

                //TODO remove spaces and set alias to a nicely formatted column name
            }
        }

        /// <summary>
        /// If this throws an exception while reading copy all the data from the excel file in to a fresh one and try with it
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hasHeader"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromExcel()
        {
            var dt = new DataTable();
            //var hasHeader = HeaderRowStart > 0;

            //OnMessage?.Invoke(this, $"Parsing Excel File");

            foreach (ExcelRangeColumn xCol in SelectedWorksheet.Columns)
            {
                if (xCol.Hidden)
                {
                    xCol.Hidden = false;
                }
            }

            var dimension = SelectedWorksheet.Dimension;
            int startColumn = dimension.Start.Column;
            int endColumn = dimension.End.Column;

            for (int columnPosition = startColumn; columnPosition <= endColumn; columnPosition++)
            {
                var cellText = SelectedWorksheet.Cells[HeaderRowNumber, columnPosition].Text;
                var colPreference = ColumnPreferences[columnPosition - 1];

                if (cellText != colPreference.Source)
                    throw new Exception($"Expected column {colPreference.Source} at index {colPreference.Index} not found!");

                dt.Columns.Add(colPreference.Destination);
            }

            var colsCount = dt.Columns.Count;

            //TODO make a setting for DataRowNumber in the future
            var dataStartRow = HeaderRowNumber + 1;

            for (int rowNum = dataStartRow; rowNum <= SelectedWorksheet.Dimension.End.Row; rowNum++)
            {
                DataRow row = dt.Rows.Add();

                for (int columnIndex = startColumn; columnIndex <= endColumn; columnIndex++)
                {
                    if (columnIndex > colsCount)
                        break;

                    var cellText = SelectedWorksheet.Cells[rowNum, columnIndex].Text;

                    if (cellText.Length > dt.Columns[columnIndex - 1].MaxLength)
                    {
                        dt.Columns[columnIndex - 1].MaxLength = cellText.Length;
                    }

                    row[columnIndex - 1] = cellText;
                }
            }

            return dt;
        }

        private SqlConnection _connection;

        public async Task Import()
        {
            try
            {
                if (string.IsNullOrEmpty(TableName)) throw new Exception("Please specify new table name.");

                //OnMessage?.Invoke(this, $"Initializing...");

                TableName = TableName.Trim();

                var dataTable = await Task.Run(() => GetDataTableFromExcel());

                _connection = new SqlConnection(ConnectionString);

                CreateTable();

                var sw = Stopwatch.StartNew();

                _connection.Open();
                var totalRows = dataTable.Rows.Count;

                await Task.Run(() =>
                {
                    var options =
                        SqlBulkCopyOptions.TableLock //When TableLock is set to true, SQL Server minimizes logging for the bulk operation. This can lead to faster data loading because it reduces the amount of log data that needs to be written and managed.
                        | SqlBulkCopyOptions.KeepNulls; //Preserve null values in the destination table regardless of the settings for default values. When not specified, null values are replaced by default values where applicable.

                    using (var bulkCopy = new SqlBulkCopy(_connection, options, null))
                    {
                        //The main advantage of enabling streaming is reducing memory usage during bulk copy of max data types.
                        //bulkCopy.EnableStreaming

                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = TableName;
                        bulkCopy.NotifyAfter = 1000;

                        if (BatchSize > 0)
                        {
                            //By default, SqlBulkCopy will process the operation in a single batch.
                            //a good way is to try different valeus and see what works for you, i found my imports worked well with batch size of 10k
                            bulkCopy.BatchSize = BatchSize;
                            bulkCopy.NotifyAfter = BatchSize;
                        }
                        else
                        {
                            bulkCopy.NotifyAfter = 1000;
                        }

                        foreach (DataColumn dataCol in dataTable.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(dataCol.ColumnName, dataCol.ColumnName);
                        }

                        var lastNotifyDate = DateTime.MinValue;
                        var notifyInterval = TimeSpan.FromSeconds(1);

                        bulkCopy.SqlRowsCopied += (s, ee) =>
                        {
                            if ((DateTime.Now - lastNotifyDate) > notifyInterval)
                            {
                                var progress = ((decimal)ee.RowsCopied / totalRows) * 100;
                                var msg = $"Importing data: {decimal.Round(progress)}%";
                                //OnMessage?.Invoke(this, msg);
                                lastNotifyDate = DateTime.Now;
                            }
                        };

                        bulkCopy.WriteToServer(dataTable);

                        dataTable.Dispose();
                        dataTable.Clear();
                        dataTable = null;

                        //OnMessage?.Invoke(this, $"Importing data: 100%");
                    }
                });

                sw.Stop();

                var total = sw.Elapsed;

                //MessageBox.Show($"Success! Foud {totalRows} in file.", $"{total}");
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.Message);
            }
        }

        private bool CreateTable()
        {
            //TODO check properly if the table has schema
            //TODO Check if the schema exists

            var tableExists = _connection.QueryFirst<int>($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{TableName}'");

            if (tableExists != 0)
            {
                //if (MessageBox.Show($"Table '{TableName}' already exists, do you wish to drop it (if no data will be appended)?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //{
                    _connection.Execute($"DROP TABLE [{TableName}]");
                //}
                //else
                //{
                //    return false;
                //}
            }

            var strCols = this.ColumnPreferences.Select(cp =>
            {
                string typeDef = null;

                switch (cp.Type)
                {
                    case SqlServerDataType.VARCHAR:
                    case SqlServerDataType.NVARCHAR:
                        if (cp.MaxLength == 0 || cp.MaxLength == null)
                            throw new Exception($"Invalid {nameof(cp.MaxLength)} on column {cp.Source}");

                        if (cp.MaxLength == -1)
                        {
                            typeDef = $"{cp.Type}(MAX)";
                        }
                        else
                        {
                            typeDef = $"{cp.Type}({cp.MaxLength})";
                        }
                        break;
                    case SqlServerDataType.DECIMAL:
                        break;
                    case SqlServerDataType.INT:
                    case SqlServerDataType.DATETIME:
                    case SqlServerDataType.BIT:
                        typeDef = $"{cp.Type}";
                        break;
                    default:
                        throw new Exception($"{nameof(SqlServerDataType)} {cp.Type} is not handled!");
                }

                var nullability = !cp.IsNullable ? $" NOT NULL" : "";
                var defaultValue = string.Empty;

                if (!string.IsNullOrWhiteSpace(cp.DefaultValue))
                    defaultValue = $" DEFAULT ({cp.DefaultValue.Replace("'", "''")})";

                return $"[{cp.Destination}] {typeDef}{nullability}{defaultValue}";
            });

            var GenerateIdentityCol = true;
            var identityColSql = GenerateIdentityCol ? "ID INT PRIMARY KEY IDENTITY(1,1), " : "";
            var createTableSql = $"CREATE TABLE {TableName} ({identityColSql}{string.Join(", ", strCols)})";

            _connection.Execute(createTableSql);

            return true;
        }
    }
}
