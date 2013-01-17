﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

using OfficeOpenXml;

namespace SQLDiagRunner
{
    public class Runner
    {
        private readonly HashSet<string> _dictWorksheet = new HashSet<string>();

        public void ExecuteQueries
        (
            string servername,
            string username,
            string password,
            string scriptLocation,
            string outputFolder,
            IList<string> databases,
            bool useTrusted,
            bool autoFitColumns,
            int queryTimeoutSeconds
        )
        {
            _dictWorksheet.Clear();

            string dateString = DateTime.Now.ToString("yyyyMMdd_hhmmss_");

            var parser = new QueryFileParser(scriptLocation);
            var queries = parser.Load();

            string outputFilepath = Path.Combine(outputFolder, dateString + servername + ".xlsx");
            using (var fs = new FileStream(outputFilepath, FileMode.Create))
            {
                using (var pck = new ExcelPackage(fs))
                {
                    string connectionString = GetConnectionStringTemplate(servername, "master", username, password, useTrusted);
                    var serverQueries = queries.Where(q => q.ServerScope).ToList();
                    ExecuteQueriesAndSaveToExcel(pck, connectionString, serverQueries, "", "", autoFitColumns, queryTimeoutSeconds);

                    if (databases.Count > 0)
                    {
                        int databaseNo = 1;
                        var dbQueries = queries.Where(q => !q.ServerScope).ToList();
                        foreach (var db in databases)
                        {
                            connectionString = GetConnectionStringTemplate(servername, db, username, password, useTrusted);
                            ExecuteQueriesAndSaveToExcel(pck, connectionString, dbQueries, db,
                                                         databaseNo.ToString(CultureInfo.InvariantCulture), 
                                                         autoFitColumns, queryTimeoutSeconds);
                            databaseNo++;
                        }
                    }

                    pck.Save();
                }
            }
        }

        private void ExecuteQueriesAndSaveToExcel
        (
            ExcelPackage pck,
            string connectionstring,
            IEnumerable<SqlQuery> queries,
            string database,
            string worksheetPrefix,
            bool autoFitColumns,
            int queryTimeoutSeconds
        )
        {
            foreach (var q in queries)
            {
                string query = GetQueryText(q, database);
                string worksheetName = GetWorkSheetName(q.Title, worksheetPrefix);

                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(worksheetName);
                try
                {
                    ws.Cells["A1"].Value = database;

                    DataTable datatable = QueryExecutor.Execute(connectionstring, query, queryTimeoutSeconds);
                    if (datatable.Rows.Count > 0)
                    {
                        ExcelRangeBase range = ws.Cells["A2"].LoadFromDataTable(datatable, true);

                        // find datetime columns and set formatting
                        int numcols = datatable.Columns.Count;
                        for (int i = 0; i < numcols; i++)
                        {
                            var column = datatable.Columns[i];
                            if (column.DataType == typeof(DateTime))
                            {
                                ws.Column(i + 1).Style.Numberformat.Format = "yyyy-mm-dd hh:MM:ss";
                            }
                        }

                        if (autoFitColumns)
                        {
                            range.AutoFitColumns();
                        }
                    }
                    else
                    {
                        ws.Cells["A2"].Value = "None Found";
                    }
                }
                catch (Exception ex)
                {
                    ws.Cells["A2"].Value = ex.Message;
                }
            }
        }

        private string GetQueryText(SqlQuery q, string database)
        {
            return string.IsNullOrEmpty(database) ? q.QueryText : "USE [" + database + "] \r\n " + q.QueryText;
        }

        private string GetWorkSheetName(string queryTitle, string worksheetPrefix)
        {
            string worksheetName = string.IsNullOrEmpty(worksheetPrefix) ? queryTitle : worksheetPrefix + " " + queryTitle;

            string worksheetNameSanitised = SanitiseWorkSheetName(worksheetName);

            // Check if name already exists. Possible due to ridiculous 31 char limit for worksheet names...
            while (_dictWorksheet.Contains(worksheetNameSanitised))
            {
                worksheetNameSanitised = worksheetNameSanitised.RandomiseLastNChars(3);
            }

            _dictWorksheet.Add(worksheetNameSanitised);

            return worksheetNameSanitised;
        }

        private string SanitiseWorkSheetName(string wsname)
        {
            string s = wsname.Replace(":", "").Replace("\\", "").Replace("/", "")
                .Replace("*", "").Replace("[", "").Replace("]", "");

            return s.Substring(0, Math.Min(31, s.Length));
        }

        private string GetConnectionStringTemplate
        (
            string servername,
            string database,
            string username,
            string password,
            bool trusted
        )
        {
            const string trustedConnectionStringTemplate = "server={0};database={1};trusted_Connection=True";
            const string sqlLoginConnectionStringTemplate = "server={0};database={1};username={2};password={3}";

            if (trusted)
            {
                return string.Format(trustedConnectionStringTemplate, servername, database);
            }

            return string.Format(sqlLoginConnectionStringTemplate, servername, database, username, password);

        }
    }
}