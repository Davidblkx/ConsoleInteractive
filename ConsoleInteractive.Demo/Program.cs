using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace ConsoleInteractive.Demo
{
    class Program
    {
        static Task<int> Main(string[] args)
        {
            var cmd = new RootCommand("") {
                BufferDemo.BuildBufferCommand(),
            };

            return cmd.InvokeAsync(args);
        }
    }
}
