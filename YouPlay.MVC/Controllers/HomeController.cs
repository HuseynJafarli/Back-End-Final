using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using YouPlay.MVC.ApiResponseMessages;
using YouPlay.MVC.ViewModels;

namespace YouPlay.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly RestClient _restClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _restClient = new RestClient("https://localhost:7283/api");
            _httpContextAccessor = httpContextAccessor;
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

        public IActionResult PageNotFound()
        {
            return View();
        }


        public IActionResult Profile()
        {
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var fullName = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
            var profileImage = jwtToken.Claims.FirstOrDefault(c => c.Type == "ProfileImageUrl")?.Value ?? "~/dark/assets/images/user-avatar.jpg";

            var profileVm = new ProfileVM
            {
                FullName = fullName,
                Email = email,
                Username = username,
                ProfileImageUrl = profileImage
            };

            return View(profileVm);
        }



    }
}
