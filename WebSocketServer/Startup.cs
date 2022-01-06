using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Threading;
using WebSocketServer.Middleware;
using WebSocketServer.Messengers;

namespace WebSocketServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketManager();
            services.AddScoped<ConnectionMessenger>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseWebSockets();

            app.UseWebSocketServer();

            app.Run(async context =>
            {
                Console.WriteLine("Http Request");
                await context.Response.WriteAsync("Hello");
            });
        }

       
    }
}
