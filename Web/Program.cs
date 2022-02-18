using AspNetCoreRateLimit;
using Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Services;
using Shared.Validators;
using Shared.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();

// rate limiting section
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddScoped<IQueryService, QueryService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IGeoIpService, GeoIpService>();
builder.Services.AddScoped<IRdapService, RdapService>();
builder.Services.AddScoped<IPingService, PingService>();
builder.Services.AddTransient<IValidator<Shared.ViewModels.ServiceInputModel>, ServiceModelValidator>();
builder.Services.AddScoped<AbstractValidator<ServiceInputModel>, ServiceModelValidator>();

builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
}).AddFluentValidation(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1",
        Title = "Services API",
        Description = "An ASP.NET Core Web API for Querying Ip or Domains",
    });
}).AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIpRateLimiting();

app.UseAuthorization();

app.MapControllers();

app.Run();
