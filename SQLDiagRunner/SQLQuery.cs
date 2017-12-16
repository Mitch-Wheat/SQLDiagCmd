
namespace SQLDiagRunner
{
    public class SqlQuery
    {
        public string QueryText { get; set; }
        public string Title { get; set; }
        public int QueryNumber { get; set; }
        public bool ServerScope { get; set; }

        public SqlQuery(string queryText, string title, bool serverScope, int queryNumber)
        {
            QueryText = queryText;
            Title = title;
            ServerScope = serverScope;
            QueryNumber = queryNumber;
        }

        public string GetQueryUseDBHelper(string database)
        {
            return string.IsNullOrEmpty(database) ? QueryText : "USE [" + database + "] \r\n " + QueryText;
        }
    }
}
