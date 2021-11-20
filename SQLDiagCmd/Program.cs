using System;
using System.Reflection;

namespace SQLDiagCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            SetEmbeddedAssembliesResolver();

            var runner = new RunnerProxy(args);
            runner.Run();

            Environment.Exit(0);
        }

        static void SetEmbeddedAssembliesResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                String resourceName = "SQLDiagCmd." + new AssemblyName(args.Name).Name + ".dll";
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    var assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
        }
    }
}
