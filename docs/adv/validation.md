# Validation
___

## 1. About

In order to validate user input, you need to create validators. Validators are 
lambdas that return a boolean with a success status and a message of the 
reason to fail.
They could be represent by: 
```C#
(T e) => (bool, string?)
```
___
## 2. Validator<T\>

In order to allow Generics usage and more flexibility, validators are build from
the class `Validator<T>`. And by default all validators are treated as a `Task`.

A validator that check if the user inputted an empty text, could be written as:
```C#
var validator = new Validator<string>(e => (!string.IsNullOrEmpty(e), "Can't be empty"))
```

Async validators are also accepted, actually all validators are treated as async. An async 
validator could be written as:
```C#
var validator = new Validator<string>(async e => (await LongCall(e), "LongCall failed to validate input"))
```
___
## 3. Exceptions

Sometimes returning a message for each error in a long validator could be 
laborious, so it's possible to write a method that report errors by throwing
an `Exception` with a custom message. By default all exception are handled, but
if the Exception `ValidatorException` is thrown the full message is shown 
instead of a default one.

Example output:

- `throw new Exception("Invalid length")` => "Error validating input: Invalid Length"
- `throw new ValidatorException("Invalid length")` => "Invalid Length"
___
## 4. Collection

The best way to represent a series of validations is using the `ValidatorCollection<T>`, 
it allows to aggregate multiple validators and when validates an Input it keeps the 
insert order.

Collections are also the main way to run a validation, which mean that
most interaction is done through this object.

An example of a collection that could check an input password:
```C#
var validators = ValidatorCollection
    .Create<string>()
    .Add(e => (e.Length >= 8, "Length must be at least 8"))
    .Add(e => (e.Length < 64, "Length must be less than 64"))
    .Add(async e => (await CheckPwnedPassword(e), "Password has appear in a data breach"));

var (valid, errorMessage) = validators.ValidateInput(password);
```
___
## 5. ValidatorProvider

A `ValidatorProvider` is a way to reference a `ValidatorCollection<T>` by name, 
which allow to call validation for an input without even knowing the type.
It also allow to use `Enum` as a name and `Flags` to aggregate multiple 
collection. Every time a provider is required and none is available, the
default `ValidatorProvider.Global` is used.

__Register a collection and validate inputs:__

```C#
var validators = ValidatorCollection
    .Create<string>()
    .Add(e => (e.Length >= 8, "Length must be at least 8"))
    .Add(e => (e.Length < 64, "Length must be less than 64"))
    .Add(async e => (await CheckPwnedPassword(e), "Password has appear in a data breach"));

ValidatorProvider.Global.Register<string>("PASSWORD", validators);

// It can be used in any moment by
var (valid, errorMessage) = await ValidatorProvider.Global.Validate("PASSWORD", password);
```

__Using Enums:__

```C#
[Flags]
enum StringValidator {
    NotEmpty = 1,
    MinLength3 = 2,
    MinLength7 = 4,
    Lowercase = 8,
}

ValidatorProvider.Global.Register(
    StringValidator.NotEmpty,
    ValidatorCollection.Create<string>()
        .Add(e => (!string.IsNullOrEmpty(e), "Can't be empty")));

ValidatorProvider.Global.Register(
    StringValidator.MinLength3,
    ValidatorCollection.Create<string>()
        .Add(e => (e.Length >= 3, "Length must be at least 3")));

ValidatorProvider.Global.Register(
    StringValidator.MinLength7,
    ValidatorCollection.Create<string>()
        .Add(e => (e.Length >= 7, "Length must be at least 7")));

ValidatorProvider.Global.Register(
    StringValidator.Lowercase,
    ValidatorCollection.Create<string>()
        .Add(e => (e == e.ToLower(), "Must be in lowercase")));

var validator = StringValidator.MinLength7 | StringValidator.Lowercase;

// Check that input has min length 7 and is in lowercase
var (valid, errorMessage) = await ValidatorProvider.Global.Validate(validator, input);
```




