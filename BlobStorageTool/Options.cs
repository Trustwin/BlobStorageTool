using Druware.CommandLineOptions;

namespace BlobStorageTool;

public class Options : CommandLineOptions
{
    /// <summary>
    /// Base Constructor used by the generic foundation to create an empty
    /// object ready to parse.
    /// </summary>
    public Options()
    {
    }

    /// <summary>
    /// Constructor that accepts the argument list and automatically parses
    /// the options from them 
    /// </summary>
    /// <param name="args"></param>
    public Options(string[] args) => base.Parse(args);

    public bool Upload => GetOption("Upload")?.Selected ?? false;
    public string? Delete => GetOption("Delete")?.Value ?? "";
    public string? Account => GetOption("Account")?.Value ?? "";
    public string? Key => GetOption("Key")?.Value ?? "";
    public string? To => GetOption("To")?.Value ?? "";
    public string? From => GetOption("From")?.Value ?? "";
    public string? Container => GetOption("Container")?.Value ?? "";
}