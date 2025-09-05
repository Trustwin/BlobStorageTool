using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BlobStorageTool;

public class BlobStorageProcess(Options options)
{
    private Options Options { get; set; } = options;

    public async Task<int> Run()
    {
        if (Options.IsVerbose) Console.WriteLine("BlobStorageTool");
        
        // Show Help
        if (Options.ShouldShowHelp)
        {
            Options.ShowHelp();
            return 0;
        }
        
        // Validate the options
        if (string.IsNullOrEmpty(options.Delete) && !Options.Upload)
        {
            Console.WriteLine("No action specified");
            return 1;
        }

        if (string.IsNullOrEmpty(options.Account))
        {
            Console.WriteLine("No account specified");
            return 1;
        }
        if (string.IsNullOrEmpty(options.Key))
        {
            Console.WriteLine("No key specified");
            return 1;
        }
        if (string.IsNullOrEmpty(options.Container))
        {
            Console.WriteLine("No container specified");
            return 1;
        }
        
        switch (Options.Upload)
        {
            case true when string.IsNullOrEmpty(options.From):
                Console.WriteLine("No source specified");
                return 1;
            case true when string.IsNullOrEmpty(options.To):
                Console.WriteLine("No destination specified");
                return 1;
        }

        // connect to the container        
        var connectionString = $"DefaultEndpointsProtocol=https;AccountName={options.Account};AccountKey={options.Key};EndpointSuffix=core.windows.net";
        var blobServiceClient = new BlobServiceClient(connectionString);
        var blobContainerClient =
            blobServiceClient.GetBlobContainerClient(options.Container);
        
        if (!string.IsNullOrEmpty(options.Delete))
        {
            await blobContainerClient.GetBlobClient(options.Delete).DeleteIfExistsAsync();
            return 0;
        }
        
        // do the process
        if (!Options.Upload) return 1;
        
        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.None);
        var blobName = options.From;
        var blobClient = blobContainerClient.GetBlobClient(blobName);

        await using var uploadFileStream = File.OpenRead(options.To!);
        await blobClient.UploadAsync(uploadFileStream, true);
        uploadFileStream.Close();
        return 0;
    }
}