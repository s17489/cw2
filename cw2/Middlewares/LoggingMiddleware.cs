using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cw2.Services;


namespace cw2.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var filePath = @"Middlewares/requestsLog.txt";
            
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string path = httpContext.Request.Path;
                string method = httpContext.Request.Method.ToString();
                string queryString = httpContext.Request.QueryString.ToString();
                string bodyStr = "";

                using (StreamReader reader
                 = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    reader.Close();
                    httpContext.Request.Body.Position = 0;
                }

                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine("METHOD: " + method);
                    writer.WriteLine("PATH: " + path);
                    writer.WriteLine("BODY: " + bodyStr);
                    writer.WriteLine("QUERY: " + queryString);
                    writer.WriteLine();
                }
            }
            if (_next != null)
                await _next(httpContext);
        }


    }
}
