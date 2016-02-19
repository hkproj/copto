Copto
===================
[![Build status](https://ci.appveyor.com/api/projects/status/s1ik7y6t076rt0k6?svg=true)](https://ci.appveyor.com/project/hkproj/copto)

Copto is a C# library that lets you *incrementally* parse command line arguments. This means you don't have to create a huge class to hold all your options: you parse/detect arguments only when you need them.

Features
-------------
Copto can parse the following:

- Switches: `/flag`, `-flag`, `--flag`
- Boolean switches: `-flag=true`, `--flag=false`, `/flag:true`
- Arguments with a single value: `--argument=value`, `/argument:value`, `--argument "value"`
- Arguments with multiple values: `--include "file1.h" --include "file2.h" /include "file3.h"`

Quick examples
-------------
#### Parse
Import the library
```csharp
using Copto;
```

Start by parsing the arguments and saving the result into a variable

```csharp
var opts = Options.Parse(args);
```

You can turn of case sensitivity
```csharp
var opts = Options.Parse(args, new ParserOptions() { CaseSensitive = false });
```

#### Switches
```csharp
opts.Apply(new RuleSet()
{
	// Use multiple aliases
	{ "generate-report|genreport|gr", () => operation = OperationType.GenerateReport }
});
```

#### Boolean switches
```csharp
opts.Apply(new RuleSet()
{
	// A null value is passed in case the user didn't specify "true" or "false" explicitly after the switch.
	// The following code activates the "verbose mode" even if the user only specified "--verbose" (or "--v")
	{ "v|verbose", (val) => verbose = val ?? true }
});
```

#### Arguments with single value
```csharp
opts.Apply(new RuleSet()
{
	{ "o|output", (val) => outputFile = val },
	// Integer value
	{ "p|priority",  delegate (int? val) { jobPriority = val; } }
	// Floating-point value
	{ "s|seed", delegate (float? val) { randomSeed = val; } }
});
```

#### Arguments with multiple value
```csharp
opts.Apply(new RuleSet()
{
	{ "l|link|link-with", (val) => linkWith.Add(val) }
});
```

#### Specify argument position
Copto allows you to detect arguments only if they are found at a specific position; this is very useful if the first argument is the type of operation to perform. For example a package management tool might have the following rules:
```csharp
opts.Apply(new RuleSet()
{
	// Only if the argument is found at position "0" (first)
	{ "update", () => operation = OperationType.Update, 0 },
	{ "install", () => operation = OperationType.Install, 0 },
	{ "delete", () => operation = OperationType.Delete, 0 },
	// and here only if the argument is found at position "1" (second argument)
	{ "package", () => objectType = ObjectTypes.Package, 1 },
	{ "system", () => objectType = ObjectTypes.System, 1 },
	{ "repository", () => objectType = ObjectTypes.Repository, 1 }
});
```

Full example
-------------
The examples shows a simple report generation program. The program is launched with the following arguments:

`./myprogram generate-report --days 4 --pi="3.14" --use "GOOG" -add "MSFT" /use:"YHOO" /Add "AMZN" --o "report.pdf"`

As you can see there are integer values, floating point values and multiple arguments with the same name (but different format).

Parsing is easy and is done incrementally, which means the arguments are parsed only when needed:
```csharp
using Copto;

static Options Opts;

static void Main(string[] args)
{
	// Consider "add" and "Add" to be the same
	Opts = Options.Parse(args, new ParserOptions() { CaseSensitive = false });
	var operation = OperationType.None;

	// The third, optional, parameter "0", indicates to only apply this rule if the argument is found in position "0".
	Opts.Apply(new RuleSet()
	{
		{ "generate-report", () => operation = OperationType.GenerateReport, 0 }
	});

	if (operation == OperationType.GenerateReport)
		GenerateReport();
	else
		Console.WriteLine("No operation specified");


	Console.ReadLine();
}

static void GenerateReport()
{
	var generator = new ReportGenerator();

	// No need to re-parse the arguments. Use existing instance of the class.
	Opts.Apply(new RuleSet()
	{
		// Automatically parses numeric values and booleans
		{ "days", (days) => generator.Days = days },
		{ "PI", delegate (float? pi) { generator.PI = pi; } },
		{ "use|add", (use) => generator.Symbols.Add(use) },
		{ "o|output", (output) => generator.OutputFile = output }
	});

	generator.Generate();
}
```
