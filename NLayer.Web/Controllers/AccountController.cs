using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLayer.Core.Models;
using NLayer.Service.Services;
using System.Text;

namespace NLayer.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public AccountController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginUser model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var token = await GetTokenAsync(model.UserName, model.Password);

			if (token != null)
			{
				TempData["AuthToken"] = token;

				return RedirectToAction("Index", "Note");
			}

			ModelState.AddModelError(string.Empty, "Invalid login attempt");
			return View(model);
		}

		private async Task<string> GetTokenAsync(string email, string password)
		{
			using (var client = _httpClientFactory.CreateClient())
			{
				var apiUrl = "https://localhost:7185/api/auth/login";

				var loginRequest = new
				{
					UserName = email,
					Password = password
				};

				var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

				var response = await client.PostAsync(apiUrl, content);

				if (response.IsSuccessStatusCode)
				{
					var tokenString = await response.Content.ReadAsStringAsync();
					return tokenString;
				}

				return null;
			}
		}
	}
}
