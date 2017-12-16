using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SQLDiagRunner
{
    public class QueryFileParser
    {
        private readonly string _filepath;
        private const string PatternQueryStart = @"^--(.*)\(Query\s*(\d*)\)(\s*\((.*)\)|\s*.*)$";
        //                                             0              1     2     3
        private const string EndOfServerWideQueriesMarker = "-- Database specific queries";

        private readonly Regex _regexQueryStart = new Regex(PatternQueryStart, RegexOptions.Compiled|RegexOptions.IgnoreCase);

        public QueryFileParser(string filepath)
        {
            _filepath = filepath;
        }

        public List<SqlQuery> Load()
        {
            if (!File.Exists(_filepath))
                throw new FileNotFoundException(_filepath);

            string[] queryText = File.ReadAllLines(_filepath);

            int endLine = FindEndofServerWideQueries(queryText);

            var result = Parse(queryText, 0, endLine, true);
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

        private List<SqlQuery> Parse(string[] queryLines, int startLine, int endLine, bool serverWide)
        {
            if (startLine >= endLine)
                throw new System.ArgumentOutOfRangeException("startLine must be less than endline!");

            var result = new List<SqlQuery>();
            Match matchStart;

            int i = startLine;
            // skip any pre-amble
            do
            {
                matchStart = _regexQueryStart.Match(queryLines[i]);
                if (matchStart.Success)
                    break;

            } while (++i < endLine);

            while (i < endLine)
            {
                string line = queryLines[i];
                var query = new StringBuilder(line);
                query.AppendLine("");

                matchStart = _regexQueryStart.Match(queryLines[i]);

                string title = GetQueryTitle(matchStart);
                int number = GetQueryNumber(matchStart);

                // End of query: readlines until next query is found or EOF
                while (i < endLine)
                {
                    i++;
                    line = queryLines[i];
                    Match matchEnd = _regexQueryStart.Match(line);
                    if (matchEnd.Success)
                        break;

                    query.AppendLine(line);
                }

                result.Add(new SqlQuery(query.ToString(), title, serverWide, number));
            }

            return result;
        }

        private string GetQueryTitle(Match match)
        {
            string result = "No Title Found";

            if (match.Groups.Count > 4)
            {
                var tmp = match.Groups[4].ToString().Trim();
                if (tmp.Length > 0)
                {
                    result = tmp;
                }
            }
            else if (match.Groups.Count > 3)
            {
                string tmp = match.Groups[3].ToString().Trim();
                if (tmp.Length > 0)
                {
                    result = tmp.RemoveStartAndEndChars("(", ")");
                }
            }

            return result;
        }

        private int GetQueryNumber(Match match)
        {
            int result = 0;

            if (match.Groups.Count > 2)
            {
                int tmp;

                if (int.TryParse(match.Groups[2].ToString().Trim(), out tmp))
                {
                    result = tmp;
                }
            }

            return result;
        }

    }
}
