using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleInteractive.Components;
using ConsoleInteractive.Form;
using ConsoleInteractive.InputConverter;
using ConsoleInteractive.InputValidation;

namespace ConsoleInteractive.Demo
{
    public static class FormDemo
    {

        public static Command BuildFormCommand() {
            var cmd = new Command("form", "Form demo")
            {
                Handler = CommandHandler.Create(() => StartDemo())
            };
            cmd.AddAlias("f");

            return cmd;
        }

        public static async Task StartDemo() {
            ConsoleI.RegisterDefaults();
            var data = await ConsoleI.RenderForm<TestFormClass>();
            Console.WriteLine($"Result: {data}");
            ConsoleI.AwaitContinue();
        }
    }

    public class TestFormClass {
        public const string REQUIRED = "TEXT_REQUIRED";
        public const string AGE_INTERVAL = "AGE_INTERVAL";
        public const string WORK_TITLES_COMPONENT = "WORK_TITLES_ENUM_COMPONENT";
        public const string URL_COMPONENT = "URL_COMPONENT";

        public TestFormClass() {
            ValidatorProvider.Global.Register(REQUIRED, ValidatorCollection
                .Create<string>().Add(s => (s.Length > 0, "Can't be empty")));
            ValidatorProvider.Global.Register(AGE_INTERVAL, ValidatorCollection
                .Create<uint>()
                    .Add(n => (n >= 13, "Age must be older than 13"))
                    .Add(n => (n <= 150, "Age must be younger then 150"))
            );

            StringConverterProvider.Global.Register(new UriConverter());

            ComponentsProvider.Global.Register(WORK_TITLES_COMPONENT, 
                InputSelection.FromEnum<WorkTitles>());

            ComponentsProvider.Global.Register(URL_COMPONENT, 
                InputText.Create<Uri>("Url"));
        }

        [FormEntry(Priority = 0, Message = "Insert name", ValidatorsKey = REQUIRED)]
        public string Name { get; set; }

        [FormEntry(Priority = 1, Message = "Insert age", ValidatorsKey = AGE_INTERVAL)]
        public uint Age { get; set; }

        [FormEntry(Priority = 2, Message = "Select work", ProviderKey = WORK_TITLES_COMPONENT)]
        public WorkTitles WorkTitle { get; set; }

        [FormEntry(Priority = 2, Message = "Url", ProviderKey = URL_COMPONENT)]
        public Uri ServerUrl { get; set; }

        public override string ToString() {
            return $"[{ServerUrl}] {Name} ({Age}) => {string.Join(';', WorkTitle.ToString())}";
        }
    }

    public enum WorkTitles {
        Programmer,
        Manager,
        Mechanic,
        Plumber,
        Electrician,
        Carpenter,
        Mason,
        ItSupport
    }

    public class UriConverter : StringConverter<Uri>
    {
        public UriConverter() : base(UriToString, StringToUri) {}

        public static string UriToString(Uri u) => u?.ToString() ?? "";
        public static Uri StringToUri(string s) {
            if (Uri.IsWellFormedUriString(s, UriKind.Absolute)) {
                return new Uri(s);
            }

            throw new ConvertStringFormatException("Url is invalid, should match [http://]");
        }
    }
}