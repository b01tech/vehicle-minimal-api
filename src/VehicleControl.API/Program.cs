using VehicleControl.API.Exceptions;
using VehicleControl.API.Extensions;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddServices(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddSwaggerDocumentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandlerMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.ApplyMigrations();

app.Run();
