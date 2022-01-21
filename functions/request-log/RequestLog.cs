using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace header_log
{
    public static class RequestLog
    {
        [FunctionName("HeaderLog")]
        public static IActionResult LogHeaders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            ILogger log)
        {
            foreach (var h in req.Headers)
            {
                log.LogInformation($"{h.Key}: {h.Value}");
            }

            return new OkResult();
        }
        
        [FunctionName("BodyLog")]
        public static IActionResult LogBody(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            ILogger log)
        {
            string body = await new StreamReader(req.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(body))
                log.LogInformation($"Body: {body}");

            return new OkResult();
        }
        
        [FunctionName("FullRequestLog")]
        public static async Task<IActionResult> LogBoth(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            ILogger log)
        {
            foreach (var h in req.Headers)
            {
                log.LogInformation($"{h.Key}: {h.Value}");
            }
            
            string body = await new StreamReader(req.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(body))
                log.LogInformation($"Body: {body}");

            return new OkResult();
        }
    }
}
