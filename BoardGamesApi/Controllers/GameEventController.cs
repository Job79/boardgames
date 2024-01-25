using BoardGamesApi.Models;
using Core.Domain;
using Core.DomainServices.Exceptions;
using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamesApi.Controllers;

[ApiController]
[Route("api/gameEvents")]
public class GameEventController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly IGameEventRepository gameEventRepository;
    private readonly IUserRepository userRepository;
    private readonly IRegistrationRepository registrationRepository;

    public GameEventController(
        UserManager<IdentityUser> userManager,
        IGameEventRepository gameEventRepository,
        IUserRepository userRepository,
        IRegistrationRepository registrationRepository)
    {
        this.userManager = userManager;
        this.gameEventRepository = gameEventRepository;
        this.userRepository = userRepository;
        this.registrationRepository = registrationRepository;
    }

    [HttpGet]
    [Route("")]
    [ResponseCache(Duration = 30)]
    [ProducesResponseType(typeof(ICollection<GameEvent>), 200)]
    public async Task<ICollection<GameEvent>> All()
    {
        return await gameEventRepository.All();
    }

    [HttpGet]
    [Route("{id}")]
    [ResponseCache(Duration = 30)]
    [ProducesResponseType(typeof(GameEvent), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<GameEvent>> ById([FromRoute] int id)
    {
        var gameEvent = await gameEventRepository.ById(id);
        if (gameEvent == null)
        {
            return NotFound(new {error = "Game event not found"});
        }

        return gameEvent;
    }

    [Authorize]
    [HttpPost]
    [Route("{id}/register")]
    [ProducesResponseType(typeof(Registration), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<ActionResult<Registration>> Register([FromRoute] int id)
    {
        var userId = userManager.GetUserId(User)!;
        var user = await userRepository.ById(userId) ?? throw new ArgumentException("Logged in user not found");
        var gameEvent = await gameEventRepository.ById(id);
        if (gameEvent == null)
        {
            return NotFound(new ErrorResponse { Error = "Game event not found" });
        }

        var registration = new Registration
        {
            GameEvent = gameEvent,
            User = user,
            Timestamp = DateTime.Now,
            DidAttend = null
        };

        try
        {
            await registrationRepository.Create(registration);
            return registration;
        }
        catch (BusinessLogicException err)
        {
            return BadRequest(new { error = err.Message });
        }
    }
}