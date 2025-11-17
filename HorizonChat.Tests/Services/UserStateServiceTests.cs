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

        [Fact]
        public void Status_ShouldBeAvailable_WhenServiceIsInitialized()
        {
            // Arrange & Act
            var service = new UserStateService();

            // Assert
            Assert.Equal("Available", service.Status);
        }

        [Fact]
        public void SetStatus_ShouldUpdateStatus_WhenValidStatusProvided()
        {
            // Arrange
            var service = new UserStateService();
            var expectedStatus = "Busy";

            // Act
            service.SetStatus(expectedStatus);

            // Assert
            Assert.Equal(expectedStatus, service.Status);
        }

        [Fact]
        public void SetStatus_ShouldThrowArgumentException_WhenStatusIsEmpty()
        {
            // Arrange
            var service = new UserStateService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.SetStatus(""));
        }

        [Fact]
        public void SetStatus_ShouldThrowArgumentException_WhenStatusIsWhitespace()
        {
            // Arrange
            var service = new UserStateService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.SetStatus("   "));
        }

        [Fact]
        public void SetStatus_ShouldThrowArgumentException_WhenStatusExceeds30Characters()
        {
            // Arrange
            var service = new UserStateService();
            var longStatus = new string('A', 31);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.SetStatus(longStatus));
        }

        [Fact]
        public void SetStatus_ShouldAcceptCustomStatus_WithinCharacterLimit()
        {
            // Arrange
            var service = new UserStateService();
            var customStatus = "Working from home";

            // Act
            service.SetStatus(customStatus);

            // Assert
            Assert.Equal(customStatus, service.Status);
        }

        [Fact]
        public void SetStatus_ShouldTriggerOnChangeEvent()
        {
            // Arrange
            var service = new UserStateService();
            var eventTriggered = false;
            service.OnChange += () => eventTriggered = true;

            // Act
            service.SetStatus("Away");

            // Assert
            Assert.True(eventTriggered);
        }

        [Fact]
        public void ClearUsername_ShouldResetStatusToAvailable()
        {
            // Arrange
            var service = new UserStateService();
            service.SetUsername("TestUser");
            service.SetStatus("Busy");

            // Act
            service.ClearUsername();

            // Assert
            Assert.Equal("Available", service.Status);
        }

        [Fact]
        public void AvailableStatuses_ShouldContainPredefinedStatuses()
        {
            // Arrange
            var service = new UserStateService();

            // Act
            var statuses = service.AvailableStatuses.ToList();

            // Assert
            Assert.Contains("Available", statuses);
            Assert.Contains("Busy", statuses);
            Assert.Contains("Away", statuses);
            Assert.Contains("Do Not Disturb", statuses);
        }

        [Fact]
        public void SetStatus_ShouldAcceptPredefinedStatus()
        {
            // Arrange
            var service = new UserStateService();

            // Act
            service.SetStatus("Do Not Disturb");

            // Assert
            Assert.Equal("Do Not Disturb", service.Status);
        }
    }
}
