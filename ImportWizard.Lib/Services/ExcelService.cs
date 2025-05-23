﻿using Dapper;
using ImportWizard.Dtos;
using ImportWizard.Models;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

namespace ImportWizard.Services
{
    public class ExcelService
    {
        /// <summary>
        /// The size of the batch when transfering data to a SqlServer database
        /// </summary>
        public int BatchSize { get; set; }

        private ExcelPackage _package = null;
        private ExcelWorkbook workbook = null;

        public List<ColumnPreference> ColumnPreferences { get; set; } = new List<ColumnPreference>();

        public List<WorksheetDto> Worksheets { get; } = new List<WorksheetDto>();

        private List<ExcelWorksheet> _worksheets = new List<ExcelWorksheet>();

        private ExcelWorksheet? _selectedWorksheet;
        public ExcelWorksheet? SelectedWorksheet => _selectedWorksheet;

        public ExcelTable SelectedTable { get; private set; }

        private DataTable _data = null;

        private int headerRowNumber;
        public int HeaderRowNumber
        {
            get => headerRowNumber;

            set
            {
                if (value != headerRowNumber)
                {
                    headerRowNumber = value;
                    ColumnPreferences.Clear();
                }
            }
        }
        public string? TableName { get; set; }
        public string? ConnectionString { get; set; }

        public ExcelService()
        {
            new EPPlusLicense().SetNonCommercialPersonal("ImportWizard");
        }

        public void LoadFile(string filePath)
        {
            _package = new ExcelPackage(filePath);

            Initialize();
        }

        public void LoadExcelPackage(ExcelPackage package)
        {
            _package = package;

            Initialize();
        }

        private void Initialize()
        {
            workbook = _package.Workbook;

            foreach (var worksheet in workbook.Worksheets)
            {
                _worksheets.Add(worksheet);

                Worksheets.Add(new WorksheetDto
                {
                    Name = worksheet.Name,
                    IsHidden = worksheet.Hidden != eWorkSheetHidden.Visible,
                    NrTables = worksheet.Tables.Count, //for now PivotTables are not supported
                    Index = worksheet.Index,
                    Tables = worksheet.Tables
                });
            }
        }

        public void LoadColumnPreferences()
        {
            if (ColumnPreferences.Count > 0)
            {
                throw new Exception("Investigate if this shoud be happening");
            }

            if (SelectedTable != null)
            {
                foreach (var excelTableColumn in SelectedTable.Columns)
                {

                    var colPref = new ColumnPreference
                    {
                        Index = excelTableColumn.Position,
                        Selected = true,
                        Source = excelTableColumn.Name,
                        Destination = excelTableColumn.Name,
                        Type = SqlServerDataType.NVARCHAR,
                        MaxLength = -1,
                        IsNullable = true
                    };

                    ColumnPreferences.Add(colPref);
                }

                return;
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
        /// Returns the data for the current worksheet
        /// </summary>
        public DataTable GetImportData()
        {
            if (_data != null)
                return _data;

            if (ColumnPreferences.Count == 0)
                throw new Exception("Column preferences are not set. Please call LoadColumnPreferences() first.");

            _data = new DataTable();

            if (SelectedTable != null)
            {
                LoadData(
                    dataRange: SelectedTable.WorkSheet.Cells[SelectedTable.Address.FullAddress],
                    headerRowNumber: 1
                    );
            }
            else
            {
                var endRow = SelectedWorksheet.Dimension.End.Row;
                var startCol = ColumnPreferences.OrderBy(cp => cp.Index).First().Index + 1;
                var endCol = ColumnPreferences.OrderByDescending(cp => cp.Index).First().Index + 1;

                LoadData(
                    dataRange: SelectedWorksheet.Cells[HeaderRowNumber, startCol, endRow, endCol],
                    headerRowNumber: HeaderRowNumber
                    );
            }

            return _data;
        }

        public void ResetImportData()
        {
            _data = null;
        }

        void LoadData(ExcelRange dataRange, int headerRowNumber)
        {
            if (_data.Columns.Count != 0)
                throw new Exception("Unexpected number of columns when loading data.");

            //There is a bug in Epplus in which shows a wrong number of rows after fetching the value of a cell from the dataRange
            var nrRows = dataRange.Rows;
            var rowInfos = dataRange.EntireRow.ToList();
            var columnInfos = dataRange.EntireColumn.ToList();

            var headerRowInfo = rowInfos[headerRowNumber - 1];

            foreach (var colPref in ColumnPreferences)
            {
                var colInfo = columnInfos[colPref.Index];

                var excelColumn = dataRange[headerRowInfo.StartRow, colInfo.StartColumn];

                if (excelColumn.Text != colPref.Source)
                    throw new Exception($"Expected column {colPref.Source} at index {colPref.Index} not found!");

                _data.Columns.Add(colPref.Destination);
            }

            var headerRowIdx = rowInfos.IndexOf(headerRowInfo);

            foreach (var rowInfo in rowInfos.Skip(headerRowIdx + 1))
            {
                DataRow dataRow = _data.Rows.Add();

                foreach (var colPref in ColumnPreferences)
                {
                    var colInfo = columnInfos[colPref.Index];

                    SetValue(colPref, dataRange[rowInfo.StartRow, colInfo.StartColumn], dataRow);
                }
            }
        }

        private void SetValue(ColumnPreference colPref, ExcelRangeBase cell, DataRow dataRow)
        {
            switch (colPref.ValueType)
            {
                case ExcelValueType.Formated:
                    dataRow[colPref.Index] = cell.Text;
                    break;
                case ExcelValueType.Literal:
                    dataRow[colPref.Index] = cell.Value;
                    break;
                default:
                    throw new Exception($"ExcelValueType {colPref.ValueType} is not handled!");
            }
        }

        private SqlConnection _connection;

        public async Task Import()
        {
            try
            {
                if (string.IsNullOrEmpty(TableName)) throw new Exception("Please specify new table name.");

                //OnMessage?.Invoke(this, $"Initializing...");

                TableName = TableName.Trim();

                var dataTable = await Task.Run(() => GetImportData());

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

        private ExcelWorksheet GetWorksheetFromDto(WorksheetDto worksheetDto)
        {
            if (worksheetDto == null) return null;
            if (_worksheets == null || _worksheets.Count == 0) return null;

            return _worksheets.FirstOrDefault(w => w.Index == worksheetDto.Index);
        }

        public void SelectWorksheet(WorksheetDto? worksheetDto)
        {
            if (worksheetDto == null)
                throw new ArgumentNullException(nameof(worksheetDto));

            _selectedWorksheet = GetWorksheetFromDto(worksheetDto);

            //Column preferences get reset when new sheet gets selected
            ColumnPreferences.Clear();

            _data = null;

            SelectedTable = null;
        }

        public void SelectTable(ExcelTable table)
        {
            SelectedTable = table;

            //reset data and column preferences when a new table is selected
            ColumnPreferences.Clear();
            _data = null;
        }

        //TODO Implement IDisposable?
    }
}
