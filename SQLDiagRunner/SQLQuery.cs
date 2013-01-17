

namespace SQLDiagRunner
{
    public class SqlQuery
    {
        public string QueryText { get; set; }
        public string Title { get; set; }

        public bool ServerScope { get; set; }

        public SqlQuery(string queryText, string title, bool serverScope)
        {
            QueryText = queryText;
            Title = title;
            ServerScope = serverScope;
        }
    }
}
