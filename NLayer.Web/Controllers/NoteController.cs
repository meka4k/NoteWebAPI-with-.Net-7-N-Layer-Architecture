using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace NLayer.Web.Controllers
{
	public class NoteController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

        public NoteController(IHttpClientFactory httpClientFactory)
        {
			_httpClientFactory = httpClientFactory;
			

        }

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();

			// TempData'dan tokeni al
			var token = TempData["AuthToken"]?.ToString();

			// Token yoksa, giriş yapma sayfasına yönlendir
			if (string.IsNullOrEmpty(token))
			{
				return RedirectToAction("Login", "Account");
			}

			// API'ye yetkilendirme yapmak için Authorization header'ı ekleyin
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// API'ye istek yap
			var responseMessage = await client.GetAsync("https://localhost:7185/api/note");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<NoteDto>>(jsonData);

				return View(values);
			}

			return View();
		}

		[HttpGet]
		public IActionResult AddNote()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddNote(CreateNoteDto createNoteDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createNoteDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var messageResponse = await client.PostAsync("https://localhost:7185/api/note", stringContent);
			if (messageResponse.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}


		public async Task<IActionResult> DeleteNote(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.DeleteAsync($"https://localhost:7185/api/note?id={id}");
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");

			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateNote(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7185/api/note/{id}");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<NoteDto>(jsonData);
				return View(values);
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateNote(NoteDto updateNoteDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateNoteDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7185/api/note", stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{

				return RedirectToAction("Index");
			}
			return View();
		}
	}
}
