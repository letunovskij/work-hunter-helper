using Common.Exceptions;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
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
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Entities.WorkHunters;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthorization();
builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration)
    => loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration));

builder.Services.AddHttpLogging(logging => { logging.LoggingFields = HttpLoggingFields.All; logging.CombineLogs = true; });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.AddSecurity("Bearer", [],
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

var MyAllowSpecificOrigins = "_MyAllowSubdomainPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            var origins = builder.Configuration.GetValue<string>("Cors")?.Split(',');
            if (origins != null && origins.Length > 0)
            {
                policy.WithOrigins(origins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin();
            }
        });
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<WorkHunterDbContext>()
    .AddDefaultTokenProviders();

var dbConnection = Environment.GetEnvironmentVariable("ConnectionStringWHunter");

builder.Services.AddDbContext<IWorkHunterDbContext, WorkHunterDbContext>(config =>
{
    //config.UseSqlite("Filename=WorkHunterDb.sqlite");``
    if (!string.IsNullOrEmpty(dbConnection))
        config.UseNpgsql(builder.Configuration.GetConnectionString(dbConnection));
    else
        config.UseNpgsql(builder.Configuration.GetConnectionString("WorkHunter"));
    //config.EnableSensitiveDataLogging();
});

var authOptions = builder.Configuration.GetSection("AuthOptions").Get<AuthOptions>() 
    ?? throw new BusinessErrorException("Auth Options is not configured!");

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

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AppPolicies.Admin, policy => policy.RequireRole(AppRoles.Admin))
    .AddPolicy(AppPolicies.All, policy => policy.RequireRole(AppRoles.Admin, AppRoles.User));

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
builder.Services.RegisterApplicationServices(builder.Configuration);

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntityType<WResponse>();
modelBuilder.EntitySet<User>("OUsers");

builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata",
        modelBuilder.GetEdmModel()));

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Client")),
    RequestPath = "/Client"
});

app.UseSimpleHttpLogging();
app.UseHttpLogging();

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapUsersEndpoints();
app.MapWResponsesEndpoints();
app.MapImportEndpoints();
app.MapVideoInterviewEndpoints();
app.MapTasksEndpoints();
app.MapEnumEndpoints();
app.MapAdminsEndpoints();

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