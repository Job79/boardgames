using Core.Domain;
using Core.DomainServices.Exceptions;
using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamesWebsite.Controllers;

public class RegistrationController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly IRegistrationRepository registrationRepository;
    private readonly IGameEventRepository gameEventRepository;
    private readonly IUserRepository userRepository;

    public RegistrationController(
        UserManager<IdentityUser> userManager,
        IRegistrationRepository registrationRepository,
        IGameEventRepository gameEventRepository,
        IUserRepository userRepository)
    {
        this.userManager = userManager;
        this.registrationRepository = registrationRepository;
        this.gameEventRepository = gameEventRepository;
        this.userRepository = userRepository;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Register(int id)
    {
        var userId = userManager.GetUserId(User)!;
        try
        {
            await registrationRepository.Create(new Registration
            {
                User = await userRepository.ById(userId) ?? throw new ArgumentException("Logged in user not found"),
                GameEvent = await gameEventRepository.ById(id) ?? throw new OperationNotValidException("Game event not found"),
                Timestamp = DateTime.Now,
                DidAttend = null
            });
            
            return RedirectToAction("Participating", "GameEvent");
        }
        catch (BusinessLogicException err)
        {
            return View("Error", err.Message);
        }
    }
}