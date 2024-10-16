// using FocusFlowWeb;
// using FocusFlowWeb.Services;
// using FocusFlowWeb.Utils;
// using Google.Apis.Auth.AspNetCore3;
// using Google.Apis.Auth.OAuth2;
// using Google.Apis.Drive.v3;
// using Google.Apis.Services;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication.Google;
// using Microsoft.AspNetCore.Components.Authorization;
//
// var builder = WebApplication.CreateBuilder(args);
//
// builder.Services.AddAuthentication(options =>
//   {
//     options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//     options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//   })
//   .AddCookie()
//   .AddGoogleOpenIdConnect(options =>
//   {
//     options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//     options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//     options.Scope.Add(DriveService.Scope.Drive);
//   });
//
// builder.Services.AddRazorPages();
// builder.Services.AddServerSideBlazor();
// builder.Services.AddHttpContextAccessor();
// builder.Services.AddScoped<GoogleAuthStateProvider>();
// builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
//   provider.GetRequiredService<GoogleAuthStateProvider>());
//
// builder.Services.AddScoped<DriveService>(provider => {
//   var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
//     new ClientSecrets {
//       ClientId = builder.Configuration["Authentication:Google:ClientId"], ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
//     },
//     new[] { DriveService.Scope.Drive },
//     "user",
//     CancellationToken.None).Result;
//   return new DriveService(new BaseClientService.Initializer {
//     HttpClientInitializer = credential, ApplicationName = "FocusFlowWeb"
//   });
// });
//
// var app = builder.Build();
//
// if (!app.Environment.IsDevelopment())
// {
//   app.UseExceptionHandler("/Error", createScopeForErrors: true);
//   Logger.AddDebug();
// }
//
// app.UseAuthentication();
// app.UseAuthorization();
//
// app.UseAntiforgery();
//
// app.UseStaticFiles();
// app.MapRazorComponents<App>()
//   .AddInteractiveServerRenderMode();
//
// app.Run();


using FocusFlowWeb.Services;
using FocusFlowWeb.Utils;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

Logger.AddConsole();
AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

var builder = WebApplication.CreateBuilder(args);

// Authentication
builder.Services.AddAuthentication(options =>
  {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
  })
  .AddCookie()
  .AddGoogleOpenIdConnect(options =>
  {
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.Scope.Add(DriveService.Scope.Drive);
  });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<GoogleAuthStateProvider>();
builder.Services.AddScoped<DriveService>(provider => {
  var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
    new ClientSecrets {
      ClientId = builder.Configuration["Authentication:Google:ClientId"], ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
    },
    new[] { DriveService.Scope.Drive },
    "user",
    CancellationToken.None).Result;
  return new DriveService(new BaseClientService.Initializer {
    HttpClientInitializer = credential, ApplicationName = "FocusFlow"
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
  
}

if (app.Environment.IsDevelopment()) {
  Logger.AddDebug();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
  var exception = (Exception)e.ExceptionObject;
  Logger.Error("Unhandled exception", exception);
}