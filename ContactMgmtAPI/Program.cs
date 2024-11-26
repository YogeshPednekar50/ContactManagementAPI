using ContactMgmtAPI.Middleware;
using ContactMgmtAPI.Repository;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.AddSerilog();

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            //.WithOrigins("https://*.theoriginalmaestro.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();

            //.AllowCredentials();
        });
});

//Using singleton instace to persist the model (JSON) changes
builder.Services.AddSingleton<IContactRepository, ContactRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(options =>
{
    options.Run(
        async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var ex = context.Features.Get<IExceptionHandlerFeature>();
            if (ex != null)
            {
                await context.Response.WriteAsync(ex.Error.Message);
            }
        }
    );

});

app.UseHttpsRedirection();
app.UseCors("AllowAll");


app.UseAuthorization();

app.MapControllers();

app.Run();
