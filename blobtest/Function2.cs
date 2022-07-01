using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace blobtest
{
    public class Function2
    {
        [FunctionName("Function2")]
        public void Run([BlobTrigger("firsterino-bloberino/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name,
            [Blob("firsterino-bloberino/testiranje4747", FileAccess.Write)] Stream outputBlob,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            using (StreamReader reader = new StreamReader(myBlob))
            {
                string s = "";
                while((s = reader.ReadLine()) != null)
                {
                    log.LogInformation(s);
                }
            }
        }
    }
}
