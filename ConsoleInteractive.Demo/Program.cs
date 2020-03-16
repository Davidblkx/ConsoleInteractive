using System.CommandLine;
using System.Threading.Tasks;
using ConsoleInteractive.Question;

namespace ConsoleInteractive.Demo
{
    class Program
    {
        static Task<int> Main(string[] args)
        {
            QuestionFactoryProvider.RegisterDefaultProviders();

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
