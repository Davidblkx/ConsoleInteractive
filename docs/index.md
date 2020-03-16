# ConsoleInteractive

## About

ConsoleInteractive is a `netstandard2` library that make it easy to parse and
 validate data from the console. It provide methods to request data in all basic
 C# types, and allow to create new input types

## Why

When I create small comand line tools I kept rewriting input parse and validation,
 and despite great tools to parse  arguments and to print output, I didn't find
 anything that could help me do it.

## Install

You can install it using nuget:

- `dotnet add package ConsoleInteractive --version 1.1.0`
- `<PackageReference Include="ConsoleInteractive" Version="1.1.0" />`
- `Install-Package ConsoleInteractive -Version 1.1.0`

## How-to start

The easiest way to start using it is to clone the repo and see how the
 ConsoleInterative.Demo is built, or by reading this documenation

## Examples

### Simple question

```C#
using ConsoleInteractive

var strQ = await ConsoleI.AskQuestion<string>("Could you write something");
Console.WriteLine(strQ); // user input, null if no input
```

### Set a default value
```C#
using ConsoleInteractive

var strQ = await ConsoleI.AskQuestion<string>("Could you write something", "Hello");
Console.WriteLine(strQ); // user input, "Hello" if no input
```

### Validate input
```C#
using ConsoleInteractive;
using ConsoleInteractive.Question.Validators;
using ConsoleInteractive.Question.Validators.Comparable;

var intValidators = QuestionValidators<int>
    .FromEmpty()
    .GreaterThan(199)
    .LessThanOrEqual(250);

var value = await ConsoleI.AskQuestion<int>("A number between 200 and 250", 0, intValidators);
Console.WriteLine(value); // a int value between 200 and 250
```

### Select a Enum
```C#
using ConsoleInteractive;

enum Fruits {
    Orange,
    Banana,
    Peach,
    Pineapple
}

Console.WriteLine("Select your favourite fruit:");
Fruits fruit = ConsoleI.Select<Fruits>();
Console.WriteLine(fruit); // Orange, Banana, Peach or Pineapple
```
### Select custom object
```C#
using ConsoleInteractive;
using ConsoleInteractive.Selection;

private class Colour {
    public string Name { get; set; };
    public string HexValue { get; set; };

    public override string ToString() {
        return Name;
    }
}

var options = new SelectionGroup<Colour>()
    .Add(new Colour { Name = "hotpink", HexValue = "#FF69B4" })
    .Add(new Colour { Name = "indigo", HexValue = "#4B0082" })
    .Add(new Colour { Name = "cobalt", HexValue = "#3D59AB" })
    .Add(new Colour { Name = "slategray", HexValue = "#708090" })
    .Add(new Colour { Name = "turquoise", HexValue = "#00F5FF" })
    .Add(new Colour { Name = "maroon", HexValue = "#800000" })

Console.WriteLine("Select your favourite colours:");
IEnumerable<Colour> colours = ConsoleI.Select(options, 3);
Console.WriteLine(colours); // 1 to 3 Colour objects
```

### Request a class using multiple questions

```C#
using ConsoleInteractive.Forms;

public class UserCV {
    [FormField("What is your name")]
    public string Name { get; set; }
    [FormField("What is your age", 30)]
    public int Age { get; set; }
    [FormField("Can list your work titles (separate by ';')")]
    public List<string> WorkTitles { get; set; }

    public override string ToString() {
        return $"{Name} ({Age}) => {string.Join(';', WorkTitles)}";
    }
}

var result = await ConsoleI.RequestForm<UserCV>();
Console.WriteLine(result); // UserCV object
```