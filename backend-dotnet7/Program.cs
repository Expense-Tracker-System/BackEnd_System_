using backend_dotnet7.Core.BackgroundJobs;
using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Services;
//using backend_dotnet7.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using backend_dotnet7.Core.Dtos.Organization;



var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddControllers()
    // Enum Configuration 
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


//auto map add organizations

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.CreateMap<Organization, OrganizationDto>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

//services.AddAutoMapper(typeof(Startup));
//builder.Services.AddAutoMapper(typeof(AutoMapperProfile));



//DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("default");
    options.UseSqlServer(connectionString);
});

//AutoMapper DI
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



//Dependency Injection
// .AddSingleton    -> only one Instance for application...
// .AddScoped       -> Per request -> given new Instance            ->  shared within the same request context
// .AddTransient    -> when we inject, then newly create Instance   -> not shared across requests
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<IUserImageService, UserImageService>();
builder.Services.AddScoped<IUserEmailService, UserEmailService>();
builder.Services.AddScoped<IBExpenseService, BExpenseService>();
builder.Services.AddScoped<IUserPasswordConfirmService, UserPasswordConfirmService>();
builder.Services.AddScoped<ITransactionReposatory, TransactionSqlService>();
builder.Services.AddScoped<ICategoryReposatory, CategoryService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOutMessageService, OutMessageService>();
builder.Services.AddScoped<IUserPhoneNumberService, UserPhoneNumberService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAdminsettingService, AdminsettingService>();
builder.Services.AddScoped<ICreateOrganizationService, CreateOrganizationService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
//atto mapper configaration
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IFinancialService, FinancialService>();



// registers CORS services during service configuration
//  global CORS settings
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); ;
        });
});



//Add Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



//Config Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
});



//Add AuthenticationSchema and JwtBearer
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter your token with this format:'Bearer YOUR_TOKEN'",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "bearer",
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddQuartz(q =>
{
    // Use a Scoped container to create jobs.
    q.UseMicrosoftDependencyInjectionJobFactory();

    var dailyJobKey = new JobKey("DailyJob");

    // Register the daily job with the DI container
    q.AddJob<DailyJob>(opts => opts.WithIdentity(dailyJobKey));

    // Create a trigger for the daily job to run every day at 12:00 AM
    q.AddTrigger(opts => opts
        .ForJob(dailyJobKey) // Link to the DailyJob
        .WithIdentity("DailyJob-trigger") // Give the trigger a unique name
        .WithCronSchedule("0 50 16 * * ?"));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);



var app = builder.Build();

//cros origin foe reports


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(app =>
    {
        app.Run(async context =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("There was error in the srver pleace contact developer");
        });
    });

}





// Avoid the cores policy
// configures CORS middleware using in the request pipeline.
// per-endpoint customization
app.UseCors(options =>
{
    options
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin();
});





app.UseHttpsRedirection();

/*
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});
*/

// Everything
app.UseCors();

//************************
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
