# CLIArgumentsParser
Library to easily manage and parse CLI arguments

[![Build Status](https://garaproject.visualstudio.com/CLIArgumentParser/_apis/build/status/CLIArgumentsParser-CI?branchName=master)](https://garaproject.visualstudio.com/CLIArgumentParser/_build/latest?definitionId=70&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=fgaravaglia_CLIArgumentsParser&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=fgaravaglia_CLIArgumentsParser)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=fgaravaglia_CLIArgumentsParser&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=fgaravaglia_CLIArgumentsParser)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=fgaravaglia_CLIArgumentsParser&metric=bugs)](https://sonarcloud.io/summary/new_code?id=fgaravaglia_CLIArgumentsParser)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=fgaravaglia_CLIArgumentsParser&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=fgaravaglia_CLIArgumentsParser)

[![Nuget](https://img.shields.io/nuget/v/CLIArgumentParser.svg?style=plastic)](https://www.nuget.org/packages/CLIArgumentParser/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/CLIArgumentParser.svg)](https://www.nuget.org/packages/CLIArgumentParser/)

To install it, use proper command:
```
dotnet add package CliArgumentParser --version 1.2.1975
```

For more details about download, see [NuGet Web Site](https://www.nuget.org/packages/CliArgumentParser#readme-body-tab)

Main Features
----------------------------------
- define a model of usage for your CLI applications
- Automatic parse and validation of your arguments against the defined model
- Add examples of usage
- Print automatic helper on CLI usage

# How to define an Usage Model
To define you model, create a class inheriting from CliCommand one.

```c#
 class ScanCommand : CliCommand
 {
    public ScanCommand() : base("scan", "Scan the target folder tree")
    {}
 }
```

Remember to:
- define the method _SetDefaultValues_ to fill proper option dictionary
- define the properties of command, with decorators (see the paragraph)
- define examples

## Option Definition
All properties you add should map an option of the verb, such:
```c#
    [Option(OPT_FOLDER, "Root folder to scan", isMandatory: true)]
    public string Folder
    {
            get { return this.GetArgumentValue<ScanCommand, string>(x => x.Folder); }
            set { this.AddOrUpdateArgument<ScanCommand, string>(x => x.Folder, value); }
    }
```

Then, remember to parse the string argument:
```c#
public override void ParseArgument(string[] tokens)
        {
            // take the expecetd value
            if (tokens.Length == 1)
            {
                throw new NotImplementedException();
            }
            else
            {
                // validate option
                switch (tokens[0])
                {
                    case OPT_FOLDER:
                    case OPT_FILENAME_SEARCH_PATTERN:
                    case OPT_FILECONTENT_SEARCH_PATTERN:
                        this.UpdateArgumentValue(tokens[0], tokens[1]);
                        break;

                    case OPT_TO:
                        this.PersistedTo = tokens[1].Trim().ToUpperInvariant();
                        break;

                    default:
                        throw new WrongOptionUsageException(this.Verb, tokens[0]);
                }
            }
        }
```


## How to define examples
Each command shold contain a list of example, to understand how to use it.
So far, you have to complete the abstract method:

```c#
        public override List<CliCommandExample> Examples()
        {
            return new List<CliCommandExample>()
            {
                new CliCommandExample("Scan the target folder to search *.csproj Files, containing the text \"NugetPackages\" and save a CSV files with output",
                                        ScanCommand.AsExampleFor(@"C:\Temp\MyFolder", ".csproj", @"\NugetPackages\", "CSV"))
            };
        }
```

# Usage
the usage is very simple:

```c#
// Set up the factory of command
var factory = new CommandFactory().RegisterCommand<ScanCommand>("scan");

// parse the argument and run the callback if properly set
int exitResult = factory.InstanceFromFactory()
                                .UsingDefaultErrorManagement()
                                .ParseTheseArguments(args)
                                .CaseWhen<ScanCommand>(x => ExploreTheTree(x))
                                .Return();
                                
// return the proper exit value               
Environment.Exit(exitResult);
```
