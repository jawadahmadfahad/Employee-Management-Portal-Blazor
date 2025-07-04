using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using EmployeeManagementSystem.DAL;
using EmployeeManagementSystem.BLL;

var builder = WebApplication.CreateBuilder(args);

// Google OAuth 2.0 Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    var googleAuth = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuth["ClientId"];
    options.ClientSecret = googleAuth["ClientSecret"];
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
});

// Authorization
builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// Razor & Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// App Services
builder.Services.AddScoped<EmployeeDAL>();
builder.Services.AddScoped<EmployeeBLL>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Blazor
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Google Login Endpoint
app.MapGet("/login", async context =>
{
    var props = new AuthenticationProperties { RedirectUri = "/" };
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, props);
});

// Google Logout Endpoint
app.MapGet("/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

app.Run();
