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
                QuestionDemo.BuildQuestionCommand(),
                SelectionDemo.BuildSelectionCommand(),
                FormDemo.BuildFormCommand()
            };

            return cmd.InvokeAsync(args);
        }
    }
}
