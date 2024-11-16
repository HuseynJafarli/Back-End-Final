using Microsoft.AspNetCore.Mvc;
using RestSharp;
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
        var request = new RestRequest("api/Auth/Login", Method.Post);
        request.AddJsonBody(loginVM);

        var response = await _restClient.ExecuteAsync<TokenResponseVM>(request);

        if (!response.IsSuccessful || response.Data == null)
        {
            ModelState.AddModelError("", "Login failed. Please try again.");
            return View();
        }

        // Save JWT to session or cookie
        HttpContext.Session.SetString("AuthToken", response.Data.AccessToken);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    //[HttpPost]
    //public async Task<IActionResult> Register(UserRegisterVM registerDto)
    //{
    //    var request = new RestRequest("api/Auth/Register", Method.Post);
    //    request.AddJsonBody(registerDto);

    //    var response = await _restClient.ExecuteAsync(request);

    //    if (!response.IsSuccessful)
    //    {
    //        ModelState.AddModelError("", "Registration failed. Please try again.");
    //        return View();
    //    }

    //    return RedirectToAction("Login");
    //}

    //[HttpPost]
    //public IActionResult Logout()
    //{
    //    HttpContext.Session.Remove("AuthToken");
    //    return RedirectToAction("Login");
    //}
}
