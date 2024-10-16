// ***********************************************************************
// File              : GoogleAuthStateProvider.cs
// Assembly          : FocusFlowWeb_Backup
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace FocusFlowWeb.Services;

public class GoogleAuthStateProvider : AuthenticationStateProvider
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public GoogleAuthStateProvider(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public override Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    var user = _httpContextAccessor.HttpContext.User;
    return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
  }

  public void NotifyUserAuthentication(ClaimsPrincipal user)
  {
    var authState = Task.FromResult(new AuthenticationState(user));
    NotifyAuthenticationStateChanged(authState);
  }

  public void NotifyUserLogout()
  {
    var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
    NotifyAuthenticationStateChanged(authState);
  }
}

