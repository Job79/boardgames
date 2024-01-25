using System.Security.Claims;
using BoardGamesWebsite.Models;
using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamesWebsite.Tests.Controllers;

public class AccountControllerTest
{
    private readonly List<User> users = new()
    {
        new()
        {
            Id = "a9b0c50c-90ff-4a82-a7b8-4d8187bc887a",
            Username = "John",
            Gender = Gender.Man,
            BirthDate = DateTime.Now.AddYears(-20),
            Address = new()
            {
                City = "City 1",
                Street = "Street 1",
                HouseNumber = "1a"
            }
        }
    };

    [Fact]
    public void Login_returns_view()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var signInManager = new Mock<SignInManager<IdentityUser>>(userManager.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
        var userRepository = new Mock<IUserRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var controller = new AccountController(userManager.Object, signInManager.Object, userRepository.Object,
            snackPreferenceRepository.Object);

        // Act
        var result = controller.Login();

        // Assert
        Assert.IsType<ViewResult>(result);
        Assert.Null(((ViewResult)result).ViewName);
    }

    [Fact]
    public async Task Login_with_valid_credentials_returns_redirect_to_all()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new IdentityUser("a9b0c50c-90ff-4a82-a7b8-4d8187bc887a"));
        var signInManager = new Mock<SignInManager<IdentityUser>>(userManager.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
        signInManager.Setup(x =>
                x.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        var userRepository = new Mock<IUserRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var controller = new AccountController(userManager.Object, signInManager.Object, userRepository.Object,
            snackPreferenceRepository.Object);

        // Act
        var result = await controller.Login(new LoginModel { Username = "John", Password = "password" });

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", ((RedirectToActionResult)result).ActionName);
    }

    [Fact]
    public async Task Login_with_invalid_credentials_returns_view_with_error()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new IdentityUser("a9b0c50c-90ff-4a82-a7b8-4d8187bc887a"));
        var signInManager = new Mock<SignInManager<IdentityUser>>(userManager.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
        signInManager.Setup(x =>
                x.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
        var userRepository = new Mock<IUserRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var controller = new AccountController(userManager.Object, signInManager.Object, userRepository.Object,
            snackPreferenceRepository.Object);

        // Act
        var result = await controller.Login(new LoginModel { Username = "John", Password = "password" });

        // Assert
        Assert.IsType<ViewResult>(result);
        Assert.Null(((ViewResult)result).ViewName);
        Assert.Equal("Ongeldige login gegevens",
            ((ViewResult)result).ViewData.ModelState["LoginError"]!.Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task Register_returns_view()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var signInManager = new Mock<SignInManager<IdentityUser>>(userManager.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
        var userRepository = new Mock<IUserRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        snackPreferenceRepository.Setup(x => x.All()).ReturnsAsync(new List<SnackPreference>());
        var controller = new AccountController(userManager.Object, signInManager.Object, userRepository.Object,
            snackPreferenceRepository.Object);

        // Act
        var result = await controller.Register();

        // Assert
        Assert.IsType<ViewResult>(result);
        Assert.Null(((ViewResult)result).ViewName);
        snackPreferenceRepository.Verify(x => x.All(), Times.Once);
    }

    [Fact]
    public async Task Register_with_valid_credentials_returns_redirect_to_all()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        var signInManager = new Mock<SignInManager<IdentityUser>>(userManager.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.Create(It.IsAny<User>()));
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        snackPreferenceRepository.Setup(x => x.All()).ReturnsAsync(new List<SnackPreference>());
        var controller = new AccountController(userManager.Object, signInManager.Object, userRepository.Object,
            snackPreferenceRepository.Object);

        // Act
        var result = await controller.Register(new UserModel
        {
            Username = "John",
            Password = "password",
            Email = "john@gmail.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            City = "City 1",
            Street = "Street 1",
            HouseNumber = "1a"
        });

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", ((RedirectToActionResult)result).ActionName);
        userManager.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        userRepository.Verify(x => x.Create(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Register_with_invalid_credentials_returns_view_with_error()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());
        var signInManager = new Mock<SignInManager<IdentityUser>>(userManager.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.Create(It.IsAny<User>()));
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        snackPreferenceRepository.Setup(x => x.All()).ReturnsAsync(new List<SnackPreference>());
        var controller = new AccountController(userManager.Object, signInManager.Object, userRepository.Object,
            snackPreferenceRepository.Object);

        // Act
        var result = await controller.Register(new UserModel
        {
            Username = "John",
            Password = "password",
            Email = "john@gmail.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            City = "City 1",
            Street = "Street 1",
            HouseNumber = "1a"
        });

        // Assert
        Assert.IsType<ViewResult>(result);
        Assert.Null(((ViewResult)result).ViewName);
        userManager.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        userRepository.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Logout_returns_redirect_to_all()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var signInManager = new Mock<SignInManager<IdentityUser>>(userManager.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
        signInManager.Setup(x => x.SignOutAsync()).Returns(Task.CompletedTask);
        var userRepository = new Mock<IUserRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var controller = new AccountController(userManager.Object, signInManager.Object, userRepository.Object,
            snackPreferenceRepository.Object);

        // Act
        var result = await controller.Logout();

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", ((RedirectToActionResult)result).ActionName);
        signInManager.Verify(x => x.SignOutAsync(), Times.Once);
    }

    [Fact]
    public async Task Settings_returns_view_with_user_model()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(users[0].Id);
        var signInManager = new Mock<SignInManager<IdentityUser>>(userManager.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.ById(users[0].Id)).ReturnsAsync(users[0]);
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        snackPreferenceRepository.Setup(x => x.All()).ReturnsAsync(new List<SnackPreference>());
        var controller = new AccountController(userManager.Object, signInManager.Object, userRepository.Object,
            snackPreferenceRepository.Object);

        // Act
        var result = await controller.Settings();

        // Assert
        var value = Assert.IsType<ViewResult>(result);
        Assert.Null(value.ViewName);
        userManager.Verify(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()), Times.Once);
        userRepository.Verify(x => x.ById(users[0].Id), Times.Once);
        snackPreferenceRepository.Verify(x => x.All(), Times.Once);
    }
}