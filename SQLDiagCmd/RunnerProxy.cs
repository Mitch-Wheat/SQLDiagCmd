using System;
using System.Collections.Generic;
using System.ComponentModel;

using CommandLine;
using CommandLine.Text;
using SQLDiagRunner;

namespace SQLDiagCmd
{
    /// <summary>
    /// NOTE: * This class's sole purpose is to delay type loads, so that the AssemblyResolver hookup in main happens early enough.
    /// </summary>
    class RunnerProxy
    {
        private sealed class Options : CommandLineOptionsBase
        {
            [Option("i", "inputfile", Required = true, HelpText = "Query diagnostic file to run.")]
            public string InputFile { get; set; }

            [Option("o", "outputfolder", Required = true, HelpText = "Folder location to write results file.")]
            public string OutputFolder { get; set; }

            [Option("E", null, HelpText = "Use trusted connection (Windows Authentication).")]
            public bool UseTrusted { get; set; }

            [Option("U", "username", HelpText = "Username for SQL Login.")]
            public string Username { get; set; }

            [Option("P", "password", HelpText = "Password for SQL Login.")]
            public string Password { get; set; }

            [Option("A", "autofit", HelpText = "Auto-fit Excel columns.", DefaultValue = true)]
            public bool AutoFitColumns { get; set; }

            [Option("t", "timeout", HelpText = "Query timeout in seconds.", DefaultValue = 360)]
            public int  QueryTimeout { get; set; }

            [OptionList("S", "servers", Required = true, Separator = ';', HelpText = "Semicolon separated list of Server names or instances to run queries against." +
                " Separate each server/instance name with a semicolon." + " Do not include spaces.")]
            [DefaultValue(null)]
            public IList<string> Servers { get; set; }

            [OptionList("d", "databases", Required = true, Separator = ';', HelpText = "Semicolon separated list of specific databases to run database specific queries against." +
    " Separate each database with a semicolon." + " Do not include spaces.")]
            [DefaultValue(null)]
            public IList<string> SpecificDatabases { get; set; }

            [OptionList("x", "excludequeries", Separator = ';', HelpText = "Semicolon separated list of query numbers that should not be run." +
    " Separate each number with a semicolon." + " Do not include spaces" + " Used to exclude long running queries (such as fragmention).")]
            [DefaultValue(null)]
            public IList<string> ExcludeQueryNumbers { get; set; }


            [HelpOption]
            public string GetUsage()
            {
                var help = new HelpText
                {
                    Heading = "SQLDiagCmd",
                    Copyright = new CopyrightInfo("Mitch Wheat", 2012, DateTime.Now.Year),
                    AdditionalNewLineAfterOption = true,
                    AddDashesToOption = true
                };
                HandleParsingErrorsInHelp(help);

                help.AddPreOptionsLine("");
                help.AddPreOptionsLine(""); 
                help.AddPreOptionsLine("This is free software. You may redistribute copies of it under the terms of");
                help.AddPreOptionsLine("the MIT License <http://www.opensource.org/licenses/mit-license.php>.");
                help.AddPreOptionsLine("It uses EPPlus <http://epplus.codeplex.com/> and");
                help.AddPreOptionsLine("the Command Line Parser Library <http://commandline.codeplex.com/>.");
                help.AddPreOptionsLine("");
                help.AddPreOptionsLine("Usage:");
                help.AddPreOptionsLine("   SQLDiagCmd -E -S servername -i queries.sql -o C:\\outputfolder -d databaselist -A");
                help.AddPreOptionsLine("   SQLDiagCmd -S servername -U username -P password -i queries.sql -o C:\\outputfolder -d databaselist");
                help.AddPreOptionsLine("   SQLDiagCmd -E -S servername -i queries.sql -o C:\\outputfolder -A");
                help.AddPreOptionsLine("   SQLDiagCmd -E -S serverlist -d databaselist -i queries.sql -o C:\\outputfolder -A");
                help.AddPreOptionsLine("   SQLDiagCmd -E -S server -d database -i 'SQL Server 2016 Diagnostic Information Queries.sql' -o C:\\output -A -x 37;65;66;71");
                help.AddPreOptionsLine("");
                help.AddPreOptionsLine("(serverlist and databaselist are semi-colon separated with no spaces)");
                help.AddOptions(this);

                return help;
            }

            private void HandleParsingErrorsInHelp(HelpText help)
            {
                if (LastPostParsingState.Errors.Count > 0)
                {
                    var errors = help.RenderParsingErrorsText(this, 2); // indent with two spaces
                    if (!string.IsNullOrEmpty(errors))
                    {
                        help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                        help.AddPreOptionsLine(errors);
                    }
                }
            }
        }

        private readonly string[] _args;

        public RunnerProxy(string[] args)
        {
            _args = args;
        }

        public void Run()
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(_args, options))
                Environment.Exit(1);

            var runner = new Runner();
            runner.ExecuteQueries(options.Servers, options.Username, options.Password,
                                  options.InputFile, options.OutputFolder, options.SpecificDatabases,
                                  options.UseTrusted, options.AutoFitColumns, options.QueryTimeout, 
                                  options.ExcludeQueryNumbers);
        }
    }
}
