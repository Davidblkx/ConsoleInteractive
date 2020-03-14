using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleInteractive.Question.Validators;
using ConsoleInteractive.Question.Validators.String;
using ConsoleInteractive.Question.Validators.Comparable;

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

            var intQ = await ConsoleI.AskQuestion("Ask for a number", 100);
            Console.WriteLine("Number response => " + intQ);
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

            var intValidators = QuestionValidators<int>
                .FromEmpty()
                .GreaterThan(200)
                .LessThanOrEqual(250);
            intQ = await ConsoleI.AskQuestion("Ask for a number between 201 and 250", 100, intValidators);
            Console.WriteLine("Number response => " + intQ);
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);
        }
    }
}