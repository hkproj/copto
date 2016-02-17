Copto
===================
Copto is a C# library that lets you *incrementally* parse your command line arguments.

Features
-------------
Copto can parse the following:

- Switches: `/flag`, `-flag`, `--flag`
- Boolean switches: `-flag=true`, `--flag=false`, `/flag:true`
- Arguments with a single value: `--argument=value`, `/argument:value`, `--argument "value"`
- Arguments with multiple values: `--include "file1.h" --include "file2.h" /include "file3.h"`

Quick examples
-------------
Command line:

	    ./myprogram generate-report --days 4 --pi="3.14" --use "GOOG" -add "MSFT" /use:"YHOO" /Add "AMZN" --o "report.pdf"

Parsing:
```csharp
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
