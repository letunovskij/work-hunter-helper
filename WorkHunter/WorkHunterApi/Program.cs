using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using System.Reflection;
using System.Text;
using WorkHunter.Api;
using WorkHunter.Api.Endpoints;
using WorkHunter.Api.Middleware;
using WorkHunter.Data;
using WorkHunter.Models.Config;
using WorkHunter.Models.Constants;
using WorkHunter.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration)
    => loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.AddSecurity("Bearer", Enumerable.Empty<string>(),
        new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            Description = "Type into the textbox: {your JWT token}."
        });
    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));

    config.SchemaSettings.GenerateEnumMappingDescription = true;
    config.DocumentName = "v1";
    config.Title = "Work Hunter";
    config.Version = "v1";
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .WithOrigins(builder.Configuration.GetValue<string>("Cors").Split(','))
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition");
    });
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<WorkHunterDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<IWorkHunterDbContext, WorkHunterDbContext>(config =>
{
    //config.UseSqlite("Filename=WorkHunterDb.sqlite");
    config.UseNpgsql(builder.Configuration.GetConnectionString("WorkHunter"));
    //config.EnableSensitiveDataLogging();
});

var authOptions = builder.Configuration.GetSection("AuthOptions").Get<AuthOptions>();
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = authOptions.Audience,
        ValidateIssuer = true,
        ValidIssuer = authOptions.Issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Key)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        AppPolicies.Admin,
        policy => policy.RequireRole(AppRoles.Admin));

    options.AddPolicy(
        AppPolicies.All,
        policy => policy.RequireRole(AppRoles.Admin, AppRoles.User));
});

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
builder.Services.RegisterApplicationServices(builder.Configuration);

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();

if (builder.Configuration.GetValue<bool>("Settings:EnableDataSeeding"))
{
    var scope = app.Services.CreateScope();
    await Initialize.SeedData(
        scope.ServiceProvider.GetRequiredService<UserManager<User>>(),
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(),
        scope.ServiceProvider.GetRequiredService<IWorkHunterDbContext>());
    scope.Dispose();
}

if (builder.Configuration.GetValue<bool>("Settings:EnableSwagger"))
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}
app.UseExceptionHandler();
app.Run();