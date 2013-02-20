using System;
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
            IList<string> servers,
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
            var parser = new QueryFileParser(scriptLocation);
            var queries = parser.Load();
            var serverQueries = queries.Where(q => q.ServerScope).ToList();
            var dbQueries = queries.Where(q => !q.ServerScope).ToList();

            foreach (var servername in servers)
            {
                string dateString = DateTime.Now.ToString("yyyyMMdd_hhmmss_");
                _dictWorksheet.Clear();

                string outputFilepath = GetOutputFilepath(outputFolder, servername, dateString);

                using (var fs = new FileStream(outputFilepath, FileMode.Create))
                {
                    using (var pck = new ExcelPackage(fs))
                    {
                        string connectionString = GetConnectionStringTemplate(servername, "master", username, password,
                                                                              useTrusted);

                        ExecuteQueriesAndSaveToExcel(pck, connectionString, serverQueries, "", "", autoFitColumns,
                                                     queryTimeoutSeconds);

                        if (databases.Count > 0)
                        {
                            int databaseNo = 1;
                            foreach (var db in databases)
                            {
                                connectionString = GetConnectionStringTemplate(servername, db, username, password,
                                                                               useTrusted);
                                ExecuteQueriesAndSaveToExcel(pck, connectionString, dbQueries, db.Trim(),
                                                             databaseNo.ToString(CultureInfo.InvariantCulture),
                                                             autoFitColumns, queryTimeoutSeconds);
                                databaseNo++;
                            }
                        }

                        pck.Save();
                    }
                }
            }
        }

        private static string GetOutputFilepath(string outputFolder, string servername, string dateString)
        {
            string ret = Directory.Exists(outputFolder)
                             ? Path.Combine(outputFolder, dateString + servername.ReplaceInvalidFilenameChars("_") + ".xlsx")
                             : outputFolder;

            return ret;
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

                        ws.Row(2).Style.Font.Bold = true;
 
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

            // Check if name already exists: 31 char limit for worksheet names!
            while (_dictWorksheet.Contains(worksheetNameSanitised))
            {
                worksheetNameSanitised = worksheetNameSanitised.RandomiseLastNChars(3);
            }

            _dictWorksheet.Add(worksheetNameSanitised);

            return worksheetNameSanitised;
        }

        private string SanitiseWorkSheetName(string wsname)
        {
            var s = wsname.RemoveInvalidExcelChars();

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
            const string applicationName = "SQL Server Diagnostic Script Runner";
            const string trustedConnectionStringTemplate = "server={0};database={1};Trusted_Connection=True;Application Name={2}";
            const string sqlLoginConnectionStringTemplate = "server={0};database={1};username={2};password={3};Application Name={4}";

            if (trusted)
            {
                return string.Format(trustedConnectionStringTemplate, servername, database, applicationName);
            }

            return string.Format(sqlLoginConnectionStringTemplate, servername, database, username, password, applicationName);

        }
    }
}
