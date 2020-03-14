using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleInteractive.Question.Validators;
using ConsoleInteractive.Question.Validators.String;

namespace ConsoleInteractive.Demo
{
    public static class QuestionDemo
    {
        public static Command BuildQuestionCommand() {
            var cmd = new Command("question", "Console questions demo")
            {
                Handler = CommandHandler.Create(() => StartDemo())
            };
            cmd.AddAlias("q");

            return cmd;
        }

        public static async Task StartDemo() {
            const string NAME = "#BUFFER_QUESTION#";
            ConsoleBuffer.MemoriseBufferPosition(NAME);
            
            var strQ = await ConsoleI.AskQuestion<string>("Simple string question");
            Console.WriteLine("String response => " + strQ);
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);

            strQ = await ConsoleI.AskQuestion("Simple string question", "default value");
            Console.WriteLine("String response => " + strQ);
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);

            var validators = QuestionValidators<string>
                .FromEmpty()
                .AddMinLength(2)
                .AddMaxLength(5);

            strQ = await ConsoleI.AskQuestion("Validate input length >= 2 and <= 5", "", validators);
            Console.WriteLine("String response => " + strQ);
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);
        }
    }
}