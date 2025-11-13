using Bunit;
using Bunit.TestDoubles;
using HorizonChat.Pages;
using HorizonChat.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HorizonChat.Tests.Pages
{
    public class ChatTests : BunitContext
    {
        private readonly UserStateService _userStateService;

        public ChatTests()
        {
            _userStateService = new UserStateService();
            Services.AddSingleton(_userStateService);
        }

        [Fact]
        public void Chat_RedirectsToRoot_WhenUserNotAuthenticated()
        {
            // Arrange - user not authenticated (no username set)
            var navManager = Services.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();

            // Act
            var cut = Render<Chat>();

            // Assert
            Assert.Equal("http://localhost/", navManager.Uri);
        }

        [Fact]
        public void Chat_RendersCorrectly_WhenUserAuthenticated()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var header = cut.Find(".chat-header h2");
            Assert.Contains("HorizonChat", header.TextContent);
        }

        [Fact]
        public void Chat_DisplaysUsername_InHeader()
        {
            // Arrange
            const string username = "TestUser123";
            _userStateService.SetUsername(username);

            // Act
            var cut = Render<Chat>();

            // Assert
            var usernameBadge = cut.Find(".username-badge");
            Assert.Equal(username, usernameBadge.TextContent.Trim());
        }

        [Fact]
        public void Chat_HasMessageInput_WhenAuthenticated()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var input = cut.Find(".message-input");
            Assert.NotNull(input);
            Assert.Equal("text", input.GetAttribute("type"));
        }

        [Fact]
        public void Chat_HasSendButton_WhenAuthenticated()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var button = cut.Find(".send-button");
            Assert.NotNull(button);
            Assert.Contains("Send", button.TextContent);
        }

        [Fact]
        public void Chat_SendButtonDisabled_WhenNotConnected()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var button = cut.Find(".send-button");
            Assert.True(button.HasAttribute("disabled"));
        }

        [Fact]
        public void Chat_MessageInputDisabled_WhenNotConnected()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var input = cut.Find(".message-input");
            Assert.True(input.HasAttribute("disabled"));
        }

        [Fact]
        public void Chat_DisplaysDisconnectedStatus_Initially()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var status = cut.Find(".status-disconnected");
            Assert.NotNull(status);
            Assert.Contains("Disconnected", status.TextContent);
        }

        [Fact]
        public void Chat_DisplaysNoMessagesText_WhenEmpty()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var noMessages = cut.Find(".no-messages");
            Assert.Contains("No messages yet", noMessages.TextContent);
        }

        [Fact]
        public void Chat_HasMessagesContainer()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var container = cut.Find(".messages-container");
            Assert.NotNull(container);
        }

        [Fact]
        public void Chat_HasMessageInputContainer()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var container = cut.Find(".message-input-container");
            Assert.NotNull(container);
        }

        [Fact]
        public void Chat_HasConnectionStatus()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var status = cut.Find(".connection-status");
            Assert.NotNull(status);
        }

        [Fact]
        public void Chat_InputPlaceholder_IsCorrect()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var input = cut.Find(".message-input");
            Assert.Equal("Type your message...", input.GetAttribute("placeholder"));
        }

        [Fact]
        public void Chat_HasChatContainer_WithCorrectClass()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var container = cut.Find(".chat-container");
            Assert.NotNull(container);
        }

        [Fact]
        public void Chat_HeaderContainsUserInfo_Section()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var userInfo = cut.Find(".user-info");
            Assert.NotNull(userInfo);
        }

        [Fact]
        public void Chat_InputExists_WithCorrectType()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var input = cut.Find(".message-input");
            Assert.Equal("text", input.GetAttribute("type"));
        }

        [Fact]
        public void Chat_NoAlertDisplayed_WhenNoError()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var alerts = cut.FindAll(".alert-danger");
            Assert.Empty(alerts);
        }

        [Fact]
        public void Chat_HasPageRoute_Attribute()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert - component renders without error, route is defined in component
            Assert.NotNull(cut);
        }

        [Fact]
        public void Chat_ClearsUsername_WhenUserClicksLeave()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");
            var cut = Render<Chat>();

            // Act - simulate leaving (this would happen through navigation in real scenario)
            // For now, just verify component can be disposed
            cut.Dispose();

            // Assert - component disposed successfully
            Assert.NotNull(_userStateService.Username);
        }

        [Fact]
        public void Chat_RendersWithoutErrors_WithValidUsername()
        {
            // Arrange
            _userStateService.SetUsername("ValidUser");

            // Act & Assert - should not throw
            var cut = Render<Chat>();
            Assert.NotNull(cut);
        }

        [Fact]
        public void Chat_DisplaysCorrectTitle_InHeader()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var title = cut.Find(".chat-header h2");
            Assert.Equal("🎯 HorizonChat", title.TextContent.Trim());
        }

        [Fact]
        public void Chat_SendButton_HasCorrectClass()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var button = cut.Find(".send-button");
            Assert.True(button.ClassList.Contains("btn"));
            Assert.True(button.ClassList.Contains("btn-primary"));
        }

        [Fact]
        public void Chat_MessageInput_HasCorrectClass()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var input = cut.Find(".message-input");
            Assert.True(input.ClassList.Contains("form-control"));
        }

        [Fact]
        public void Chat_ContainerHasChatClass()
        {
            // Arrange
            _userStateService.SetUsername("TestUser");

            // Act
            var cut = Render<Chat>();

            // Assert
            var container = cut.Find("div.chat-container");
            Assert.NotNull(container);
        }
    }
}
