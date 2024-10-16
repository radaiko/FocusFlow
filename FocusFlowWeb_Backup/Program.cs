using FocusFlowWeb.Components;
using FocusFlowWeb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using FocusFlowWeb.Utils;
using Microsoft.AspNetCore.Authentication.Google;

Logger.AddConsole();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
  {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
  })
  .AddCookie()
  .AddGoogle(googleOptions =>
  {
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

  });


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GoogleAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
  provider.GetRequiredService<GoogleAuthStateProvider>());

// Add services to the container.
builder.Services.AddRazorComponents()
  .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  Logger.AddDebug();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
  .AddInteractiveServerRenderMode();

app.Run();