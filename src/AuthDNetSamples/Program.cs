using AuthDNetLib.Data;
using AuthDNetLib.Helper.Crypt;
using AuthDNetLib.Helper.Messages;
using AuthDNetLib.Helper.Session;
using AuthDNetLib.Helper.Validation;
using AuthDNetLib.Interfaces.Crypt;
using AuthDNetLib.Interfaces.Tokens;
using AuthDNetLib.Interfaces.Users;
using AuthDNetLib.Interfaces.Users.Session;
using AuthDNetLib.Interfaces.Validation;
using AuthDNetLib.Services.Tokens;
using AuthDNetLib.Services.Users;
using AuthDNetSamples.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

void ConfigureDbContext()
{
    string? connectionString = builder.Configuration.GetConnectionString("AuthenticatorConnection") ?? throw new InvalidOperationException("Connection string 'AuthenticatorConnection' not found.");

    string? envconnectionString = Environment.GetEnvironmentVariable(connectionString) ?? throw new InvalidOperationException(FeedbackMessages.IsNullConnectionString);

    connectionString = envconnectionString;

    builder.Services.AddDbContext<ApplicationContext>(options =>
    {
        options.UseSqlServer(connectionString, c => c.MigrationsAssembly("AuthDNetSamples"));
    });

    builder.Services.AddScoped<ApplicationDbContext>(provider =>
    {
        return provider.GetService<ApplicationContext>() ?? throw new InvalidOperationException(FeedbackMessages.ContextIsNotRegistered);
    });
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.Strict;
    });

    builder.Services.AddSession(options =>
    {
        IConfigurationSection sessionSettings = builder.Configuration.GetSection("Session");
        options.IdleTimeout = TimeSpan.FromMinutes(sessionSettings.GetValue<int>("IdleTimeout"));
        options.Cookie.Name = sessionSettings.GetValue<string>("CookieName");
        options.Cookie.HttpOnly = sessionSettings.GetValue<bool>("HttpOnly");
        options.Cookie.IsEssential = sessionSettings.GetValue<bool>("IsEssential");
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

    builder.Services.AddAntiforgery(options =>
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

    ConfigureDbContext();

    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    builder.Services.AddScoped(typeof(IUserService<>), typeof(UserService<>));
    builder.Services.AddScoped(typeof(IValidator<>), typeof(UserValidator<>));
    builder.Services.AddScoped(typeof(ISessionMenager<>), typeof(SessionMenager<>));
    builder.Services.AddScoped<ICryptography, Cryptography>();
    builder.Services.AddScoped<ITokenService, TokenService>();

    // Configure Cookie Authentication
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.SlidingExpiration = true;
            options.LoginPath = "/Auth/Login/SignIn";
        });

    // Configure JWT Authentication
    IConfigurationSection tokenSession = builder.Configuration.GetSection("JwtConfig:Secret");

    if(tokenSession.Value != null)
    {
        byte[] key = Encoding.ASCII.GetBytes(tokenSession.Value);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetSection("JwtConfig:Issuer").Value,
                ValidAudience = builder.Configuration.GetSection("JwtConfig:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }

    builder.Services.AddControllersWithViews();
}

void ConfigurePipeline(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseCookiePolicy();
        
    PhysicalFileProvider? rclProvider = new( Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "AuthDNetRCL", "assets")) ?? throw new DirectoryNotFoundException($"The directory '../AuthDNetRCL/assets/' does not exist."));

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new CompositeFileProvider(app.Environment.WebRootFileProvider, rclProvider)
    });

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSession();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    app.MapDefaultControllerRoute();
}

ConfigureServices(builder);

var app = builder.Build();

ConfigurePipeline(app);

app.Run();