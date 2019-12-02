using IronPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Web.Http;

namespace IronPdfFunction
{
    public static class HtmlToIronPdfFunction
    {
        [FunctionName("HtmlToIronPdf")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            //IronPdf.Installation.TempFolderPath = context.FunctionDirectory;


            var html = "<html><head></head><body><h2>Test</h2><p>This is a test. Can you see me.</p></body></html>";

            var renderer = new IronPdf.HtmlToPdf();
            renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            {
                Height = 15,
                HtmlFragment = "<center>Page: {page}/{total-pages}</center>",
            };

            var headerHtml = "<h1>Iron PDF Function</h1>";
            renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {
                Height = 20,
                HtmlFragment = headerHtml,
            };

            try
            {
                // Errors on this line (but exception is not caught)
                var pdf = renderer.RenderHtmlAsPdf(html);
                
                return new FileContentResult(pdf.BinaryData, "application/pdf");
            }
            catch (Exception e)
            {
                return new ExceptionResult(e, true);
            }
        }
    }
}
