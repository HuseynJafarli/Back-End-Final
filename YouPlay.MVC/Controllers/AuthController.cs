using Microsoft.AspNetCore.Mvc;
using RestSharp;
using YouPlay.MVC.ApiResponseMessages;
using YouPlay.MVC.ViewModels;
public class AuthController : Controller
{
    private readonly RestClient _restClient;

    public AuthController()
    {
        _restClient = new RestClient("https://localhost:7283/api");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginVM loginVM)
    {
        var request = new RestRequest("Auth/Login", Method.Post);
        request.AddJsonBody(loginVM);

        var response = await _restClient.ExecuteAsync<ApiResponseMessage<TokenResponseVM>>(request);

        if (!response.IsSuccessful || response.Data == null)
        {
            ModelState.AddModelError("", "Login failed. Please try again.");
            return View();
        }

        var tokenExpirationDate = DateTime.UtcNow.AddDays(10);
        HttpContext.Response.Cookies.Append("AuthToken", response.Data.Data.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = tokenExpirationDate
        });

        return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var request = new RestRequest("Auth/Register", Method.Post);

        request.AddParameter("Fullname", model.Fullname);
        request.AddParameter("Username", model.Username);
        request.AddParameter("Email", model.Email);
        request.AddParameter("Password", model.Password);
        request.AddParameter("ConfirmPassword", model.ConfirmPassword);

        if (model.ProfileImage != null)
        {
            using var stream = model.ProfileImage.OpenReadStream();
            var fileBytes = new byte[model.ProfileImage.Length];
            await stream.ReadAsync(fileBytes, 0, (int)model.ProfileImage.Length);
            request.AddFile("ProfileImage", fileBytes, model.ProfileImage.FileName, model.ProfileImage.ContentType);
        }

        var response = await _restClient.ExecuteAsync<ApiResponseMessage<object>>(request);

        if (response.IsSuccessful && response.Data != null)
        {
            return RedirectToAction("Login");
        }
        else
        {
            ModelState.AddModelError("", response.ErrorMessage ?? "Registration failed. Please try again.");
            return View(model);
        }
    }


    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("AuthToken"); 
        return RedirectToAction("Login");
    }
}
