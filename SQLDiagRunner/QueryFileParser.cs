
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SQLDiagRunner
{
    public class QueryFileParser
    {
        private readonly string _filepath;
        private const string PatternQueryStart = @"^--(.*)\(Query\s*\d*\)(\s*\((.*)\)|\s*.*)$";
        private const string EndOfServerWideQueriesMarker = "-- Database specific queries";

        private readonly Regex _regexQueryStart = new Regex(PatternQueryStart, RegexOptions.Compiled|RegexOptions.IgnoreCase);

        public QueryFileParser(string filepath)
        {
            _filepath = filepath;
        }

        public List<SqlQuery> Load()
        {
            if (!File.Exists(_filepath))
            {
                throw new FileNotFoundException(_filepath);
            }

            string[] queryText = File.ReadAllLines(_filepath);

            int endLine = FindEndofServerWideQueries(queryText);

            List<SqlQuery> result = Parse(queryText, 0, endLine, true);
            result.AddRange(Parse(queryText, endLine, queryText.Count() - 1, false));

            return result;
        }

        private int FindEndofServerWideQueries(IEnumerable<string> queryAllLines)
        {
            int i = 0;
            foreach (var s in queryAllLines)
            {
                if (s.StartsWith(EndOfServerWideQueriesMarker))
                {
                    return i;
                }
                i++;
            }

            return i - 1;
        }

        private List<SqlQuery> Parse(string[] queryAllLines, int startLine, int endLine, bool serverWide)
        {
            var result = new List<SqlQuery>();

            int i = startLine;

            // skip any pre-amble
            while (i < endLine)
            {
                Match matchStart = _regexQueryStart.Match(queryAllLines[i]);
                if (matchStart.Success)
                    break;

                i++;
            }

            while (i < endLine)
            {
                string line = queryAllLines[i];
                var query = new StringBuilder(queryAllLines[i]);
                query.AppendLine("");

                string title = GetQueryTitle(line);

                // End of query: readlines until next query is found or EOF
                while (i < endLine)
                {
                    i++;
                    line = queryAllLines[i];
                    Match matchEnd = _regexQueryStart.Match(line);
                    if (matchEnd.Success)
                        break;

                    query.AppendLine(line);
                }

                result.Add(new SqlQuery(query.ToString(), title, serverWide));
            }

            return result;
        }

        private string GetQueryTitle(string line)
        {
            Match match = _regexQueryStart.Match(line);
            string title = line;

            if (match.Groups.Count > 2 && !string.IsNullOrEmpty(match.Groups[2].ToString().Trim()))
            {
                string tmp = match.Groups[2].ToString().Trim();
                int start = 0;
                int len = tmp.Length;
                if (tmp.StartsWith("("))
                {
                    start = 1;
                    len--;
                }
                if (tmp.EndsWith(")"))
                    len--;
                title = tmp.Substring(start, len);
            }
            else if (match.Groups.Count > 1)
            {
                title = match.Groups[1].ToString().Trim();
            }

            return title;
        }
    }
}
