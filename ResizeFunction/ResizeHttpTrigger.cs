using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Company.Function
{
    public static class ResizeHttpTrigger
    {
        [FunctionName("ResizeHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                int w = 0;
                int h = 0;

                if (!Int32.TryParse(req.Query["w"], out w) || !Int32.TryParse(req.Query["h"], out h))
                {

                    return new BadRequestObjectResult("Invalid or missing width (w) and height (h) parameters.");
                }

                if (w <= 0 || h <= 0)
                {
                    return new BadRequestObjectResult("Width (w) and height (h) must be greater than zero.");
                }

                if (req.Body == null || req.ContentLength == 0)
                {
                    return new BadRequestObjectResult("Request body is empty. Please upload an image.");
                }

                byte[] targetImageBytes;
                using (var msInput = new MemoryStream())
                {
                    // Récupère le corps du message en mémoire
                    await req.Body.CopyToAsync(msInput);
                    msInput.Position = 0;

                    // Charge l'image       
                    using (var image = Image.Load(msInput))
                    {
                        // Effectue la transformation
                        image.Mutate(x => x.Resize(w, h));

                        // Sauvegarde en mémoire               
                        using (var msOutput = new MemoryStream())
                        {
                            image.SaveAsJpeg(msOutput);
                            targetImageBytes = msOutput.ToArray();
                        }
                    }
                }

                return new FileContentResult(targetImageBytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                log.LogError($"Unexpected error: {ex.Message}");
                return new BadRequestObjectResult($"Unexpected error: {ex.Message}");
            }
        }
    }
}


