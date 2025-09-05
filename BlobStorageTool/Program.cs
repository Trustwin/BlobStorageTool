using System.Reflection;
using System.Text;
using Druware.CommandLineOptions;

namespace BlobStorageTool;

class Program
{
    static async Task<int> Main(string[] args)
    {
        // Parse the options
        var assembly = Assembly.GetExecutingAssembly(); 
        var resources = assembly.GetManifestResourceNames();
        if (resources.Length == 0) return 1;
        var resourceName = "";
        foreach (var resource in resources)
        {
            if (!resource.Contains("CommandLineOptions.json")) continue;
            resourceName = resource;
            break;
        }
        
        var options = CommandLineOptions.GetInstance<Options>(resourceName, assembly);
        options.Parse(args);
        
        // Create the Authenticator Class and run the process from there.
        var process = new BlobStorageProcess(options);
        return await process.Run();    
    }
}