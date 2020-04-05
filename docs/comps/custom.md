# Custom Components
---
## About

Its possible to create custom components, for that we only need to extend the
abstract class `BaseInputRender<T>` and implement the method `Task<T> RequestInput()`.
Then we can register it in the `InputRenderProvider` just like any other component

---
## Example

```
public class InputText<T> : BaseInputRender<T>
{
    public Type Target => typeof(T);

    /// <summary>
    /// Message to show before requesting input
    /// </summary>
    /// <value></value>
    public string Message { get; set; }

    /// <summary>
    /// Default value to use if no input
    /// </summary>
    /// <value></value>
    public object? DefaultValue { get; set; }

    public InputText() {
        Message = $"Input value for a {Target}";
    }

    public override async Task<T> RequestInput()
    {
        const string BUFFER_NAME = "#INTERNAL.COMPONENT.TEXTINPUT.TOP";
        // Write message with default value
        Console.WriteLine(BuildMessage());
        // Save current console position
        ConsoleBuffer.MemoriseBufferPosition(BUFFER_NAME);

        T ToObject(string v) => GetConverter().ToObject(v);

        while (true) {
            try {
                var res = Console.ReadLine();

                var data = (string.IsNullOrEmpty(res), DefaultValue is null) switch
                {
                    (false, _) => ToObject(res), // is not empty
                    (true, false) => (T)DefaultValue!, // empty but has default value
                    (_, _) => ToObject("") // empty without default value
                };

                var (valid, message) = await Validators.ValidateInput(data);
                if (!valid) throw new Exception(message ?? "Error validating input");

                // return converted input
                return data;
            } catch (Exception ex) {
                // Write to console
                Console.WriteLine(ex.Message + ",\nPress [Return] to retry...");

                // Wait user confirmation
                Console.ReadKey();

                // Clear input and return to question
                ConsoleBuffer.ClearBufferFrom(BUFFER_NAME);
            }
        }
    }

    private string BuildMessage() {
        var converter = GetConverter();
        var defaultVal = DefaultValue is null ? "" : $"({converter.ToString(DefaultValue)})";
        return $"{Message} {defaultVal}";
    }
}
```