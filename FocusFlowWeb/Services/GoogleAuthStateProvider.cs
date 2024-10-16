// ***********************************************************************
// File              : GoogleAuthStateProvider.cs
// Assembly          : FocusFlowWeb
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
using System.Security.Claims;
using FocusFlowWeb.Utils;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FocusFlowWeb.Services;

public class GoogleAuthStateProvider : AuthenticationStateProvider {
  private readonly IHttpContextAccessor _httpContextAccessor;

  public GoogleAuthStateProvider(IHttpContextAccessor httpContextAccessor) {
    _httpContextAccessor = httpContextAccessor;
  }

  public string GetClientId => _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Authentication:Google:ClientId"];

  public override Task<AuthenticationState> GetAuthenticationStateAsync() {
    var user = _httpContextAccessor.HttpContext.User;
    Logger.Debug($"User: {user.Identity?.Name} IsAuthenticated: {user.Identity?.IsAuthenticated}");
    return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
  }

  public void NotifyUserAuthentication(ClaimsPrincipal user) {
    Logger.Debug($"User: {user.Identity?.Name} logged in");
    var authState = Task.FromResult(new AuthenticationState(user));
    NotifyAuthenticationStateChanged(authState);
  }

  public void NotifyUserLogout() {
    Logger.Debug("User logged out");
    var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
    NotifyAuthenticationStateChanged(authState);
  }

  public async Task<bool> HandleGoogleLoginAsync(string code, HttpContext httpContext) {
    try {
      var clientSecrets = new ClientSecrets {
        ClientId = GetClientId, ClientSecret = httpContext.RequestServices.GetRequiredService<IConfiguration>()["Authentication:Google:ClientSecret"]
      };

      var tokenResponse = await new AuthorizationCodeFlow(new AuthorizationCodeFlow.Initializer(
        GoogleAuthConsts.AuthorizationUrl,
        GoogleAuthConsts.TokenUrl) {
        ClientSecrets = clientSecrets
      }).ExchangeCodeForTokenAsync("user", code, "postmessage", CancellationToken.None);

      if (tokenResponse == null) {
        Logger.Debug("Token response is null");
        return false;
      }

      var payload = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken);
      if (payload == null) {
        Logger.Debug("Payload is null");
        return false;
      }

      var claims = new List<Claim> {
        new(ClaimTypes.NameIdentifier, payload.Subject), new(ClaimTypes.Name, payload.Name), new(ClaimTypes.Email, payload.Email)
      };

      var identity = new ClaimsIdentity(claims, "Google");
      var user = new ClaimsPrincipal(identity);

      NotifyUserAuthentication(user);
      return true;
    }
    catch (Exception ex) {
      Logger.Error($"Error during Google login: {ex.Message}");
      return false;
    }
  }
}