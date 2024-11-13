using Microsoft.AspNetCore.Mvc;
using RestSharp;
using YouPlay.MVC.ApiResponseMessages;
using YouPlay.MVC.ViewModels;

namespace YouPlay.MVC.Controllers
{
    public class StoreController : Controller
    {
        private readonly RestClient _restClient;
        public StoreController()
        {
            _restClient = new RestClient("https://localhost:7283/api");
        }

        public async Task<IActionResult> Index()
        {
            var request = new RestRequest("Games", Method.Get);
            //var response = await _restClient.ExecuteAsync(request);
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<List<GameGetVM>>>(request);

            if (!response.IsSuccessful)
            {
                ViewBag.Err = response.ErrorMessage;
                return View();
            }

            //List<GenreGetVM> vm = JsonSerializer.Deserialize<List<GenreGetVM>>(response.Content, new JsonSerializerOptions {PropertyNameCaseInsensitive = true });
            List<GameGetVM> vm = response.Data.Data;

            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var request = new RestRequest($"Games/{id}", Method.Get);
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<GameGetVM>>(request);

            if (!response.IsSuccessful)
            {
                ViewBag.Err = response.ErrorMessage;
                return View("Error");
            }

            GameGetVM game = response.Data.Data;
            return View(game);
        }

    }
}
