using HorizonChat.Services;
using Xunit;

namespace HorizonChat.Tests.Services
{
    public class UserStateServiceTests
    {
        [Fact]
        public void Username_ShouldBeEmpty_WhenServiceIsInitialized()
        {
            // Arrange & Act
            var service = new UserStateService();

            // Assert
            Assert.Equal(string.Empty, service.Username);
        }

        [Fact]
        public void IsUserAuthenticated_ShouldBeFalse_WhenUsernameIsEmpty()
        {
            // Arrange
            var service = new UserStateService();

            // Act & Assert
            Assert.False(service.IsUserAuthenticated);
        }

        [Fact]
        public void SetUsername_ShouldUpdateUsername_WhenValidUsernameProvided()
        {
            // Arrange
            var service = new UserStateService();
            var expectedUsername = "TestUser";

            // Act
            service.SetUsername(expectedUsername);

            // Assert
            Assert.Equal(expectedUsername, service.Username);
        }

        [Fact]
        public void SetUsername_ShouldSetIsUserAuthenticatedToTrue_WhenValidUsernameProvided()
        {
            // Arrange
            var service = new UserStateService();

            // Act
            service.SetUsername("TestUser");

            // Assert
            Assert.True(service.IsUserAuthenticated);
        }

        [Fact]
        public void SetUsername_ShouldThrowArgumentException_WhenUsernameIsNull()
        {
            // Arrange
            var service = new UserStateService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.SetUsername(null!));
        }

        [Fact]
        public void SetUsername_ShouldThrowArgumentException_WhenUsernameIsEmpty()
        {
            // Arrange
            var service = new UserStateService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.SetUsername(string.Empty));
        }

        [Fact]
        public void SetUsername_ShouldThrowArgumentException_WhenUsernameIsWhitespace()
        {
            // Arrange
            var service = new UserStateService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.SetUsername("   "));
        }

        [Fact]
        public void SetUsername_ShouldTriggerOnChangeEvent_WhenUsernameIsSet()
        {
            // Arrange
            var service = new UserStateService();
            var eventTriggered = false;
            service.OnChange += () => eventTriggered = true;

            // Act
            service.SetUsername("TestUser");

            // Assert
            Assert.True(eventTriggered);
        }

        [Fact]
        public void ClearUsername_ShouldSetUsernameToEmpty()
        {
            // Arrange
            var service = new UserStateService();
            service.SetUsername("TestUser");

            // Act
            service.ClearUsername();

            // Assert
            Assert.Equal(string.Empty, service.Username);
        }

        [Fact]
        public void ClearUsername_ShouldSetIsUserAuthenticatedToFalse()
        {
            // Arrange
            var service = new UserStateService();
            service.SetUsername("TestUser");

            // Act
            service.ClearUsername();

            // Assert
            Assert.False(service.IsUserAuthenticated);
        }

        [Fact]
        public void ClearUsername_ShouldTriggerOnChangeEvent()
        {
            // Arrange
            var service = new UserStateService();
            service.SetUsername("TestUser");
            var eventTriggered = false;
            service.OnChange += () => eventTriggered = true;

            // Act
            service.ClearUsername();

            // Assert
            Assert.True(eventTriggered);
        }

        [Fact]
        public void SetUsername_ShouldAllowUpdatingExistingUsername()
        {
            // Arrange
            var service = new UserStateService();
            service.SetUsername("OldUser");

            // Act
            service.SetUsername("NewUser");

            // Assert
            Assert.Equal("NewUser", service.Username);
        }

        [Fact]
        public void OnChangeEvent_ShouldSupportMultipleSubscribers()
        {
            // Arrange
            var service = new UserStateService();
            var subscriber1Called = false;
            var subscriber2Called = false;
            
            service.OnChange += () => subscriber1Called = true;
            service.OnChange += () => subscriber2Called = true;

            // Act
            service.SetUsername("TestUser");

            // Assert
            Assert.True(subscriber1Called);
            Assert.True(subscriber2Called);
        }
    }
}
