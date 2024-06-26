﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using WebApp_OpenIDConnect_DotNet.Models;

namespace WebApp_OpenIDConnect_DotNet.Controllers
{
    public class HomeController : Controller
    {
        AzureAdB2COptions AzureAdB2COptions;
        public HomeController(IOptions<AzureAdB2COptions> azureAdB2COptions)
        {
            AzureAdB2COptions = azureAdB2COptions.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = string.Format("Claims available for the user {0}",
                (User.FindFirst("name")?.Value));
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Api()
        {
            string responseString = "";
            try
            {
                // Retrieve the token with the specified scopes
                var scope = AzureAdB2COptions.ApiScopes.Split(' ');
                string signedInUserID = HttpContext.User.FindFirst(
                    ClaimTypes.NameIdentifier).Value;

                IConfidentialClientApplication cca =
                ConfidentialClientApplicationBuilder.Create(AzureAdB2COptions.ClientId)
                    .WithRedirectUri(AzureAdB2COptions.RedirectUri)
                    .WithClientSecret(AzureAdB2COptions.ClientSecret)
                    .WithB2CAuthority(AzureAdB2COptions.Authority)
                    .Build();
                new MSALStaticCache(signedInUserID, this.HttpContext).EnablePersistence(
                    cca.UserTokenCache);

                var accounts = await cca.GetAccountsAsync();
                AuthenticationResult result = await cca.AcquireTokenSilent(
                    scope, accounts.FirstOrDefault())
                        .ExecuteAsync();

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(
                    HttpMethod.Get, AzureAdB2COptions.ApiUrl);

                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer", result.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        responseString = await response.Content.ReadAsStringAsync();
                        break;
                    case HttpStatusCode.Unauthorized:
                        responseString = $"Please sign in again. {response.ReasonPhrase}";
                        break;
                    default:
                        responseString = $"Error calling API. StatusCode=${response.StatusCode}";
                        break;
                }
            }
            catch (MsalUiRequiredException ex)
            {
                responseString = $"Session has expired. Please sign in again. {ex.Message}";
            }
            catch (Exception ex)
            {
                responseString = $"Error calling API: {ex.Message}";
            }

            ViewData["Payload"] = $"{responseString}";
            return View();
        }

        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}
