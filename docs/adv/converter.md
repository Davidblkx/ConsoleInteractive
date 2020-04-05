# Input Conversion
---
## 1. About

Most data user inputs in a terminal is in string format, in order to allow for flexibility 
and to support complex types there are multiple ways to allow that conversion.

1. Use types that implement `IConvertible`, works for every common language runtime type.
2. Have type implement `IStringConverter<T>`, valid in most situation
3. Register a `IStringConverter<T>` for a specific type in a `StringConverterProvider`
---
## 2. IStringConverter

Interface to be implement by types to allow conversion from and to string.
It could be used in 2 ways

__1. Self implement:__
```C#
public class Person : StringConverter<Person> {
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(): base(PersonToString, StringToPerson) { }

    public static string PersonToString(Person p) => $"{p.Name}:{p.Age}";
    public static Person StringToPerson(string s) {
        var data = s.Split(':');
        return new Person { Name = data[0], Age = int.Parse(data[1]) };
    }
}
```

__2. Register a converter:__
```C#
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }

    public Person() { }

    public static string PersonToString(Person p) => $"{p.Name}:{p.Age}";
    public static Person StringToPerson(string s) {
        var data = s.Split(':');
        return new Person { Name = data[0], Age = int.Parse(data[1]) };
    }
}

// Register in a provider
var converter = new StringConverter<Person>(Person.PersonToString, Person.StringToPerson);
StringConverterProvider.Global.Register(converter);
```
---
## 3. StringConverter Provider

A `StringConverterProvider` is a class that allows to register and retrieve `IStringConverter` 
for a type. By default is used the `StringConverterProvider.Global` instance, so if 
a converter is supposed to be reused, it should be register here.

__Registration example:__
```C#
var converter = new StringConverter<Person>(Person.PersonToString, Person.StringToPerson);
StringConverterProvider.Global.Register(converter);
```
---
## 4. Factory

When it's time to convert a input, the `StringConverterFactory` is used to 
create the `IStringConverter` for the target type. To Create a converter the factory
tries to get it following the steps:

1. Current Provider (`StringConverterProvider.Global` by default)
2. Self implementation, if type implements `IStringConverter`
3. `IConvertible` if type implements it
4. Throw an `Exception`
