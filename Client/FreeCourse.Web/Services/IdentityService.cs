﻿using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace FreeCourse.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
                throw discovery.Exception;

            var refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address = discovery.TokenEndpoint,
                RefreshToken = refreshToken
            };

            var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
                return null;

            var authenticationTokens = new List<AuthenticationToken>
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            };

            var authenticationResult = await _contextAccessor.HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;

            properties.StoreTokens(authenticationTokens);

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return token;
        }

        public async Task RevokeRefreshToken()
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
                throw discovery.Exception;

            var refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            //Token silme işlemleri için dataları tutucak class IdentityModel'den geliyor.
            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address = discovery.RevocationEndpoint,
                Token = refreshToken,
                //hangi tokeni gönderdiğimizi bilgilendirmek için olan property
                TokenTypeHint = "refresh_token"
            };

            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignIn(SignInInput signInInput)
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signInInput.Email,
                Password = signInInput.Password,
                Address = discovery.TokenEndpoint
            };

            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Response<bool>.Fail(errorDto.Errors, 400);
            }

            #region
            //IdentityServer daki userInfo endpointden kullanıcı bilgileri alınır
            //User Info Endpoint Claimler
            //            {
            //                "sub": "248289761001",
            //    "name": "Bob Smith",
            //    "given_name": "Bob",
            //    "family_name": "Smith",
            //    "role": [
            //        "user",
            //        "admin"
            //    ]
            //}
            var userInfoReq = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = discovery.UserInfoEndpoint
            };

            var userInfo = await _httpClient.GetUserInfoAsync(userInfoReq);

            if (userInfo.IsError)
                throw userInfo.Exception;
            #endregion

            //cookie oluşturma kimliği
            ClaimsIdentity claimsIdentity = new(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            //Cookie oluştu
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });
            //beni hatırla seçeneği aktif ise kalıcı olur
            authenticationProperties.IsPersistent = signInInput.IsRemember;

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
            
            return Response<bool>.Success(200);
        }
    }
}
