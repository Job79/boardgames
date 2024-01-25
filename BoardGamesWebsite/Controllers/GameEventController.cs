using BoardGamesWebsite.Models;
using Core.Domain;
using Core.DomainServices.Exceptions;
using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoardGamesWebsite.Controllers;

public class GameEventController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly IGameEventRepository gameEventRepository;
    private readonly IGameRepository gameRepository;
    private readonly IUserRepository userRepository;
    private readonly ISnackPreferenceRepository snackPreferenceRepository;
    private readonly IRegistrationRepository registrationRepository;

    public GameEventController(
        UserManager<IdentityUser> userManager,
        IGameEventRepository gameEventRepository,
        IUserRepository userRepository,
        IGameRepository gameRepository,
        ISnackPreferenceRepository snackPreferenceRepository,
        IRegistrationRepository registrationRepository)
    {
        this.userManager = userManager;
        this.gameEventRepository = gameEventRepository;
        this.userRepository = userRepository;
        this.gameRepository = gameRepository;
        this.snackPreferenceRepository = snackPreferenceRepository;
        this.registrationRepository = registrationRepository;
    }

    public async Task<IActionResult> All()
    {
        return View(await gameEventRepository.All());
    }

    public async Task<IActionResult> Details(int id)
    {
        var gameEvent = await gameEventRepository.ById(id);
        if (gameEvent == null)
        {
            return NotFound();
        }

        var userId = userManager.GetUserId(User);
        if (userId != null)
        {
            var user = await userRepository.ById(userId);
            ViewBag.UserSnackPreferences = user?.SnackPreferences ?? new List<SnackPreference>();
        }
        else
        {
            ViewBag.UserSnackPreferences = new List<SnackPreference>();
        }

        return View(gameEvent);
    }

    [Authorize]
    public async Task<IActionResult> Organised()
    {
        var userId = userManager.GetUserId(User)!;
        return View(await gameEventRepository.ByOrganiser(userId));
    }

    [Authorize]
    public async Task<IActionResult> Participating()
    {
        var userId = userManager.GetUserId(User)!;
        return View(await gameEventRepository.ByRegistrations(userId));
    }

    [Authorize]
    public async Task<IActionResult> Create()
    {
        await loadGamesAndSnackPreferences();
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(GameEventModel gameEventModel)
    {
        if (!ModelState.IsValid)
        {
            await loadGamesAndSnackPreferences();
            return View(gameEventModel);
        }

        try
        {
            var userId = userManager.GetUserId(User)!;
            await gameEventRepository.Create(new GameEvent
            {
                Name = gameEventModel.Name,
                MaxPlayers = gameEventModel.MaxPlayers,
                Is18Plus = gameEventModel.Is18Plus,
                DateTime = gameEventModel.DateTime,
                Duration = gameEventModel.Duration,
                ImageUri = gameEventModel.ImageUri,
                Organizer =
                    await userRepository.ById(userId) ?? throw new ArgumentException("Logged in user not found"),
                Address = new Address
                {
                    City = gameEventModel.City,
                    Street = gameEventModel.Street,
                    HouseNumber = gameEventModel.HouseNumber
                },
                Games = await gameRepository.ByIds(gameEventModel.Games),
                AvailableSnacks = await snackPreferenceRepository.ByIds(gameEventModel.AvailableSnacks)
            });
            return RedirectToAction("Organised");
        }
        catch (BusinessLogicException err)
        {
            return View("Error", err.Message);
        }
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var gameEvent = await gameEventRepository.ById(id);
        if (gameEvent == null || gameEvent.Organizer.Id != userManager.GetUserId(User))
            return NotFound();

        await loadGamesAndSnackPreferences();
        return View(new GameEventModel
        {
            Id = gameEvent.Id,
            Name = gameEvent.Name,
            MaxPlayers = gameEvent.MaxPlayers,
            Is18Plus = gameEvent.Is18Plus,
            DateTime = gameEvent.DateTime,
            Duration = gameEvent.Duration,
            ImageUri = gameEvent.ImageUri,
            City = gameEvent.Address.City,
            Street = gameEvent.Address.Street,
            HouseNumber = gameEvent.Address.HouseNumber,
            Games = gameEvent.Games.Select(g => g.Id).ToList(),
            AvailableSnacks = gameEvent.AvailableSnacks.Select(s => s.Id).ToList()
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(GameEventModel gameEventModel)
    {
        if (!ModelState.IsValid)
        {
            await loadGamesAndSnackPreferences();
            return View(gameEventModel);
        }

        var gameEvent = await gameEventRepository.ById(gameEventModel.Id);
        if (gameEvent == null || gameEvent.Organizer.Id != userManager.GetUserId(User))
        {
            return NotFound();
        }

        gameEvent.Name = gameEventModel.Name;
        gameEvent.MaxPlayers = gameEventModel.MaxPlayers;
        gameEvent.Is18Plus = gameEventModel.Is18Plus;
        gameEvent.DateTime = gameEventModel.DateTime;
        gameEvent.Duration = gameEventModel.Duration;
        gameEvent.Address.City = gameEventModel.City;
        gameEvent.Address.Street = gameEventModel.Street;
        gameEvent.Address.HouseNumber = gameEventModel.HouseNumber;
        gameEvent.Games = await gameRepository.ByIds(gameEventModel.Games);
        gameEvent.AvailableSnacks = await snackPreferenceRepository.ByIds(gameEventModel.AvailableSnacks);

        try
        {
            await gameEventRepository.Update(gameEvent);
            return RedirectToAction("Organised");
        }
        catch (BusinessLogicException err)
        {
            return View("Error", err.Message);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var gameEvent = await gameEventRepository.ById(id);
        if (gameEvent == null || gameEvent.Organizer.Id != userManager.GetUserId(User))
        {
            return NotFound();
        }


        try
        {
            await gameEventRepository.Delete(gameEvent);
            return RedirectToAction("Organised");
        }
        catch (BusinessLogicException err)
        {
            return View("Error", err.Message);
        }
    }

    [Authorize]
    public async Task<IActionResult> Registrations(int id)
    {
        var gameEvent = await gameEventRepository.ById(id);
        if (gameEvent == null || gameEvent.Organizer.Id != userManager.GetUserId(User))
        {
            return NotFound();
        }

        var registrations = await registrationRepository.ByGameEvent(id);
        var attendance = await registrationRepository.CalculateAttendanceForUsers(
            registrations.Select(r => r.User.Id).ToList());

        return View(registrations.Select(r =>
        {
            attendance.TryGetValue(r.User.Id, out var stats);
            return new RegistrationModel
            {
                Id = r.Id,
                DidAttend = r.DidAttend,
                User = r.User,
                AttendanceCount = stats?.Item1 ?? 0,
                NonAttendanceCount = stats?.Item2 ?? 0
            };
        }).ToList());
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Registrations([FromRoute] int id, RegistrationModel registrationModel)
    {
        var gameEvent = await gameEventRepository.ById(id);
        if (gameEvent == null || gameEvent.Organizer.Id != userManager.GetUserId(User))
        {
            return NotFound();
        }

        var registration = await registrationRepository.ById(registrationModel.Id);
        if (registration == null)
        {
            return NotFound();
        }

        registration.DidAttend = registrationModel.DidAttend;

        try
        {
            await registrationRepository.Update(registration);
            return RedirectToAction("Registrations");
        }
        catch (BusinessLogicException err)
        {
            return View("Error", err.Message);
        }
    }

    private async Task loadGamesAndSnackPreferences()
    {
        ViewBag.Games = new SelectList(
            await gameRepository.All(),
            "Id",
            "Name");

        ViewBag.SnackPreferences = new SelectList(
            await snackPreferenceRepository.All(),
            "Id",
            "Name");
    }
}