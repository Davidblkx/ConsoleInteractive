![Create Release](https://github.com/Davidblkx/ConsoleInteractive/workflows/Create%20Release/badge.svg?branch=master)
![Nuget](https://img.shields.io/nuget/v/ConsoleInteractive)

[Documentation](https://davidblkx.github.io/ConsoleInteractive/)

# ConsoleInteractive

![gif](docs/gifs/input-form.gif)

## About

ConsoleInteractive is a `netstandard2` library that make it easy to parse and
 validate data from the console. It provide methods to request data in all basic
 C# types, and allow to create new input types

## Why

When I create small command line tools I kept rewriting input parse and validation,
 and despite great tools to parse  arguments and to print output, I didn't find
 anything that could help me do it.

## Install

You can install it using nuget:

- `dotnet add package ConsoleInteractive --version 2.0.1`
- `<PackageReference Include="ConsoleInteractive" Version="2.0.1" />`
- `Install-Package ConsoleInteractive -Version 2.0.1`

Register default components:

- `ConsoleI.RegisterDefaults()`

## How-to start

The easiest way to start using it is to clone the repo and see how the
 ConsoleInteractive.Demo is built, or by reading the [Documentation](https://davidblkx.github.io/ConsoleInteractive/)

## Example

##### Class with attributes and validations

```C#
public class TestFormClass {
    public const string REQUIRED = "TEXT_REQUIRED";
    public const string AGE_INTERVAL = "AGE_INTERVAL";
    public const string WORK_TITLES_COMPONENT = "WORK_TITLES_ENUM_COMPONENT";

    public TestFormClass() {
        /* For demo purposes validators and components are register at the constructor 
           In real world, would be better to do it at start up, or before the
           first call */

        // Validation for string
        ValidatorProvider.Global.Register(REQUIRED, ValidatorCollection
            .Create<string>().Add(s => (s.Length > 0, "Can't be empty")));
        
        // Validation for uint
        ValidatorProvider.Global.Register(AGE_INTERVAL, ValidatorCollection
            .Create<uint>()
                .Add(n => (n >= 13, "Age must be older than 13"))
                .Add(n => (n <= 150, "Age must be younger then 150"))
        );

        // Register a custom component with name <WORK_TITLES_COMPONENT>
        ComponentsProvider.Global.Register(WORK_TITLES_COMPONENT, 
            InputSelection.FromEnum<WorkTitles>());
    }

    [FormEntry(Priority = 0, Message = "Insert name", ValidatorsKey = REQUIRED)]
    public string Name { get; set; }

    [FormEntry(Priority = 1, Message = "Insert age", ValidatorsKey = AGE_INTERVAL)]
    public uint Age { get; set; }

    [FormEntry(Priority = 2, Message = "Select work", ProviderKey = WORK_TITLES_COMPONENT)]
    public WorkTitles WorkTitle { get; set; } // Enum

    public override string ToString() {
        return $"{Name} ({Age}) => {string.Join(';', WorkTitle.ToString())}";
    }
}
```

##### Request class properties

```C#
TestFormClass data = await ConsoleI.RenderForm<TestFormClass>();
```

### Check [Documentation](https://davidblkx.github.io/ConsoleInteractive/)