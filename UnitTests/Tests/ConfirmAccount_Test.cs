using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using RikaWebApp.Components.Pages;
using System.Net;
using System.Net.Http.Json;

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


        component.Instance.signUpModel.Password = "bytmig123"; //placeholder
        component.Instance.confirmPassword = "bytmig123!!"; //placeholder, samma som ovan

        //Act
        await component.InvokeAsync(() => component.Instance.RegisterUserAsync()); //testar registeruserasync genmom att kolla så att lösenorden matchar!
        component.Render();

        //Assert 
        Assert.Contains("Password does not match", component.Markup); //ska returnera att det inte fungerar. Detta test för att man inte ska kunna skapa en user utan att password och confirm password matchar. 

    }

    [Fact]
    public async Task RegisterUserAsync_RegisterSuccess_NavigatesToConfirmationPage()
    {


        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://rika-authenticationprovider-drfta9bhdaf0g0dr.westeurope-01.azurewebsites.net/api/Auth/signup")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"UserId\":\"12345\"}")
            });

        var httpClient = new HttpClient(mockHandler.Object)
        {
            BaseAddress = new Uri("https://rika-authenticationprovider-drfta9bhdaf0g0dr.westeurope-01.azurewebsites.net")
        };

        Services.AddSingleton<HttpClient>(httpClient);

        var component = RenderComponent<SignUp>();

        component.Instance.signUpModel.UserName = "testuser";
        component.Instance.signUpModel.Email = "testemail@domain.se";
        component.Instance.signUpModel.Password = "Bytmig123!";
        component.Instance.confirmPassword = "Bytmig123!"; //Testar "formuläret" med ett matchande lösenord också 
        component.Instance.signUpModel.Terms = true;

        // Act
        await component.InvokeAsync(() => component.Instance.RegisterUserAsync()); //genomför testet

        component.Render(); //renderar om 

        // Assert
        Assert.Contains("Registration successful. Redirecting to confirmation page...", component.Markup); //om det fungerar
    }

}
