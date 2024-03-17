using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SmartRefund.WebAPI.Middlewares
{
    [ExcludeFromCodeCoverage]
    public class AntiXSSMiddleware
    {
        private readonly RequestDelegate _next;

        public AntiXSSMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "PATCH")
            {
                var requestBody = await ReadRequestBody(context);
                if (ContainsDangerousCharacters(requestBody))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json"; 
                    var responseObject = new
                    {
                        error = new
                        {
                            message = "Detected potential XSS attack!",
                            statusCode = 400
                        }
                    };
                    var responseJson = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                    });
                    await context.Response.WriteAsync(responseJson);
                    return;
                }
            }

            await _next(context);
        }


        private static async Task<string> ReadRequestBody(HttpContext context)
        {
            var buffer = new MemoryStream();
            await context.Request.Body.CopyToAsync(buffer);
            context.Request.Body = buffer;
            buffer.Position = 0;

            var encoding = Encoding.UTF8;

            var requestContent = await new StreamReader(buffer, encoding).ReadToEndAsync();
            context.Request.Body.Position = 0;

            return requestContent;
        }

        private bool ContainsDangerousCharacters(string input)
        {

            if (Regex.IsMatch(input, @"<\s*script\s*\/?>", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, @"<\s*iframe\s*\/?>", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, @"<\s*object\s*\/?>", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, @"<\s*embed\s*\/?>", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, @"<\s*frame\s*\/?>", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, @"on\w+\s*=", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, @"javascript\s*:", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, @"expression\s*\(", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, @"<%\s*", RegexOptions.IgnoreCase) ||
                input.Contains("&") || input.Contains("$"))
            {
                return true;
            }

            return false;
        }
    }

}