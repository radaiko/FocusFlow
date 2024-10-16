using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FocusFlowWasm;
using FocusFlowWasm.Services;
using FocusFlowWasm.Utils;
using Microsoft.AspNetCore.Components.Authorization;

RLog.AddConsole();

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options => {
  options.ProviderOptions.Authority = "https://accounts.google.com/";
  options.ProviderOptions.ClientId = "971981360596-cbr2qrvoi1r3min298p3jaf8k97li2cf.apps.googleusercontent.com";
  options.ProviderOptions.ResponseType = "id_token token";
  options.ProviderOptions.DefaultScopes.Add("openid");
  options.ProviderOptions.DefaultScopes.Add("profile");
  options.ProviderOptions.DefaultScopes.Add("email");
  options.ProviderOptions.DefaultScopes.Add("https://www.googleapis.com/auth/drive.appdata");
});


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddApiAuthorization();
builder.Services.AddScoped<GoogleDriveService>();


if (builder.HostEnvironment.IsDevelopment()) RLog.AddDebug();

await builder.Build().RunAsync();