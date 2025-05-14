using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AgriEnergyConnectMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace AgriEnergyConnectMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var client = _clientFactory.CreateClient("UserService");
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid login");
                return View(model);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenObj = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenObj.Token);

            // Get claims from token and add access_token manually
            var claims = jwt.Claims.ToList();
            claims.Add(new Claim("access_token", tokenObj.Token));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public IActionResult RegisterFarmer()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> RegisterFarmer(RegisterFarmerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _clientFactory.CreateClient("UserService");
            var farmerDto = new RegisterFarmerDto
            {
                Email = model.Email,
                Password = model.Password
            };

            var json = JsonConvert.SerializeObject(farmerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Attach JWT for authorization
            var token = await HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.PostAsync("/api/auth/register/farmer", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("FarmerList");
            }
            else
            {
                ModelState.AddModelError("", "Error registering farmer. Please try again.");
                return View(model);
            }
        }

        public async Task<IActionResult> FarmerList()
        {
            var client = _clientFactory.CreateClient("UserService");
            

            var response = await client.GetAsync("api/auth/farmers");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var farmers = JsonConvert.DeserializeObject<List<FarmerViewModel>>(json);
                return View(farmers);
            }
            else
            {
                ModelState.AddModelError("", "Failed to fetch the list of farmers.");
                return View(new List<FarmerViewModel>()); // Pass an empty list to avoid null reference
            }
        }

        private class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
