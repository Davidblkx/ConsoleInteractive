namespace ConsoleInteractive.Selection
{
    public class SelectionOption<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }

        public SelectionOption(T value, string? text = default) {
            Value = value;
            Text = text ?? value?.ToString() ?? "Unkown";
        }
    }
}