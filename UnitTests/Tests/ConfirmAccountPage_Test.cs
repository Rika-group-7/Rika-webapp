using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using RikaWebApp.Components.Pages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;

namespace UnitTests.Tests;

public class ConfirmAccountPage_Test
{
    //Testar confirmaccount page 

    [Fact]
    public void PageComponent_ShouldRenderCorrectly()
    {
        //Arrange
        using var testContext = new TestContext();

        //Act
        var cut = testContext.RenderComponent<ConfirmAccount>();

        //Assert
        Assert.Contains("Verify account", cut.Markup); //texten
        Assert.Contains("Enter your verification code", cut.Markup ); //texten
        var inputs = cut.FindAll("input.input-code");
        Assert.Equal(6, inputs.Count()); //en för varje input till verification code
    }
    
    [Fact]
    public void UserIdMissing_ShouldShowErrorMsg()
    {
        //Arrange
        var testContext = new TestContext();
        var fakeNavigationManager = testContext.Services.GetRequiredService<FakeNavigationManager>();
        fakeNavigationManager.NavigateTo("http://localhost/confirmaccount");

        //Act
        var cut = testContext.RenderComponent<ConfirmAccount>();

        //Assert
        Assert.Contains("User ID is missing from the URL", cut.Markup); //meddelandet som kommer upp i koden
    }

    [Fact]
    public async Task GetEmailFromUserId_ShouldShowEmailIfSucceeded()
    {
        //Arrange
        var testContext = new TestContext();
        var mockMsgHandler = new Mock<HttpMessageHandler>();

        mockMsgHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK, //returnera ok! 
                Content = JsonContent.Create(new {Email = "test@domain.se"})
            });

        var httpClient = new HttpClient(mockMsgHandler.Object);
        testContext.Services.AddSingleton(httpClient);

        //Act
        var cut = testContext.RenderComponent<ConfirmAccount>();

        //Assert
        Assert.Equal("test@domain.se", cut.Instance.inputModel.Email);
    }
}
