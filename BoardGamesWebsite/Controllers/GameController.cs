using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamesWebsite.Controllers;

public class GameController : Controller
{
    private readonly IGameRepository gameRepository;

    public GameController(IGameRepository gameRepository)
    {
        this.gameRepository = gameRepository;
    }

    public async Task<IActionResult> Details(int id)
    {
        var game = await gameRepository.ById(id);
        if (game == null)
        {
            return NotFound();
        }

        return View(game);
    }
}