using AgileX.Api.Middleware;
using AgileX.Application;
using AgileX.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder
    .Services
    .AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>()
    .AddTransient<ErrorHandlingMiddleware>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
