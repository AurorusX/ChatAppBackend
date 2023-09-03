using Api.Hubs;
using Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//builder.Services.AddSingleton<ChatService>();
builder.Services.AddScoped<ChatService>();

//builder.Services.AddSignalR();
builder.Services.AddSignalR(options =>
{
    // Enable detailed error messages for debugging (for development only).
    options.EnableDetailedErrors = true;

    // Set maximum message size (in bytes).
    options.MaximumReceiveMessageSize = 65536; // 64 KB (lower for improved speed).

    // Keep-alive interval for pinging clients.
    options.KeepAliveInterval = TimeSpan.FromSeconds(2); // 15 seconds (lower for quicker detection).

    // Client timeout interval for detecting idle clients.
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(2); // 2 minutes (lower for quicker disconnect).

    // Stream buffer capacity for high-volume streaming.
    options.StreamBufferCapacity = 819200; // 8 KB (lower for faster streaming).

   
}).AddMessagePackProtocol();



builder.Services.Configure<HubOptions<ChatHub>>(options =>
{
    // Maximum message size per hub.
    options.MaximumReceiveMessageSize = 65536; // 64 KB (lower for improved speed).

    // Maximum parallel invocations per client for this hub.
    options.MaximumParallelInvocationsPerClient = 10; // Lower based on concurrency needs.
});


builder.Services.AddWebSockets(options => { /* Empty configuration action */ });

builder.Services.AddCors();
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));



//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//	app.UseSwagger();
//	app.UseSwaggerUI();
//}

//Middleware
//app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));
//app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200", "http://localhost:45429", "http://localhost:44366"));
app.UseCors(x => x
   .AllowAnyHeader()
   .AllowAnyMethod()
    .AllowAnyOrigin());// Add your allowed origins here

app.UseWebSockets();
app.UseAuthorization(); 
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");
app.MapFallbackToController("Index", "Fallback");

app.Run();
