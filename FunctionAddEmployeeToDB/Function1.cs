using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAddEmployeeToDB
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", 
            Route = null)] HttpRequest req,
            ILogger log,
             [Sql("dbo.Employees", 
                ConnectionStringSetting = "SqlConnectionString")] 
                IAsyncCollector<Employee> empList)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Employee emp = JsonConvert.DeserializeObject<Employee>(requestBody);

            await empList.AddAsync(emp);
            await empList.FlushAsync();

            return new OkObjectResult(emp);
        }
    }
}
