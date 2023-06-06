using MarmaraMovieWorld.Model;
using MarmaraMovieWorld.Pages;
using MarmaraMovieWorld.Services;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MarmaraMovieWorld;

var builder = WebApplication.CreateBuilder(args);
// Configuration
var configuration = builder.Configuration;

//var connectionString = builder.Configuration.GetConnectionString("MarmaraMovieWorldContextConnection") ?? throw new InvalidOperationException("Connection string 'MarmaraMovieWorldContextConnection' not found.");

//builder.Services.AddDbContext<MarmaraMovieWorldContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<MarmaraMovieWorldContext>();

// Add services to the container
builder.Services.AddRazorPages(); builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Account/Logout");
    options.Conventions.AuthorizePage("/Account/Profile");
}); 
builder.Services.AddScoped<TMDbService>();
builder.Services.AddHttpClient();
builder.Services.Configure<ApiKeysOptions>(builder.Configuration.GetSection("ApiKeys")); // Yap�land�rmay� ekleyin
builder.Services.AddScoped<MovieDetailModel>();
builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
        options.Scope = "openid profile email";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();


app.Run();
