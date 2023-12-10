
using System.Globalization;
using System.Text;
using Core.Identity;
using Core.Interfaces;
using Core.Servicses;
using Core.Settings;
using Infrastructure.Data.Contexts;
using Infrastructure.Data.SeedData;
using Infrastructure.Repositories;
using Infrastructure.Services;
using JsonBasedLocalization.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("No connection string was found");


// Add services to the container.


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();




//configure Auth to make token as default auth
builder.Services.AddAuthentication(option =>
{

    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(option =>
{
    option.SaveToken = true;

    //check the url came from https
    option.RequireHttpsMetadata = false;

    option.TokenValidationParameters = new TokenValidationParameters()
    {

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudiance"],
        IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))

    };

});





// Sending Email services

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));



// Inject services

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAccountServicse, AccountServicse>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ITokenServicse, TokenServicse>();
builder.Services.AddScoped<IImageServicse, ImageServicse>();
builder.Services.AddScoped<ICouponServicse, CouponServicse>();


// Add Email Services
builder.Services.AddTransient<IMailServicse, MailServicse>();


///localization
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddMvc()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(JsonStringLocalizerFactory));
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0]);
    options.SupportedCultures = supportedCultures;
});

/*
//External Register (Google)
// Google Auth

builder.Services.AddAuthentication()
    .AddGoogle(option =>
    {
        IConfigurationSection googleAuthSection = builder.Configuration.GetSection("Authentication");
        option.ClientId = googleAuthSection["ClientId"];
        option.ClientSecret = googleAuthSection["ClientSecret"];
    });

*/


// ADD Cors Policy

builder.Services.AddCors(corsOption =>
{

    corsOption.AddPolicy("Policy", CorsPolicyBuilder =>
    {
        CorsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


///Enable JWT in Swagger
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "VezeetaApi", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

//Localization
var supportedCultures = new[] { "en-US", "ar-EG" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);



app.UseStaticFiles();

app.UseCors("Policy");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();




using var scope = app.Services.CreateScope();

var Services = scope.ServiceProvider;

var _context = Services.GetRequiredService<ApplicationDbContext>();


var userManager = Services.GetRequiredService<UserManager<ApplicationUser>>();

var Logger = Services.GetRequiredService<ILogger<Program>>();

try
{
    await ApplicationSeed.seedSpecialization(_context);
    await ApplicationSeed.seedAdminData(userManager);

}
catch (Exception ex)
{
    Logger.LogError(ex, "Error occured while process");
}





app.Run();
