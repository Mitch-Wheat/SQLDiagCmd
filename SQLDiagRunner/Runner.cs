using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

using OfficeOpenXml;

namespace SQLDiagRunner
{
    public class Runner
    {
        private readonly HashSet<string> _worksheetNames = new HashSet<string>();

        public void ExecuteQueries
        (
            IEnumerable<string> servers,
            string username,
            string password,
            string scriptLocation,
            string outputFolder,
            IEnumerable<string> databases,
            bool useTrusted,
            bool autoFitColumns,
            int queryTimeoutSeconds,
            IEnumerable<string> ExcludeQueryNumbers
        )
        {
            var parser = new QueryFileParser(scriptLocation);
            var queries = parser.Load();

            var serverQueries = queries.Where(q => q.ServerScope).ToList();
            var dbQueries = queries.Where(q => !q.ServerScope).ToList();

            if (ExcludeQueryNumbers != null)
            {
                serverQueries = serverQueries.Where(q => !ExcludeQueryNumbers.Contains(q.QueryNumber)).ToList();
                dbQueries = dbQueries.Where(q => !ExcludeQueryNumbers.Contains(q.QueryNumber)).ToList();
            }

            foreach (var servername in servers)
            {
                _worksheetNames.Clear();

                var outputFilepath = GetOutputFilepath(outputFolder, servername);

                using (var fs = new FileStream(outputFilepath, FileMode.Create))
                {
                    using (var pck = new ExcelPackage(fs))
                    {
                        var connectionString = GetConnectionStringTemplate(servername, "master", username, password, useTrusted);

                        ExecuteQueriesAndSaveToExcel(pck, connectionString, serverQueries, "", "", autoFitColumns, queryTimeoutSeconds);

                        // Not enough room in Excel WorkSheet tab names for the full database name, so prefix with DB number 1 to N.
                        int databaseNo = 1;
                        foreach (var db in databases)
                        {
                            connectionString = GetConnectionStringTemplate(servername, db, username, password, useTrusted);

                            // TODO: check if database exists...


                            ExecuteQueriesAndSaveToExcel(pck, connectionString, dbQueries, db.Trim(),
                                                         databaseNo.ToString(), autoFitColumns, queryTimeoutSeconds);
                            databaseNo++;
                        }

                        pck.Save();
                    }
                }
            }
        }

        private static string GetOutputFilepath(string outputFolder, string servername)
        {
            var dateString = DateTime.Now.ToString("yyyyMMdd_hhmmss_");

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
            string databaseName,
            string worksheetPrefix,
            bool autoFitColumns,
            int queryTimeoutSeconds
        )
        {
            foreach (var q in queries)
            {
                string query = q.GetQueryUseDBHelper(databaseName);
                string worksheetName = GetWorkSheetName(q.Title, worksheetPrefix);
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(worksheetName);

                try
                {
                    ws.Cells["A1"].Value = databaseName;

                    var dt = QueryExecutor.Execute(connectionstring, query, queryTimeoutSeconds);

                    SaveDataTableToWorksheet(dt, ws, autoFitColumns);
                }
                catch (Exception ex)
                {
                    ws.Cells["A2"].Value = ex.Message;
                }
            }
        }

        private void SaveDataTableToWorksheet(DataTable dt, ExcelWorksheet ws, bool autoFitColumns)
        {
            if (dt.Rows.Count > 0)
            {
                ExcelRangeBase range = ws.Cells["A2"].LoadFromDataTable(dt, true);

                ws.Row(2).Style.Font.Bold = true;

                // find all datetime columns and set formatting
                int numcols = dt.Columns.Count;
                for (int i = 0; i < numcols; i++)
                {
                    var column = dt.Columns[i];
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

        private string GetWorkSheetName(string queryTitle, string worksheetPrefix)
        {
            var worksheetName = string.IsNullOrEmpty(worksheetPrefix) ? queryTitle : worksheetPrefix + " " + queryTitle;

            var worksheetNameSanitised = worksheetName.SanitiseWorkSheetName();

            // Check if name already exists: 31 char limit for worksheet names!
            while (_worksheetNames.Contains(worksheetNameSanitised))
            {
                worksheetNameSanitised = worksheetNameSanitised.RandomiseLastNChars(3);
            }

            _worksheetNames.Add(worksheetNameSanitised);

            return worksheetNameSanitised;
        }

        private static string GetConnectionStringTemplate
        (
            string servername,
            string database,
            string username,
            string password,
            bool trusted
        )
        {
            const string applicationName = "SQL Server Diagnostic Script Runner";
            const string trustedConnectionStringTemplate = "Server={0};Database={1};Trusted_Connection=True;Application Name={2}";
            const string sqlLoginConnectionStringTemplate = "Server={0};Database={1};User Id={2};Password={3};Application Name={4}";

            if (trusted)
            {
                return string.Format(trustedConnectionStringTemplate, servername, database, applicationName);
            }

            return string.Format(sqlLoginConnectionStringTemplate, servername, database, username, password, applicationName);

        }
    }
}
