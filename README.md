# CLIArgumentsParser
Library to easily manage and parse CLI arguments

Current Version: <b>BETA</b>

Main Features
----------------------------------
- define a model of usage for your CLI applications
- Automatic parse and validation of your arguments against the defined model
- Add examples of usage
- Print automatic helper on CLI usage

# How to define an Usage Model
To define you model, create a class inheriting from CliArguments one.

```
public class TestArguments : CLIArguments
{
  [LOVOptionDefinition("v", "verbosity", "Sets the verbosity of logging", true, new string[] { "DEBUG", "INFO", "WARN", "ERR"})]
  public string Verbosity { get; set; }

  [OptionDefinition("s", "singleOutput", "Writes the output in a single file or not")]
  public bool UseSingleFileAsOutput { get; set; }

  public CopyFilesWith1MandatoryOption CopyWithArguments { get; set; }
}

[VerbDefinition("copy", "copy files from SRC to OUTPUT")]
public class CopyFilesWith1MandatoryOption : CLIArguments
{
  [OptionDefinition("src", "source", "source folder to use to copy files", mandatory: true)]
  public string SrcFolder { get; set; }
}
```

there are 2 different entities in the model: Verbs and Options
- Verbs stand for action that CLI has to do (in the example, Copying files).
  They are marked by <c>VerbDefinition</c> Attribute.
- Options stand for additional settings you can apply. They are marked by <c>OptionDefinition</c> Attributes; the options can be defined on Verbs or on root class as well.
  
## Option Definition
You can define 2 kind of options, marked with 2 specific attributes.

<b>OptionDefinition</b>

It lets you to define an option that can be a boolean or a specific value; you can also set if it is mandatory or not.
There is no specific check upon the option value.

<b>LOVOptionDefinition</b>

It lets you to define an option that can be set inside an array of admitted values.

## Verb Definition
a Verb is defined as a CLiArguments as well, with its own options.


# How to define examples
Each CliArguments can define a list of example, to be use in Helper to understand how the argument can be specified.
To use this feature, you should overwite the proper property, like in example:

```
public override string ToCLIString()
{
  StringBuilder builder = new StringBuilder();

  builder.Append($" --v={Verbosity}");
  if (UseSingleFileAsOutput)
    builder.Append($" --s");

  return builder.ToString();
}

protected override IEnumerable<CLIUsageExample> BuildExamples()
{
  var list = new List<CLIUsageExample>();
  list.Add(new CLIUsageExample("Set custom verbsity", new OnlyOptionsArguments() { Verbosity = "WARN" }));
  list.Add(new CLIUsageExample("Save output in a single file", new OnlyOptionsArguments() { UseSingleFileAsOutput = true }));
  return list;
}
```
