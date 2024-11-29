using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using optique.Data;
using optique.Models;
using optique.Mappers;
using optique.IServices;
using optique.Services;
using MyAspNetApp.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection string is missing.")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT authentication
builder.Services.Configure<Parametres>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<Parametres>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Key ?? throw new InvalidOperationException("JWT Key is missing."));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Authentication failed: ", context.Exception);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated.");
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers["Authorization"].ToString();
            Console.WriteLine($"Token received: {token}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("user"));
});

// Add AutoMapper
builder.Services.AddAutoMapper(
    typeof(FournisseurProfile),
    typeof(ArticleProfile),
    typeof(SocieteProfile),
    typeof(RefMarqueProfile),
    typeof(RefTypeProfile),
    typeof(ClientProfile),
    typeof(RefTypeClientProfile),
    typeof(ArrivageProfile),
    typeof(ArrivageDetailsProfile),
    typeof(RetourFournisseurProfile),
    typeof(RefTypeRetourProfile),
    typeof(RefStatutDistributionProfile),
    typeof(DistributionProfile),
    typeof(DistributionDetailsProfile),
    typeof(DistributionSummaryProfile),
    typeof(VenteProfile),
    typeof(RefTypeDePaiementProfile),
    typeof(RefDeviseProfile));

// Add services
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ISocieteService, SocieteService>();
builder.Services.AddScoped<IFournisseurService, FournisseurService>();
builder.Services.AddScoped<IRefDeviseService, RefDeviseService>();
builder.Services.AddScoped<IRefMarqueService, RefMarqueService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ITypeClientService, RefTypeClientService>();
builder.Services.AddScoped<IArrivageService, ArrivageService>();
builder.Services.AddScoped<IArrivageDetailsService, ArrivageDetailsService>();
builder.Services.AddScoped<IDistributionService, DistributionService>();
builder.Services.AddScoped<IMouvementArticleService, MouvementArticleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRefTypeRetourService, RefTypeRetourService>();
builder.Services.AddScoped<IRefStatutDistributionService, RefStatutDistributionService>();
builder.Services.AddScoped<IRetourFournisseurService, RetourFournisseurService>();
builder.Services.AddScoped<IVenteService, VenteService>();
builder.Services.AddScoped<IRefTypeDePaiementService, RefTypeDePaiementService>();
builder.Services.AddScoped<IDistributionDetailsService, DistributionDetailsService>();
builder.Services.AddScoped<IRefTypeService, RefTypeService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("https://localhost:5003")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Your API", 
        Version = "v1",
        Description = "A simple example ASP.NET Core Web API",
    });
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120); // Durée de vie de la session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<JwtTokenMiddleware>();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseSession();
//app.UseMiddleware<JwtTokenMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("Page not found.");
    });
});

DataInitializer.Initialize(app.Services);

app.Run();
