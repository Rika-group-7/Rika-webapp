using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RikaWebApp.Components.Pages;

namespace UnitTests.Tests;

public class ConfirmAccount_Test : TestContext
{
    /// <summary>
    /// Testar så att modellen är ifylld korrekt och informationen kommer in (inputfälten har placeholders för att simulera att det fungerar som det ska)
    /// </summary>
    [Fact]
    public void SignUpComponent_ShouldReturnOk()
    {
        // Arrange
        var component = RenderComponent<SignUp>();

        // Act - skriver in placeholders för en user enligt modellen 
        var userNameInput = component.Find("input[placeholder='User Name']");
        var emailInput = component.Find("input[placeholder='Email']");
        var passwordInput = component.Find("input[placeholder='******']");
        var confirmPasswordInput = component.Find("input[placeholder='******']");

        // Assert
        Assert.NotNull(userNameInput);
        Assert.NotNull(emailInput);
        Assert.NotNull(passwordInput);
        Assert.NotNull(confirmPasswordInput);
    }

    [Fact]
    public async Task RegisterUserAsync_PasswordsDoesntMatch_ShouldShowError()
    {

        //Arrange
        var mockHttpClient = new Mock<HttpClient>();
        Services.AddSingleton(mockHttpClient.Object);

        var component = RenderComponent<SignUp>();

    }
}
