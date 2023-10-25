using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoWebService;
using TodoWebService.BackgoundServices;
using TodoWebService.Data;
using TodoWebService.Models.DTOs.Validations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TodoDbConnectionString"));
});

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddSwagger();


builder.Services.AddDomainServices();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();


builder.Services.AddHostedService<EmailBackgroundService>(s=> new EmailBackgroundService(s));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseOutputCache();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
