using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs;
using System.Text;

namespace blobtest
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            //[Blob("firsterino-bloberino/testing-sine", FileAccess.Write)] Stream outputBlob,
            ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Http trigger function executed at: {DateTime.Now}");


            /*Get file from form
            try
            {
                var formdata = await req.ReadFormAsync();
                var file = req.Form.Files["file"];
                return new OkObjectResult("UploadBlobHttpTrigger function executed successfully!!");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
            */



            //Write in Blob file
            //outputBlob.Write(Encoding.Default.GetBytes("boyyyyyyy123"));



            string blob_ime = "testing-sine" + Guid.NewGuid().ToString() + ".txt";


            //Upload File
            
            BlobClient blobClientWrite = new BlobClient(
                connectionString:GetConnectionString(context),
                blobContainerName: "firsterino-bloberino",
                blobName:blob_ime
                );

            blobClientWrite.Upload(@"C:\Users\gjorge.gogov\Desktop\azuriraj.txt");



            //Read File
            BlobClient blobClientRead = new BlobClient(GetConnectionString(context),"firsterino-bloberino","azuriraj_blob.txt");

            if (await blobClientRead.ExistsAsync())
            {
                var response = await blobClientRead.DownloadAsync();
                using (var streamReader = new StreamReader(response.Value.Content))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = await streamReader.ReadLineAsync();
                        log.LogInformation(line);
                    }
                }
            }
            
            //Download File
            await blobClientRead.DownloadToAsync(@"C:\Users\gjorge.gogov\Downloads\" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".txt");

            return new OkObjectResult("File uploaded and read with an Azure Function");
        }

        private static string GetConnectionString(ExecutionContext executionContext)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(executionContext.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", true, true)
                            .AddEnvironmentVariables().Build();
            string connectionString = config["AzureWebJobsStorage"];
            return connectionString;
        }
    }
}