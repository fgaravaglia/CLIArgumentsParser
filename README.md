# CLIArgumentsParser
Library to easily manage and parse CLI arguments

| Branch | Status |
|-|-|
| master |[![Build Status](https://garaproject.visualstudio.com/CLIArgumentParser/_apis/build/status/CLIArgumentsParser-CI?branchName=master)](https://garaproject.visualstudio.com/CLIArgumentParser/_build/latest?definitionId=70&branchName=master) |

Current Version: <b>Stable - 1.1.1944</b>: branch MASTER

To install it, use proper command:
```
dotnet add package CliArgumentParser --version 1.1.1944
```

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
- define the method _SetDefaultValues_ to fill rpoper option dictionary
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


# How to define examples
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
