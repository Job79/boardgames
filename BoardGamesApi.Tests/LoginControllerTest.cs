using BoardGamesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BoardGamesApi.Tests;

public class LoginControllerTest
{
    [Fact]
    public async Task Login_with_valid_credentials_returns_token()
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
        var config = new Mock<IConfiguration>();
        config.Setup(x => x["Jwt:Key"]).Returns("test_keyofatleast16chars");
        config.Setup(x => x["Jwt:Issuer"]).Returns("test_issuer");
        config.Setup(x => x["Jwt:Audience"]).Returns("test_audience");
        var controller = new LoginController(config.Object, userManager.Object, signInManager.Object);

        // Act
        var result = await controller.Login(new LoginModel { Username = "John", Password = "password" });

        // Assert
        Assert.IsType<JsonResult>(result);
    }

    [Fact]
    public async Task Login_with_invalid_credentials_returns_unauthorized()
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
        var config = new Mock<IConfiguration>();
        config.Setup(x => x["Jwt:Key"]).Returns("test_keyofatleast16chars");
        config.Setup(x => x["Jwt:Issuer"]).Returns("test_issuer");
        config.Setup(x => x["Jwt:Audience"]).Returns("test_audience");
        var controller = new LoginController(config.Object, userManager.Object, signInManager.Object);

        // Act
        var result = await controller.Login(new LoginModel { Username = "John", Password = "password" });

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}