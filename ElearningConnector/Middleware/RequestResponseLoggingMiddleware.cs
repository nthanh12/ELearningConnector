using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;

namespace ElearningConnector.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log request
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
            context.Request.Body.Position = 0;

            _logger.LogInformation(
                "\n--- REQUEST ---\nTime: {Time}\nMethod: {Method}\nPath: {Path}\nBody: {Body}\n",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                context.Request.Method,
                context.Request.Path,
                requestBody
            );

            // Log response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation(
                "\n--- RESPONSE ---\nTime: {Time}\nStatusCode: {StatusCode}\nBody: {Body}\n-----------------\n",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                context.Response.StatusCode,
                responseText
            );

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
} 