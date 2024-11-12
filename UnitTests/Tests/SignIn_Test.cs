
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using RikaWebApp.Components.Pages;
using System.Net;
using System.Runtime.InteropServices;

namespace UnitTests.Tests;

public class SignIn_Test : TestContext
{
    [Fact]
    //testar att inte fylla i något i formuläret, ergo är email tomt. Ska inte fungera. 
    public void SignIn_EmailEmpty_ShouldShowValidationMessage()
    {
        //Arrange
        var testContext =  new TestContext(); //använder mig av bunit för att simulera en context 
        var cut = testContext.RenderComponent<SignIn>(); //renderar min komponent SignIn

        //Act
        cut.Find("button.primary-btn").Click(); //simulerar att man klickar på login-knappen

        //Assert
        Assert.Contains("text-danger", cut.Markup); //kollar valideringen och ska få error
    }

    [Fact]
    //testar om det går att logga in och får ett ok
    public async void SignIn_AvailableToSignIn_ShouldReturnSuccessMessage()
    {
        //Arrange
        using var testContext = new TestContext();
        var msgHandler = new Mock<HttpMessageHandler>();
        msgHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK, //ska returnera ok om det funkar när man skickar formuläret
            });

        var httpClient = new HttpClient(msgHandler.Object);
        testContext.Services.AddSingleton(httpClient);
        
        var fakeNavigationManager = testContext.Services.GetRequiredService<NavigationManager>(); //testar att skapa en fake navigation manager eftersom det inte fungerade


        //Act
        var cut = testContext.RenderComponent<SignIn>(parameters => parameters
        .Add(p => p.httpClient, httpClient)
        .Add(p => p.@object, fakeNavigationManager));

        //Assert
        Assert.Equal("Redirect to home...", cut.Instance.successMessage); //ger success och sen navigatar till home
        Assert.Equal("/", ((FakeNavigationManager)fakeNavigationManager).Uri); //måste lägga till forceload på denna
    }

    [Fact]
    public void SignInNavigation_ShouldNavigateToSignUpPage()
    {
        // Arrange
        var testContext = new TestContext();
        var navigationMock = new Mock<NavigationManager>();
        testContext.Services.AddSingleton<NavigationManager, FakeNavigationManager>();

        // Rendera SignIn-komponenten
        var cut = testContext.RenderComponent<SignIn>();

        // Act
        var signUpButton = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Sign Up"));
        Assert.NotNull(signUpButton);
        signUpButton.Click();

        // Assert
        var navigationManager = (FakeNavigationManager)testContext.Services.GetRequiredService<NavigationManager>();
        Assert.Contains("/signup", navigationManager.Uri); // Kontrollera att URI innehåller "/signup"

        ////Arrange
        //var testContext = new TestContext();
        //var navigationManager = testContext.Services.GetRequiredService<FakeNavigationManager>();
        //var cut = testContext.RenderComponent<SignUp>();

        ////Act
        //cut.Find("button.btn").Click(); //simulerar att man klickar på knappen



        ////Assert
        //Assert.Contains("/signup", navigationManager.Uri);
    }

    [Fact]
    //ett renderingstest för formuläret
    public void SignInFormShouldRenderCorrectly_ReturnOk()
    {
        //Arrange
        var testContext = new TestContext();
        var cut = testContext.RenderComponent<SignIn>(); //rendera komponenten

        //Act (and assert)
        Assert.NotNull(cut.Find("input[type='email']"));
        Assert.NotNull(cut.Find("input[type='password']"));
        Assert.NotNull(cut.Find("button.primary-btn"));
        Assert.NotNull(cut.Find("button.secondary-btn")); //renderar inputfälten


    }
}
