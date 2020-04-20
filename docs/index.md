# ConsoleInteractive

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
 ConsoleInteractive.Demo is built, or by exploring this documentation