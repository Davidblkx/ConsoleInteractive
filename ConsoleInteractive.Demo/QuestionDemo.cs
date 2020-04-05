using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleInteractive.Components.Factory;
using ConsoleInteractive.InputValidation;

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

            var inputText = InputTextFactory.Create<string>("Write something");
            var strQ = await inputText.RequestInput();
            Console.WriteLine("String response => " + strQ);
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);

            var inputNum = InputTextFactory.Create("Write a number", 10L);
            var numQ = await inputNum.RequestInput();
            Console.WriteLine("String response => " + numQ);
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);

            var validatorMinMax = ValidatorCollection.Create<int>()
                .Add(l => (l > 10, "Value must be higher than 10"))
                .Add(l => (l < 50, "Value must be less than 50"));
            var inputInt = InputTextFactory.Create("Write a number between 10 and 50", 0, validatorMinMax);
            var intQ = await inputInt.RequestInput();
            Console.WriteLine("String response => " + intQ);
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);
        }
    }
}