using System;
using System.Collections.Generic;
using System.ComponentModel;

using CommandLine;
using CommandLine.Text;
using SQLDiagRunner;

namespace SQLDiagCmd
{
    /// <summary>
    /// This class's sole purpose is to delay type loads, so that the AssemblyResolver hookup in main
    /// is early enough.
    /// </summary>
    class RunnerProxy
    {
        private sealed class Options : CommandLineOptionsBase
        {
            #region Standard Option Attribute

            [Option("S", "servername", Required = true, HelpText = "Server name or instance to run queries against.")]
            public string ServerName { get; set; }

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

            [Option("A", "autofit", HelpText = "Auto-fit Excel columns.")]
            public bool AutoFitColumns { get; set; }

            [Option("t", "timeout", HelpText = "Query timeout in seconds.", DefaultValue = 360)]
            public int  QueryTimeout { get; set; }

            #endregion

            #region Specialized Option Attribute

            [OptionList("d", "databases", Separator = ';', HelpText = "Semicolon separated list of specific databases to examine." +
                " Separate each database with a semicolon." + " Do not include spaces between databases and semicolon.")]
            [DefaultValue(null)]
            public IList<string> SpecificDatabases { get; set; }

            #endregion

            #region Help Screen


            /*
            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, delegate(HelpText current) {
                    if (this.LastPostParsingState.Errors.Count > 0)
                    {
                        var errors = current.RenderParsingErrorsText(this, 2); // indent with two spaces
                        if (!string.IsNullOrEmpty(errors))
                        {
                            current.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                            current.AddPreOptionsLine(errors);
                        }
                    }
                });
            }
                      
            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
            */


            [HelpOption]
            public string GetUsage()
            {
                var help = new HelpText
                {
                    Heading = "SQLDiagCmd",
                    Copyright = new CopyrightInfo("Mitch Wheat", 2012, 2013),
                    AdditionalNewLineAfterOption = true,
                    AddDashesToOption = true
                };
                HandleParsingErrorsInHelp(help);
                help.AddPreOptionsLine(""); 
                help.AddPreOptionsLine("This is free software. You may redistribute copies of it under the terms of");
                help.AddPreOptionsLine("the MIT License <http://www.opensource.org/licenses/mit-license.php>.");
                help.AddPreOptionsLine("It uses EPPlus <http://epplus.codeplex.com/> and");
                help.AddPreOptionsLine("the Command Line Parser Library <http://commandline.codeplex.com/>.");
                help.AddPreOptionsLine("");
                help.AddPreOptionsLine("Usage: SQLDiagCmd -E -S DEVPC -i queries.sql -o C:\\outputfolder -d databaselist -A");
                help.AddPreOptionsLine("       SQLDiagCmd -S DEVPC -U username -P password -i queries.sql -o C:\\outputfolder -d databaselist");
                help.AddPreOptionsLine("       SQLDiagCmd -E -S DEVPC -i queries.sql -o C:\\outputfolder -A");
                help.AddOptions(this);

                //  -E -S MUTLEY -i "M:\Develop\Samples\SQLDiagRunner\TestProject1\SQL Server 2008 Diagnostic Information Queries.sql" 
                //  -o c:\temp -d "AdventureWorks2008;AdventureWorks2008R2"

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

            #endregion
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
            runner.ExecuteQueries(options.ServerName, options.Username, options.Password,
                       options.InputFile, options.OutputFolder, options.SpecificDatabases,
                       options.UseTrusted, options.AutoFitColumns, options.QueryTimeout);
        }
    }
}
