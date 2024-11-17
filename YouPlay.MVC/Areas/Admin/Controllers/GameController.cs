using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using YouPlay.MVC.ApiResponseMessages;
using YouPlay.MVC.Services.Implementations;
using YouPlay.MVC.ViewModels;

namespace YouPlay.MVC.Areas.Admin.Controllers
{
    [Area("admin")]
    [ServiceFilter(typeof(TokenFilter))]
    public class GameController : Controller
    {
        private RestClient _restClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GameController(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
            _restClient = new RestClient("https://localhost:7283/api");
            var token = _httpContextAccessor.HttpContext.Request.Cookies["AuthToken"];

            if (token != null)
            {
                _restClient.AddDefaultHeader("Authorization", "Bearer " + token);
            }
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameCreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = new RestRequest("Games", Method.Post);

            request.AddParameter("Title", model.Title);
            request.AddParameter("Description", model.Description ?? "");
            request.AddParameter("Rating", model.Rating);
            request.AddParameter("CostPrice", model.CostPrice);
            request.AddParameter("Discount", model.Discount);
            request.AddParameter("Genre", model.Genre);
            request.AddParameter("Developer", model.Developer);
            request.AddParameter("ReleaseDate", model.ReleaseDate.ToString("o"));
            request.AddParameter("TrailerUrl", model.TrailerUrl ?? "");

            foreach (var file in model.GameImages)
            {
                using var stream = file.OpenReadStream();
                var fileBytes = new byte[file.Length];
                await stream.ReadAsync(fileBytes, 0, (int)file.Length);
                request.AddFile("GameImages", fileBytes, file.FileName, file.ContentType);
            }

            var response = await _restClient.ExecuteAsync<ApiResponseMessage<object>>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", response.ErrorMessage ?? "Failed to create game.");
                return View(model);
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            var request = new RestRequest($"Games/{id}", Method.Get);
            //var response = await _restClient.ExecuteAsync(request);
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<GameGetVM>> (request);

            if (!response.IsSuccessful)
            {
                ViewBag.Err = response.ErrorMessage;
                return View();
            }

            //List<GenreGetVM> vm = JsonSerializer.Deserialize<List<GenreGetVM>>(response.Content, new JsonSerializerOptions {PropertyNameCaseInsensitive = true });
            GameGetVM vm = response.Data.Data;
            GameUpdateVM updateVM = new()
            {
                Title = vm.Title,
                Description = vm.Description,
                Genre = vm.Genre,
                ReleaseDate = vm.ReleaseDate,
                CostPrice = vm.CostPrice,
                Developer = vm.Developer,
                Discount = vm.Discount,
                Rating = vm.Rating,
                TrailerUrl = vm.TrailerUrl,

            };
            return View(updateVM);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, GameUpdateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = new RestRequest($"Games{id}", Method.Put);

            request.AddParameter("Title", model.Title);
            request.AddParameter("Description", model.Description ?? "");
            request.AddParameter("Rating", model.Rating);
            request.AddParameter("CostPrice", model.CostPrice);
            request.AddParameter("Discount", model.Discount);
            request.AddParameter("Genre", model.Genre);
            request.AddParameter("Developer", model.Developer);
            request.AddParameter("ReleaseDate", model.ReleaseDate.ToString("o"));
            request.AddParameter("TrailerUrl", model.TrailerUrl ?? "");

            if (model.GameImages != null)
            {
                foreach (var file in model.GameImages)
                {
                    using var stream = file.OpenReadStream();
                    var fileBytes = new byte[file.Length];
                    await stream.ReadAsync(fileBytes, 0, (int)file.Length);
                    request.AddFile("GameImages", fileBytes, file.FileName, file.ContentType);
                }
            }
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<object>>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", response.ErrorMessage ?? "Failed to update game.");
                return View(model);
            }
        }
    }
}
