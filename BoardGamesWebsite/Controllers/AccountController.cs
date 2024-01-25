using BoardGamesWebsite.Models;
using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoardGamesWebsite.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IUserRepository userRepository;
    private readonly ISnackPreferenceRepository snackPreferenceRepository;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IUserRepository userRepository,
        ISnackPreferenceRepository snackPreferenceRepository)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.userRepository = userRepository;
        this.snackPreferenceRepository = snackPreferenceRepository;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var user = await userManager.FindByNameAsync(loginModel.Username ?? "");
        if (user == null)
        {
            ModelState.AddModelError("LoginError", "Ongeldige login gegevens");
            return View();
        }

        var result = await signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("LoginError", "Ongeldige login gegevens");
            return View();
        }

        return RedirectToAction("All", "GameEvent");
    }

    public async Task<IActionResult> Register()
    {
        ViewBag.SnackPreferences = new SelectList(
            await snackPreferenceRepository.All(),
            "Id",
            "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserModel userModel)
    {
        if (!ModelState.IsValid)
        {
            await loadSnackPreferences();
            return View();
        }

        var user = new IdentityUser { Email = userModel.Email, UserName = userModel.Username };
        var result = await userManager.CreateAsync(user, userModel.Password);
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("IdentityFramework", err.Description);
            }

            await loadSnackPreferences();
            return View();
        }

        var snackPreferences = await snackPreferenceRepository.ByIds(userModel.SnackPreferences);
        await userRepository.Create(new User
        {
            Id = user.Id,
            BirthDate = userModel.BirthDate.ToDateTime(TimeOnly.MinValue),
            Username = userModel.Username,
            Gender = userModel.Gender,
            SnackPreferences = snackPreferences,
            Address = new Address
            {
                City = userModel.City,
                Street = userModel.Street,
                HouseNumber = userModel.HouseNumber,
            },
        });

        await signInManager.SignInAsync(user, true);
        return RedirectToAction("All", "GameEvent");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("All", "GameEvent");
    }

    [Authorize]
    public async Task<IActionResult> Settings()
    {
        var userId = userManager.GetUserId(User)!;
        var user = await userRepository.ById(userId) ?? throw new ArgumentException("Logged in user not found");
        var model = new UserModel
        {
            Username = user.Username,
            BirthDate = DateOnly.FromDateTime(user.BirthDate),
            Gender = user.Gender,
            SnackPreferences = user.SnackPreferences.Select(s => s.Id).ToList(),
            City = user.Address.City,
            Street = user.Address.Street,
            HouseNumber = user.Address.HouseNumber,
            Email = "",
            Password = ""
        };

        await loadSnackPreferences();
        return View(model);
    }

    private async Task loadSnackPreferences()
    {
        ViewBag.SnackPreferences = new SelectList(
            await snackPreferenceRepository.All(),
            "Id",
            "Name");
    }
}