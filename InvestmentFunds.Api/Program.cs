using InvestmentFunds.Api.Middleware;
using Microsoft.IdentityModel.Tokens;
using InvestmentFunds.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

var secretManager = app.Services.GetRequiredService<AwsSecretManagerService>();
var cognitoAuthority = await secretManager.GetSecretAsync(app.Configuration["AWS:CognitoAuthoritySecretName"]!);

builder.Services.AddAuthentication("Bearer")
.AddJwtBearer(options =>
{
    options.Authority = cognitoAuthority ?? $"https://cognito-idp.{app.Configuration["AWS:Region"]}.amazonaws.com/{app.Configuration["AWS:CognitoUserPoolId"]}";
    options.TokenValidationParameters = new TokenValidationParameters { ValidateAudience = false };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Investor", p => p.RequireClaim("cognito:groups", "Investor"));
    options.AddPolicy("Admin", p => p.RequireClaim("cognito:groups", "Admin"));
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.Run();