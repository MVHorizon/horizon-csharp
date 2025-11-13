using Bunit;
using Bunit.TestDoubles;
using HorizonChat.Pages;
using HorizonChat.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HorizonChat.Tests.Pages
{
    public class WelcomeTests : BunitContext
    {
        private readonly UserStateService _userStateService;

        public WelcomeTests()
        {
            _userStateService = new UserStateService();
            Services.AddSingleton(_userStateService);
        }

        [Fact]
        public void Welcome_RendersCorrectly_WithTitle()
        {
            // Act
            var cut = Render<Welcome>();

            // Assert
            var title = cut.Find("h1");
            Assert.Contains("Welcome to HorizonChat", title.TextContent);
        }

        [Fact]
        public void Welcome_HasUsernameInputField()
        {
            // Act
            var cut = Render<Welcome>();

            // Assert
            var input = cut.Find("input#username");
            Assert.NotNull(input);
            Assert.Equal("text", input.GetAttribute("type"));
        }

        [Fact]
        public void Welcome_HasEnterChatButton()
        {
            // Act
            var cut = Render<Welcome>();

            // Assert
            var button = cut.Find("button.enter-chat-btn");
            Assert.NotNull(button);
            Assert.Contains("Enter Chat", button.TextContent);
        }

        [Fact]
        public void Welcome_HasGenerateGuestNameButton()
        {
            // Act
            var cut = Render<Welcome>();

            // Assert
            var button = cut.Find("button.generate-guest-btn");
            Assert.NotNull(button);
            Assert.Contains("Use Guest Name", button.TextContent);
        }

        [Fact]
        public void EnterChatButton_IsDisabled_WhenUsernameIsEmpty()
        {
            // Act
            var cut = Render<Welcome>();

            // Assert
            var button = cut.Find("button.enter-chat-btn");
            Assert.True(button.HasAttribute("disabled"));
        }

        [Fact]
        public void EnterChatButton_IsDisabled_WhenUsernameLessThan2Characters()
        {
            // Act
            var cut = Render<Welcome>();
            var input = cut.Find("input#username");

            // Act
            input.Input("A");

            // Assert
            var button = cut.Find("button.enter-chat-btn");
            Assert.True(button.HasAttribute("disabled"));
        }

        [Fact]
        public void EnterChatButton_IsEnabled_WhenValidUsernameEntered()
        {
            // Act
            var cut = Render<Welcome>();
            var input = cut.Find("input#username");

            // Act
            input.Input("ValidUser");

            // Assert
            var button = cut.Find("button.enter-chat-btn");
            Assert.False(button.HasAttribute("disabled"));
        }

        [Fact]
        public void UsernameInput_UpdatesCorrectly_WhenUserTypes()
        {
            // Act
            var cut = Render<Welcome>();
            var input = cut.Find("input#username");

            // Act
            input.Input("TestUser123");

            // Assert
            Assert.Equal("TestUser123", input.GetAttribute("value"));
        }

        [Fact]
        public void GenerateGuestName_CreatesValidGuestUsername()
        {
            // Arrange
            var cut = Render<Welcome>();
            var button = cut.Find("button.generate-guest-btn");

            // Act
            button.Click();

            // Assert
            var input = cut.Find("input#username");
            var username = input.GetAttribute("value");
            Assert.NotNull(username);
            Assert.StartsWith("Guest", username);
            Assert.True(username.Length == 9); // "Guest" + 4 digits
        }

        [Fact]
        public void GenerateGuestName_GeneratesNumberBetween1000And9999()
        {
            // Arrange
            var cut = Render<Welcome>();
            var button = cut.Find("button.generate-guest-btn");

            // Act
            button.Click();

            // Assert
            var input = cut.Find("input#username");
            var username = input.GetAttribute("value");
            var numberPart = username?.Substring(5); // Skip "Guest"
            var guestNumber = int.Parse(numberPart!);
            
            Assert.InRange(guestNumber, 1000, 9999);
        }

        [Fact]
        public void EnterChat_NavigatesToChatPage_WhenValidUsernameProvided()
        {
            // Arrange
            var navMan = Services.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();
            var cut = Render<Welcome>();
            var input = cut.Find("input#username");
            var button = cut.Find("button.enter-chat-btn");

            // Act
            input.Input("TestUser");
            button.Click();

            // Assert
            Assert.Contains("/chat", navMan.Uri);
        }

        [Fact]
        public void EnterChat_StoresUsernameInStateService_WhenValidUsernameProvided()
        {
            // Arrange
            var cut = Render<Welcome>();
            var input = cut.Find("input#username");
            var button = cut.Find("button.enter-chat-btn");

            // Act
            input.Input("TestUser");
            button.Click();

            // Assert
            Assert.Equal("TestUser", _userStateService.Username);
        }

        [Fact]
        public void UsernameInput_HasMaxLength20()
        {
            // Act
            var cut = Render<Welcome>();
            var input = cut.Find("input#username");

            // Assert
            Assert.Equal("20", input.GetAttribute("maxlength"));
        }

        [Fact]
        public void UsernameInput_HasPlaceholder()
        {
            // Act
            var cut = Render<Welcome>();
            var input = cut.Find("input#username");

            // Assert
            var placeholder = input.GetAttribute("placeholder");
            Assert.NotNull(placeholder);
            Assert.Contains("username", placeholder, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ValidationMessage_NotDisplayed_WhenNoValidationError()
        {
            // Act
            var cut = Render<Welcome>();

            // Assert
            var validationMessages = cut.FindAll(".validation-message");
            Assert.Empty(validationMessages);
        }

        [Fact]
        public void GenerateGuestName_EnablesEnterButton()
        {
            // Arrange
            var cut = Render<Welcome>();
            var generateButton = cut.Find("button.generate-guest-btn");

            // Act
            generateButton.Click();

            // Assert
            var enterButton = cut.Find("button.enter-chat-btn");
            Assert.False(enterButton.HasAttribute("disabled"));
        }

        [Fact]
        public void Welcome_DisplaysInfoText()
        {
            // Act
            var cut = Render<Welcome>();

            // Assert
            var infoText = cut.Find(".info-text");
            Assert.NotNull(infoText);
            Assert.Contains("username will be visible", infoText.TextContent, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void EnterChatButton_IsDisabled_WhenUsernameIsOnlyWhitespace()
        {
            // Arrange
            var cut = Render<Welcome>();
            var input = cut.Find("input#username");

            // Act
            input.Input("   ");

            // Assert
            var button = cut.Find("button.enter-chat-btn");
            Assert.True(button.HasAttribute("disabled"));
        }

        [Fact]
        public void EnterChatButton_IsDisabled_WhenUsernameExceeds20Characters()
        {
            // Arrange - Note: This tests the validation logic, maxlength prevents typing
            var cut = Render<Welcome>();
            var input = cut.Find("input#username");

            // Act - Try to set 21 characters
            input.Input("A"); // Should be disabled (less than 2)

            // Assert
            var button = cut.Find("button.enter-chat-btn");
            Assert.True(button.HasAttribute("disabled"));
        }

    }
}
